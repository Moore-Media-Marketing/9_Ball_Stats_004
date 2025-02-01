using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SQLite;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	#region Singleton
	public static DatabaseManager Instance { get; private set; }
	#endregion

	#region Inspector Fields

	[Header("Database Settings")]
	[Tooltip("Path to the SQLite database file.")]
	public string dbPath;

	[Header("Data (Read Only)")]
	[Tooltip("List of all teams in the database.")]
	public List<Team> allTeams = new List<Team>();

	[Tooltip("List of all players in the database.")]
	public List<Player> allPlayers = new List<Player>();

	#endregion

	#region Private Fields
	private SQLiteConnection dbConnection;
	#endregion

	#region Unity Methods

	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			}
		else
			{
			Destroy(gameObject);
			return;
			}

		dbPath = Path.Combine(Application.persistentDataPath, "game_data.db");
		dbConnection = new SQLiteConnection(dbPath);

		CreateTables();
		LoadData();
		EnsureDefaultTeamExists();
		}

	private void OnApplicationQuit()
		{
		dbConnection.Close();
		}

	#endregion

	#region Database Methods

	private void CreateTables()
		{
		dbConnection.CreateTable<Team>();
		dbConnection.CreateTable<Player>();
		dbConnection.CreateTable<Match>();
		Debug.Log("Database tables created or already exist.");
		}

	private void LoadData()
		{
		allTeams = dbConnection.Table<Team>().ToList();
		allPlayers = dbConnection.Table<Player>().ToList();
		Debug.Log($"Loaded {allTeams.Count} teams and {allPlayers.Count} players from the database.");
		}

	private void EnsureDefaultTeamExists()
		{
		if (allTeams.Count == 0)
			{
			Team defaultTeam = new Team("Default Team");
			AddTeam(defaultTeam);
			Debug.Log("No teams found. Default team added.");
			}
		}

	public void AddTeam(Team team)
		{
		if (string.IsNullOrWhiteSpace(team.Name))
			{
			Debug.LogError("Team name cannot be empty!");
			return;
			}
		dbConnection.Insert(team);
		Debug.Log($"Team '{team.Name}' added to the database.");
		LoadData();
		}

	public void AddPlayer(Player player)
		{
		if (string.IsNullOrWhiteSpace(player.Name))
			{
			Debug.LogError("Player name cannot be empty!");
			return;
			}
		if (player.TeamId <= 0 || dbConnection.Find<Team>(player.TeamId) == null)
			{
			Debug.LogError("Player must be assigned to a valid team.");
			return;
			}
		dbConnection.Insert(player);
		Debug.Log($"Player '{player.Name}' added to the database.");
		LoadData();
		}

	public void SavePlayer(Player player)
		{
		if (string.IsNullOrWhiteSpace(player.Name))
			{
			Debug.LogError("Player name cannot be empty!");
			return;
			}
		if (player.TeamId <= 0 || dbConnection.Find<Team>(player.TeamId) == null)
			{
			Debug.LogError("Player must be assigned to a valid team.");
			return;
			}
		if (dbConnection.Table<Player>().Any(p => p.Name == player.Name))
			{
			dbConnection.Update(player);
			Debug.Log($"Player '{player.Name}' updated.");
			}
		else
			{
			dbConnection.Insert(player);
			Debug.Log($"Player '{player.Name}' added to the database.");
			}
		LoadData();
		}

	public List<Team> GetAllTeams() => dbConnection.Table<Team>().ToList();

	public Team GetTeamById(int teamId)
		{
		var team = dbConnection.Find<Team>(teamId);
		if (team == null) Debug.LogError($"Team with ID {teamId} not found.");
		return team;
		}

	public Team GetTeamByName(string teamName)
		{
		var team = dbConnection.Table<Team>().FirstOrDefault(t => string.Equals(t.Name, teamName, StringComparison.OrdinalIgnoreCase));
		if (team == null) Debug.LogError($"Team '{teamName}' not found.");
		return team;
		}

	public List<Player> GetAllPlayers() => dbConnection.Table<Player>().ToList();

	public List<Player> GetPlayersByTeam(int teamId) => dbConnection.Table<Player>().Where(p => p.TeamId == teamId).ToList();

	public List<Match> GetAllMatches() => dbConnection.Table<Match>().ToList();

	public void AddMatchResult(Match match)
		{
		dbConnection.Insert(match);
		Debug.Log("Match result added to the database.");
		}

	public void UpdateTeam(Team team)
		{
		if (dbConnection.Table<Team>().Any(t => t.Id == team.Id))
			{
			dbConnection.Update(team);
			Debug.Log($"Team '{team.Name}' updated.");
			}
		else
			{
			Debug.LogError("Team not found for update.");
			}
		}

	public void RemoveTeam(Team team)
		{
		if (dbConnection.Table<Player>().Any(p => p.TeamId == team.Id))
			{
			Debug.LogError("Cannot remove a team with assigned players.");
			return;
			}
		dbConnection.Delete(team);
		Debug.Log($"Team '{team.Name}' removed.");
		LoadData();
		}

	#endregion

	#region Custom Methods for Inspector Display

	[ContextMenu("Refresh Data")]
	private void RefreshData()
		{
		LoadData();
		}

	[ContextMenu("Show Teams and Players")]
	private void ShowTeamsAndPlayers()
		{
		foreach (var team in allTeams)
			{
			Debug.Log($"Team: {team.Name}");
			var playersOnTeam = GetPlayersByTeam(team.Id);
			if (playersOnTeam.Count > 0)
				{
				foreach (var player in playersOnTeam)
					{
					Debug.Log($"    Player: {player.Name} | Season: {player.MatchesPlayed}/{player.MatchesWon}");
					Debug.Log($"    Lifetime: {player.LifetimeMatchesPlayed}/{player.LifetimeMatchesWon}");
					Debug.Log($"    Current Season: {player.CurrentSeasonMatchesPlayed}/{player.CurrentSeasonMatchesWon}");
					}
				}
			else
				{
				Debug.Log("    No players on this team.");
				}
			}
		}

	#endregion
	}
