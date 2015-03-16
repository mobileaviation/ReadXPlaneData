using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace SQLiteTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SQLite Spatial DB test");

            SQLiteConnection.CreateFile("test.sqlite");
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=test.sqlite;Version=3;");
            m_dbConnection.Open();

            SQLiteCommand cmd = new SQLiteCommand(m_dbConnection);
            cmd.CommandText = "create table highscores (name varchar(20), score int)";
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand(m_dbConnection);
            cmd.CommandText = "SELECT load_extension('libspatialite-2.dll')";
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand(m_dbConnection);
            cmd.CommandText = "SELECT InitSpatialMetaData();";
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand(m_dbConnection);
            cmd.CommandText = "CREATE TABLE	test_geom (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL,measured_value DOUBLE NOT NULL);";
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand(m_dbConnection);
            cmd.CommandText = "SELECT AddGeometryColumn('test_geom', 'the_geom', 4326, 'POINT', 'XY');";
            cmd.ExecuteNonQuery();

            m_dbConnection.Close();
            Console.ReadKey();
        }
    }
}
