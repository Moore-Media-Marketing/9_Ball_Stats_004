using MyGame.Database;  // Reference to your custom SQLite helper namespace
using UnityEngine;
using System.Collections.Generic;

public static class SQLiteHelper
	{
	// Use the correct database path here
	private static SQLiteConnection db = new SQLiteConnection("Database.db");

	static SQLiteHelper()
		{
		// Initialize SQLite database connection
		db.CreateTable<Team>();
		db.CreateTable<Player>();
		}

	public static void InsertTeam(Team team)
		{
		db.Insert(team);
		}

	public static void InsertPlayer(Player player)
		{
		db.Insert(player);
		}

	public static void DeletePlayer(Player player)
		{
		db.Delete(player);
		}

	public static List<Player> GetPlayersForTeam(int teamId)
		{
		return db.Table<Player>().Where(p => p.TeamId == teamId).ToList();
		}

	public static List<Team> GetAllTeams()
		{
		return db.Table<Team>().ToList();
		}

	public static Team GetTeamById(int teamId)
		{
		return db.Table<Team>().Where(t => t.TeamId == teamId).FirstOrDefault();
		}
	}
