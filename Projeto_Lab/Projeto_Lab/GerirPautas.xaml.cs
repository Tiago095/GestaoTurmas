using Lab;
using Projeto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Projeto_Lab
{
    public partial class GerirPautas : UserControl
    {
        private App app;
        private List<LinhaPauta> todasLinhasPauta;
        private List<LinhaPauta> linhasFiltradas;

        public GerirPautas()
        {
            InitializeComponent();
            app = App.Current as App;
            
            app.VMTurmaAtual.TurmasAtualizadas += AtualizarListaPautas;
            app.VMTurmaAtual.AlunoEditado += VMTurmaAtual_AlunoEditado;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ApresentarResultados_Click(null, null);
        }
        
        private void AtualizarListaPautas(object? sender, EventArgs e)
        {
            HistogramaContentControl.Content = null;
            HistogramaContentControl.Visibility = Visibility.Collapsed;
            PautaGrid.Visibility = Visibility.Visible;
            
            ApresentarResultados_Click(null, null);
        }
        
        private void VMTurmaAtual_AlunoEditado(object? sender, EventArgs e)
        {
            ApresentarResultados_Click(null, null);
        }

        private void ApresentarResultados_Click(object sender, RoutedEventArgs e)
        {
            if (HistogramaContentControl.Visibility == Visibility.Visible)
            {
                MostrarPauta();
                return;
            }

            if (app.VMTurmaAtual?.TurmaAtual_ == null)
            {
                MessageBox.Show("Nenhuma turma selecionada.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var gridView = PautaListView.View as GridView;
            gridView.Columns.Clear();

            // Adicionar coluna para número do aluno
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Número",
                DisplayMemberBinding = new Binding("NumeroAluno"),
                Width = 80
            });

            // Adicionar coluna para nome do aluno
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Nome do Aluno",
                DisplayMemberBinding = new Binding("Nome"),
                Width = 200
            });

            // Adicionar colunas para cada tarefa
            foreach (var tarefa in app.VMTurmaAtual.TurmaAtual_.Tarefas.OrderBy(t => t.Id))
            {
                gridView.Columns.Add(new GridViewColumn
                {
                    Header = $"{tarefa.Titulo}\n(Peso: {tarefa.Peso:N1})",
                    DisplayMemberBinding = new Binding($"Notas[{tarefa.Id.ToString()}]") 
                    { 
                        StringFormat = "N1",
                        TargetNullValue = "-"
                    },
                    Width = 120
                });
            }

            // Adicionar colunas para média e aprovação
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Média Final",
                DisplayMemberBinding = new Binding("Media") { StringFormat = "N1", TargetNullValue = "-" },
                Width = 100
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Aprovado",
                DisplayMemberBinding = new Binding("Aprovado"),
                Width = 80
            });

            try
            {
                // Criar linhas da pauta com dados reais e ordenar por nome
                var linhasPauta = app.VMTurmaAtual.TurmaAtual_.Alunos
                    .Select(aluno => new LinhaPauta(aluno, app.VMTurmaAtual.TurmaAtual_.Tarefas))
                    .OrderBy(l => l.Nome)
                    .ToList();

                // Guardar a lista completa para filtrar mais tarde
                todasLinhasPauta = linhasPauta;
                
                PautaListView.ItemsSource = linhasPauta;
                
                // Calcular a taxa de aprovação e média total
                CalcularEstatisticas(linhasPauta);
                
                // Limpar o filtro caso exista
                txtPesquisa.Text = string.Empty;
                cbFiltro.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar a pauta: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApresentarHistograma_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar se há dados para mostrar
                if (PautaListView.ItemsSource == null)
                {
                    MessageBox.Show("Nenhum dado disponível para mostrar no histograma.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Esconder a grade da pauta
                PautaGrid.Visibility = Visibility.Collapsed;
                
                // Mostrar o ContentControl do histograma
                HistogramaContentControl.Visibility = Visibility.Visible;

                // Obter as linhas da pauta e as tarefas
                var linhasPauta = PautaListView.ItemsSource as List<LinhaPauta>;
                if (linhasPauta == null)
                {
                    linhasPauta = new List<LinhaPauta>();
                    foreach (var item in PautaListView.ItemsSource)
                    {
                        if (item is LinhaPauta linha)
                        {
                            linhasPauta.Add(linha);
                        }
                    }
                }
                
                var tarefas = app.VMTurmaAtual?.TurmaAtual_?.Tarefas?.ToList() ?? new List<Tarefa>();

                // Criar e mostrar o histograma
                var histograma = new HistogramaView(this, linhasPauta, tarefas);
                HistogramaContentControl.Content = histograma;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao apresentar o histograma: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void MostrarPauta()
        {
            // Remove o histograma e mostra a pauta novamente
            HistogramaContentControl.Content = null;
            HistogramaContentControl.Visibility = Visibility.Collapsed;
            PautaGrid.Visibility = Visibility.Visible;
            
            // Atualiza os dados da pauta
            ApresentarResultados_Click(null, null);
        }

        private void txtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (todasLinhasPauta == null || todasLinhasPauta.Count == 0)
                return;

            var tipoFiltro = (cbFiltro.SelectedItem as ComboBoxItem)?.Content.ToString();
            var textoPesquisa = txtPesquisa.Text;

            if (!string.IsNullOrWhiteSpace(textoPesquisa) && tipoFiltro != null)
            {
                linhasFiltradas = new List<LinhaPauta>();

                // Filtrar por número ou nome
                if (tipoFiltro == "Número")
                {
                    linhasFiltradas = todasLinhasPauta
                        .Where(l => l.NumeroAluno.ToString().Contains(textoPesquisa, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                else if (tipoFiltro == "Nome")
                {
                    linhasFiltradas = todasLinhasPauta
                        .Where(l => l.Nome.Contains(textoPesquisa, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                PautaListView.ItemsSource = linhasFiltradas;
                
                // Atualizar estatísticas com base nos resultados filtrados
                CalcularEstatisticas(linhasFiltradas);
            }
            else
            {
                // Se não houver filtro, mostrar todos
                PautaListView.ItemsSource = todasLinhasPauta;
            }
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            // Limpar o filtro
            linhasFiltradas = null;
            txtPesquisa.Text = string.Empty;
            cbFiltro.SelectedIndex = 0;
            
            // Restaurar a lista completa
            if (todasLinhasPauta != null)
            {
                PautaListView.ItemsSource = todasLinhasPauta;
                
                // Recalcular estatísticas ao limpar filtro
                CalcularEstatisticas(todasLinhasPauta);
            }
        }
        
        private void CalcularEstatisticas(List<LinhaPauta> linhasPauta)
        {
            if (linhasPauta == null || linhasPauta.Count == 0)
            {
                txtTaxaAprovacao.Text = "0%";
                txtMediaTotal.Text = "-";
                return;
            }
            
            // Calcular a taxa de aprovação (considerando todos os alunos)
            int totalAlunos = linhasPauta.Count;
            int alunosAprovados = linhasPauta.Count(l => l.Aprovado == "Sim");
            double taxaAprovacao = (double)alunosAprovados / totalAlunos * 100;
            txtTaxaAprovacao.Text = $"{taxaAprovacao:N1}%";
            
            // Calcular a média total (apenas para alunos que têm nota - estão em grupos)
            var alunosComNota = linhasPauta.Where(l => l.Media.HasValue).ToList();
            if (alunosComNota.Count > 0)
            {
                double mediaTotal = alunosComNota.Average(l => l.Media.Value);
                txtMediaTotal.Text = $"{mediaTotal:N1}";
            }
            else
            {
                txtMediaTotal.Text = "-";
            }
        }
    }
}
