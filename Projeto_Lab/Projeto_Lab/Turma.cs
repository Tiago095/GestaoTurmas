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

namespace Lab
{
    public class Turma
    {
        public string Id_turma { get; set; }
        public ObservableCollection<Aluno> Alunos { get; set; }
        public ObservableCollection<Grupo> grupos_ { get; set; }
        public ObservableCollection<Tarefa> Tarefas { get; set; }

        public Turma()
        {
            Alunos = new ObservableCollection<Aluno>();
            grupos_ = new ObservableCollection<Grupo>();
            Tarefas = new ObservableCollection<Tarefa>();
        }
    }
}
