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
    /// Lógica interna para AdicionarNotas.xaml
    /// </summary>
    public partial class AdicionarNotas : UserControl
    {
        private App app;
        public AdicionarNotas()
        {
            InitializeComponent();
            app = App.Current as App;

            app.VMTurmaAtual.TurmasAtualizadas += AtualizarListaAlunos;

        }

        private void AtualizarListaAlunos(object? sender, EventArgs e)
        {
            cb_grupos.SelectedItem = null;
            cb_tarefas.SelectedItem = null;
            content_notas.Content = null;
        }

        private void btn_individual_Click(object sender, RoutedEventArgs e)
        {
            if (cb_grupos.SelectedItem is Grupo grupoSelecionado && cb_tarefas.SelectedItem is Tarefa tarefaSelecionada)
            {
                content_notas.Content = new NotasIndividuais(grupoSelecionado, tarefaSelecionada.Id);
            }
            else
            {
                MessageBox.Show("Selecione um grupo e uma tarefa antes de continuar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cb_grupos.ItemsSource = app.VMTurmaAtual.TurmaAtual_.grupos_;
            cb_tarefas.ItemsSource = app.VMTurmaAtual.TurmaAtual_.Tarefas;
        }

        private void btn_global_Click(object sender, RoutedEventArgs e)
        {
            if (cb_grupos.SelectedItem is Grupo grupoSelecionado && cb_tarefas.SelectedItem is Tarefa tarefaSelecionada)
            {
                content_notas.Content = new NotaGlobal(grupoSelecionado, tarefaSelecionada.Id);
                app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);
            }
            else
            {
                MessageBox.Show("Selecione um grupo e uma tarefa antes de continuar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
