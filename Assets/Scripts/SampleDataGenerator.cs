using System.Collections.Generic;

using UnityEngine;

// --- Region: Sample Data Generator --- //
public class SampleDataGenerator:MonoBehaviour
	{
	// --- Region: Generate Sample Teams --- //
	public List<Team> GenerateSampleTeams(int numberOfTeams)
		{
		List<Team> teams = new List<Team>();

		for (int i = 0; i < numberOfTeams; i++)
			{
			Team team = new Team("Team " + (i + 1));
			for (int j = 0; j < Random.Range(5, 9); j++)
				{
				Player player = GenerateRandomPlayer();
				if (team.AddPlayer(player)) // Only add player if the 23-Rule is respected
					{
					Debug.Log($"Added {player.PlayerName} to {team.TeamName}");
					}
				else
					{
					Debug.LogWarning($"Could not add {player.PlayerName} to {team.TeamName} due to 23-Rule");
					}
				}
			teams.Add(team);
			}

		return teams;
		}

	// --- Region: Generate Random Player --- //
	private Player GenerateRandomPlayer()
		{
		string playerName = "Player_" + Random.Range(1000, 9999); // Generate random player name
		int skillLevel = Random.Range(1, 10); // Random skill level between 1 and 9
		int totalGames = Random.Range(20, 100); // Random total games played
		int totalWins = Random.Range(10, totalGames); // Random total wins (cannot exceed total games)
		int totalPoints = Random.Range(50, 500); // Random total points scored

		return new Player(playerName, skillLevel, totalGames, totalWins, totalPoints); // Return new player
		}
	}
