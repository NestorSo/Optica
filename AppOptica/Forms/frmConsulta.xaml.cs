using AppOptica.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

namespace AppOptica.Forms;

public partial class frmConsulta : ContentPage
{
    ConsultaViewModel ConsultaviewModel;
    ObservableCollection<Cliente> clientes;
    ObservableCollection<Consulta> consultas = new ObservableCollection<Consulta>();

    public frmConsulta()
    {
        InitializeComponent();

        clientes = new ObservableCollection<Cliente>();
        consultas = new ObservableCollection<Consulta>();
        ConsultaviewModel = new ConsultaViewModel( consultas);
        BindingContext = ConsultaviewModel;
        lvConsulta.ItemsSource = consultas; // Usa la propiedad Consultas directamente
        ClientesListView.IsVisible = false;
        

    }

    void OnAgregarClicked(object sender, EventArgs e)
    {
        try
        {
            if (ConsultaviewModel == null)
            {
                ConsultaviewModel = new ConsultaViewModel( consultas);
                lvConsulta.ItemsSource = consultas; // Cambia a clientes
            }

            // Asumiendo que tienes un Entry para el ID del cliente seleccionado
            if (int.TryParse(Id_Client.Text, out int clienteID))
            {
                // Crea una instancia de la clase Consulta con los valores obtenidos
                Consulta nuevaConsulta = new Consulta
                {
                    FechaC = DateTime.Now,
                    Cliente_ID = clienteID,
                    Motivo = Motivo_E.Text,
                    Antecedentes = Antecedentes_E.Text,
                    OD = float.Parse(OD_E.Text),
                    OI = float.Parse(OI_E.Text),
                    TipoL = TipoL.Text,
                    ADD_ = float.Parse(ADD_E.Text),
                    DIP = float.Parse(DIP_E.Text),
                    Altura = float.Parse(Altura_E.Text)
                };

                // Llama al método AgregarConsulta de SQLiteHelper.Instance
                bool exito = SQLiteHelper.Instance.AgregarConsulta(nuevaConsulta);

                if (exito)
                {
                    // Limpiar los controles de entrada después de agregar la consulta
                    LimpiarControlesEntrada();

                    // Cargar las consultas después de agregar una nueva
                    ConsultaviewModel.CargarConsultas();

                    // Notifica al usuario que la consulta se agregó correctamente
                    DisplayAlert("Éxito", "Consulta agregada correctamente", "OK");
                }
                else
                {
                    Debug.WriteLine("Error al agregar la consulta en la base de datos.");
                }
            }
            else
            {
                Debug.WriteLine("Error al convertir el ID del cliente a un entero.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al agregar la consulta: {ex.Message}");
        }
    }


    // Método para limpiar los controles de entrada después de agregar una consulta
    void LimpiarControlesEntrada()
    {
        Id_Client.Text= string.Empty;
        Motivo_E.Text= string.Empty;
        Antecedentes_E.Text= string.Empty;
        OD_E.Text= string.Empty;
        OI_E.Text= string.Empty;
        TipoL.Text = string.Empty;
        ADD_E.Text= string.Empty;
        DIP_E.Text= string.Empty;
        Altura_E.Text= string.Empty;
        // Limpiar otros controles de entrada según sea necesario
    }


    private void OnBuscarClicked(object sender, EventArgs e)
    {
        try
        {
            // Obtén el texto del Entry de búsqueda
            string searchText = S_Client.Text;

            // Llama al método BuscarClientesPorNombre de SQLiteHelper.Instance
            var resultados = SQLiteHelper.Instance.BuscarClientesPorNombre(searchText);

            // Muestra los resultados en el ListView independiente
            ClientesListView.ItemsSource = resultados;

            // Hacer visible el ListView de clientes
            ClientesListView.IsVisible = true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al buscar clientes: {ex.Message}");
        }
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        // Obtener el cliente seleccionado del evento
        Cliente clienteSeleccionado = (Cliente)e.SelectedItem;

        // Asignar los valores del cliente a los Entry respectivos
        Id_Client.Text = clienteSeleccionado.Cliente_ID.ToString();

        // Actualizar la propiedad ClienteSeleccionado en el ViewModel
        ConsultaviewModel.ClienteSeleccionado = clienteSeleccionado;

        // Desmarcar la selección del ListView
        ClientesListView.SelectedItem = null;

        // Ocultar el ListView después de procesar la selección
        Device.BeginInvokeOnMainThread(() =>
        {
            ClientesListView.IsVisible = false;
        });
    }





    void OnActualizarClicked(object sender, EventArgs e)
    {
        // Código para actualizar
    }

    void OnEliminarClicked(object sender, EventArgs e)
    {
        // Código para eliminar
    }
    private async void OnRegresarClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

}