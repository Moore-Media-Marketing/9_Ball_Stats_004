using System;
using System.Collections.Generic;
using System.Linq;

using SQLite;

using UnityEngine;

[System.Serializable]
public class Team
	{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }  // Must be a property

	[NotNull]
	public string Name { get; set; }  // Must be a property

	[Ignore] // Prevents it from being stored in SQLite
	public List<Player> Players { get; private set; } = new();  // Now the team has a list of players

	public Team()
		{ }

	public Team(string name)
		{
		Name = name;
		}

	// --- Add Player to Team ---
	public void AddPlayer(Player player)
		{
		if (player.TeamId != 0)
			{
			Debug.LogWarning($"Player {player.Name} is already part of a team!");
			return;
			}

		if (Players.Any(p => string.Equals(p.Name, player.Name, StringComparison.OrdinalIgnoreCase)))
			{
			Debug.LogWarning($"Player {player.Name} already exists in team {Name}.");
			return;
			}

		// Link the player to this team
		player.TeamId = this.Id;
		Players.Add(player);  // Add player to the team's list

		DatabaseManager.Instance.SaveData();
		Debug.Log($"Player {player.Name} added to team {Name}.");
		}

	// --- Get Players from Team ---
	public List<Player> GetPlayers()
		{
		if (Players.Count == 0) // Only fetch if not already loaded
			{
			Players = DatabaseManager.Instance.GetPlayersByTeam(this.Id);
			}
		return Players;
		}
	}