using AppOptica.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

namespace AppOptica.Model
{
    public class SQLiteHelper
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["AtunConnection"].ConnectionString;
        private static SQLiteHelper _instance = null;

        public static SQLiteHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SQLiteHelper();
                }
                return _instance;
            }
        }

        public bool AgregarPersona(Persona persona)
        {
            bool exito = true;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Personas (PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, Direccion, Cedula) VALUES (@primerNombre, @segundoNombre, @primerApellido, @segundoApellido, @direccion, @cedula)";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.Add(new SQLiteParameter("@primerNombre", persona.PrimerNombre));
                cmd.Parameters.Add(new SQLiteParameter("@segundoNombre", persona.SegundoNombre));
                cmd.Parameters.Add(new SQLiteParameter("@primerApellido", persona.PrimerApellido));
                cmd.Parameters.Add(new SQLiteParameter("@segundoApellido", persona.SegundoApellido));
                cmd.Parameters.Add(new SQLiteParameter("@direccion", persona.Direccion));
                cmd.Parameters.Add(new SQLiteParameter("@cedula", persona.Cedula));

                cmd.CommandType = System.Data.CommandType.Text;

                if (cmd.ExecuteNonQuery() < 1)
                {
                    exito = false;
                }
            }

            return exito;
        }

        public List<Persona> ObtenerPersonas()
        {
            List<Persona> listaPersonas = new List<Persona>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, Direccion, Cedula FROM Personas";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaPersonas.Add(new Persona()
                        {
                            Id = int.Parse(reader["Id"].ToString()),
                            PrimerNombre = reader["PrimerNombre"].ToString(),
                            SegundoNombre = reader["SegundoNombre"].ToString(),
                            PrimerApellido = reader["PrimerApellido"].ToString(),
                            SegundoApellido = reader["SegundoApellido"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            Cedula = reader["Cedula"].ToString()
                        });
                    }
                }
            }

            return listaPersonas;
        }

        public List<Persona> CargarPersonasDesdeBaseDeDatos()
        {
            return ObtenerPersonas();
        }

        internal void InitializeDatabase()
        {
            using (var db = GetConnection())
            {
                // Verificar si la tabla existe
                var tableExists = db.GetTableInfo("Personas").Any();

                // Si la tabla no existe, créala
                if (!tableExists)
                {
                    db.CreateTable<Persona>();
                }
            }
        }
    }

    //public bool Edit(product obj)
    //    {
    //        bool ask = true;

    //        using (SQLiteConnection connec = new SQLiteConnection(connection))
    //        {
    //            connec.Open();
    //            string query = "UPDATE Product set ProductName = @productName, ProductType = @productType WHERE ProductID = @productID";

    //            SQLiteCommand cmd = new SQLiteCommand(query, connec);
    //            cmd.Parameters.Add(new SQLiteParameter("@productID", obj.ProductID));
    //            cmd.Parameters.Add(new SQLiteParameter("@productName", obj.ProductName));
    //            cmd.Parameters.Add(new SQLiteParameter("@productType", obj.ProductType));
    //            cmd.CommandType = System.Data.CommandType.Text;

    //            if (cmd.ExecuteNonQuery() < 1)
    //            {
    //                ask = false;
    //            }

    //        }

    //        return ask;
    //    }

    //    public bool Delete(product obj)
    //    {
    //        bool ask = true;

    //        using (SQLiteConnection connec = new SQLiteConnection(connection))
    //        {
    //            connec.Open();
    //            string query = "DELETE FROM Product WHERE ProductID = @productID";

    //            SQLiteCommand cmd = new SQLiteCommand(query, connec);
    //            cmd.Parameters.Add(new SQLiteParameter("@productID", obj.ProductID));
    //            cmd.CommandType = System.Data.CommandType.Text;

    //            if (cmd.ExecuteNonQuery() < 1)
    //            {
    //                ask = false;
    //            }

    //        }

    //        return ask;
    // }
}



