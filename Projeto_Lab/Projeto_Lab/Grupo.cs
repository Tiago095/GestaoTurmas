using Projeto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ObservableCollection<Aluno> Alunos { get; set; }
        public int NumeroAlunos { get; set; }

        public Grupo()
        {
            Alunos = new ObservableCollection<Aluno>();
            NumeroAlunos = 0;
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
