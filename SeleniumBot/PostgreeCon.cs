using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace SeleniumBot
{
    // Defines the PostgreeSQL connection using the Interface Disposable to free the connection automatically
    public class PostgreCon : IDisposable
    {
        public NpgsqlConnection Connection { get; set; }
        public PostgreCon()
        {
            Connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=ra;Database=typing_test_db");
            Connection.Open();
        }
        // Close the conection after use
        public void Dispose()
        {
            Connection.Dispose();
        }
    }

}
