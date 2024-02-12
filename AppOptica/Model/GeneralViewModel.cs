using AppOptica.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppOptica.Model
{
    public class GeneralViewModel : INotifyPropertyChanged
    {



        ObservableCollection<ConsultaGeneral> ConsultasGenerales;

        public GeneralViewModel(ObservableCollection<ConsultaGeneral> consultas)
        {
            this.ConsultasGenerales = consultas ?? throw new ArgumentNullException(nameof(consultas));
            // Inicializa la lista de consultas generales con los datos que necesites
            ObtenerGeneral();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void ObtenerGeneral()
        {
            try
            {
                // Aquí iría la lógica para obtener las consultas generales, similar a la que tienes en ObtenerGeneral() pero adaptada para ConsultaGeneral
                // Asumiendo que tienes un método en SQLiteHelper para obtener los datos
                var consultasGenerales = SQLiteHelper.Instance.ObtenerGeneral();

                // Limpiamos la lista actual y agregamos las nuevas consultas
                ConsultasGenerales.Clear();
                foreach (var consulta in consultasGenerales)
                {
                    ConsultasGenerales.Add(consulta);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener consultas generales: {ex.Message}");
            }
        }

        public ICommand BuscarClienteCommand => new Command(BuscarCliente);

        private string nombreClienteSeleccionado;

        public string NombreClienteSeleccionado
        {
            get => nombreClienteSeleccionado;
            set
            {
                if (nombreClienteSeleccionado != value)
                {
                    nombreClienteSeleccionado = value;
                    OnPropertyChanged(nameof(NombreClienteSeleccionado));
                }
            }
        }

        public void BuscarCliente()
        {

        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
