using AppOptica.Model;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

namespace AppOptica.Forms;

public partial class frmConsulta : ContentPage
{
    ConsultaViewModel viewModel;

    public frmConsulta()
    {
        InitializeComponent();
        
        viewModel = new ConsultaViewModel(clientes, consultas);
        BindingContext = viewModel;

        // Asigna el método de búsqueda al evento TextChanged del Entry
        S_Client.TextChanged += OnBuscarClienteTextChanged;
    }

    void OnAgregarClicked(object sender, EventArgs e)
    {
        try
        {
           

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
                    TipoL = TipoL.Text ,
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

    void OnBuscarClienteTextChanged(object sender, TextChangedEventArgs e)
    {
        string textoBusqueda = S_Client.Text;

        // Realiza la búsqueda en la lista de clientes
        var clienteEncontrado = viewModel.Clientes.FirstOrDefault(c => c.PNC.Contains(textoBusqueda) || c.SNC.Contains(textoBusqueda));

        if (clienteEncontrado != null)
        {
            // Muestra el ID del cliente en el Entry correspondiente
            Id_Client.Text = clienteEncontrado.Cliente_ID.ToString();
        }
        else
        {
            // Si no se encuentra el cliente, limpia el Entry del ID
            Id_Client.Text = string.Empty;
        }
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