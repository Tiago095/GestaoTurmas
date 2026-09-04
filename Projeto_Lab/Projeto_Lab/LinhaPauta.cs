using System;
using System.Collections.Generic;
using System.Linq;
using Projeto;

namespace Lab
{
    public class LinhaPauta
    {
        public int NumeroAluno { get; set; }
        public string Nome { get; set; }
        public Dictionary<string, double?> Notas { get; set; }
        public double? Media { get; set; }
        public string Aprovado { get; set; }

        public LinhaPauta(Aluno aluno, IEnumerable<Tarefa> tarefas)
        {
            NumeroAluno = aluno.Numero;
            Nome = aluno.Nome;
            Notas = new Dictionary<string, double?>();

            // Se o aluno não estiver em nenhum grupo (Id_Grupo = 0), todas as notas serão null
            if (aluno.Id_Grupo == 0)
            {
                foreach (var tarefa in tarefas)
                {
                    Notas[tarefa.Id.ToString()] = null;
                }
                Media = null;
                Aprovado = "Não";
                return;
            }

            foreach (var tarefa in tarefas)
            {
                double? nota = 0;
                string idString = tarefa.Id.ToString();
                
                if (aluno.Notas_Aluno != null && aluno.Notas_Aluno.TryGetValue(tarefa.Id, out float notaValue))
                {
                    nota = notaValue;
                }
                
                Notas[idString] = nota; 
            }

            CalcularMedia(tarefas);
        }

        private void CalcularMedia(IEnumerable<Tarefa> tarefas)
        {
            if (!tarefas.Any())
            {
                Media = null;
                Aprovado = "Não";
                return;
            }

            double somaNotas = 0;
            double somaPesos = 0;

            foreach (var tarefa in tarefas)
            {
                string idString = tarefa.Id.ToString();
                if (Notas.ContainsKey(idString) && Notas[idString].HasValue) 
                {
                    somaNotas += Notas[idString].Value * tarefa.Peso;
                    somaPesos += tarefa.Peso;
                }
            }

            Media = somaPesos > 0 ? somaNotas / somaPesos : null;
            Aprovado = Media >= 9.45 ? "Sim" : "Não";
        }
    }
}
