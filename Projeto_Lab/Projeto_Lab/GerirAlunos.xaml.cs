using Lab;
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
    /// Lógica interna para GerirAlunos.xaml
    /// </summary>
    public partial class GerirAlunos : UserControl
    {
        private App app;
        private ObservableCollection<Aluno> alunosFiltrados;
        public GerirAlunos()
        {
            InitializeComponent();
            app = App.Current as App;

            app.VMTurmaAtual.TurmasAtualizadas += AtualizarListaAlunos;
            app.VMTurmaAtual.AlunoEditado += VMTurmaAtual_AlunoEditado;
        }

        private void AtualizarListaAlunos(object? sender, EventArgs e)
        {
            lv_alunos.ItemsSource = null;
            lv_alunos.ItemsSource = app.VMTurmaAtual.TurmaAtual_?.Alunos;
            AlunosContent.Content = null;
            txtPesquisa.Text = string.Empty;
        }

        private void VMTurmaAtual_AlunoEditado(object? sender, EventArgs e)
        {
            AlunosContent.Content = null;
            lv_alunos.ItemsSource = null;
            lv_alunos.ItemsSource = app.VMTurmaAtual.TurmaAtual_?.Alunos;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lv_alunos.ItemsSource = app.VMTurmaAtual.TurmaAtual_.Alunos;
        }

        private void btn_Adicionar_Click(object sender, RoutedEventArgs e)
        {
            AlunosContent.Content = new AddAluno();
        }

        private void btn_editar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Aluno alunoselecionado = lv_alunos.SelectedItem as Aluno;

                if (alunoselecionado == null)
                    throw new OperacaoInvalida("Por favor, selecione um aluno para prosseguir!");

                AlunosContent.Content = new EditAluno(alunoselecionado);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Aluno alunoselecionado = lv_alunos.SelectedItem as Aluno;

                if (alunoselecionado == null)
                    throw new OperacaoInvalida("Por favor, selecione um aluno para prosseguir!");

                app.VMTurmaAtual.TurmaAtual_.Alunos.Remove(alunoselecionado);
                app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void txtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tipo = (cbFiltro.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (!string.IsNullOrWhiteSpace(txtPesquisa.Text) && tipo != null)
            {
                alunosFiltrados = app.VMTurmaAtual.FiltrarAlunos(tipo, txtPesquisa.Text,app.VMTurmaAtual.TurmaAtual_.Alunos);
                lv_alunos.ItemsSource = alunosFiltrados;
            }
            else
            {
                lv_alunos.ItemsSource = app.VMTurmaAtual.TurmaAtual_?.Alunos;
            }
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            alunosFiltrados = null;
            txtPesquisa.Text = string.Empty;
            cbFiltro.SelectedIndex = 0;
            lv_alunos.ItemsSource = app.VMTurmaAtual.TurmaAtual_?.Alunos;
        }
    }
}
