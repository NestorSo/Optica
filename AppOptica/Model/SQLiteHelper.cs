﻿using AppOptica.Forms;
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

        public bool AgregarCliente(Cliente cliente)
        {
            bool exito = true;

            using (SQLiteConnection connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Clientes (FechaR, PNC, SNC, PAC, SAC, TelC, DirC, Ocupacion) VALUES (@fechaR, @pnc, @snc, @pac, @sac, @telC, @dirC, @ocupacion)";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@fechaR", cliente.FechaR);
                cmd.Parameters.AddWithValue("@pnc", cliente.PNC);
                cmd.Parameters.AddWithValue("@snc", cliente.SNC);
                cmd.Parameters.AddWithValue("@pac", cliente.PAC);
                cmd.Parameters.AddWithValue("@sac", cliente.SAC);
                cmd.Parameters.AddWithValue("@telC", cliente.TelC);
                cmd.Parameters.AddWithValue("@dirC", cliente.DirC);
                cmd.Parameters.AddWithValue("@ocupacion", cliente.Ocupacion);

                if (cmd.ExecuteNonQuery() < 1)
                {
                    exito = false;
                }
                connection.Close();
            }

            return exito;
        }


        public List<Cliente> ObtenerClientes()
        {
            List<Cliente> listaClientes = new List<Cliente>();

            using (SQLiteConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT Cliente_ID, FechaR, PNC, SNC, PAC, SAC, TelC, DirC, Ocupacion FROM Clientes";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaClientes.Add(new Cliente()
                        {
                            Cliente_ID = int.Parse(reader["Cliente_ID"].ToString()),
                            FechaR = DateTime.Parse(reader["FechaR"].ToString()),
                            PNC = reader["PNC"].ToString(),
                            SNC = reader["SNC"].ToString(),
                            PAC = reader["PAC"].ToString(),
                            SAC = reader["SAC"].ToString(),
                            TelC = reader["TelC"].ToString(),
                            DirC = reader["DirC"].ToString(),
                            Ocupacion = reader["Ocupacion"].ToString()
                        });
                    }
                    connection.Close();
                }
            }

            return listaClientes;
        }

        public bool ActualizarCliente(Cliente cliente)
        {
            bool exito = true;

            using (SQLiteConnection connection = GetConnection())
            {
                connection.Open();
                string query = "UPDATE Clientes SET FechaR = @fechaR, PNC = @pnc, SNC = @snc, PAC = @pac, SAC = @sac, TelC = @telC, DirC = @dirC, Ocupacion = @ocupacion WHERE Cliente_ID = @clienteID";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@fechaR", cliente.FechaR);
                cmd.Parameters.AddWithValue("@pnc", cliente.PNC);
                cmd.Parameters.AddWithValue("@snc", cliente.SNC);
                cmd.Parameters.AddWithValue("@pac", cliente.PAC);
                cmd.Parameters.AddWithValue("@sac", cliente.SAC);
                cmd.Parameters.AddWithValue("@telC", cliente.TelC);
                cmd.Parameters.AddWithValue("@dirC", cliente.DirC);
                cmd.Parameters.AddWithValue("@ocupacion", cliente.Ocupacion);
                cmd.Parameters.AddWithValue("@clienteID", cliente.Cliente_ID);

                if (cmd.ExecuteNonQuery() < 1)
                {
                    exito = false;
                }
            }

            return exito;
        }

       
        internal void InitializeDatabase()
        {
            using (SQLiteConnection connection = GetConnection())
            {
                connection.Open();

                // Crear la tabla Cliente
                string createTableClienteQuery = "CREATE TABLE IF NOT EXISTS Clientes (Cliente_ID INTEGER PRIMARY KEY AUTOINCREMENT, FechaR DATETIME, PNC VARCHAR(15) NOT NULL, SNC VARCHAR(15), PAC VARCHAR(15) NOT NULL, SAC VARCHAR(15), TelC VARCHAR(8), DirC VARCHAR(75) NOT NULL, Ocupacion VARCHAR(35) NOT NULL)";
                SQLiteCommand cmdCliente = new SQLiteCommand(createTableClienteQuery, connection);
                cmdCliente.ExecuteNonQuery();

                // Crear la tabla Consulta
                string createTableConsultaQuery = "CREATE TABLE IF NOT EXISTS Consulta (IdCon INTEGER PRIMARY KEY AUTOINCREMENT, FechaC DATETIME, Cliente_ID INT NOT NULL, Motivo TEXT NOT NULL, Antecedentes TEXT NOT NULL, OD FLOAT, OI FLOAT, TipoL NVARCHAR(25) NOT NULL, ADD_ FLOAT, DIP FLOAT, Altura FLOAT, FOREIGN KEY (Cliente_ID) REFERENCES Cliente(Cliente_ID))";
                SQLiteCommand cmdConsulta = new SQLiteCommand(createTableConsultaQuery, connection);
                cmdConsulta.ExecuteNonQuery();

                // Crear la tabla Consulta_Ant
                string createTableConsultaAntQuery = "CREATE TABLE IF NOT EXISTS Consulta_Ant (IdCon INTEGER PRIMARY KEY, FechaC DATETIME, Cliente_ID INT, Motivo TEXT NOT NULL, Antecedentes TEXT NOT NULL, OD FLOAT, OI FLOAT, TipoL NVARCHAR(25) NOT NULL, ADD_ FLOAT, DIP FLOAT, Altura FLOAT)";
                SQLiteCommand cmdConsultaAnt = new SQLiteCommand(createTableConsultaAntQuery, connection);
                cmdConsultaAnt.ExecuteNonQuery();

            }

        }
        public bool AgregarConsulta(Consulta consulta)
        {
            bool exito = true;

            using (SQLiteConnection connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Consulta (FechaC, Cliente_ID, Motivo, Antecedentes, OD, OI, TipoL, ADD_, DIP, Altura) " +
                               "VALUES (@fechaC, @clienteID, @motivo, @antecedentes, @od, @oi, @tipoL, @add, @dip, @altura)";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@fechaC", consulta.FechaC);
                cmd.Parameters.AddWithValue("@clienteID", consulta.Cliente_ID);
                cmd.Parameters.AddWithValue("@motivo", consulta.Motivo);
                cmd.Parameters.AddWithValue("@antecedentes", consulta.Antecedentes);
                cmd.Parameters.AddWithValue("@od", consulta.OD);
                cmd.Parameters.AddWithValue("@oi", consulta.OI);
                cmd.Parameters.AddWithValue("@tipoL", consulta.TipoL);
                cmd.Parameters.AddWithValue("@add", consulta.ADD_);
                cmd.Parameters.AddWithValue("@dip", consulta.DIP);
                cmd.Parameters.AddWithValue("@altura", consulta.Altura);

                if (cmd.ExecuteNonQuery() < 1)
                {
                    exito = false;
                }

                connection.Close();
            }

            return exito;
        }

        public List<Cliente> BuscarClientesPorNombre(string nombre)
        {
            List<Cliente> resultados = new List<Cliente>();

            using (SQLiteConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT Cliente_ID, FechaR, PNC, SNC, PAC, SAC, TelC, DirC, Ocupacion FROM Clientes WHERE PNC LIKE @nombre OR SNC LIKE @nombre OR PAC LIKE @nombre OR SAC LIKE @nombre";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@nombre", $"%{nombre}%");

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resultados.Add(new Cliente()
                        {
                            Cliente_ID = int.Parse(reader["Cliente_ID"].ToString()),
                            FechaR = DateTime.Parse(reader["FechaR"].ToString()),
                            PNC = reader["PNC"].ToString(),
                            SNC = reader["SNC"].ToString(),
                            PAC = reader["PAC"].ToString(),
                            SAC = reader["SAC"].ToString(),
                            TelC = reader["TelC"].ToString(),
                            DirC = reader["DirC"].ToString(),
                            Ocupacion = reader["Ocupacion"].ToString()
                        });
                    }
                    connection.Close();
                }
            }

            return resultados;
        }
    }

 

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



