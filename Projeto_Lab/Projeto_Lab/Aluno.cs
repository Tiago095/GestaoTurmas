using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab
{
    public class Aluno
    {
        public int Numero { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public int Id_Grupo { get; set; }

        public Dictionary<int, float> Notas_Aluno { get; set; }

        public Aluno()
        {
            Notas_Aluno = new Dictionary<int, float>();
        }
    }
}
