using AppOptica.Forms;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;


namespace AppOptica.Model
{
    public class ConsultaViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Cliente> clientes { get; set; } = new ObservableCollection<Cliente>();


        ObservableCollection<Consulta> consultas;

        public ObservableCollection<Consulta> Consultas
        {
            get => consultas;
            set
            {
                if (consultas != value)
                {
                    consultas = value;
                    OnPropertyChanged(nameof(Consultas));
                }
            }
        }
        public ConsultaViewModel( ObservableCollection<Consulta> consultas)
        {
             this.consultas = consultas ?? throw new ArgumentNullException(nameof(consultas));

                // Inicializar la lista de consultas
                this.consultas = new ObservableCollection<Consulta>();
        }




        public ICommand BuscarClienteCommand => new Command(BuscarCliente);

        public ICommand AgregarConsultaCommand => new Command(AgregarConsulta);

        private string nombreClienteSeleccionado;

        public string NombreClienteSeleccionado
        {
            get => nombreClienteSeleccionado;
            set
            {
                if (nombreClienteSeleccionado != value)
                {
                    nombreClienteSeleccionado = value;
                    OnPropertyChanged(nameof(NombreClienteSeleccionado));
                }
            }
        }
        private Cliente clienteSeleccionado;

        public Cliente ClienteSeleccionado
        {
            get => clienteSeleccionado;
            set
            {
                if (clienteSeleccionado != value)
                {
                    clienteSeleccionado = value;
                    OnPropertyChanged(nameof(ClienteSeleccionado));
                }
            }
        }

        public void BuscarCliente()
        {
            
        }





        #region Agregar
        void AgregarConsulta()
        {
            try
            {
                // Obtén el cliente seleccionado de la lista
                var clienteSeleccionado = clientes.FirstOrDefault(c => c.PNC == NombreClienteSeleccionado);

                if (clienteSeleccionado != null)
                {
                    // Crea una nueva consulta asociada al cliente
                    var nuevaConsulta = new Consulta
                    {
                        FechaC = DateTime.Now,
                        Cliente_ID = clienteSeleccionado.Cliente_ID,
                        Motivo = "Motivo de la consulta",  // Puedes personalizar este valor
                        Antecedentes = "Antecedentes de la consulta",  // Puedes personalizar este valor
                        // Agrega otros campos de la consulta según sea necesario
                    };

                    // Llama al método para agregar la consulta en SQLiteHelper
                    bool exito = SQLiteHelper.Instance.AgregarConsulta(nuevaConsulta);

                    if (exito)
                    {
                        // Solo si la inserción en la base de datos fue exitosa, actualiza la lista con la nueva consulta
                        Consultas.Add(nuevaConsulta);
                        OnPropertyChanged(nameof(Consultas));
                    }
                    else
                    {
                        Debug.WriteLine("Error al agregar la consulta en la base de datos.");
                    }
                }
                else
                {
                    Debug.WriteLine("Cliente no encontrado para asociar la consulta.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al agregar consulta: {ex.Message}");
            }
        }
        #endregion
        public void CargarConsultas()
        {
            try
            {
                // Obtén los consultas de la base de datos y agrégales a la lista
                var consultasBd = SQLiteHelper.Instance.ObtenerConsultas();
                foreach (var consulta in consultasBd)
                {
                    consultas.Add(consulta);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar consultas desde la base de datos: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
