// --- Region: Matchup Result Data --- //
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

[System.Serializable]
public class MatchupResultData
	{
	public float teamAWins;  // Percentage of wins for Team A
	public float teamBWins;  // Percentage of wins for Team B

	// --- Constructor for initializing the data --- //
	public MatchupResultData(float teamAWins, float teamBWins)
		{
		this.teamAWins = teamAWins;
		this.teamBWins = teamBWins;
		}

	// --- Method to save the matchup result data to SQLite --- //
	public void SaveToSQLite(string connectionString)
		{
		using (var connection = new SqliteConnection(connectionString))
			{
			connection.Open();

			// Create table if it doesn't exist
			string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS MatchupResults (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    teamAWins FLOAT,
                    teamBWins FLOAT
                );";
			using (var command = new SqliteCommand(createTableQuery, connection))
				{
				command.ExecuteNonQuery();
				}

			// Insert data into the table
			string insertQuery = "INSERT INTO MatchupResults (teamAWins, teamBWins) VALUES (@teamAWins, @teamBWins)";
			using (var command = new SqliteCommand(insertQuery, connection))
				{
				command.Parameters.AddWithValue("@teamAWins", this.teamAWins);
				command.Parameters.AddWithValue("@teamBWins", this.teamBWins);
				command.ExecuteNonQuery();
				}

			Debug.Log("Matchup result data saved to SQLite.");
			}
		}

	// --- Method to load the latest matchup result data from SQLite --- //
	public static MatchupResultData LoadFromSQLite(string connectionString)
		{
		using (var connection = new SqliteConnection(connectionString))
			{
			connection.Open();

			string selectQuery = "SELECT teamAWins, teamBWins FROM MatchupResults ORDER BY id DESC LIMIT 1";
			using (var command = new SqliteCommand(selectQuery, connection))
			using (var reader = command.ExecuteReader())
				{
				if (reader.Read())
					{
					float teamAWins = reader.GetFloat(0);
					float teamBWins = reader.GetFloat(1);

					Debug.Log("Matchup result data loaded from SQLite.");
					return new MatchupResultData(teamAWins, teamBWins);
					}
				else
					{
					Debug.LogError("No matchup result data found.");
					return null;
					}
				}
			}
		}
	}
// --- End Region --- //
