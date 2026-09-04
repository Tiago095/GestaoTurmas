using Lab;
using Projeto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projeto_Lab
{
    /// <summary>
    /// Lógica interna para AddTarefa.xaml
    /// </summary>
    public partial class AddTarefa : UserControl
    {
        private App app;
        public AddTarefa()
        {
            InitializeComponent();
            app = App.Current as App;
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tb_id.Text))
                    throw new OperacaoInvalida("Preencha todos os campos obrigatórios!");
                if (!int.TryParse(tb_id.Text, out int numero))
                    throw new OperacaoInvalida("O ID deve ser um número");
                if (app.VMTurmaAtual.VerificarIDTarefa(Convert.ToInt32(tb_id.Text)))
                    throw new OperacaoInvalida("O ID colocado já existe");
                if (string.IsNullOrWhiteSpace(tb_titulo.Text))
                    throw new OperacaoInvalida("Preencha todos os campos obrigatórios!");
                if (!dpInicio.SelectedDate.HasValue)
                    throw new OperacaoInvalida("Selecione a data de início");
                if (!dpFim.SelectedDate.HasValue)
                    throw new OperacaoInvalida("Selecione a data de termino");
                if (dpFim.SelectedDate.Value < dpInicio.SelectedDate.Value)
                    throw new OperacaoInvalida("A data de termino não pode ser anterior a de ínicio");
                if (app.VMTurmaAtual.VerificarPeso(sldPeso.Value) > 100.0)
                    throw new OperacaoInvalida("O peso de todas as tarefas em conjunto não devem ultrapassar 100%, neste momento as tarefas equivalem a " + (app.VMTurmaAtual.VerificarPeso(sldPeso.Value) - sldPeso.Value) + "% da nota.");
                if (string.IsNullOrWhiteSpace(tb_descricao.Text))
                    tb_descricao.Text = "Sem descrição";

                ; Tarefa tarefa = new Tarefa
                {
                    Id = Convert.ToInt32(tb_id.Text),
                    Titulo = tb_titulo.Text,
                    Descricao = tb_descricao.Text,
                    Peso = Convert.ToDouble(sldPeso.Value),
                    DataHoraInicio = dpInicio.SelectedDate.Value,
                    DataHoraTermino = dpFim.SelectedDate.Value
                };

                app.VMTurmaAtual.TurmaAtual_.Tarefas.Add(tarefa);
                app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}