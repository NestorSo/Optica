using AppOptica.Forms;
using AppOptica.Model;
using System.Collections.ObjectModel;
namespace AppOptica
{
    public partial class App : Application
    {
        InicioViewModel viewModel;

        public App()
        {
            InitializeComponent();

            MainPage = new Inicio();

            // Inicialización única de la base de datos
            SQLiteHelper.Instance.InitializeDatabase();

            // Crear instancia de InicioViewModel
            viewModel = new InicioViewModel(new ObservableCollection<Persona>());

            // Cargar personas desde la base de datos al inicio de la aplicación
            viewModel.CargarPersonasDesdeBaseDeDatos();
        }


        protected override void OnStart()
        {
            base.OnStart();

            // Inicialización única de la base de datos

            // Crear instancia de InicioViewModel
            viewModel = new InicioViewModel(new ObservableCollection<Persona>());

            // Cargar personas desde la base de datos al inicio de la aplicación
            viewModel.CargarPersonasDesdeBaseDeDatos();
        }
    }

}
