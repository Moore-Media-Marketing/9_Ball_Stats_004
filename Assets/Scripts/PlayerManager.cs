using System.Collections.Generic;

using UnityEngine;

public class PlayerManager:MonoBehaviour
	{
	// --- PlayerManager Variables ---
	[Header("Player Management")]
	[Tooltip("List of all players")]
	public List<Player> players;

	// --- Add Player Method ---
	public void AddPlayer(string playerName, int skillLevel)
		{
		// Create a new player and add them to the players list
		Player newPlayer = new(playerName, skillLevel);
		players.Add(newPlayer);
		Debug.Log("Player added: " + playerName);
		}

	// --- Remove Player Method ---
	public void RemovePlayer(Player playerToRemove)
		{
		// Remove the player from the list
		if (players.Contains(playerToRemove))
			{
			players.Remove(playerToRemove);
			Debug.Log("Player removed: " + playerToRemove.PlayerName);
			}
		else
			{
			Debug.LogWarning("Player not found: " + playerToRemove.PlayerName);
			}
		}

	// --- Get Player by Name ---
	public Player GetPlayerByName(string playerName)
		{
		foreach (Player player in players)
			{
			if (player.PlayerName == playerName)
				{
				return player;
				}
			}
		return null; // Return null if no player with that name is found
		}

	// --- Display All Players ---
	public void DisplayAllPlayers()
		{
		Debug.Log("Displaying all players:");
		foreach (Player player in players)
			{
			Debug.Log(player.PlayerName + " (Skill Level: " + player.SkillLevel + ")");
			}
		}
	}