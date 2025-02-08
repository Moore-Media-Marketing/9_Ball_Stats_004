using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

// Main class for managing teams and players
public class PlayersAndTeamsManager:MonoBehaviour
	{
	public static PlayersAndTeamsManager Instance { get; private set; }

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
			}
		}

	// --- Get All Teams --- //
	public List<Team> GetAllTeams()
		{
		return DatabaseManager.Instance.GetAllTeams();
		}

	// --- Get All Players --- //
	public List<Player> GetAllPlayers()
		{
		return DatabaseManager.Instance.GetAllPlayers();
		}

	// --- Get Players by Team --- //
	public List<Player> GetPlayersByTeam(int teamId)
		{
		return DatabaseManager.Instance.GetAllPlayers().Where(p => p.TeamId == teamId).ToList();
		}

	// --- Add Team --- //
	public void AddTeam(string teamName)
		{
		DatabaseManager.Instance.AddTeam(teamName);
		}

	// --- Modify Team Name --- //
	public void ModifyTeam(int teamId, string newTeamName)
		{
		DatabaseManager.Instance.ModifyTeam(teamId, newTeamName);
		}

	// --- Delete Team (Moves Players to Unassigned) --- //
	public void DeleteTeam(int teamId)
		{
		DatabaseManager.Instance.DeleteTeam(teamId);
		}

	// --- Add Player --- //
	public void AddPlayer(string playerName, string teamName, int skillLevel)
		{
		DatabaseManager.Instance.AddPlayer(playerName, teamName, skillLevel);
		}

	// --- Delete Player --- //
	public void DeletePlayer(string playerName)
		{
		DatabaseManager.Instance.DeletePlayer(playerName);
		}

	// --- Assign Player to Another Team --- //
	public void AssignPlayerToTeam(string playerName, string newTeamName)
		{
		DatabaseManager.Instance.AssignPlayerToTeam(playerName, newTeamName);
		}

	// --- Update Player Stats --- //
	public void UpdatePlayerStats(string playerName, PlayerStats newStats)
		{
		DatabaseManager.Instance.UpdatePlayerStats(playerName, newStats);
		}
	}

// --- Team Class --- //
public class Team
	{
	public int TeamId { get; set; }
	public string TeamName { get; set; }

	public Team(int teamId, string teamName)
		{
		TeamId = teamId;
		TeamName = teamName;
		}
	}

// --- Player Class --- //
public class Player
	{
	public string PlayerName { get; set; }
	public string TeamName { get; set; }
	public int SkillLevel { get; set; }
	public int TeamId { get; set; }
	public PlayerStats Stats { get; set; }

	public Player(string playerName, string teamName, int skillLevel, PlayerStats stats, int teamId)
		{
		PlayerName = playerName;
		TeamName = teamName;
		SkillLevel = skillLevel;
		Stats = stats;
		TeamId = teamId;
		}
	}
