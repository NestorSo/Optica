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
                    consultas.Clear();
                    // Limpiar los controles de entrada despu�s de agregar la consulta
                    LimpiarControlesEntrada();

                    // Cargar las consultas despu�s de agregar una nueva
                    ConsultaviewModel.CargarConsultas();

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


    private void OnBuscarClicked(object sender, EventArgs e)
    {
        if (S_Client.Text!=null)
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
        else
        {
            DisplayAlert("Error", "Ingrese un nombre para poder buscar", "OK");
        }
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        if (sender == ClientesListView) // Manejar la selecci�n de ClientesListView
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
        else if (sender == lvConsulta) // Manejar la selecci�n de lvConsulta
        {
            Consulta consultaSeleccionada = (Consulta)e.SelectedItem;
            Id_Client.Text = consultaSeleccionada.Cliente_ID.ToString();
            Motivo_E.Text = consultaSeleccionada.Motivo;
            Antecedentes_E.Text = consultaSeleccionada.Antecedentes;
            OD_E.Text = consultaSeleccionada.OD.ToString();
            OI_E.Text = consultaSeleccionada.OI.ToString();
            ADD_E.Text = consultaSeleccionada.ADD_.ToString();
            DIP_E.Text = consultaSeleccionada.DIP.ToString();
            Altura_E.Text = consultaSeleccionada.Altura.ToString();
            TipoL.Text = consultaSeleccionada.TipoL;
            ConsultaviewModel.SelectConsul = consultaSeleccionada;
            lvConsulta.SelectedItem = null;
        }
    }





    // ...

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

            // Mover datos a Consulta_Ant antes de la actualizaci�n
            MoverAConsultaAnterior(ConsultaviewModel.SelectConsul);

            // Obt�n el ID de la consulta seleccionada
            int consultaId = ConsultaviewModel.SelectConsul.IdCon;

            // Crea una nueva instancia de Consulta con los datos actualizados
            Consulta consultaActualizada = new Consulta
            {
                IdCon = consultaId, // Usa el ID de la consulta obtenido
                FechaC = DateTime.Now,
                Cliente_ID = int.Parse(Id_Client.Text),
                Motivo = Motivo_E.Text,
                Antecedentes = Antecedentes_E.Text,
                OD = float.Parse(OD_E.Text),
                OI = float.Parse(OI_E.Text),
                TipoL = TipoL.Text,
                ADD_ = float.Parse(ADD_E.Text),
                DIP = float.Parse(DIP_E.Text),
                Altura = float.Parse(Altura_E.Text)
            };

            // Llamar al m�todo para actualizar la consulta en el modelo de vista
            bool exito = SQLiteHelper.Instance.ActualizarConsulta(consultaActualizada);

            if (exito)
            {
                // Limpiar los controles de entrada despu�s de actualizar la consulta
                LimpiarControlesEntrada();
                consultas.Clear();
                // Cargar las consultas despu�s de actualizar una existente
                ConsultaviewModel.CargarConsultas();

                // Notificar al usuario que la consulta se actualiz� correctamente
                DisplayAlert("�xito", "Consulta actualizada correctamente", "OK");
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

    // Funci�n para mover datos a Consulta_Anterior
    void MoverAConsultaAnterior(Consulta consulta)
    {
        try
        {
            // Crear una instancia de Consulta_Ant con los datos actuales
            Consulta_Ant consultaAnterior = new Consulta_Ant
            {
                FechaC = consulta.FechaC,
                Cliente_ID = consulta.Cliente_ID,
                Motivo = consulta.Motivo,
                Antecedentes = consulta.Antecedentes,
                OD = consulta.OD,
                OI = consulta.OI,
                TipoL = consulta.TipoL,
                ADD_ = consulta.ADD_,
                DIP = consulta.DIP,
                Altura = consulta.Altura
            };

            // Agregar manualmente la consulta anterior a la lista ConsultasAnteriores
            consulta.ConsultasAnteriores.Add(consultaAnterior);

            // Actualizar la consulta en la base de datos
            SQLiteHelper.Instance.ActualizarConsulta(consulta);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al mover a Consulta_Anterior: {ex.Message}");
        }
    }


    void OnLimpiarClicked(object sender, EventArgs e)
    {
        LimpiarControlesEntrada();
    }
    private async void OnRegresarClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

}