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
    /// Lógica interna para AddGrupo.xaml
    /// </summary>
    public partial class AddGrupo : UserControl
    {
        private App app;
        private ObservableCollection<Aluno> alunosFiltrados;
        public AddGrupo()
        {
            InitializeComponent();
            app = App.Current as App;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Aluno> alunosSemgrupo = new ObservableCollection<Aluno>(app.VMTurmaAtual.TurmaAtual_.Alunos.Where(x => x.Id_Grupo == 0));
            lv_alunos.ItemsSource = alunosSemgrupo;
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var alunosselcionados = new ObservableCollection<Aluno>(lv_alunos.SelectedItems.OfType<Aluno>());
                if (string.IsNullOrWhiteSpace(tb_id.Text))
                    throw new OperacaoInvalida("Preencha todos os campos obrigatórios!");
                if (string.IsNullOrWhiteSpace(tb_nome.Text))
                    throw new OperacaoInvalida("Preencha todos os campos obrigatórios!");
                if (!int.TryParse(tb_id.Text, out int numero))
                    throw new OperacaoInvalida("O ID deve ser um número");
                if (app.VMTurmaAtual.VerificarID(Convert.ToInt32(tb_id.Text)))
                    throw new OperacaoInvalida("Esse Id já existe");
                if (lv_alunos.SelectedItems.Count > Convert.ToInt32(tb_num.Text))
                    throw new OperacaoInvalida("O limite de alunos a selecionar deve ser " + tb_num.Text);
                if (lv_alunos.SelectedItems.Count != Convert.ToInt32(tb_num.Text))
                    throw new OperacaoInvalida("O número de alunos selecionado deve ser " + tb_num.Text);

                Grupo grupo = new Grupo
                {
                    Id = Convert.ToInt32(tb_id.Text),
                    Nome = tb_nome.Text,
                    NumeroAlunos = Convert.ToInt32(tb_num.Text),
                    Alunos = alunosselcionados
                };

                app.VMTurmaAtual.TurmaAtual_.grupos_.Add(grupo);
                app.VMTurmaAtual.AtualizarIdGrupo(alunosselcionados, Convert.ToInt32(tb_id.Text));
                app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void txtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<Aluno> alunosSemgrupo = new ObservableCollection<Aluno>(app.VMTurmaAtual.TurmaAtual_.Alunos.Where(x => x.Id_Grupo == 0));

            var tipo = (cbFiltro.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (!string.IsNullOrWhiteSpace(txtPesquisa.Text) && tipo != null)
            {
                alunosFiltrados = app.VMTurmaAtual.FiltrarAlunos(tipo, txtPesquisa.Text, alunosSemgrupo);
                lv_alunos.ItemsSource = alunosFiltrados;
            }
            else
            {
                lv_alunos.ItemsSource = alunosSemgrupo;
            }
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Aluno> alunosSemgrupo = new ObservableCollection<Aluno>(app.VMTurmaAtual.TurmaAtual_.Alunos.Where(x => x.Id_Grupo == 0));

            alunosFiltrados = null;
            txtPesquisa.Text = string.Empty;
            cbFiltro.SelectedIndex = 0;
            lv_alunos.ItemsSource = alunosSemgrupo;
        }
    }
}
