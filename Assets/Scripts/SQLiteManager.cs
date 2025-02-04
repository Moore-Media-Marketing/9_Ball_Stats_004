using SQLite;

using System;
using System.Collections.Generic;
using System.IO;

public static class SQLiteManager
	{
	private static string DatabasePath = Path.Combine(Application.persistentDataPath, "gameDatabase.db");
	private static SQLiteConnection connection;

	static SQLiteManager()
		{
		// Create database if it doesn't exist
		if (!File.Exists(DatabasePath))
			{
			connection = new SQLiteConnection(DatabasePath);
			connection.CreateTable<Team>();
			connection.CreateTable<Player>();
			}
		else
			{
			connection = new SQLiteConnection(DatabasePath);
			}
		}

	public static SQLiteConnection Connection => connection;
	}
