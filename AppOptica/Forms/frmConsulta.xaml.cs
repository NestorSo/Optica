using AppOptica.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
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
        ConsultaviewModel = new ConsultaViewModel(consultas);
        lvConsulta.ItemsSource = consultas; // Usa la propiedad Consultas directamente
        ClientesListView.IsVisible = false;
    }

    void OnAgregarClicked(object sender, EventArgs e)
    {
        try
        {
            if (ConsultaviewModel == null)
            {
                ConsultaviewModel = new ConsultaViewModel(consultas);
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
                    TipoL = TipoL.Text,
                    AddOD = float.Parse(AddOD_E.Text),
                    AddOI = float.Parse(AddOI_E.Text),
                    DipOD = float.Parse(DipOD_E.Text),
                    DipOI = float.Parse(DipOI_E.Text),
                    AlturaOD = float.Parse(AlturaOD_E.Text),
                    AlturaOI = float.Parse(AlturaOI_E.Text)
                };

                // Llama al método AgregarConsulta de SQLiteHelper.Instance
                bool exito = SQLiteHelper.Instance.AgregarConsulta(nuevaConsulta);
                

                    if (Id_Client.Text != null || Motivo_E.Text != null || Antecedentes_E.Text != null || TipoL.Text != null || AddOD_E.Text != null || AddOI_E.Text != null || DipOD_E.Text != null || DipOI_E.Text != null || AlturaOD_E.Text != null || AlturaOI_E.Text != null)
                    {
                    if (exito)
                    {
                        consultas.Clear();
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
                    DisplayAlert("Error", "No puede agregar datos sin antes rellenar las casillas", "OK");
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
        TipoL.Text = string.Empty;
        AddOD_E.Text= string.Empty;
        AddOI_E.Text= string.Empty;
        DipOD_E.Text= string.Empty;
        DipOI_E.Text= string.Empty;
        AlturaOD_E.Text= string.Empty;
        AlturaOI_E.Text= string.Empty;
        // Limpiar otros controles de entrada según sea necesario
    }
  


    private void OnBuscarClienteClicked(object sender, EventArgs e)
    {
        if (S_Client.Text!=null)
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
        else
        {
            DisplayAlert("Error", "Ingrese un nombre para poder buscar", "OK");
        }
    }

    private void OnBuscarConsultaClicked(object sender, EventArgs e)
    {
        if (S_Client.Text != null)
        {
            try
            {
                // Obtén el texto del Entry de búsqueda
                string searchText = S_Client.Text;

                // Llama al método BuscarClientesPorNombre de SQLiteHelper.Instance
                var resultados = SQLiteHelper.Instance.BuscarClientesPorNombre(searchText);

                // Muestra los resultados en el ListView independiente
                lvConsulta.ItemsSource = resultados;

                // Hacer visible el ListView de clientes
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al buscar clientes: {ex.Message}");
            }
        }
        else
        {
            DisplayAlert("Error", "Ingrese un nombre para poder buscar", "OK");
        }

    }
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        if (sender == ClientesListView) // Manejar la selección de ClientesListView
        {
            Cliente clienteSeleccionado = (Cliente)e.SelectedItem;
            Id_Client.Text = clienteSeleccionado.Cliente_ID.ToString();
            ConsultaviewModel.ClienteSeleccionado = clienteSeleccionado;
            ClientesListView.SelectedItem = null;
            Device.BeginInvokeOnMainThread(() =>
            {
                ClientesListView.IsVisible = false;
            });
        }
        else if (sender == lvConsulta) // Manejar la selección de lvConsulta
        {
            Consulta consultaSeleccionada = (Consulta)e.SelectedItem;
            Id_Client.Text = consultaSeleccionada.Cliente_ID.ToString();
            Motivo_E.Text = consultaSeleccionada.Motivo;
            Antecedentes_E.Text = consultaSeleccionada.Antecedentes;
            AddOD_E.Text = consultaSeleccionada.AddOD.ToString();
            AddOI_E.Text = consultaSeleccionada.AddOI.ToString();
            DipOD_E.Text = consultaSeleccionada.DipOD.ToString();
            DipOI_E.Text = consultaSeleccionada.DipOI.ToString();
            AlturaOD_E.Text = consultaSeleccionada.AlturaOD.ToString();
            AlturaOI_E.Text = consultaSeleccionada.AlturaOI.ToString();
            TipoL.Text = consultaSeleccionada.TipoL;
            ConsultaviewModel.SelectConsul = consultaSeleccionada;
            lvConsulta.SelectedItem = null;
        }
    }
    void OnActualizarClicked(object sender, EventArgs e)
    {
        try
        {
            // Verifica si se ha seleccionado una consulta en lvConsulta
            if (ConsultaviewModel.SelectConsul == null)
            {
                DisplayAlert("Error", "Selecciona una consulta para actualizar", "OK");
                return;
            }

            // Mover datos a Consulta_Ant antes de la actualización

            // Obtén el ID de la consulta seleccionada
            int consultaId = ConsultaviewModel.SelectConsul.IdCon;

            // Crea una nueva instancia de Consulta con los datos actualizados
            Consulta consultaActualizada = new Consulta
            {
                IdCon = consultaId, // Usa el ID de la consulta obtenido
                FechaC = DateTime.Now,
                Cliente_ID = int.Parse(Id_Client.Text),
                Motivo = Motivo_E.Text,
                Antecedentes = Antecedentes_E.Text,
                TipoL = TipoL.Text,
                AddOD = float.Parse(AddOD_E.Text),
                AddOI = float.Parse(AddOI_E.Text),
                DipOD = float.Parse(DipOD_E.Text),
                DipOI = float.Parse(DipOI_E.Text),
                AlturaOD = float.Parse(AlturaOD_E.Text),
                AlturaOI = float.Parse(AlturaOI_E.Text)
            };

            // Llamar al método para actualizar la consulta en el modelo de vista
            bool exito = SQLiteHelper.Instance.ActualizarConsulta(consultaActualizada);

            if (exito)
            {
                // Limpiar los controles de entrada después de actualizar la consulta
                LimpiarControlesEntrada();
                consultas.Clear();
                // Cargar las consultas después de actualizar una existente
                ConsultaviewModel.CargarConsultas();

                // Notificar al usuario que la consulta se actualizó correctamente
                DisplayAlert("Éxito", "Consulta actualizada correctamente", "OK");
            }
            else
            {
                Debug.WriteLine("Error al actualizar la consulta en la base de datos.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al actualizar la consulta: {ex.Message}");
        }
    }

    // ...

    // Función para mover datos a Consulta_Anterior
   




    void OnLimpiarClicked(object sender, EventArgs e)
    {
        LimpiarControlesEntrada();
    }
    private async void OnRegresarClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

}