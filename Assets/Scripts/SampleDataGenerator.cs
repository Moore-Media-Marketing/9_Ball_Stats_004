using UnityEngine;
using System.Collections.Generic;

public class SampleDataGenerator:MonoBehaviour
	{
	public int numberOfTeams = 10;
	public int numberOfPlayersPerTeam = 8;

	private static readonly string[] firstNamesMale = { "James", "John", "Robert", "Michael", "William" };
	private static readonly string[] firstNamesFemale = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth" };
	private static readonly string[] lastNames = { "Smith", "Johnson", "Williams", "Jones", "Brown" };
	private static readonly string[] teamNames = { "Sharks", "Tigers", "Wolves", "Eagles", "Lions" };

	private void Start()
		{
		GenerateSampleTeamsAndPlayers();
		}

	private void GenerateSampleTeamsAndPlayers()
		{
		List<Player> players = new List<Player>();
		int teamId = 1;
		int playerId = 1;

		foreach (string teamName in teamNames)
			{
			for (int j = 0; j < numberOfPlayersPerTeam; j++)
				{
				string firstName = (Random.Range(0, 2) == 0) ? firstNamesMale[Random.Range(0, firstNamesMale.Length)] : firstNamesFemale[Random.Range(0, firstNamesFemale.Length)];
				string lastName = lastNames[Random.Range(0, lastNames.Length)];
				string playerName = $"{firstName} {lastName}";
				int skillLevel = Random.Range(1, 10);

				int currentSeasonMatchesPlayed = Random.Range(1, 30);
				int currentSeasonMatchesWon = Random.Range(1, currentSeasonMatchesPlayed);
				float currentSeasonDefensiveShotAvg = Random.Range(0.0f, 1.0f);
				int lifetimeMatchesPlayedInLast2Years = Random.Range(10, 100);
				int lifetimeMatchesWon = Random.Range(5, lifetimeMatchesPlayedInLast2Years);
				int lifetimeMiniSlams = Random.Range(0, 5);

				Player player = new Player(
					playerId++, teamId, playerName, teamName, skillLevel,
					lifetimeMatchesPlayedInLast2Years, lifetimeMatchesWon, lifetimeMiniSlams,
					Random.Range(0, 5), currentSeasonMatchesPlayed, currentSeasonMatchesWon,
					currentSeasonDefensiveShotAvg, currentSeasonDefensiveShotAvg,
					lifetimeMatchesPlayedInLast2Years, currentSeasonMatchesWon,
					lifetimeMatchesPlayedInLast2Years, lifetimeMatchesPlayedInLast2Years,
					Random.Range(0, 10), Random.Range(0, 50),
					Random.Range(0, 100), Random.Range(0, 10)
				);

				players.Add(player);
				}
			teamId++;
			}

		DatabaseManager.Instance.SavePlayersToCsv(players);
		}
	}
