using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite; // For SQLite support

public class MatchupComparisonManager:MonoBehaviour
	{
	// --- Region: Player and Team Data --- //
	public class Player
		{
		public string name;
		public float lifetimeGamesWon;
		public float lifetimeGamesPlayed;
		public float lifetimeDefensiveShotAvg;
		public float lifetimeBreakAndRun;
		public float lifetimeMiniSlams;
		public float lifetimeShutouts;

		public float currentSeasonGamesWon;
		public float currentSeasonGamesPlayed;
		public float currentSeasonTotalPoints;
		public float currentSeasonPPM;
		public float currentSeasonSkillLevel;
		public float currentSeasonPointsAwarded;
		public int pointsRequiredToWin;
		}

	public class Team
		{
		public string teamName;
		public List<Player> players;
		}

	// --- End Region --- //

	// --- Region: UI Elements --- //
	// Reference to the MatchupResultsPanel for displaying results
	public MatchupResultsPanel matchupResultsPanel;
	// --- End Region --- //

	// --- Region: Team Selection --- //
	public Team selectedTeamA;
	public Team selectedTeamB;

	// Fetching selected teams from the database
	private Team GetSelectedTeamA()
		{
		return GetTeamFromDatabase("Team A");
		}

	private Team GetSelectedTeamB()
		{
		return GetTeamFromDatabase("Team B");
		}

	// --- End Region --- //

	// --- Region: Compare Matchups --- //
	public void CompareMatchups()
		{
		selectedTeamA = GetSelectedTeamA();
		selectedTeamB = GetSelectedTeamB();

		List<string> matchupResults = new();

		for (int i = 0; i < selectedTeamA.players.Count; i++)
			{
			for (int j = 0; j < selectedTeamB.players.Count; j++)
				{
				Player playerA = selectedTeamA.players[i];
				Player playerB = selectedTeamB.players[j];

				string result = ComparePlayers(playerA, playerB);
				matchupResults.Add(result);
				}
			}

		DisplayMatchupResults(matchupResults);
		}

	// --- End Region --- //

	// --- Region: Player Comparison --- //
	private string ComparePlayers(Player playerA, Player playerB)
		{
		float playerAScore = CalculatePlayerScore(playerA);
		float playerBScore = CalculatePlayerScore(playerB);

		if (playerAScore > playerBScore)
			return $"{playerA.name} wins vs {playerB.name}";
		else if (playerBScore > playerAScore)
			return $"{playerB.name} wins vs {playerA.name}";
		else
			return $"{playerA.name} vs {playerB.name} is a tie";
		}

	private float CalculatePlayerScore(Player player)
		{
		float lifetimeScore = (player.lifetimeGamesWon / player.lifetimeGamesPlayed) * 0.3f +
							  player.lifetimeBreakAndRun * 0.2f +
							  player.lifetimeMiniSlams * 0.2f +
							  player.lifetimeShutouts * 0.3f;

		float currentSeasonScore = (player.currentSeasonGamesWon / player.currentSeasonGamesPlayed) * 0.4f +
								   player.currentSeasonPPM * 0.3f +
								   player.currentSeasonSkillLevel * 0.3f;

		float pointsAdjustment = player.pointsRequiredToWin * 0.1f;

		return lifetimeScore + currentSeasonScore + pointsAdjustment;
		}

	// --- End Region --- //

	// --- Region: UI Update --- //
	private void DisplayMatchupResults(List<string> results)
		{
		matchupResultsPanel.UpdateMatchupResults("Matchup Results", selectedTeamA.teamName, selectedTeamB.teamName, string.Join("\n", results));
		}

	// --- End Region --- //

	// --- Region: Database Integration --- //
	private Team GetTeamFromDatabase(string teamName)
		{
		Team team = new Team { teamName = teamName, players = new List<Player>() };

		string connectionString = "URI=file:" + Application.persistentDataPath + "/gameDatabase.db"; // Path to your SQLite database

		using (IDbConnection connection = new SqliteConnection(connectionString))
			{
			connection.Open();

			// Fetch players for the selected team
			IDbCommand command = connection.CreateCommand();
			command.CommandText = $"SELECT * FROM Players WHERE teamName = '{teamName}'";
			IDataReader reader = command.ExecuteReader();

			while (reader.Read())
				{
				Player player = new Player
					{
					name = reader.GetString(reader.GetOrdinal("name")),
					lifetimeGamesWon = reader.GetFloat(reader.GetOrdinal("lifetimeGamesWon")),
					lifetimeGamesPlayed = reader.GetFloat(reader.GetOrdinal("lifetimeGamesPlayed")),
					lifetimeBreakAndRun = reader.GetFloat(reader.GetOrdinal("lifetimeBreakAndRun")),
					lifetimeMiniSlams = reader.GetFloat(reader.GetOrdinal("lifetimeMiniSlams")),
					lifetimeShutouts = reader.GetFloat(reader.GetOrdinal("lifetimeShutouts")),
					currentSeasonGamesWon = reader.GetFloat(reader.GetOrdinal("currentSeasonGamesWon")),
					currentSeasonGamesPlayed = reader.GetFloat(reader.GetOrdinal("currentSeasonGamesPlayed")),
					currentSeasonPPM = reader.GetFloat(reader.GetOrdinal("currentSeasonPPM")),
					currentSeasonSkillLevel = reader.GetFloat(reader.GetOrdinal("currentSeasonSkillLevel")),
					pointsRequiredToWin = reader.GetInt32(reader.GetOrdinal("pointsRequiredToWin"))
					};

				team.players.Add(player);
				}
			}

		return team;
		}

	// --- End Region --- //
	}
