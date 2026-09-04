using Lab;
using Projeto;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Projeto_Lab;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public TurmaAtual VMTurmaAtual { get; set; }

    public Utilizador Muser { get; set; }

    public App()
    {
        VMTurmaAtual = new TurmaAtual();
        Muser = new Utilizador();
    }
}

