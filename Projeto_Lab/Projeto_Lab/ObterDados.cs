using Lab;
using Projeto;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

public class ObterDados
{
    public static ObservableCollection<Aluno> LerAlunos(string caminhoFicheiro)
    {
        var alunos = new ObservableCollection<Aluno>();
        var numerosExistentes = new HashSet<int>();

        var linhas = LerLinhasFicheiro(caminhoFicheiro);

        foreach (var linha in linhas)
        {
            var partes = linha.Split(',');
            if (partes.Length >= 3)
            {
                int numero = int.Parse(partes[1]);

                // Verifica duplicação e resolve
                while (numerosExistentes.Contains(numero))
                {
                    numero++;
                }

                numerosExistentes.Add(numero);

                var aluno = new Aluno
                {
                    Numero = numero,
                    Nome = partes[0],
                    Email = partes[2],
                    Id_Grupo = 0
                };

                alunos.Add(aluno);
            }
        }

        return alunos;
    }

    public static ObservableCollection<Grupo> LerGrupos(string caminhoFicheiro, ObservableCollection<Aluno> todosAlunos)
    {
        var grupos = new ObservableCollection<Grupo>();
        var linhas = File.ReadAllLines(caminhoFicheiro);
        var idsExistentes = new HashSet<int>();

        foreach (var linha in linhas)
        {
            var partes = linha.Split(';');
            if (partes.Length >= 4)
            {
                int idOriginal = int.Parse(partes[0]);
                int id = idOriginal;
                while (idsExistentes.Contains(id))
                {
                    id++;
                }
                idsExistentes.Add(id);

                string nome = partes[1];
                // Os números dos alunos estão no índice 3 (4º campo)
                string[] numerosStr = partes[3].Split(',');
                var alunosDoGrupo = new ObservableCollection<Aluno>();

                foreach (var numeroStr in numerosStr)
                {
                    if (int.TryParse(numeroStr.Trim(), out int numeroAluno))
                    {
                        var aluno = todosAlunos.FirstOrDefault(a => a.Numero == numeroAluno);
                        if (aluno != null)
                        {
                            alunosDoGrupo.Add(aluno);
                            aluno.Id_Grupo = id;
                        }
                    }
                }

                var grupo = new Grupo
                {
                    Id = id,
                    Nome = nome,
                    Alunos = alunosDoGrupo,
                    NumeroAlunos = alunosDoGrupo.Count
                };
                grupos.Add(grupo);
            }
        }

        return grupos;
    }


    public static ObservableCollection<Tarefa> LerTarefas(string caminhoFicheiro)
    {
        var tarefas = new ObservableCollection<Tarefa>();
        var linhas = File.ReadAllLines(caminhoFicheiro);
        var idsExistentes = new HashSet<int>();
        double pesoTotal = 0;

        foreach (var linha in linhas)
        {
            var partes = linha.Split(';');
            if (partes.Length >= 6)
            {
                int id = int.Parse(partes[0]);
                // Verifica duplicação e resolve
                while (idsExistentes.Contains(id))
                {
                    id++;
                }
                idsExistentes.Add(id);

                double peso = double.Parse(partes[5], System.Globalization.CultureInfo.InvariantCulture);

                // Verifica se adicionar este peso ultrapassaria 100
                if (pesoTotal + peso > 100)
                {
                    peso = 0;
                }
                else
                {
                    pesoTotal += peso;
                }

                var tarefa = new Tarefa
                {
                    Id = id,
                    Titulo = partes[1],
                    Descricao = partes[2],
                    DataHoraInicio = DateTime.Parse(partes[3]),
                    DataHoraTermino = DateTime.Parse(partes[4]),
                    Peso = peso
                };
                tarefas.Add(tarefa);
            }
        }
        return tarefas;
    }

    public static List<string> LerLinhasFicheiro(string caminhoFicheiro)
    {
        var linhas = new List<string>();

        var encodings = new[]
        {
            Encoding.Latin1
        };

        foreach (var encoding in encodings)
        {
            try
            {
                using (var reader = new StreamReader(caminhoFicheiro, encoding))
                {
                    string linha;
                    while ((linha = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(linha))
                        {
                            linhas.Add(linha);
                        }
                    }
                }

                // Se chegou aqui, funcionou
                return linhas;
            }
            catch
            {
                linhas.Clear();
                continue;
            }
        }

        throw new InvalidOperationException($"Não foi possível ler o ficheiro: {caminhoFicheiro}");
    }
}
