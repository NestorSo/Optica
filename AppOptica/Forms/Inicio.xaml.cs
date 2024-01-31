using AppOptica.Model;
using System.Collections.ObjectModel;

namespace AppOptica.Forms;

public partial class Inicio : ContentPage
{
    ObservableCollection<Persona> personas = new ObservableCollection<Persona>();
    InicioViewModel viewModel;
    public Inicio()
    {

        InitializeComponent();

        viewModel = new InicioViewModel(personas);

        PersonasListView.ItemsSource = personas;
    }

    void OnAgregarClicked(object sender, EventArgs e)
    {
        if (viewModel == null)
        {
            viewModel = new InicioViewModel(personas);
            PersonasListView.ItemsSource = personas;
        }

        var persona = new Persona
        {
            PrimerNombre = PrimerNombreEntry.Text,
            SegundoNombre = SegundoNombreEntry.Text,
            PrimerApellido = PrimerApellidoEntry.Text,
            SegundoApellido = SegundoApellidoEntry.Text,
            Direccion = DireccionEntry.Text,
            Cedula = CedulaEntry.Text
        };

        viewModel.AgregarPersona(persona);

        PrimerNombreEntry.Text = string.Empty;
        SegundoNombreEntry.Text = string.Empty;
        PrimerApellidoEntry.Text = string.Empty;
        SegundoApellidoEntry.Text = string.Empty;
        DireccionEntry.Text = string.Empty;
        CedulaEntry.Text = string.Empty;
    }

}

