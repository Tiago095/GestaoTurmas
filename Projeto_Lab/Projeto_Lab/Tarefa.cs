using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraTermino { get; set; }
        public double Peso { get; set; }

        public Tarefa()
        {
            Titulo = " ";
            Descricao = " ";
        }

        public override string ToString()
        {
            return Titulo;
        }

    }

}
