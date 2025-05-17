using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinal.Models
{
    internal class Cliente : Entidade
    {
        #region atributos
        private float _divida;
        #endregion

        #region construtor
        public Cliente() : base()
        {
            _divida = 0;
        }
        #endregion
        #region propriedades
        public float Divida
        {
            get { return _divida; }
            set
            {
                if (value < 0)
                {
                    _divida = 0;
                    throw new Exception("Divida inválida, deve ser maior ou igual a 0!");
                }
                else
                {
                    _divida = value;
                }
            }
        }
        #endregion
    }
}
