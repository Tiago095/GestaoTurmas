using Lab;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Projeto_Lab;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private App app;
    public MainWindow()
    {
        InitializeComponent();
        app = App.Current as App;

        app.VMTurmaAtual.TurmasAtualizadas += AtualizarComboBoxTurmas;

        app.VMTurmaAtual.CarregarNomesDasTurmasDoFicheiro();

        app.Muser.PerfilAtualizado += Muser_PerfilAtualizado;

        MainContent.Content = new Inicial();

    }

    private void Muser_PerfilAtualizado(object? sender, EventArgs e)
    {
        tb_nome_main.Text = app.Muser.Nome;

        if (!string.IsNullOrWhiteSpace(app.Muser.Fotografia) && File.Exists(app.Muser.Fotografia))
        {
            imagem_main.Source = new BitmapImage(new Uri(app.Muser.Fotografia));
            imagem_main.Visibility = Visibility.Visible;
            ProfileInitials.Visibility = Visibility.Collapsed;
        }
        else
        {
            imagem_main.Source = null;
            imagem_main.Visibility = Visibility.Collapsed;

            if (!string.IsNullOrWhiteSpace(app.Muser.Nome))
                ProfileInitials.Text = app.Muser.Nome.Substring(0, 1).ToUpper();
            else
                ProfileInitials.Text = "?";

            ProfileInitials.Visibility = Visibility.Visible;
        }
    }

    private void AtualizarComboBoxTurmas(object? sender, EventArgs e)
    {
        cb_turmas.ItemsSource = null;
        cb_turmas.ItemsSource = app.VMTurmaAtual.NomeTurmas;
    }

    private void btn_refresh_Click(object sender, RoutedEventArgs e)
    {
        string nomeTurmaSelecionada = cb_turmas.SelectedItem as string;

        if (!string.IsNullOrEmpty(nomeTurmaSelecionada))
        {
            var turma = app.VMTurmaAtual.CarregarTurmaDeJson(nomeTurmaSelecionada);
            if (turma != null)
            {
                app.VMTurmaAtual.TurmaAtual_ = turma;
                app.VMTurmaAtual.CarregarNomesDasTurmasDoFicheiro();
            }
            else
            {
                MessageBox.Show("Não foi possível carregar a turma.");
            }
        }
        else
        {
            MessageBox.Show("Selecione uma turma válida.");
        }
    }

    private void btn_alunos_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (app.VMTurmaAtual.TurmaAtual_ == null)
                throw new OperacaoInvalida("Por favor, selecione uma turma primeiro.");

            MainContent.Content = new GerirAlunos();
        }
        catch (OperacaoInvalida ex)
        {
            MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

    }

    private void btn_grupos_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (app.VMTurmaAtual.TurmaAtual_ == null)
                throw new OperacaoInvalida("Por favor, selecione uma turma primeiro.");

            MainContent.Content = new GerirGrupos();
        }
        catch (OperacaoInvalida ex)
        {
            MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void btn_tarefas_Click(object sender, RoutedEventArgs e)
    {

        try
        {
            if (app.VMTurmaAtual.TurmaAtual_ == null)
                throw new OperacaoInvalida("Por favor, selecione uma turma primeiro.");

            MainContent.Content = new GerirTarefas();
        }
        catch (OperacaoInvalida ex)
        {
            MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void btn_perfil_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new Perfil();
    }

    private void btn_turma_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new CriarTurma();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        app.Muser.CriarFicheiroJsonSeNaoExistir();

        app.Muser.LerDeJson();

        // Só atualiza se Nome não for vazio
        if (!string.IsNullOrWhiteSpace(app.Muser.Nome))
            tb_nome_main.Text = app.Muser.Nome;

        // Se a foto existir e o ficheiro existir no disco
        if (!string.IsNullOrWhiteSpace(app.Muser.Fotografia) && File.Exists(app.Muser.Fotografia))
        {
            imagem_main.Source = new BitmapImage(new Uri(app.Muser.Fotografia));
            imagem_main.Visibility = Visibility.Visible;
            ProfileInitials.Visibility = Visibility.Collapsed;
        }
        else
        {
            imagem_main.Source = null;
            imagem_main.Visibility = Visibility.Collapsed;

            // Se Nome existir, usa primeira letra. Se não, usa "?" ou deixa vazio
            ProfileInitials.Text = !string.IsNullOrWhiteSpace(app.Muser.Nome)
                                      ? app.Muser.Nome.Substring(0, 1).ToUpper()
                                      : "?";

            ProfileInitials.Visibility = Visibility.Visible;
        }
    }

    private void btn_notas_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (app.VMTurmaAtual.TurmaAtual_ == null)
                throw new OperacaoInvalida("Por favor, selecione uma turma primeiro.");

            MainContent.Content = new AdicionarNotas();
        }
        catch (OperacaoInvalida ex)
        {
            MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void btn_pautas_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (app.VMTurmaAtual.TurmaAtual_ == null)
                throw new OperacaoInvalida("Por favor, selecione uma turma primeiro.");

            MainContent.Content = new GerirPautas();
        }
        catch (OperacaoInvalida ex)
        {
            MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void btn_home_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new Inicial();
    }

    private void ButtonSair_Click(object sender, RoutedEventArgs e)
    {
        if (app.VMTurmaAtual.TurmaAtual_ != null)
            app.VMTurmaAtual.GravarTurmaEmJson(app.VMTurmaAtual.TurmaAtual_);
        this.Close();
    }

    private void ButtonDef_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new Defenicoes();
    }
}