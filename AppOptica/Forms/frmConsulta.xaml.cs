using AppOptica.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.Arm;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;

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
        QuestPDF.Settings.License = LicenseType.Community;
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

    void OnImprimirClicked(object sender, EventArgs e)
    {
        try
        {
            int fileIndex = 1;
            string fileName;
            string filePath;
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            do
            {
                // Generar un nombre dinámico para el archivo PDF
                fileName = $"Consulta {fileIndex}.pdf";
                filePath = Path.Combine(documentsPath, fileName);
                fileIndex++;


            } while (File.Exists(filePath));




            var data = Document.Create(document =>
            {
                
                

                document.Page(page =>
                {

                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Opticas XXX").Bold().FontSize(14);
                            col.Item().AlignCenter().Text("Jr. Las mercedes N378 - Lima").FontSize(9);
                            col.Item().AlignCenter().Text("987 987 123").FontSize(9);
                            col.Item().AlignCenter().Text("optica@gmail.com").FontSize(9);
                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Border(1).BorderColor("#257272")
                                .AlignCenter().Text("RUC 21312312312");

                            col.Item().Background("#257272").Border(1)
                                .BorderColor("#257272").AlignCenter()
                                .Text("Registro de Clientes").FontColor("#fff");

                            col.Item().Border(1).BorderColor("#257272")
                                .AlignCenter().Text($"B000{fileIndex - 1} - 234");
                        });
                    });

                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        col1.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            // Encabezado de la tabla
                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Border(1).AlignCenter().Height(50)
                                    .Padding(0).Text(" ").FontColor("#fff");

                                header.Cell().Background("#257272").Border(1).AlignCenter().Height(50)
                                    .Padding(0).Text("ADD").FontColor("#fff");

                                header.Cell().Background("#257272").Border(1).AlignCenter().Height(50)
                                    .Padding(0).Text("DIP").FontColor("#fff");

                                header.Cell().Background("#257272").Border(1).AlignCenter().Height(50)
                                    .Padding(0).Text("ALTURA").FontColor("#fff");
                            });

                            // Fila OD
                            tabla.Cell().Row(1).Column(0).Background("#257272").Border(1).AlignCenter().Height(50).Padding(3).Text("OD").FontColor("#fff");
                            tabla.Cell().Padding(0).Border(1).AlignCenter().Height(50).Text($"{AddOD_E.Text}").FontSize(10);
                            tabla.Cell().Padding(0).Border(1).AlignCenter().Height(50).Text($"{DipOD_E.Text}").FontSize(10);
                            tabla.Cell().Padding(0).Border(1).AlignCenter().Height(50).Text($"{AlturaOD_E.Text}").FontSize(10);

                            // Fila OS
                            tabla.Cell().Row(2).Column(0).Background("#257272").Border(1).AlignCenter().Height(50).Padding(3).Text("OS").FontColor("#fff");
                            tabla.Cell().Padding(0).Border(1).AlignCenter().Height(50).Text($"{AddOI_E.Text}").FontSize(10);
                            tabla.Cell().Padding(0).Border(1).AlignCenter().Height(50).Text($"{DipOI_E.Text}").FontSize(10);
                            tabla.Cell().Padding(0).Border(1).AlignCenter().Height(50).Text($"{AlturaOI_E.Text}").FontSize(10);
                        });
                        col1.Spacing(50);

                        // Agrega la sección de Antecedentes y Tipo de Lente
                        col1.Item().Text($"Antecedentes: {Antecedentes_E.Text}");

                        col1.Spacing(50);

                        col1.Item().Text($"Tipo de Lentes: {TipoL.Text}");
                    });



                    page.Footer()
                        .AlignRight()
                        .Text(txt =>
                        {
                            txt.Span("Pagina ").FontSize(10);
                            txt.CurrentPageNumber().FontSize(10);
                            txt.Span(" de ").FontSize(10);
                            txt.TotalPages().FontSize(10);
                        });


                });


            }).GeneratePdf();


            // Guardar el archivo PDF en el almacenamiento de documentos
            File.WriteAllBytes(filePath, data);

            // Mostrar mensaje de éxito
            DisplayAlert("Éxito", "El archivo PDF se ha guardado correctamente en Mis Documentos.", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al generar el PDF: {ex.Message}");
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
        string searchText = S_Client.Text;

        if (S_Client.Text != null)
        {
            try
            {

                // Llama al método BuscarClientesPorNombre de SQLiteHelper.Instance
                var resultados = SQLiteHelper.Instance.SearchConsul_Name(searchText);

                lvConsulta.ItemsSource = resultados;
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