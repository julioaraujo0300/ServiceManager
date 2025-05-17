using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinal.Models
{
    internal class Empresa : Entidade
    {
        #region atributos
        private string _iban;
        #endregion
        #region construtor
        public Empresa() : base()
        {
            _iban = "-------------------------";
        }
        #endregion

        #region propriedades
        public string Iban
        {
            get { return _iban; }
            set
            {
                if (value.Length != 25)
                {
                    _iban = "-------------------------";
                    throw new Exception("IBAN inválido, deve ter 25 carateres!");
                }
                else
                {
                    _iban = value;
                }
            }
        }
        #endregion
    }
}
