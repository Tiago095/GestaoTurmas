using Lab;
using Microsoft.Win32;
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
    /// Lógica interna para CriarTurmas.xaml
    /// </summary>
    public partial class CriarTurma : UserControl
    {
        private App app;
        public CriarTurma()
        {
            InitializeComponent();
            app = App.Current as App;
        }

        private void btn_a_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                tb_a.Text = dlg.FileName;
            }
        }

        private void btn_t_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                tb_t.Text = dlg.FileName;
            }
        }

        private void btn_gr_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                tb_g.Text = dlg.FileName;
            }
        }

        private void btn_salvar_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(id.Text) || string.IsNullOrWhiteSpace(tb_a.Text))
            {
                MessageBox.Show("O ID da turma e o ficheiro de alunos são obrigatórios.");
                return;
            }

            try
            {
                app.VMTurmaAtual.CarregarDeFicheiros(id.Text, tb_a.Text, tb_g.Text, tb_t.Text);
                var turmaRecemCriada = app.VMTurmaAtual.TurmaAtual_;

                if (app.VMTurmaAtual.VerificarIdTurma(id.Text))
                    throw new OperacaoInvalida("Este Id já existe");

                if (turmaRecemCriada != null)
                {
                    app.VMTurmaAtual.GravarTurmaEmJson(turmaRecemCriada);
                    app.VMTurmaAtual.AdicionarNomeTurmaAoFicheiro(id.Text);
                    MessageBox.Show($"Turma " + turmaRecemCriada.Id_turma + " guardada com sucesso!");
                }
                else
                {
                    MessageBox.Show("Erro ao obter a turma carregada.");
                }

                tb_a.Clear();
                tb_g.Clear();
                tb_t.Clear();
                id.Clear();
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro inesperado: " + ex.Message);
            }
        }
    }
}
