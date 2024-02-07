using AppOptica.Forms;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel;


namespace AppOptica.Model
{
    public class InicioViewModel :INotifyPropertyChanged
    {
        ObservableCollection<Cliente> clientes;
        

        public InicioViewModel(ObservableCollection<Cliente> clientes)
        {
            this.clientes = clientes ?? throw new ArgumentNullException(nameof(clientes));
            CargarClientesDesdeBaseDeDatos();
        }

        private Cliente _clienteSeleccionado;
        public Cliente ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set
            {
                if (_clienteSeleccionado != value)
                {
                    _clienteSeleccionado = value;
                    OnPropertyChanged(nameof(ClienteSeleccionado));
                }
            }
        }
        public void AgregarCliente(Cliente cliente)
        {
            try
            {
                // Llamas al método AgregarCliente de SQLiteHelper.Instance
                bool exito = SQLiteHelper.Instance.AgregarCliente(cliente);

                if (exito)
                {
                    clientes.Clear();
                    // Solo si la inserción en la base de datos fue exitosa, actualizas la lista con los datos recién insertados
                    clientes.Add(cliente);
                    OnPropertyChanged(nameof(cliente));

                }
                else
                {
                    Debug.WriteLine("Error al agregar clientes en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al agregar cliente: {ex.Message}");
            }
        }
        public void ActualizarCliente(Cliente cliente)
        {
            try
            {
                bool exito = SQLiteHelper.Instance.ActualizarCliente(cliente);

                if (exito)
                {
                    // Buscar y actualizar el cliente en la lista
                    var clienteExistente = clientes.FirstOrDefault(c => c.Cliente_ID == cliente.Cliente_ID);

                    if (clienteExistente != null)
                    {
                        // Actualizar los datos del cliente en la lista
                        clienteExistente.FechaR = cliente.FechaR;
                        clienteExistente.PNC = cliente.PNC;
                        clienteExistente.SNC = cliente.SNC;
                        clienteExistente.PAC = cliente.PAC;
                        clienteExistente.SAC = cliente.SAC;
                        clienteExistente.TelC = cliente.TelC;
                        clienteExistente.DirC = cliente.DirC;
                        clienteExistente.Ocupacion = cliente.Ocupacion;

                        // Notificar a la interfaz de usuario que la lista 'clientes' ha cambiado
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            OnPropertyChanged(nameof(clientes));
                        });
                    }
                }
                else
                {
                    Debug.WriteLine("Error al actualizar cliente en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al actualizar cliente: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void CargarClientesDesdeBaseDeDatos()
        {
            try
            {
                // Obtén los clientes de la base de datos y agrégales a la lista
                var clientesDb = SQLiteHelper.Instance.ObtenerClientes();
                foreach (var cliente in clientesDb)
                {
                    clientes.Add(cliente);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar clientes desde la base de datos: {ex.Message}");
            }
        }
    }
}
