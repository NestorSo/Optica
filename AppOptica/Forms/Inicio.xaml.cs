using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using AppOptica.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices.JavaScript;
using Colors = QuestPDF.Helpers.Colors;

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

        // Configura la licencia de QuestPDF
        QuestPDF.Settings.License = LicenseType.Community;
    }

    void OnAgregarClicked(object sender, EventArgs e)
    {
        if (viewModel == null)
        {
            viewModel = new InicioViewModel(Clientes);
            ClientesListView.ItemsSource = Clientes;
        }


        string telefono = TelefonoEntry.Text;
        string _FName = PrimerNombreEntry.Text;
        string _SName = SegundoNombreEntry.Text;
        string _FLName = PrimerApellidoEntry.Text;
        string _SLName = SegundoApellidoEntry.Text;
        string _Dir = DireccionEntry.Text;
        string _Ocup = OcupacionEntry.Text;

        // Realiza la validación del número de teléfono solo al presionar el botón de agregar
        if (ValidatePhoneNumberAndNotEmpty(telefono) && _IsEmpty(_FName)
            && _IsEmpty(_FLName) && _IsEmpty(_Dir) && _IsEmpty(_Ocup))
        {
            var cliente = new Cliente
            {
                FechaR = DateTime.Now,
                PNC = _FName,
                SNC = _SName,
                PAC = _FLName,
                SAC = _SLName,
                TelC = telefono,
                DirC = _Dir,
                Ocupacion = _Ocup
            };

            viewModel.AgregarCliente(cliente);
            viewModel.CargarClientesDesdeBaseDeDatos();
            LimpiarEntry();

        }

        Clientes.Clear();

        // Limpiar los Entry después de la actualización
        LimpiarEntry();

        // Actualizar la lista de clientes en el ListView
        viewModel.CargarClientesDesdeBaseDeDatos();
        OnPropertyChanged(nameof(Clientes));

    }
    void OnActualizarClicked(object sender, EventArgs e)
    {
        //Verificar si se ha seleccionado un cliente en el ListView
        if (viewModel.ClienteSeleccionado == null)
        {
            DisplayAlert("Error", "Selecciona un cliente para actualizar", "OK");
            return;
        }

        // Obtener el Cliente_ID del cliente seleccionado
        int clienteId = viewModel.ClienteSeleccionado.Cliente_ID;

        // Crear una nueva instancia de Cliente con los datos actualizados
        Cliente clienteActualizado = new Cliente
        {
            Cliente_ID = clienteId, // Usar el Cliente_ID obtenido
            FechaR = DateTime.Now,
            PNC = PrimerNombreEntry.Text,
            SNC = SegundoNombreEntry.Text,
            PAC = PrimerApellidoEntry.Text,
            SAC = SegundoApellidoEntry.Text,
            TelC = TelefonoEntry.Text,
            DirC = DireccionEntry.Text,
            Ocupacion = OcupacionEntry.Text
        };
        Clientes.Clear();
        // Llamar al método para actualizar el cliente en el modelo de vista
        viewModel.ActualizarCliente(clienteActualizado);

        // Limpiar los Entry después de la actualización
        LimpiarEntry();

        // Actualizar la lista de clientes en el ListView
        viewModel.CargarClientesDesdeBaseDeDatos();
        OnPropertyChanged(nameof(Clientes));

    }


    private bool _IsEmpty(string a)
    {
        if (string.IsNullOrWhiteSpace(a))
        {
            DisplayAlert("Error", "No puede haber campos importantes vacios", "OK");
            return false;
        }

        return true;
    }

    void LimpiarEntry()
    {
        // Limpiar los Entry
        PrimerNombreEntry.Text = string.Empty;
        SegundoNombreEntry.Text = string.Empty;
        PrimerApellidoEntry.Text = string.Empty;
        SegundoApellidoEntry.Text = string.Empty;
        TelefonoEntry.Text = string.Empty;
        DireccionEntry.Text = string.Empty;
        OcupacionEntry.Text = string.Empty;
    }



    void OnEliminarClicked(object sender, EventArgs e)
    {

    }

    private async void OnRegresarClicked(object sender, EventArgs e)
    {
        
        await Navigation.PopModalAsync();
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
                fileName = $"Cliente{fileIndex}.pdf";
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
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            // Encabezado de la tabla
                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272")
                                    .Padding(3).Text("Nombre").FontColor("#fff");

                                header.Cell().Background("#257272")
                                    .Padding(3).Text("Telefono").FontColor("#fff");

                                header.Cell().Background("#257272")
                                    .Padding(3).Text("Dirección").FontColor("#fff");

                                header.Cell().Background("#257272")
                                    .Padding(3).Text("Ocupacion").FontColor("#fff");

                            });

                            // Filas de la tabla con datos de los clientes
                            foreach (var cliente in Clientes)
                            {

                                    tabla.Cell().Padding(3).Text($"{cliente.PNC} {cliente.SNC} {cliente.PAC} {cliente.SAC}").FontSize(10);
                                    tabla.Cell().Padding(3).Text($"{cliente.TelC}").FontSize(10);
                                    tabla.Cell().Padding(3).Text($"{cliente.DirC}").FontSize(10);
                                    tabla.Cell().Padding(3).Text($"{cliente.Ocupacion}").FontSize(10);
                            }
                        });

                        col1.Spacing(10);
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

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        // Obtener el cliente seleccionado del evento
        Cliente clienteSeleccionado = (Cliente)e.SelectedItem;

        // Asignar los valores del cliente a los Entry respectivos
        PrimerNombreEntry.Text = clienteSeleccionado.PNC;
        SegundoNombreEntry.Text = clienteSeleccionado.SNC;
        PrimerApellidoEntry.Text = clienteSeleccionado.PAC;
        SegundoApellidoEntry.Text = clienteSeleccionado.SAC;
        DireccionEntry.Text = clienteSeleccionado.DirC;
        TelefonoEntry.Text = clienteSeleccionado.TelC;
        OcupacionEntry.Text = clienteSeleccionado.Ocupacion;
        IDEntry.Text = clienteSeleccionado.Cliente_ID.ToString();

        // Actualizar la propiedad ClienteSeleccionado en el ViewModel
        viewModel.ClienteSeleccionado = clienteSeleccionado;

        // Desmarcar la selección del ListView
        ClientesListView.SelectedItem = null;
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

