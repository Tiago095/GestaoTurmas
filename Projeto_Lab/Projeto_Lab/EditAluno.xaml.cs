using Lab;
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
    /// Lógica interna para EditAluno.xaml
    /// </summary>
    public partial class EditAluno : UserControl
    {
        private App app;
        private Aluno aluno_;
        public EditAluno(Aluno aluno)
        {
            InitializeComponent();
            app = App.Current as App;
            aluno_ = aluno;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbNumero.Text = Convert.ToString(aluno_.Numero);
            tbNome.Text = aluno_.Nome;
            tbEmail.Text = aluno_.Email;
        }

        private void btn_Adicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbNome.Text))
                    throw new OperacaoInvalida("Preencha todos os campos obrigatórios!");
                if (string.IsNullOrWhiteSpace(tbEmail.Text))
                    throw new OperacaoInvalida("Preencha todos os campos obrigatórios!");

                aluno_.Nome = tbNome.Text;
                if(aluno_.Id_Grupo != 0)
                    app.VMTurmaAtual.AtualizarAlunoemGrupo(aluno_.Id_Grupo,aluno_);
                

                app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}