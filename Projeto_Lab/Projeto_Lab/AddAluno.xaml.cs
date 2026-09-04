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
    /// Lógica interna para AddAluno.xaml
    /// </summary>
    public partial class AddAluno : UserControl
    {
        private App app;

        public AddAluno()
        {
            InitializeComponent();
            app = App.Current as App;
        }

        private void btn_Adicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (app.VMTurmaAtual.VerificarNum(Convert.ToInt32(tbNumero.Text)))
                    throw new OperacaoInvalida("Esse número já existe!");

                Aluno aluno = new Aluno
                {
                    Nome = tbNome.Text,
                    Email = "al" + tbNumero.Text + "@alunos.utad.pt",
                    Numero = Convert.ToInt32(tbNumero.Text)
                };

                app.VMTurmaAtual.TurmaAtual_.Alunos.Add(aluno);
                app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
