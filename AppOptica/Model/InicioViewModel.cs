using AppOptica.Forms;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;

namespace AppOptica.Model
{
    public class InicioViewModel
    {
        ObservableCollection<Persona> personas;

        public InicioViewModel(ObservableCollection<Persona> personas)
        {
            this.personas = personas ?? throw new ArgumentNullException(nameof(personas));
        }

        public void AgregarPersona(Persona persona)
        {
            try
            {
                // Llamas al método AgregarPersona de SQLiteHelper.Instance
                bool exito = SQLiteHelper.Instance.AgregarPersona(persona);

                if (exito)
                {
                    // Solo si la inserción en la base de datos fue exitosa, actualizas la lista con los datos recién insertados
                    personas.Add(persona);
                }
                else
                {
                    Debug.WriteLine("Error al agregar persona en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al agregar persona: {ex.Message}");
            }
        }

        public void CargarPersonasDesdeBaseDeDatos()
        {
            try
            {
                // Obtén las personas de la base de datos y agrégales a la lista
                var personasDesdeBD = SQLiteHelper.Instance.CargarPersonasDesdeBaseDeDatos();
                foreach (var persona in personasDesdeBD)
                {
                    personas.Add(persona);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar personas desde la base de datos: {ex.Message}");
            }
        }
    }
}
