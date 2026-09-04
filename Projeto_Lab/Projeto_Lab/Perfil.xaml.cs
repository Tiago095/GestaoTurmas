using Lab;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica interna para Perfil.xaml
    /// </summary>
    public partial class Perfil : UserControl
    {
        private App app;

        public Perfil()
        {
            InitializeComponent();
            app = App.Current as App;
            app.Muser.CriarFicheiroJsonSeNaoExistir();
            app.Muser.LerDeJson();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tb_email.Text = app.Muser.Email;
            tb_nome_.Text = app.Muser.Nome;

            if (!string.IsNullOrEmpty(app.Muser.Fotografia) && File.Exists(app.Muser.Fotografia))
            {
                ImagemPerfil.Source = new BitmapImage(new Uri(app.Muser.Fotografia));
                ImagemPerfil.Visibility = Visibility.Visible;
                ProfileInitials.Visibility = Visibility.Collapsed;
            }
            else
            {
                ImagemPerfil.Visibility = Visibility.Collapsed;
                ProfileInitials.Text = !string.IsNullOrEmpty(app.Muser.Nome) ? app.Muser.Nome.Substring(0, 1).ToUpper() : "?";
                ProfileInitials.Visibility = Visibility.Visible;
            }
        }


        private void btn_foto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Selecionar Foto de Perfil",
                Filter = "Imagens|*.jpg;*.jpeg;*.png;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapImage image = new BitmapImage(new Uri(openFileDialog.FileName));
                    ImagemPerfil.Source = image;
                    ImagemPerfil.Visibility = Visibility.Visible;
                    ProfileInitials.Visibility = Visibility.Collapsed;

                    app.Muser.Fotografia = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar imagem: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void bnt_salvar_Click(object sender, RoutedEventArgs e)
        {
            app.Muser.Nome = tb_nome_.Text;
            try
            {
                if (!app.VMTurmaAtual.IsValidEmail(tb_email.Text))
                    throw new OperacaoInvalida("Email Inválido");
                app.Muser.Email = tb_email.Text;
            }
            catch (OperacaoInvalida ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (!string.IsNullOrWhiteSpace(app.Muser.Fotografia) && File.Exists(app.Muser.Fotografia))
            {
                ImagemPerfil.Source = new BitmapImage(new Uri(app.Muser.Fotografia));
                ImagemPerfil.Visibility = Visibility.Visible;
                ProfileInitials.Visibility = Visibility.Collapsed;
            }
            else
            {
                ImagemPerfil.Source = null;
                ImagemPerfil.Visibility = Visibility.Collapsed;

                // Atualiza a letra inicial a partir do nome
                if (!string.IsNullOrWhiteSpace(app.Muser.Nome))
                    ProfileInitials.Text = app.Muser.Nome.Substring(0, 1).ToUpper();
                else
                    ProfileInitials.Text = "?";

                ProfileInitials.Visibility = Visibility.Visible;
            }

            app.Muser.GuardarEmJson();
        }


        private void btn_remover_Click(object sender, RoutedEventArgs e)
        {
            // Limpa os dados
            app.Muser.Nome = "";
            app.Muser.Email = "";
            app.Muser.Fotografia = null;

            tb_nome_.Text = "";
            tb_email.Text = "";

            // Esconde a imagem de perfil
            ImagemPerfil.Source = null;
            ImagemPerfil.Visibility = Visibility.Collapsed;

            // Mostra o "?" como inicial
            ProfileInitials.Text = "?";
            ProfileInitials.Visibility = Visibility.Visible;

            app.Muser.GuardarEmJson();
        }

    }
}
