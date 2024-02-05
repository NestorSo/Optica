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
