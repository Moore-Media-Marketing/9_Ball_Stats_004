using System;
using System.Collections.Generic;

using SQLite;

using UnityEngine;

[Serializable]
public class Team
	{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	[Tooltip("The name of the team.")]
	public string Name { get; set; }

	public Team() { }

	public Team(string name)
		{
		Name = name;
		}

	// --- Add Player to Team ---
	public void AddPlayer(Player player)
		{
		player.TeamId = this.Id;
		DatabaseManager.Instance.SavePlayer(player);
		}

	// --- Get Players from Team ---
	public List<Player> GetPlayers() => DatabaseManager.Instance.GetPlayersByTeam(this.Id);
	}
