using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoFinal.Models
{
    internal class Funcionario : Entidade
    {
        #region atributos
        private float _salario;
        private float _diasFerias;
        private string _iban;
        private float _precoHora;
        #endregion

        #region construtor
        public Funcionario() : base()
        {
            _salario = 0;
            _diasFerias = 0;
            _iban = "-------------------------";
            _precoHora = 0;
        }
        #endregion

        #region propriedades
        public float Salario
        {
            get { return _salario; }
            set
            {
                if (value < 0)
                {
                    _salario = 0;
                    throw new Exception("Salário inválido, deve ser maior que 0!");
                }
                else
                {
                    _salario = value;
                }
            }
        }

        public float DiasFerias
        {
            get { return _diasFerias; }
            set
            {
                if (value < 0)
                {
                    _diasFerias = 0;
                    throw new Exception("Dias de férias inválidos, deve ser maior ou igual a 0!");
                }
                else
                {
                    _diasFerias = value;
                }
            }
        }

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

        public float PrecoHora
        {
            get { return _precoHora; }
            set
            {
                if (value < 0)
                {
                    _precoHora = 0;
                    throw new Exception("Preço por hora inválido, deve ser maior que 0!");
                }
                else
                {
                    _precoHora = value;
                }
            }
        }
        #endregion
    }
}
