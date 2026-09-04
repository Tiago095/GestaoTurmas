using Lab;
using Projeto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Projeto_Lab
{
    public partial class HistogramaView : UserControl
    {
        private GerirPautas parentControl;
        private List<LinhaPauta> linhasPauta;
        private List<Tarefa> tarefas;

        public HistogramaView(GerirPautas parent, List<LinhaPauta> linhas, List<Tarefa> tarefasList)
        {
            InitializeComponent();
            parentControl = parent;
            linhasPauta = linhas;
            tarefas = tarefasList;

            ConfigurarComboBox();
            
            this.Loaded += HistogramaView_Loaded;
        }

        private void HistogramaView_Loaded(object sender, RoutedEventArgs e)
        {
            if (HistogramaCanvas.ActualWidth > 0 && HistogramaCanvas.ActualHeight > 0)
            {
                DesenharHistograma();
            }
            else
            {
                HistogramaCanvas.LayoutUpdated += Canvas_LayoutUpdated;
            }
        }

        private void Canvas_LayoutUpdated(object sender, EventArgs e)
        {
            if (HistogramaCanvas.ActualWidth > 0 && HistogramaCanvas.ActualHeight > 0)
            {
                HistogramaCanvas.LayoutUpdated -= Canvas_LayoutUpdated;
                DesenharHistograma();
            }
        }

        private void ConfigurarComboBox()
        {
            var itens = new List<ComboBoxItem>();

            // Adicionar opção para média final
            var itemMedia = new ComboBoxItem
            {
                Content = "Média Final",
                Tag = "media",
                Foreground = Brushes.White
            };
            itens.Add(itemMedia);

            // Adicionar opções para cada tarefa
            foreach (var tarefa in tarefas.OrderBy(t => t.Id))
            {
                var item = new ComboBoxItem
                {
                    Content = tarefa.Titulo,
                    Tag = tarefa.Id.ToString(),
                    Foreground = Brushes.White
                };
                itens.Add(item);
            }

            TarefaComboBox.ItemsSource = itens;
            TarefaComboBox.SelectedIndex = 0; // Selecionar "Média Final" por padrão
        }

        private void TarefaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DesenharHistograma();
        }

        private void DesenharHistograma()
        {
            if (TarefaComboBox.SelectedItem == null) return;

            var selectedItem = TarefaComboBox.SelectedItem as ComboBoxItem;
            var tag = selectedItem.Tag.ToString();

            HistogramaCanvas.Children.Clear();

            List<double> valores = ObterValores(tag);
            if (valores.Count == 0) return;

            // Arredondar valores e criar distribuição
            var valoresArredondados = valores.Select(v => ArredondarNota(v)).ToList();
            var distribuicao = CriarDistribuicao(valoresArredondados);

            DesenharGrafico(distribuicao, tag);
            AtualizarEstatisticas(valores, tag);
        }

        private List<double> ObterValores(string tag)
        {
            var valores = new List<double>();

            if (tag == "media")
            {
                valores = linhasPauta
                    .Where(l => l.Media.HasValue) // Filtrar apenas alunos com média válida
                    .Select(l => l.Media.Value)   // Converter para non-nullable
                    .ToList();
            }
            else
            {
                int tarefaId = int.Parse(tag);
                foreach (var linha in linhasPauta)
                {
                    string idString = tarefaId.ToString();
                    if (linha.Notas.ContainsKey(idString) && linha.Notas[idString].HasValue)
                    {
                        valores.Add(linha.Notas[idString].Value);
                    }
                }
            }

            return valores.Where(v => v >= 0).ToList(); // Filtrar valores válidos
        }

        private int ArredondarNota(double nota)
        {
            return (int)Math.Round(nota, MidpointRounding.AwayFromZero);
        }

        private Dictionary<int, int> CriarDistribuicao(List<int> valoresArredondados)
        {
            var distribuicao = new Dictionary<int, int>();

            // Inicializar todas as notas de 0 a 20 com 0 ocorrências
            for (int i = 0; i <= 20; i++)
            {
                distribuicao[i] = 0;
            }

            // Contar ocorrências
            foreach (var valor in valoresArredondados)
            {
                if (valor >= 0 && valor <= 20)
                {
                    distribuicao[valor]++;
                }
            }

            return distribuicao;
        }

        private void DesenharGrafico(Dictionary<int, int> distribuicao, string tag)
        {
            double canvasWidth = HistogramaCanvas.ActualWidth;
            double canvasHeight = HistogramaCanvas.ActualHeight;

            if (canvasWidth <= 0 || canvasHeight <= 0) return;

            double margemEsquerda = 50;
            double margemDireita = 50;
            double margemSuperior = 50;
            double margemInferior = 80;

            double larguraGrafico = canvasWidth - margemEsquerda - margemDireita;
            double alturaGrafico = canvasHeight - margemSuperior - margemInferior;

            int maxContagem = distribuicao.Values.Max();
            if (maxContagem == 0) maxContagem = 1;

            // Calcular largura das barras (21 barras para notas 0-20)
            double larguraBarra = larguraGrafico / 21.0;
            double espacamento = larguraBarra * 0.1; // 10% de espaçamento
            double larguraBarraReal = larguraBarra - espacamento;

            // Desenhar eixos
            DesenharEixos(margemEsquerda, margemSuperior, larguraGrafico, alturaGrafico, maxContagem);

            // Desenhar barras
            for (int nota = 0; nota <= 20; nota++)
            {
                int contagem = distribuicao[nota];
                if (contagem > 0)
                {
                    double x = margemEsquerda + (nota * larguraBarra) + (espacamento / 2);
                    double alturaRelativa = (double)contagem / maxContagem;
                    double alturaBarra = alturaRelativa * alturaGrafico;
                    double y = margemSuperior + alturaGrafico - alturaBarra;

                    // Escolher cor da barra
                    Brush corBarra = ObterCorBarra(nota);

                    // Desenhar barra com cantos superiores arredondados
                    var borda = new Border
                    {
                        Width = larguraBarraReal,
                        Height = alturaBarra,
                        Background = corBarra,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        // Arredondar apenas os cantos superiores (ordem: canto superior esquerdo, canto superior direito, canto inferior direito, canto inferior esquerdo)
                        CornerRadius = new CornerRadius(5, 5, 0, 0)
                    };

                    Canvas.SetLeft(borda, x);
                    Canvas.SetTop(borda, y);
                    HistogramaCanvas.Children.Add(borda);

                    // Adicionar texto com a contagem no topo da barra
                    var textoContagem = new TextBlock
                    {
                        Text = contagem.ToString(),
                        Foreground = Brushes.White,
                        FontSize = 12,
                        FontWeight = FontWeights.Bold
                    };

                    Canvas.SetLeft(textoContagem, x + (larguraBarraReal / 2) - 5);
                    Canvas.SetTop(textoContagem, y - 20);
                    HistogramaCanvas.Children.Add(textoContagem);
                }

                // Desenhar rótulos do eixo X 
                var textoNota = new TextBlock
                {
                    Text = nota.ToString(),
                    Foreground = Brushes.White,
                    FontSize = 11,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                double xTexto = margemEsquerda + (nota * larguraBarra) + (larguraBarra / 2) - 5;
                Canvas.SetLeft(textoNota, xTexto);
                Canvas.SetTop(textoNota, margemSuperior + alturaGrafico + 10);
                HistogramaCanvas.Children.Add(textoNota);
            }
        }

        private void DesenharEixos(double margemEsquerda, double margemSuperior, double larguraGrafico, double alturaGrafico, int maxContagem)
        {
            // Eixo Y 
            var linhaY = new Line
            {
                X1 = margemEsquerda,
                Y1 = margemSuperior,
                X2 = margemEsquerda,
                Y2 = margemSuperior + alturaGrafico,
                Stroke = Brushes.White,
                StrokeThickness = 2
            };
            HistogramaCanvas.Children.Add(linhaY);

            // Eixo X 
            var linhaX = new Line
            {
                X1 = margemEsquerda,
                Y1 = margemSuperior + alturaGrafico,
                X2 = margemEsquerda + larguraGrafico,
                Y2 = margemSuperior + alturaGrafico,
                Stroke = Brushes.White,
                StrokeThickness = 2
            };
            HistogramaCanvas.Children.Add(linhaX);

            // Linhas horizontais de grade e rótulos do eixo Y
            int numLinhasGrade = Math.Min(maxContagem, 10);
            for (int i = 0; i <= numLinhasGrade; i++)
            {
                double valorY = (double)i * maxContagem / numLinhasGrade;
                double yPosicao = margemSuperior + alturaGrafico - (i * alturaGrafico / numLinhasGrade);

                // Linha de grade
                var linhaGrade = new Line
                {
                    X1 = margemEsquerda,
                    Y1 = yPosicao,
                    X2 = margemEsquerda + larguraGrafico,
                    Y2 = yPosicao,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 0.5,
                    StrokeDashArray = new DoubleCollection { 5, 5 }
                };
                HistogramaCanvas.Children.Add(linhaGrade);

                // Rótulo
                var rotulo = new TextBlock
                {
                    Text = ((int)valorY).ToString(),
                    Foreground = Brushes.White,
                    FontSize = 10
                };
                Canvas.SetLeft(rotulo, margemEsquerda - 30);
                Canvas.SetTop(rotulo, yPosicao - 8);
                HistogramaCanvas.Children.Add(rotulo);
            }
        }

        private Brush ObterCorBarra(int nota)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7289DA"));
        }

        private void AtualizarEstatisticas(List<double> valores, string tag)
        {
            if (valores.Count == 0)
            {
                txtMedia.Text = "-";
                txtMinimo.Text = "-";
                txtMaximo.Text = "-";
                txtTotal.Text = "0";
                return;
            }

            double media = valores.Average();
            double minimo = valores.Min();
            double maximo = valores.Max();
            int total = valores.Count;

            txtMedia.Text = $"{media:N1}";
            txtMinimo.Text = $"{minimo:N1}";
            txtMaximo.Text = $"{maximo:N1}";
            txtTotal.Text = total.ToString();
        }

        private void VoltarButton_Click(object sender, RoutedEventArgs e)
        {
            parentControl.MostrarPauta();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (IsLoaded)
            {
                DesenharHistograma();
            }
        }
    }
}