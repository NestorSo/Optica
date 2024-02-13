using AppOptica.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AppOptica.Forms;

public partial class VistaGeneral : ContentPage
{
    private GeneralViewModel generalViewModel;
    ObservableCollection<ConsultaGeneral> consultas = new ObservableCollection<ConsultaGeneral>();
    SQLiteHelper helper;
    public VistaGeneral()
    {
        InitializeComponent();
        generalViewModel = new GeneralViewModel(consultas);
        lvGeneral.ItemsSource = consultas;
        generalViewModel.ObtenerGeneral();
    }

    private void OnBuscarClicked(object sender, EventArgs e)
    {
        try
        {
            // Obt�n el texto del Entry de b�squeda
            string searchText = S_Client.Text;
            if (S_Client.Text!=null)
            {
                // Llama al m�todo BuscarClientesPorNombre del ViewModel
                var resultados = SQLiteHelper.Instance.SearchGeneral(searchText);

                // Muestra los resultados en el ListView lvGeneral
                lvGeneral.ItemsSource = resultados;
            }
            else
            {
                DisplayAlert("Error", "Ingresa un nombre para buscar", "OK");
               
            }

 
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al buscar clientes: {ex.Message}");
        }
    }
    private void  OnLimpiarClicked(object sender, EventArgs e)
    {
        S_Client.Text=string.Empty;
        consultas.Clear();
        lvGeneral.ItemsSource = consultas;

        generalViewModel.ObtenerGeneral();

    }



    private async void OnRegresarClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}