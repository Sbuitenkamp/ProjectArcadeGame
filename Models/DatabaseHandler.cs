using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Tron_Mario.Models
{
    public class DatabaseHandler
    {
        private const string ConnectionString = "SERVER=localhost;DATABASE=project_arcade_game;user=root;PASSWORD=;SSL Mode=None;";

        public readonly List<KeyValuePair<string, int>> HighScoresSinglePlayer = new List<KeyValuePair<string, int>>();
        public readonly List<KeyValuePair<string, int>> HighScoresMultiPlayer = new List<KeyValuePair<string, int>>();

        /// <summary>
        /// Connects to database and retrives everything from the table singleplayer
        /// </summary>
        public void GetHighScoresSinglePlayer()
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();
            // MessageBox.Show("Connected to database succesfully");

            string query = "Select * FROM singleplayer ORDER BY score DESC, name LIMIT 5";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = command.ExecuteReader();
            
            try {
                while (dataReader.Read()) HighScoresSinglePlayer.Add(new KeyValuePair<string, int>((string)dataReader[1], (int)dataReader[2]));
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
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();
            // MessageBox.Show("Connected to database succesfully");

            string query = "SELECT * FROM multiplayer ORDER BY score DESC, name_player_one,name_player_two DESC LIMIT 5;";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = command.ExecuteReader();
            
            try {
                while (dataReader.Read()) {
                    HighScoresMultiPlayer.Add(new KeyValuePair<string, int>(
                        (string) dataReader[1] + " & " + (string) dataReader[2], (int) dataReader[3]));
                }
                connection.Close();

            }
            catch (Exception exception) {
                MessageBox.Show(exception.Message);
                connection.Close();
            }
        }
        
        /// <summary>
        /// Connects to database and adds highscore in database
        /// </summary>
        public void SetHighScoreSinglePlayer(object name, object score)
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConnectionString;
        
            MySqlCommand command = new MySqlCommand();
            
            string query = "INSERT INTO `singleplayer` (`name`, `score`) VALUES ('"+name+"','"+score+"');";
        
            try {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Succesfully added to database");
            }
            catch (Exception exception) {
                MessageBox.Show(exception.Message);
                connection.Close();
            }
        }
        
        public void SetHighScoreMultiPlayer(object namePlayerOne, object namePlayerTwo, object score)
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConnectionString;
        
            MySqlCommand command = new MySqlCommand();
            
            string query = "INSERT INTO `multiplayer` (`name_player_one`, `name_player_two`, `score`) VALUES ('" +namePlayerOne+"','"+namePlayerTwo+"', '"+score+"');";
            try {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Succesfully added to database");
            }
            catch (Exception exception) {
                MessageBox.Show(exception.Message);
                connection.Close();
            }
        }
    }

}