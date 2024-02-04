using AppOptica.Model;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace AppOptica.Forms;
public partial class Inicio : ContentPage
{
    ObservableCollection<Cliente> Clientes = new ObservableCollection<Cliente>();
    InicioViewModel viewModel;

    public Inicio()
    {
        InitializeComponent();

        viewModel = new InicioViewModel(Clientes);

        ClientesListView.ItemsSource = Clientes;
    }

    void OnAgregarClicked(object sender, EventArgs e)
    {
        if (viewModel == null)
        {
            viewModel = new InicioViewModel(Clientes);
            ClientesListView.ItemsSource = Clientes;
        }


        var telefono = TelefonoEntry.Text;

        // Realiza la validación del número de teléfono solo al presionar el botón de agregar
        if (ValidatePhoneNumberAndNotEmpty(telefono))
        {
            var cliente = new Cliente
            {
                FechaR = DateTime.Now,
                PNC = PrimerNombreEntry.Text,
                SNC = SegundoNombreEntry.Text,
                PAC = PrimerApellidoEntry.Text,
                SAC = SegundoApellidoEntry.Text,
                TelC = telefono,
                DirC = DireccionEntry.Text,
                Ocupacion = OcupacionEntry.Text
            };

            viewModel.AgregarCliente(cliente);

            PrimerNombreEntry.Text = string.Empty;
            SegundoNombreEntry.Text = string.Empty;
            PrimerApellidoEntry.Text = string.Empty;
            SegundoApellidoEntry.Text = string.Empty;
            DireccionEntry.Text = string.Empty;
            TelefonoEntry.Text = string.Empty;
            OcupacionEntry.Text = string.Empty;
        }
        viewModel.CargarClientesDesdeBaseDeDatos();
    }
    private bool ValidatePhoneNumberAndNotEmpty(string phoneNumber)
    {
        // Verifica si el número de teléfono es válido
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            DisplayAlert("Error", "No puede estar el campo vacío", "OK");
            return false;
        }

        // Patrón para validar un número de teléfono de 8 dígitos que comienza con 2, 5, 7 u 8
        string pattern = "^[2|5|7|8][0-9]{7}$";
        if (!Regex.IsMatch(phoneNumber, pattern))
        {
            // Muestra un mensaje de error
            DisplayAlert("Error", "Número de teléfono no válido", "OK");
            return false;
        }

        return true;
    }
    //private void OnTelefonoEntryUnfocused(object sender, FocusEventArgs e)
    //{
    //    // Obtén el valor ingresado en el Entry
    //    string telefono = TelefonoEntry.Text;

    //    // Verifica si el número de teléfono es válido
    //    if (!IsValidPhoneNumber(telefono))
    //    {
    //        // Muestra un mensaje de error
    //        DisplayAlert("Error", "Número de teléfono no válido", "OK");

    //        // Limpia el contenido del Entry
    //        TelefonoEntry.Text = string.Empty;
    //    }
    //    else if (TelefonoEntry.Text == "")
    //    {
    //        DisplayAlert("Error", "No puede estar el campo vacio", "OK");
    //    }
    //}

    //private bool IsValidPhoneNumber(string phoneNumber)
    //{
    //    // Patrón para validar un número de teléfono de 8 dígitos que comienza con 2, 5, 7 u 8
    //    string pattern = "^[2|5|7|8][0-9]{7}$";
    //    return Regex.IsMatch(phoneNumber, pattern);
    //}
    //Unfocused="OnTelefonoEntryUnfocused" 

}

