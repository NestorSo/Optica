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
        private static string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AppOptica.db");
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

        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection($"Data Source={databasePath};Version=3;");
        }

        public bool AgregarPersona(Persona persona)
        {
            bool exito = true;

            using (SQLiteConnection connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Personas (PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, Direccion, Cedula) VALUES (@primerNombre, @segundoNombre, @primerApellido, @segundoApellido, @direccion, @cedula)";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@primerNombre", persona.PrimerNombre);
                cmd.Parameters.AddWithValue("@segundoNombre", persona.SegundoNombre);
                cmd.Parameters.AddWithValue("@primerApellido", persona.PrimerApellido);
                cmd.Parameters.AddWithValue("@segundoApellido", persona.SegundoApellido);
                cmd.Parameters.AddWithValue("@direccion", persona.Direccion);
                cmd.Parameters.AddWithValue("@cedula", persona.Cedula);

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

            using (SQLiteConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT Id, PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, Direccion, Cedula FROM Personas";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

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

        internal void InitializeDatabase()
        {
            using (SQLiteConnection connection = GetConnection())
            {
                connection.Open();
                string createTableQuery = "CREATE TABLE IF NOT EXISTS Personas (Id INTEGER PRIMARY KEY AUTOINCREMENT, PrimerNombre  VARCHAR, SegundoNombre VARCHAR, PrimerApellido VARCHAR, SegundoApellido VARCHAR, Direccion VARCHAR, Cedula VARCHAR)";
                SQLiteCommand cmd = new SQLiteCommand(createTableQuery, connection);
                cmd.ExecuteNonQuery();
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



