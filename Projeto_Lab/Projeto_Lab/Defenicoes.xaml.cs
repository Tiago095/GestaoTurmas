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
    /// Lógica interna para Defenicoes.xaml
    /// </summary>
    public partial class Defenicoes : UserControl
    {
        private App app;
        public Defenicoes()
        {
            InitializeComponent();
            app = App.Current as App;

            app.VMTurmaAtual.TurmasAtualizadas += AtualizarComboBoxTurmas;
            app.VMTurmaAtual.CarregarNomesDasTurmasDoFicheiro();
        }

        private void AtualizarComboBoxTurmas(object? sender, EventArgs e)
        {
            cb_turmas.ItemsSource = null;
            cb_turmas.ItemsSource = app.VMTurmaAtual.NomeTurmas;
        }

        private void btn_apagar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nomeTurmaSelecionada = cb_turmas.SelectedItem as string;

                if (string.IsNullOrEmpty(nomeTurmaSelecionada))
                    throw new OperacaoInvalida("Selecione um grupo");

                if (nomeTurmaSelecionada == app.VMTurmaAtual.TurmaAtual_?.Id_turma)
                    app.VMTurmaAtual.TurmaAtual_ = null;
                app.VMTurmaAtual.RemoverNomeTurmaDoFicheiro(nomeTurmaSelecionada);

            }
            catch(OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
