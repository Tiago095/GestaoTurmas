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
    /// Lógica interna para NotasIndividuas.xaml
    /// </summary>
    public partial class NotasIndividuais : UserControl
    {
        private App app;
        private Grupo grupoSelecionado;
        private int tarefaSelecionada;
        public NotasIndividuais(Grupo grupo, int tarefa)
        {
            InitializeComponent();
            app = App.Current as App;
            grupoSelecionado = grupo;
            tarefaSelecionada = tarefa;
            lv_alunos.ItemsSource = grupoSelecionado.Alunos;
        }

        private void tb_nota_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Lógica para mostrar ou esconder o "placeholder"
            tb_placeholder.Visibility = string.IsNullOrWhiteSpace(tb_nota.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void AtribuirNota_Click(object sender, RoutedEventArgs e)
        {
            if(lv_alunos.SelectedItem is Aluno alunoSelecionado)
            {
                if (float.TryParse(tb_nota.Text, out float nota))
                {
                    if(nota>=0 && nota <= 20)
                    {
                        // Atribuir ou atualizar a nota
                        alunoSelecionado.Notas_Aluno[tarefaSelecionada] = nota;
                        app.VMTurmaAtual.AtualizarNota(alunoSelecionado, nota, tarefaSelecionada);

                        app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);

                        MessageBox.Show($"Nota {nota} atribuída a {alunoSelecionado.Nome} para '{tarefaSelecionada}'.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb_nota.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show("A nota deve estar entre 0 e 20.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    
                }
                else
                {
                    MessageBox.Show("Introduza um valor numérico válido para a nota.");
                }
            }
            else 
            {
                MessageBox.Show("Selecione um aluno.");
            }

        }
    }
}
