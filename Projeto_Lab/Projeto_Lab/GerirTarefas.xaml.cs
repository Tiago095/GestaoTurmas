using Lab;
using Projeto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica interna para GerirTarefas.xaml
    /// </summary>
    public partial class GerirTarefas : UserControl
    {
        private App app;
        private ObservableCollection<Tarefa> tarefasFiltrados;
        public GerirTarefas()
        {
            InitializeComponent();
            app = App.Current as App;

            app.VMTurmaAtual.TurmasAtualizadas += AtualizarListaAlunos;
            app.VMTurmaAtual.AlunoEditado += VMTurmaAtual_AlunoEditado;
        }

        private void AtualizarListaAlunos(object? sender, EventArgs e)
        {
            lv_tarefas.ItemsSource = null;
            lv_tarefas.ItemsSource = app.VMTurmaAtual.TurmaAtual_?.Tarefas;
            TarefasContent.Content = null;
            txtPesquisaTarefa.Text = string.Empty;
        }

        private void VMTurmaAtual_AlunoEditado(object? sender, EventArgs e)
        {
            TarefasContent.Content = null;
            lv_tarefas.ItemsSource = null;
            lv_tarefas.ItemsSource = app.VMTurmaAtual.TurmaAtual_?.Tarefas;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lv_tarefas.ItemsSource = app.VMTurmaAtual.TurmaAtual_.Tarefas;
        }

        private void btn_adicionar_Click(object sender, RoutedEventArgs e)
        {
            TarefasContent.Content = new AddTarefa();
        }

        private void btn_editar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tarefa tarefaselecionada = lv_tarefas.SelectedItem as Tarefa;

                if (tarefaselecionada == null)
                    throw new OperacaoInvalida("Selecione uma Tarefa para editar!");

                TarefasContent.Content = new EditTarefa(tarefaselecionada);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btn_remover_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tarefa tarefaselecionada = lv_tarefas.SelectedItem as Tarefa;

                if (tarefaselecionada == null)
                    throw new OperacaoInvalida("Selecione uma Tarefa para remover!");

                app.VMTurmaAtual.TurmaAtual_.Tarefas.Remove(tarefaselecionada);
                app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void txtPesquisaTarefa_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPesquisaTarefa.Text))
            {
                tarefasFiltrados = app.VMTurmaAtual.FiltrarTarefas(txtPesquisaTarefa.Text);
                lv_tarefas.ItemsSource = tarefasFiltrados;
            }
            else
            {
                lv_tarefas.ItemsSource = app.VMTurmaAtual.TurmaAtual_?.Tarefas;
            }
        }

        private void btnLimparPesquisa_Click(object sender, RoutedEventArgs e)
        {
            tarefasFiltrados = null;
            txtPesquisaTarefa.Text = string.Empty;

            lv_tarefas.ItemsSource = app.VMTurmaAtual.TurmaAtual_?.Tarefas;
        }
    }
}
