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

            //SQLiteConnection.CreateFile("test.sqlite");
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=db1.sqlite;Version=3;");
            m_dbConnection.Open();

            SQLiteCommand cmd;
<<<<<<< HEAD
            //cmd = new SQLiteCommand(m_dbConnection);
            //cmd.CommandText = "create table highscores (name varchar(20), score int)";
            //cmd.ExecuteNonQuery();
=======
>>>>>>> 5c849b4ceb7412cc24b068b908cf5d928088b3ea

            cmd = new SQLiteCommand(m_dbConnection);
            cmd.CommandText = "SELECT load_extension('libspatialite-2.dll')";
            cmd.ExecuteNonQuery();

            //cmd = new SQLiteCommand(m_dbConnection);
            //cmd.CommandText = "SELECT InitSpatialMetaData();";
            //cmd.ExecuteNonQuery();

            //cmd = new SQLiteCommand(m_dbConnection);
            //cmd.CommandText = "CREATE TABLE	test_geom (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL,measured_value DOUBLE NOT NULL);";
            //cmd.ExecuteNonQuery();

            //cmd = new SQLiteCommand(m_dbConnection);
            //cmd.CommandText = "SELECT AddGeometryColumn('test_geom', 'the_geom', 4326, 'POINT', 2);";
            //cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand(m_dbConnection);
            cmd.CommandText = "INSERT INTO test_geom(id, name, measured_value, the_geom) VALUES (NULL, 'first point', 1.23456, GeomFromText('POINT(1.01 2.02)', 4326));";
            cmd.ExecuteNonQuery();

            m_dbConnection.Close();
            Console.ReadKey();
        }
    }
}
