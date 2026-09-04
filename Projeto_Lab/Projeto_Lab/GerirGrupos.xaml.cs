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
    /// Lógica interna para GerirGrupos.xaml
    /// </summary>
    public partial class GerirGrupos : UserControl
    {
        private App app;
        private ObservableCollection<Grupo> gruposFiltrados;
        public GerirGrupos()
        {
            InitializeComponent();
            app = App.Current as App;

            app.VMTurmaAtual.TurmasAtualizadas += AtualizarListaAlunos;
            app.VMTurmaAtual.AlunoEditado += VMTurmaAtual_AlunoEditado;
        }

        private void AtualizarListaAlunos(object? sender, EventArgs e)
        {
            lv_grupos.ItemsSource = null;
            lv_grupos.ItemsSource = app.VMTurmaAtual.TurmaAtual_?.grupos_;
            GruposContent.Content = null;
        }

        private void VMTurmaAtual_AlunoEditado(object? sender, EventArgs e)
        {
            GruposContent.Content = null;
            lv_grupos.ItemsSource = null;
            lv_grupos.ItemsSource = app.VMTurmaAtual.TurmaAtual_?.grupos_;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lv_grupos.ItemsSource = app.VMTurmaAtual.TurmaAtual_.grupos_;
        }

        private void btn_adicionar_Click(object sender, RoutedEventArgs e)
        {
            GruposContent.Content = new AddGrupo();
        }

        private void btn_remover_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Grupo gruposelecionado = lv_grupos.SelectedItem as Grupo;

                if (gruposelecionado == null)
                    throw new OperacaoInvalida("Por favor, selecione um grupo para prosseguir!");

                app.VMTurmaAtual.AtualizarIdGrupo(gruposelecionado.Alunos, 0);
                app.VMTurmaAtual.TurmaAtual_.grupos_.Remove(gruposelecionado);
                app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show("Por favor, selecione um grupo para prosseguir!");
            }
        }

        private void btn_editar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Grupo alunoselecionado = lv_grupos.SelectedItem as Grupo;

                if (alunoselecionado == null)
                    throw new OperacaoInvalida("Por favor, selecione um grupo para prosseguir!");

                GruposContent.Content = new EditGrupo(alunoselecionado);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void txtPesquisaGrupo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPesquisaGrupo.Text))
            {
                gruposFiltrados = app.VMTurmaAtual.FiltrarGrupos(txtPesquisaGrupo.Text);
                lv_grupos.ItemsSource = gruposFiltrados;
            }
            else
            {
                lv_grupos.ItemsSource = app.VMTurmaAtual.TurmaAtual_?.grupos_;
            }
        }

        private void btnLimparPesquisaGrupo_Click(object sender, RoutedEventArgs e)
        {
            gruposFiltrados = null;
            txtPesquisaGrupo.Text = string.Empty;

            lv_grupos.ItemsSource = app.VMTurmaAtual.TurmaAtual_?.grupos_;
        }
    }
}

