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
        ObservableCollection<Cliente> clientes;
        ObservableCollection<Consulta> consultas;
        string NombreClienteSeleccionado;

        public ConsultaViewModel(ObservableCollection<Cliente> clientes, ObservableCollection<Consulta> consultas)
        {
            this.clientes = clientes ?? throw new ArgumentNullException(nameof(clientes));
            this.consultas = consultas ?? throw new ArgumentNullException(nameof(consultas));
        }

        int clienteIdSeleccionado;

        public int ClienteIdSeleccionado
        {
            get => clienteIdSeleccionado;
            set
            {
                if (clienteIdSeleccionado != value)
                {
                    clienteIdSeleccionado = value;
                    OnPropertyChanged(nameof(ClienteIdSeleccionado));
                }
            }
        }

        public ICommand BuscarClienteCommand => new Command(BuscarCliente);

        public ICommand AgregarConsultaCommand => new Command(AgregarConsulta);

        void BuscarCliente()
        {
            if (!string.IsNullOrEmpty(NombreClienteSeleccionado))
            {
                // Llama al método BuscarClientesPorNombre de SQLiteHelper.Instance
                var resultados = SQLiteHelper.Instance.BuscarClientesPorNombre(NombreClienteSeleccionado);

                // Actualiza la lista de clientes según los resultados
                clientes.Clear();
                foreach (var cliente in resultados)
                {
                    clientes.Add(cliente);
                }

                if (resultados.Count > 0)
                {
                    // Asigna el ID del primer cliente encontrado (puedes ajustar esto según tus necesidades)
                    ClienteIdSeleccionado = resultados[0].Cliente_ID;
                }
                else
                {
                    // Si no se encuentra el cliente, limpia el Entry del ID
                    ClienteIdSeleccionado = 0;
                }

                OnPropertyChanged(nameof(clientes));
            }
        }



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
                        consultas.Add(nuevaConsulta);
                        OnPropertyChanged(nameof(consultas));
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
