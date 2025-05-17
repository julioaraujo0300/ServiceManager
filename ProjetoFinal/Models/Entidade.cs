using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinal.Models
{
    public class Entidade
    {
        #region atributos
        private string _nome;
        private string _nif;
        #endregion

        #region propriedades
        public string Nome { 
            get { return _nome; }
            set { 
                value.Trim();
                if (value.Length < 3)
                {
                    _nome = "indefinido";
                    throw new Exception("Nome inválido");
                }
                else
                {
                    _nome = value;
                }
            }
        }

        public string Nif { 
            get { return _nif; }
            set { 
                if (value.Length != 9)
                {
                    _nif = "---------";
                    throw new Exception("NIF inválido, deve ter 9 carateres!");
                }
                else
                {
                    _nif = value;
                }
            }
        }
        #endregion
    }
}
