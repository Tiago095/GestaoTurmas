using Lab;
using Projeto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica interna para NotaGlobal.xaml
    /// </summary>
    public partial class NotaGlobal : UserControl
    {
        private App app;
        private Grupo grupoSelecionado;
        private int tarefaSelecionada;
        public NotaGlobal(Grupo grupo, int tarefa)
        {
            InitializeComponent();
            app = App.Current as App;
            grupoSelecionado = grupo;
            tarefaSelecionada = tarefa;
        }

        private void tb_notaGlobal_TextChanged(object sender, TextChangedEventArgs e)
        {
            tb_placeholder.Visibility = string.IsNullOrEmpty(tb_notaGlobal.Text) ? Visibility.Visible : Visibility.Hidden;
        }

        private void AplicarNota_Click(object sender, RoutedEventArgs e)
        {
            if (float.TryParse(tb_notaGlobal.Text, out float nota))
            {
                if(nota>= 0 && nota <= 20)
                {
                    foreach (var aluno in grupoSelecionado.Alunos)
                    {
                        aluno.Notas_Aluno[tarefaSelecionada] = nota;
                        app.VMTurmaAtual.AtualizarNota(aluno, nota, tarefaSelecionada);
                    }
                    app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);

                    MessageBox.Show($"Nota global {nota} atribuída a todos os alunos do grupo.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("A nota deve estar entre 0 e 20.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Insira uma nota válida (número).", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
