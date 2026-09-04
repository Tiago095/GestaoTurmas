using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Projeto;
using Projeto_Lab;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace Lab
{
    public class TurmaAtual
    {
        public Turma TurmaAtual_ { get; set; }

        public ObservableCollection<string> NomeTurmas { get; set; }

        public event EventHandler TurmasAtualizadas;
        public event EventHandler AlunoEditado;
        public event EventHandler GrupoEditado;

        public TurmaAtual()
        {
            NomeTurmas = new ObservableCollection<string>();
        }

        public void CarregarDeFicheiros(string id, string caminhoAlunos, string caminhoGrupos, string caminhoTarefas)
        {

            if (string.IsNullOrWhiteSpace(caminhoAlunos))
                throw new OperacaoInvalida("É obrigatório fornecer o ficheiro de alunos.");

            var alunos = ObterDados.LerAlunos(caminhoAlunos);

            var grupos = string.IsNullOrWhiteSpace(caminhoGrupos)
                        ? new ObservableCollection<Grupo>()
                        : ObterDados.LerGrupos(caminhoGrupos, alunos);

            var tarefas = string.IsNullOrWhiteSpace(caminhoTarefas)
                         ? new ObservableCollection<Tarefa>()
                         : ObterDados.LerTarefas(caminhoTarefas);

            Turma turma = new Turma
            {
                Id_turma = id,
                Alunos = alunos,
                grupos_ = grupos,
                Tarefas = tarefas
            };

            TurmaAtual_ = turma;
        }


        public void GravarTurmaEmJson(Turma turma)
        {
            var opcoes = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            // Pasta no AppData do usuário
            string pastaDados = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GestaoTurmas", "DadosTurmas");

            string nomeFicheiro = turma.Id_turma + ".json";
            string caminho = Path.Combine(pastaDados, nomeFicheiro);

            // Cria a pasta se não existir (funciona em qualquer sistema)
            Directory.CreateDirectory(pastaDados);

            string json = JsonSerializer.Serialize(turma, opcoes);
            File.WriteAllText(caminho, json);
            AlunoEditado?.Invoke(this, EventArgs.Empty);
        }

        public Turma? CarregarTurmaDeJson(string idTurma)
        {
            // Pasta no AppData do usuário
            string pastaDados = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GestaoTurmas", "DadosTurmas");

            string caminho = Path.Combine(pastaDados, idTurma + ".json");

            if (!File.Exists(caminho))
                return null;

            var opcoes = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNameCaseInsensitive = true
            };

            string json = File.ReadAllText(caminho);
            return JsonSerializer.Deserialize<Turma>(json, opcoes);
        }

        public string ObterCaminhoPastaDados()
        {
            string pastaDados = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GestaoTurmas", "DadosTurmas");
            string caminhoCompleto = Path.GetFullPath(pastaDados);
            return caminhoCompleto;
        }

        public void AdicionarNomeTurmaAoFicheiro(string nomeTurma)
        {
            // Pasta no AppData do usuário
            string pastaDados = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GestaoTurmas", "DadosTurmas");
            Directory.CreateDirectory(pastaDados); // Garante que a pasta existe

            string caminhoFicheiro = Path.Combine(pastaDados, "NomesTurmas.json");

            List<string> nomes;
            if (File.Exists(caminhoFicheiro))
            {
                var json = File.ReadAllText(caminhoFicheiro);
                nomes = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
            else
            {
                nomes = new List<string>();
            }

            if (!nomes.Contains(nomeTurma))
            {
                nomes.Add(nomeTurma);
                File.WriteAllText(caminhoFicheiro, JsonSerializer.Serialize(nomes, new JsonSerializerOptions { WriteIndented = true }));
            }

            // Atualiza a lista interna
            if (!NomeTurmas.Contains(nomeTurma))
                NomeTurmas.Add(nomeTurma);

            TurmasAtualizadas?.Invoke(this, EventArgs.Empty);
        }

        public void RemoverNomeTurmaDoFicheiro(string nomeTurma)
        {
            // Pasta no AppData do usuário
            string pastaDados = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GestaoTurmas", "DadosTurmas");
            string caminhoFicheiro = Path.Combine(pastaDados, "NomesTurmas.json");

            List<string> nomes;
            if (File.Exists(caminhoFicheiro))
            {
                var json = File.ReadAllText(caminhoFicheiro);
                nomes = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();

                if (nomes.Contains(nomeTurma))
                {
                    nomes.Remove(nomeTurma);
                    File.WriteAllText(caminhoFicheiro, JsonSerializer.Serialize(nomes, new JsonSerializerOptions { WriteIndented = true }));
                }
            }

            // Apaga o ficheiro específico da turma
            string caminhoFicheiroTurma = Path.Combine(pastaDados, $"{nomeTurma}.json");
            if (File.Exists(caminhoFicheiroTurma))
            {
                try
                {
                    File.Delete(caminhoFicheiroTurma);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao apagar ficheiro da turma: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // Atualiza a lista interna
            if (NomeTurmas.Contains(nomeTurma))
                NomeTurmas.Remove(nomeTurma);

            TurmasAtualizadas?.Invoke(this, EventArgs.Empty);
        }

        // Método para carregar os nomes das turmas do ficheiro (útil na inicialização)
        public void CarregarNomesDasTurmasDoFicheiro()
        {
            string pastaDados = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GestaoTurmas", "DadosTurmas");
            string caminhoFicheiro = Path.Combine(pastaDados, "NomesTurmas.json");

            if (File.Exists(caminhoFicheiro))
            {
                var json = File.ReadAllText(caminhoFicheiro);
                NomeTurmas = JsonSerializer.Deserialize<ObservableCollection<string>>(json) ?? new ObservableCollection<string>();
            }
            else
            {
                NomeTurmas = new ObservableCollection<string>();
            }

            TurmasAtualizadas?.Invoke(this, EventArgs.Empty);
        }
        public bool VerificarID(int id)
        {
            foreach (var grupo in TurmaAtual_.grupos_)
            {
                if (grupo.Id == id)
                    return true;
            }
            return false;
        }

        public bool VerificarNum(int num)
        {
            foreach (var aluno in TurmaAtual_.Alunos)
            {
                if (aluno.Numero == num)
                    return true;
            }
            return false;
        }

        public void AtualizarIdGrupo(ObservableCollection<Aluno> alunos, int id)
        {
            foreach (var aluno in TurmaAtual_.Alunos)
            {
                foreach (var Alunos in alunos)
                {
                    if (aluno.Numero == Alunos.Numero)
                        aluno.Id_Grupo = id;
                }
            }

            GrupoEditado?.Invoke(this, EventArgs.Empty);
        }

        public bool VerificarIDTarefa(int id)
        {
            foreach (var grupo in TurmaAtual_.Tarefas)
            {
                if (grupo.Id == id)
                    return true;
            }
            return false;
        }

        public double VerificarPeso(double peso)
        {
            double total = 0;

            foreach (var tarefa in TurmaAtual_.Tarefas)
            {
                total = total + tarefa.Peso;
            }

            total = total + peso;

            return total;
        }


        public ObservableCollection<Aluno> FiltrarAlunos(string tipo, string aluno,ObservableCollection<Aluno> alunos)
        {
            if (tipo == "Número")
            {
                if (!string.IsNullOrWhiteSpace(aluno) && aluno.All(char.IsDigit))
                {
                    return new ObservableCollection<Aluno>(
                        alunos.Where(x =>
                            x.Numero.ToString().Contains(aluno)
                        )
                    );
                }
                else
                {
                    return new ObservableCollection<Aluno>();
                }
            }

            if (tipo == "Nome")
            {
                return new ObservableCollection<Aluno>(
                    alunos.Where(x =>
                        !string.IsNullOrEmpty(x.Nome) &&
                        x.Nome.Contains(aluno, StringComparison.OrdinalIgnoreCase)
                    )
                );
            }

            return new ObservableCollection<Aluno>();
        }

        public ObservableCollection<Tarefa> FiltrarTarefas (string id)
        {
            if (!string.IsNullOrWhiteSpace(id) && id.All(char.IsDigit))
            {
                return new ObservableCollection<Tarefa>(
                    TurmaAtual_.Tarefas.Where(x =>
                        x.Id.ToString().Contains(id)
                    )
                );
            }
            else
            {
                return new ObservableCollection<Tarefa>();
            }
        }

        public ObservableCollection<Grupo> FiltrarGrupos(string id)
        {
            if (!string.IsNullOrWhiteSpace(id) && id.All(char.IsDigit))
            {
                return new ObservableCollection<Grupo>(
                    TurmaAtual_.grupos_.Where(x =>
                        x.Id.ToString().Contains(id)
                    )
                );
            }
            else
            {
                return new ObservableCollection<Grupo>();
            }
        }

        public void AtualizarNota (Aluno aluno,float nota,int idTarefa)
        {
            foreach (var aluno_ in TurmaAtual_.Alunos)
            {
                if (aluno_.Numero == aluno.Numero)
                    aluno_.Notas_Aluno[idTarefa] = nota;
            }
            GravarTurmaEmJson(TurmaAtual_);
        }
    
        public bool VerificarIdTurma(string id)
        {
            foreach(var id_ in NomeTurmas)
            {
                if (id == id_)
                    return true;
            }
            return false;
        }
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";//formato do email
            return Regex.IsMatch(email, pattern);
        }

        public void AtualizarAlunoemGrupo(int id,Aluno aluno)
        {
            Grupo grupo = TurmaAtual_.grupos_.FirstOrDefault(g => g.Id == id);
            var alunosParaGrupo = grupo.Alunos;
            if (grupo == null) return;

            foreach (var alunoParaGrupo in alunosParaGrupo)
            {
                if (alunoParaGrupo.Numero == aluno.Numero)
                {
                    alunoParaGrupo.Nome = aluno.Nome;
                }
            }
        }
    }
}
