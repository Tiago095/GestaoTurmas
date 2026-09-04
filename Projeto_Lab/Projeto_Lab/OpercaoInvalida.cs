using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab
{
    public class OperacaoInvalida : ApplicationException
    {
        public OperacaoInvalida(string msg)
           : base(msg)
        {

        }
    }
}
