using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjetoFinal.Models;

namespace ProjetoFinal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Servico> _listaServicos;
        // necessidade de usar ObservableCollection para que a lista seja atualizada automaticamente
        private ObservableCollection<Servico> _listaFiltrada;
        private Servico _servicoPivot;
        //contadores para mostrar o total pago e o total em divida
        private float _totalPago;
        private float _totalDivida;

        public MainWindow()
        {
            InitializeComponent();
            _listaServicos = new List<Servico>();
            _listaFiltrada = new ObservableCollection<Servico>();
            InicializarForm();
            //adicionado aqui porque caso sejam adicionados dentro do XAML a lógica não funciona visto que não retornam só a string
            ComboEstado.Items.Add("Pago");
            ComboEstado.Items.Add("Em dívida");

        }

        private async void InicializarForm()
        {
            //cria uma instancia do CRUP
            ServicoHelperCRUD shc = new ServicoHelperCRUD();
            //vai buscar os serviços à base de dados
            _listaServicos = await shc.GetServicos();
            //Associa a lista de serviços ao displayInfo
            displayInfo.ItemsSource = _listaServicos;
            //cria uma instancia de Serviço para ser usada como pivot
            _servicoPivot = new Servico();
            txtCliente.Text = "";
            //faz com que o campo seja editável para assegurar que este só pode ser usado quando cria um novo registo 
            txtCliente.IsEnabled = true;
            txtFuncionarios.Text = "";
            txtCusto.Text = "";
            txtHoras.Text = "";
            DataPicker.SelectedDate = null;
            //tal como o campo de cliente este campo só pode ser editável quando se cria um novo registo
            DataPicker.IsEnabled = true;
            chkPago.IsChecked = false;
            btnGravar.Content = "Criar";
            btnCancelar.IsEnabled = false;
            btnApagar.Visibility = Visibility.Hidden;
            btnLimparFiltros.Visibility = Visibility.Hidden;
            displayInfo.IsEnabled = true;
            //ativa os botoes de novo pois estes são desativados quando se seleciona um registo para editar
            btnLimparFiltros.IsEnabled = true;
            btnFiltrar.IsEnabled = true;
            //Chama esta função para popular a combobox de clientes, garantido que se for adicionado um cliente novo este fica logo na lista
            PopularComboBoxes();
            //atualiza o total pago e o total em divida
            lblTotal();
        }

        private void displayInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count == 0)
            {
                //seleciona o item que foi clicado
                _servicoPivot = displayInfo.SelectedItem as Servico;
                //preenche os campos com os valores do objeto selecionado
                txtCliente.Text = _servicoPivot.Cliente;
                //desativa o campo para este não poder ser alterado
                txtCliente.IsEnabled = false;
                txtFuncionarios.Text = _servicoPivot.Funcionario;
                txtCusto.Text = _servicoPivot.Custo.ToString();
                txtHoras.Text = _servicoPivot.Horas.ToString();
                DataPicker.SelectedDate = _servicoPivot.Data;
                //desativa o datepicker para não poder alterar a data
                DataPicker.IsEnabled = false;
                chkPago.IsChecked = _servicoPivot.Pago;
                //desativa o displayInfo para não poder selecionar outro registo enquanto se está a editar
                displayInfo.IsEnabled = false;
                //ativa o botão de cancelar e o botão de apagar
                btnCancelar.IsEnabled = true;
                btnApagar.Visibility = Visibility.Visible;
                //desativa os botões de filtro para não estar a alterar a lista enquanto se está a editar
                btnLimparFiltros.IsEnabled = false;
                btnFiltrar.IsEnabled = false;
                //muda o botão de gravar para atualizar
                btnGravar.Content = "Atualizar";
            }
        }

        private void btnSair_Click(object sender, RoutedEventArgs e)
        {
            //fecha a aplicação
            this.Close();
        }

        private String ValidarData()
        {
            if (DataPicker.SelectedDate == null)
            {
                return "Selecione uma data!";
            }
            else if (DataPicker.SelectedDate.Value > DateTime.Today)
            {
                return "Data inválida!";
            }
            return "OK";

        }

        private async void btnGravar_Click(object sender, RoutedEventArgs e)
        {
            //garante que todos os camps foram preenchidos
            if (txtCliente.Text == "" || txtFuncionarios.Text == "" || txtCusto.Text == "" || txtHoras.Text == "" || DataPicker.SelectedDate == null)
            {
                MessageBox.Show("Preencha todos os campos!");
                return;
            }
            else if (ValidarData() != "OK")
            {
                MessageBox.Show(ValidarData());
                return;
            }
            else
            {
                _servicoPivot.Cliente = txtCliente.Text;
                _servicoPivot.Funcionario = txtFuncionarios.Text;
                //garante que o custo e as horas são convertidos para float
                float custoConvertido = 0;
                float.TryParse(txtCusto.Text, out custoConvertido);
                _servicoPivot.Custo = custoConvertido;
                float horasConvertidas = 0;
                float.TryParse(txtHoras.Text, out horasConvertidas);
                _servicoPivot.Horas = horasConvertidas;
                //garante que a data é convertida para DateTime
                _servicoPivot.Data = DateTime.Parse(DataPicker.Text);
                _servicoPivot.Pago = (bool)chkPago.IsChecked;
                ServicoHelperCRUD shc = new ServicoHelperCRUD();
                //guarda o registo na base de dados
                await shc.SaveServico(_servicoPivot);
                MessageBox.Show("Inserido na base de dados!");
                //atualiza o formulário e a lista 
                InicializarForm();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            //cancela a edição e atualiza o formulário e a lista
            InicializarForm();
        }

        private async void btnApagar_Click(object sender, RoutedEventArgs e)
        {
            //apaga o registo selecionado e atualiza o formulário e a lista
            ServicoHelperCRUD shc = new ServicoHelperCRUD();
            await shc.DeleteServico(_servicoPivot);
            MessageBox.Show("Serviço apagado!");
            InicializarForm();
        }

        //função que calcula o total pago e o total em divida
        private void lblTotal()
        {
            _totalPago = 0;
            _totalDivida = 0;
            foreach (Servico s in _listaServicos)
            {
                if (s.Pago)
                {
                    _totalPago += s.Custo;
                }
                else
                {
                    _totalDivida += s.Custo;
                }
            }
            lblTotalizador.Content = $"Total Pago: {_totalPago} € | Total Divida: {_totalDivida} €";
        }

        //função que popula a combobox com os clientes
        private void PopularComboBoxes()
        {
            List<string> listaClientes = new List<string>();
            foreach (Servico s in _listaServicos)
            {
                listaClientes.Add(s.Cliente);
            }
            //a funcção list.Distinct() assegura-se que não há repetições na lista
            ComboClientes.ItemsSource = listaClientes.Distinct();
        }

        //função para filtrar os registos
        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            //garante que pelo menos um dos campos está preenchido
            if (ComboClientes.SelectedItem == null && ComboEstado.SelectedItem == null)
            {
                MessageBox.Show("Selecione um cliente ou especifique um estado!");
                return;
            }
            else
            {
                //limpa a lista para garantir que não duplica registos
                btnLimparFiltros.Visibility = Visibility.Visible;
                _listaFiltrada.Clear();
                //lógica que garante que retorna uma lista para qualque combinação de filtros
                if (ComboEstado.SelectedItem != null)
                {
                    if (ComboClientes.SelectedItem == null)
                    {
                        if (ComboEstado.SelectedItem.ToString() == "Pago")
                        {
                            foreach (Servico s in _listaServicos)
                            {
                                if (s.Pago)
                                {
                                    _listaFiltrada.Add(s);
                                }
                            }
                        }
                        else
                        {
                            foreach (Servico s in _listaServicos)
                            {
                                if (!s.Pago)
                                {
                                    _listaFiltrada.Add(s);
                                }
                            }
                        }
                    }
                    else if (ComboEstado.SelectedItem.ToString() == "Pago")
                    {
                        foreach (Servico s in _listaServicos)
                        {
                            if (s.Cliente == ComboClientes.SelectedItem.ToString() && s.Pago)
                            {
                                _listaFiltrada.Add(s);
                            }
                        }
                    }
                    else
                    {
                        foreach (Servico s in _listaServicos)
                        {
                            if (s.Cliente == ComboClientes.SelectedItem.ToString() && !s.Pago)
                            {
                                _listaFiltrada.Add(s);
                            }
                        }
                    }
                }
                else
                {
                    foreach (Servico s in _listaServicos)
                    {
                        if (s.Cliente == ComboClientes.SelectedItem.ToString())
                        {
                            Debug.WriteLine(ComboClientes.SelectedItem.ToString());
                            _listaFiltrada.Add(s);
                        }
                    }
                }
                //caso não haja registos que correspondam aos filtros selecionados informa o utilizador e limpa os filtros
                if (_listaFiltrada.Count == 0)
                {
                    MessageBox.Show("Não há registos que correspondam aos filtros selecionados!");
                    LimparFiltros();

                }
                //caso contrário mostra a lista filtrada
                else
                {
                    displayInfo.ItemsSource = _listaFiltrada;
                }
            }
        }


        private void btnLimparFiltros_Click(object sender, RoutedEventArgs e)
        {
            LimparFiltros();
        }

        //função que limpa os filtros
        private void LimparFiltros()
        {
            ComboEstado.SelectedItem = null;
            ComboClientes.SelectedItem = null;
            btnLimparFiltros.Visibility = Visibility.Hidden;
            InicializarForm();
        }
    }
}