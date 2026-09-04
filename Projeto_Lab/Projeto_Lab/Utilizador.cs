using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Projeto
{
    public class Utilizador
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Fotografia { get; set; } // Caminho para a foto

        public event EventHandler PerfilAtualizado;

        public void GuardarEmJson()
        {
            string caminhoFicheiro = "utilizador.json";

            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(caminhoFicheiro, json);

            PerfilAtualizado?.Invoke(this, EventArgs.Empty);
        }

        public void CriarFicheiroJsonSeNaoExistir()
        {
            string caminho = "utilizador.json";
            if (!File.Exists(caminho))
            {
                var dadosVazios = new Utilizador
                {
                    Nome = "UserUnknow",
                    Email = "email@example.com",
                    Fotografia = ""
                };

                string json = JsonSerializer.Serialize(dadosVazios, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(caminho, json);
            }
        }

        public void LerDeJson()
        {
            string caminhoFicheiro = "utilizador.json";

            // Ler o conteúdo do ficheiro e atualizar os atributos da instância
            string json = File.ReadAllText(caminhoFicheiro);
            Utilizador temp = JsonSerializer.Deserialize<Utilizador>(json);

            this.Nome = temp.Nome;
            this.Email = temp.Email;
            this.Fotografia = temp.Fotografia;
        }

    }
}

    