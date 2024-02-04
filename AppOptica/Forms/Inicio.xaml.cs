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

        // Realiza la validaci�n del n�mero de tel�fono solo al presionar el bot�n de agregar
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
        // Verifica si el n�mero de tel�fono es v�lido
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            DisplayAlert("Error", "No puede estar el campo vac�o", "OK");
            return false;
        }

        // Patr�n para validar un n�mero de tel�fono de 8 d�gitos que comienza con 2, 5, 7 u 8
        string pattern = "^[2|5|7|8][0-9]{7}$";
        if (!Regex.IsMatch(phoneNumber, pattern))
        {
            // Muestra un mensaje de error
            DisplayAlert("Error", "N�mero de tel�fono no v�lido", "OK");
            return false;
        }

        return true;
    }
    //private void OnTelefonoEntryUnfocused(object sender, FocusEventArgs e)
    //{
    //    // Obt�n el valor ingresado en el Entry
    //    string telefono = TelefonoEntry.Text;

    //    // Verifica si el n�mero de tel�fono es v�lido
    //    if (!IsValidPhoneNumber(telefono))
    //    {
    //        // Muestra un mensaje de error
    //        DisplayAlert("Error", "N�mero de tel�fono no v�lido", "OK");

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
    //    // Patr�n para validar un n�mero de tel�fono de 8 d�gitos que comienza con 2, 5, 7 u 8
    //    string pattern = "^[2|5|7|8][0-9]{7}$";
    //    return Regex.IsMatch(phoneNumber, pattern);
    //}
    //Unfocused="OnTelefonoEntryUnfocused" 

}

