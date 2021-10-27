using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Tron_Mario.Models
{
    public class DatabaseHandler
    {
        public readonly Dictionary<string, int> HighScoresSinglePlayer = new Dictionary<string, int>();
        public readonly Dictionary<string, int> HighScoresMultiPlayer = new Dictionary<string, int>();
        private readonly string connectionString =
            "SERVER=localhost;DATABASE=project_arcade_game;user=root;PASSWORD=;SSL Mode=None;";

        // public DatabaseHandler()
        // {
        //     GetHighScoresSinglePlayer();
        //     GetHighScoresMultiPlayer();
        //     // SetHighScore();
        // }
        
        /// <summary>
        /// Connects to database and retrives everything from the table singleplayer
        /// </summary>
        public void GetHighScoresSinglePlayer()
        {
            var connection = new MySqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            // MessageBox.Show("Connected to database succesfully");

            var query = "Select * FROM singleplayer ORDER BY score DESC, name LIMIT 5";
            var command = new MySqlCommand(query, connection);
            var dataReader = command.ExecuteReader();
            try {
                // var dataAdapter = new MySqlDataAdapter(command);
                // var dataTable = new DataTable("singleplayer");
                // dataAdapter.Fill(dataTable);
                // // DataGrid.ItemsSource = dataTable.DefaultView;
                // dataAdapter.Update(dataTable);
                while (dataReader.Read()) {
                    HighScoresSinglePlayer.Add((string)dataReader[1], (int)dataReader[2]);
                }
                connection.Close();

            }catch (Exception exception) {
                MessageBox.Show(exception.Message);
                connection.Close();
            }
        }
        
        /// <summary>
        /// Connects to database and retrives everything from the table multiplayer
        /// </summary>
        public void GetHighScoresMultiPlayer()
        {
            var connection = new MySqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            // MessageBox.Show("Connected to database succesfully");

            var query = "SELECT * FROM multiplayer ORDER BY score DESC, name_player_one,name_player_two DESC LIMIT 5;";
            var command = new MySqlCommand(query, connection);
            var dataReader = command.ExecuteReader();
            
            try {
                while (dataReader.Read()) {
                    HighScoresMultiPlayer.Add((string)dataReader[1] + " & " + (string)dataReader[2], (int)dataReader[3]);
                }
                connection.Close();

            }
            catch (Exception exception) {
                MessageBox.Show(exception.Message);
                connection.Close();
            }
        }
        
        // public void SetHighScore()
        // {
        //     MySqlConnection connection = new MySqlConnection();
        //     connection.ConnectionString = connectionString;
        //
        //     MySqlCommand command = new MySqlCommand();
        //     
        //     string query = "INSERT INTO `singleplayer` (`name`, `score`) VALUES ('Jorn', '69413');";
        //
        //     try {
        //         command.CommandText = query;
        //         command.CommandType = CommandType.Text;
        //         command.Connection = connection;
        //         connection.Open();
        //         command.ExecuteNonQuery();
        //         connection.Close();
        //         MessageBox.Show("Succesfully added");
        //     }
        //     catch (Exception exception) {
        //         MessageBox.Show(exception.Message);
        //         connection.Close();
        //     }
        // }

        public IEnumerable<KeyValuePair<string, int>> Select(Func<object, object> func)
        {
            throw new NotImplementedException();
        }
    }

}