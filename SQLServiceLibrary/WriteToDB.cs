﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServiceLibrary
{
    public class WriteToDB
    {
        /// <summary>
        /// METODO SCRITTURA DB (Si connette al DB e inserisce in tabella i dati)
        /// </summary>
        /// <param name="message"></param>
        public static void WriteToDatabase(string message)
        {
            //crea il percorso prendendo il dominio e la cd (cambiare i valori)
            string connectionString = "Server=localhost;Database=prova_sql;Trusted_Connection=True;";

            //query per inserire i dati nella tabella
            string query = "INSERT INTO service_logs (Message, LoggedAt) VALUES (@Message, @LoggedAt)";

            //fai la connessione a SQL con la stringa data e fai al query
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //apre la connessione
                    connection.Open();
                    Console.WriteLine("Connessione riuscita");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //aggiungi parametri
                        command.Parameters.AddWithValue("@Message", message);
                        command.Parameters.AddWithValue("@LoggedAt", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Eventuale gestione dell'errore (ad esempio log nel file in caso di errore)
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\ErrorLog.txt",
                    $"Errore: {ex.Message} - {DateTime.Now}\n");
                Console.WriteLine("Errore di connessione " + ex.Message);
            }
        }
    }

    public class DeleteFromDB
    {
        /// <summary>
        /// METODO SCRITTURA DB (Si connette al DB e inserisce in tabella i dati)
        /// </summary>
        /// <param name="message"></param>
        public static void DeleteFromDatabase()
        {
            //crea il percorso prendendo il dominio e la cd (cambiare i valori)
            string connectionString = "Server=localhost;Database=prova_sql;Trusted_Connection=True;";

            //query per inserire i dati nella tabella
            string query = "DELETE FROM service_logs WHERE Id IN (SELECT TOP 5 Id FROM service_logs ORDER BY Id)";

            //fai la connessione a SQL con la stringa data e fai al query
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //apre la connessione
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery(); // Esegue la query di eliminazione
                        Console.WriteLine($"{rowsAffected} righe eliminate con successo.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Eventuale gestione dell'errore
                string errorPath = AppDomain.CurrentDomain.BaseDirectory + @"\ErrorLog.txt";
                File.AppendAllText(errorPath, $"Errore: {ex.Message} - {DateTime.Now}\n");
                Console.WriteLine($"Errore di connessione: {ex.Message}");
            }
        }
    }
}
