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
    /// Lógica interna para Inicial.xaml
    /// </summary>
    public partial class Inicial : UserControl
    {
        private App app;
        public Inicial()
        {
            InitializeComponent();
            app = App.Current as App;

            app.VMTurmaAtual.TurmasAtualizadas += AtualizarListaAlunos;
        }

        private void AtualizarListaAlunos(object? sender, EventArgs e)
        {
            if (app.VMTurmaAtual.TurmaAtual_?.Alunos == null || app.VMTurmaAtual.TurmaAtual_?.grupos_ == null || app.VMTurmaAtual.TurmaAtual_?.Tarefas == null)
            {
                txtTarefas.Text = "0";
                txtGrupos.Text = "0";
                txtAlunos.Text = "0";
            }
            else
            {
                txtAlunos.Text = Convert.ToString(app.VMTurmaAtual.TurmaAtual_?.Alunos?.Count());
                txtGrupos.Text = Convert.ToString(app.VMTurmaAtual.TurmaAtual_?.grupos_?.Count());
                txtTarefas.Text = Convert.ToString(app.VMTurmaAtual.TurmaAtual_?.Tarefas?.Count());

                ObservableCollection<Tarefa> tarefas = new ObservableCollection<Tarefa>(app.VMTurmaAtual.TurmaAtual_.Tarefas.Where(x => x.DataHoraTermino > DateTime.Today));
                lv_Atividades.ItemsSource = tarefas;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (app.VMTurmaAtual.TurmaAtual_?.Alunos == null || app.VMTurmaAtual.TurmaAtual_?.grupos_ == null || app.VMTurmaAtual.TurmaAtual_?.Tarefas == null)
            {
                txtTarefas.Text = "0";
                txtGrupos.Text = "0";
                txtAlunos.Text = "0";
            }
            else
            {
                txtAlunos.Text = Convert.ToString(app.VMTurmaAtual.TurmaAtual_?.Alunos?.Count());
                txtGrupos.Text = Convert.ToString(app.VMTurmaAtual.TurmaAtual_?.grupos_?.Count());
                txtTarefas.Text = Convert.ToString(app.VMTurmaAtual.TurmaAtual_?.Tarefas?.Count());

                ObservableCollection<Tarefa> tarefas = new ObservableCollection<Tarefa>(app.VMTurmaAtual.TurmaAtual_.Tarefas.Where(x => x.DataHoraTermino > DateTime.Today));
                lv_Atividades.ItemsSource = tarefas;
            }


        }
    }
}
