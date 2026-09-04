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
    /// Lógica interna para EditGrupo.xaml
    /// </summary>
    public partial class EditGrupo : UserControl
    {
        private App app;
        private Grupo grupo_;
        private ObservableCollection<Aluno> alunosFiltrados;
        private ObservableCollection<Aluno> alunosFiltradosDisponiveis;
        public EditGrupo(Grupo grupo)
        {
            InitializeComponent();
            app = App.Current as App;
            grupo_ = grupo;

            app.VMTurmaAtual.GrupoEditado += VMTurmaAtual_GrupoEditado;
        }

        private void VMTurmaAtual_GrupoEditado(object? sender, EventArgs e)
        {
            ObservableCollection<Aluno> alunosSemgrupo = new ObservableCollection<Aluno>(app.VMTurmaAtual.TurmaAtual_.Alunos.Where(x => x.Id_Grupo == 0));

            lv_alunosDisponiveis.ItemsSource = null;
            lv_alunosDisponiveis.ItemsSource = alunosSemgrupo;
            lv_alunosGrupo.ItemsSource = null;
            lv_alunosGrupo.ItemsSource = grupo_.Alunos;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Aluno> alunosSemgrupo = new ObservableCollection<Aluno>(app.VMTurmaAtual.TurmaAtual_.Alunos.Where(x => x.Id_Grupo == 0));

            tb_id.Text = Convert.ToString(grupo_.Id);
            tb_nome.Text = grupo_.Nome;
            tb_num.Text = Convert.ToString(grupo_.NumeroAlunos);
            lv_alunosDisponiveis.ItemsSource = alunosSemgrupo;
            lv_alunosGrupo.ItemsSource = grupo_.Alunos;
        }

        private void btn_guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tb_nome.Text))
                    throw new OperacaoInvalida("Preencha todos os campos obrigatórios!");

                grupo_.Nome = tb_nome.Text;

                app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Aluno alunoselecionado = lv_alunosDisponiveis.SelectedItem as Aluno;
                ObservableCollection<Aluno> alunos = new ObservableCollection<Aluno>();
                alunos.Add(alunoselecionado);

                if (alunoselecionado == null)
                    throw new OperacaoInvalida("Por favor, selecione um aluno para prosseguir!");

                grupo_.Alunos.Add(alunoselecionado);

                tb_num.Text = Convert.ToString(grupo_.Alunos.Count());

                grupo_.NumeroAlunos = grupo_.Alunos.Count();

                app.VMTurmaAtual.AtualizarIdGrupo(alunos, grupo_.Id);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void BtnRemover_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Aluno alunoselecionado = lv_alunosGrupo.SelectedItem as Aluno;
                ObservableCollection<Aluno> alunos = new ObservableCollection<Aluno>();
                alunos.Add(alunoselecionado);

                if (alunoselecionado == null)
                    throw new OperacaoInvalida("Por favor, selecione um aluno para prosseguir!");

                grupo_.Alunos.Remove(alunoselecionado);

                tb_num.Text = Convert.ToString(grupo_.Alunos.Count());

                grupo_.NumeroAlunos = grupo_.Alunos.Count();

                app.VMTurmaAtual.AtualizarIdGrupo(alunos, 0);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void txtPesquisaGrupo_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tipo = (cbFiltroGrupo.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (!string.IsNullOrWhiteSpace(txtPesquisaGrupo.Text) && tipo != null)
            {
                alunosFiltrados = app.VMTurmaAtual.FiltrarAlunos(tipo, txtPesquisaGrupo.Text,grupo_.Alunos);
                lv_alunosGrupo.ItemsSource = alunosFiltrados;
            }
            else
            {
                lv_alunosGrupo.ItemsSource = grupo_.Alunos;
            }
        }

        private void btnLimparGrupo_Click(object sender, RoutedEventArgs e)
        {
            alunosFiltrados = null;
            txtPesquisaGrupo.Text = string.Empty;
            cbFiltroGrupo.SelectedIndex = 0;
            lv_alunosGrupo.ItemsSource = grupo_.Alunos;
        }

        private void txtPesquisaDisponiveis_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<Aluno> alunosSemgrupo = new ObservableCollection<Aluno>(app.VMTurmaAtual.TurmaAtual_.Alunos.Where(x => x.Id_Grupo == 0));

            var tipo = (cbFiltroDisponiveis.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (!string.IsNullOrWhiteSpace(txtPesquisaDisponiveis.Text) && tipo != null)
            {
                alunosFiltradosDisponiveis = app.VMTurmaAtual.FiltrarAlunos(tipo, txtPesquisaDisponiveis.Text,alunosSemgrupo);
                lv_alunosDisponiveis.ItemsSource = alunosFiltradosDisponiveis;
            }
            else
            {
                lv_alunosDisponiveis.ItemsSource = alunosSemgrupo;
            }
        }

        private void btnLimparDisponiveis_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Aluno> alunosSemgrupo = new ObservableCollection<Aluno>(app.VMTurmaAtual.TurmaAtual_.Alunos.Where(x => x.Id_Grupo == 0));

            alunosFiltradosDisponiveis = null;
            txtPesquisaDisponiveis.Text = string.Empty;
            cbFiltroDisponiveis.SelectedIndex = 0;
            lv_alunosDisponiveis.ItemsSource = alunosSemgrupo;
        }
    }
}
