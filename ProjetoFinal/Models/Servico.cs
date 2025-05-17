using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ProjetoFinal.Models
{
    public class Servico
    {
        #region atributos
        private Guid _id;
        private string _cliente;
        private string _funcionario;
        private float _custo;
        private float _horas;
        private DateTime _data;
        private string _dataFormatada;
        private bool _pago;
        private string _estado;

        //campo que vai ter a chave primária acossiada ao objeto
        [PrimaryKey, AutoIncrement] 
        public int AutoId { get; set; }
        #endregion

        #region construtor

        public Servico()
        {
            _id = Guid.NewGuid();
            _cliente = ".....";
            _funcionario = ".....";
            _custo = 0;
            _horas = 0;
            _data = DateTime.Now;
            _pago = false;
            _dataFormatada = DateTime.Now.ToString("dd/MM/yyyy");
            _estado = "Em dívida";
        }
        #endregion
        #region propriedades
        public Guid Id
        {
            get { return _id; }
        }

        public string Cliente
        {
            get { return _cliente; }
            set
            {
                value.Trim();
                if (value.Length < 3)
                {
                    _cliente = "indefinido";
                }
                else
                {
                    _cliente = value;
                }
            }
        }

        public string Funcionario
        {
            get { return _funcionario; }
            set
            {
                value.Trim();
                if (value.Length < 3)
                {
                    _funcionario = "indefinido";
                }
                else
                {
                    _funcionario = value;
                }
            }
        }

        public float Custo
        {
            get { return _custo; }
            set
            {
                if (value < 0)
                {
                    _custo = 0;
                }
                else
                {
                    _custo = value;
                }
            }
        }

        public float Horas
        {
            get { return _horas; }
            set
            {
                if (value < 0)
                {
                    _horas = 0;
                }
                else
                {
                    _horas = value;
                }
            }
        }

        public DateTime Data
        {
            get { return _data; }
            set { _data = value; }
        }

        //formata a data para uma string para ser apresentada na interface
        public string DataFormatada
        {
            get { return _data.ToString("dd/MM/yyyy"); }
        }

        public bool Pago
        {
            get { return _pago; }
            set { _pago = value; }
        }

        //retorna o estado como uma string para ser apresentada na interface, ao invés de mostrar true ou false
        public string Estado
        {
            get {
                if (_pago)
                {
                    return "Pago";
                }
                else
                {
                   return "Em dívida";
                }
            }
                
        }
        #endregion
    }
}
