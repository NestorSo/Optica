using Microsoft.Maui.Controls;

namespace AppOptica.Forms
{
    public partial class Principal : ContentPage
    {
        private bool isTimerRunning = false;
        public Principal()
        {
            InitializeComponent();
            StartTimer();
        }
        private void StartTimer()
        {
            if (!isTimerRunning)
            {
                // Establecer un temporizador que se ejecutará cada segundo
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    while (true)
                    {
                        // Actualizar el texto del Label con la hora actual
                        HoraLabel.Text = DateTime.Now.ToString("HH:mm:ss");

                        // Esperar 1 segundo
                        await Task.Delay(1000);
                    }
                });

                isTimerRunning = true;
            }
        }


        private async void OnIrAInicioClicked(object sender, EventArgs e)
        {
            // Abrir la página de Inicio y cerrar la página actual
            var inicioPage = new Inicio();
            await Navigation.PushModalAsync(inicioPage);
        }

        private async void OnIrAConsultaClicked(object sender, EventArgs e)
        {
            // Abrir la página de Consulta y cerrar la página actual
            var consultaPage = new frmConsulta();
            await Navigation.PushModalAsync(consultaPage);
        }

        private async void OnSalirClicked(object sender, EventArgs e)
        {
            // Mostrar un cuadro de diálogo para confirmar la salida
            bool confirmacion = await DisplayAlert("Salir", "¿Está seguro que desea salir?", "Sí", "No");

            if (confirmacion)
            {
                // Si el usuario hace clic en "Sí", cerrar la aplicación
                Application.Current.Quit();
            }
            // Si hace clic en "No", no se realiza ninguna acción
        }

    }
}
