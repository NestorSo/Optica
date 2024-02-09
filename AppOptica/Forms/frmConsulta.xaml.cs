using AppOptica.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

namespace AppOptica.Forms;

public partial class frmConsulta : ContentPage
{
    ConsultaViewModel viewModel;
    ObservableCollection<Cliente> clientes;
    ObservableCollection<Consulta> consultas = new ObservableCollection<Consulta>();

    public frmConsulta()
    {
        InitializeComponent();

        clientes = new ObservableCollection<Cliente>();
        viewModel = new ConsultaViewModel(clientes, consultas);
        BindingContext = viewModel;
        ClientesListView.IsVisible = false;
        //Asigna el m�todo de b�squeda al evento TextChanged del Entry
        //S_Client.TextChanged += OnBuscarClienteTextChanged;
    }

    void OnAgregarClicked(object sender, EventArgs e)
    {
        try
        {
            if (viewModel == null)
            {
                viewModel = new ConsultaViewModel(clientes, consultas);
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

                // Llama al m�todo AgregarConsulta de SQLiteHelper.Instance
                bool exito = SQLiteHelper.Instance.AgregarConsulta(nuevaConsulta);

                if (exito)
                {
                    // Limpiar los controles de entrada despu�s de agregar la consulta
                    LimpiarControlesEntrada();

                    // Cargar las consultas despu�s de agregar una nueva
                    viewModel.CargarConsultas();

                    // Notifica al usuario que la consulta se agreg� correctamente
                    DisplayAlert("�xito", "Consulta agregada correctamente", "OK");
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


    // M�todo para limpiar los controles de entrada despu�s de agregar una consulta
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
        // Limpiar otros controles de entrada seg�n sea necesario
    }
    void OnItemSelected(object sender, SelectedItemChangedEventArgs e) 
    { 
    
    }

    private void OnBuscarClicked(object sender, EventArgs e)
    {
        try
        {
            // Obt�n el texto del Entry de b�squeda
            string searchText = S_Client.Text;

            // Llama al m�todo BuscarClientesPorNombre de SQLiteHelper.Instance
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




    void OnActualizarClicked(object sender, EventArgs e)
    {
        // C�digo para actualizar
    }

    void OnEliminarClicked(object sender, EventArgs e)
    {
        // C�digo para eliminar
    }
    private async void OnRegresarClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

}