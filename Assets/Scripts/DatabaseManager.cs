using System;
using System.Collections.Generic;

using SQLite;

public class DatabaseManager
	{
	// --- Singleton Pattern --- //
	private static DatabaseManager _instance;
	public static DatabaseManager Instance
		{
		get
			{
			if (_instance == null)
				_instance = new DatabaseManager();
			return _instance;
			}
		}

	private SQLiteConnection connection;

	// --- Constructor --- //
	private DatabaseManager()
		{
		// Initialize the SQLite connection (assuming your DB file path is correct)
		connection = new SQLiteConnection("your_database_file_path.db");
		}

	// --- Save Teams to Database --- //
	public void SaveTeams(List<Team> teams)
		{
		foreach (var team in teams)
			{
			connection.InsertOrReplace(team);
			}
		}

	// --- Save Player to Database --- //
	public void SavePlayer(Player player)
		{
		connection.InsertOrReplace(player);
		}

	// --- Get Last Inserted Row ID --- //
	public int GetLastInsertRowId()
		{
		return connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
		}
	}
