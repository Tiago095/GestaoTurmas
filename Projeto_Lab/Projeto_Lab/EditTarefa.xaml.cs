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
    /// Lógica interna para EditTarefa.xaml
    /// </summary>
    public partial class EditTarefa : UserControl
    {
        private App app;
        private Tarefa tarefa_;
        public EditTarefa(Tarefa tarefa)
        {
            InitializeComponent();
            app = App.Current as App;
            tarefa_ = tarefa;


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tb_id.Text = Convert.ToString(tarefa_.Id);
            tb_titulo.Text = tarefa_.Titulo;
            tb_descricao.Text = tarefa_.Descricao;
            sldPeso.Value = tarefa_.Peso;
            dpInicio.SelectedDate = tarefa_.DataHoraInicio;
            dpFim.SelectedDate = tarefa_.DataHoraTermino;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tb_titulo.Text))
                    throw new OperacaoInvalida("Preencha todos os campos obrigatórios!");
                if (!dpFim.SelectedDate.HasValue)
                    throw new OperacaoInvalida("Selecione a data de termino");
                if (dpFim.SelectedDate.Value < dpInicio.SelectedDate.Value)
                    throw new OperacaoInvalida("A data de termino não pode ser anterior a de ínicio");
                if ((app.VMTurmaAtual.VerificarPeso(sldPeso.Value) - tarefa_.Peso) > 100.0)
                    throw new OperacaoInvalida("O peso de todas as tarefas em conjunto não devem ultrapassar 100%, neste momento as tarefas equivalem a " + (app.VMTurmaAtual.VerificarPeso(sldPeso.Value) - sldPeso.Value) + "% da nota.");
                if (string.IsNullOrWhiteSpace(tb_descricao.Text))
                    tb_descricao.Text = "Sem descrição";

                tarefa_.Titulo = tb_titulo.Text;
                tarefa_.Descricao = tb_descricao.Text;
                tarefa_.Peso = sldPeso.Value;
                tarefa_.DataHoraTermino = dpFim.SelectedDate.Value;

                app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

