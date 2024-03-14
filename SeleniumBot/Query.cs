using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace SeleniumBot
{
    internal class Query
    {
        // Insert type data results into the table "WORD_RESULT" 
        public static void InsertData(NpgsqlConnection conn,List<object> listWords)
        {
            Console.WriteLine("Salvado resultado no banco de dados...");
            try
            {
                using (var query = new NpgsqlCommand("INSERT INTO \"WORD_RESULT\" (\"WPM\", \"KEYSTROKES\", \"ACCURACY\", \"CORRECT_WORD\", \"WRONG_WORD\") VALUES (@wpm, @keystrokes, @accuracy, @correct_word, @wrong_word);", conn))
                //using (var query = new NpgsqlCommand("INSERT INTO \"WORD_RESULT\" (\"WPM\", \"KEYSTROKES\", \"ACCURACY\", \"CORRECT_WORD\", \"WRONG_WORD\") VALUES (53, 278, 91.95, 52, 3);", conn))
                {
                    query.Parameters.AddWithValue("@wpm", listWords[0]);
                    query.Parameters.AddWithValue("@keystrokes", listWords[1]);
                    query.Parameters.AddWithValue("@accuracy", listWords[2]);
                    query.Parameters.AddWithValue("@correct_word", listWords[3]);
                    query.Parameters.AddWithValue("@wrong_word", listWords[4]);
                    query.ExecuteNonQuery();
                }
                Console.WriteLine("Resultados incluídos no banco");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Execução interrompida, falha ao executar a inserção devido a: " + ex.ToString());
                return;
            }
        }
        // Return All de data from the table WORD_RESULT, just for tests
        public static List<string[]> SelectDataAll(NpgsqlConnection conn)
        {
            string query = @"SELECT * FROM ""WORD_RESULT"";";
            List<string[]> results = [];

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string[] rowValues = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            rowValues[i] = reader[i].ToString();
                        }
                        results.Add(rowValues);
                    }
                }
            }

            return results;
        }
    }
}
