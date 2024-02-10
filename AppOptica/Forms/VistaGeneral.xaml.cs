using AppOptica.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AppOptica.Forms;

public partial class VistaGeneral : ContentPage
{
    private GeneralViewModel generalViewModel;
    ObservableCollection<ConsultaGeneral> consultas = new ObservableCollection<ConsultaGeneral>();
    public VistaGeneral()
    {
        InitializeComponent();
        generalViewModel = new GeneralViewModel(consultas);
        BindingContext = generalViewModel;
        lvGeneral.ItemsSource = consultas;
    }

    private void OnBuscarClicked(object sender, EventArgs e)
    {
        try
        {
            // Obtén el texto del Entry de búsqueda
            string searchText = S_Client.Text;

            // Llama al método BuscarClientesPorNombre del ViewModel
            var resultados = SQLiteHelper.Instance.SearchGeneral(searchText);

            // Muestra los resultados en el ListView lvGeneral
            lvGeneral.ItemsSource = resultados;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al buscar clientes: {ex.Message}");
        }
    }

    private async void OnRegresarClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}