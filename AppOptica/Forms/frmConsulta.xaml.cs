using System.Diagnostics;

namespace AppOptica.Forms;

public partial class frmConsulta : ContentPage
{
    public frmConsulta()
    {
        InitializeComponent();
    }

    void OnAgregarClicked(object sender, EventArgs e)
    {
        // C�digo para agregar
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