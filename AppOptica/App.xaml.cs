using AppOptica.Forms;
using AppOptica.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AppOptica
{
    public partial class App : Application
    {
        InicioViewModel viewModel;
        ConsultaViewModel consultaViewModel;
        GeneralViewModel generalViewModel;
        public App()
        {
            InitializeComponent();

            MainPage = new Principal();
           
            // Inicialización única de la base de datos
            SQLiteHelper.Instance.InitializeDatabase();

            // Crear instancia de InicioViewModel
            viewModel = new InicioViewModel(new ObservableCollection<Cliente>());
            consultaViewModel = new ConsultaViewModel(new ObservableCollection<Consulta>());
            generalViewModel = new GeneralViewModel(new ObservableCollection<ConsultaGeneral>());

        }

        protected override void OnStart()
        {
            base.OnStart();

            // Inicialización única de la base de datos

            // Crear instancia de InicioViewModel
            viewModel = new InicioViewModel(new ObservableCollection<Cliente>());
            consultaViewModel = new ConsultaViewModel(new ObservableCollection<Consulta>());
            generalViewModel = new GeneralViewModel(new ObservableCollection<ConsultaGeneral>());

            // Cargar clientes desde la base de datos al inicio de la aplicación
            viewModel.CargarClientesDesdeBaseDeDatos();
            consultaViewModel.CargarConsultas();
            generalViewModel.ObtenerGeneral();
        }
    }
}