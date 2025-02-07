using System.Collections.Generic;
using System.IO;
using System.Text;

using UnityEngine;

public class DatabaseManager:MonoBehaviour
	{
	// --- Singleton Implementation ---
	public static DatabaseManager Instance { get; private set; }

	private string playersCsvPath;
	private string teamsCsvPath;
	private string matchupsCsvPath;

	private void Awake()
		{
		if (Instance != null && Instance != this)
			{
			Destroy(gameObject);
			return;
			}
		Instance = this;
		DontDestroyOnLoad(gameObject);

		// Use persistentDataPath for cross-platform compatibility
		playersCsvPath = Path.Combine(Application.persistentDataPath, "players.csv");
		teamsCsvPath = Path.Combine(Application.persistentDataPath, "teams.csv");
		matchupsCsvPath = Path.Combine(Application.persistentDataPath, "matchups.csv");
		}

	// --- Load Players from CSV ---
	public List<Player> LoadPlayersFromCsv(int teamId)
		{
		List<Player> players = new();

		if (!File.Exists(playersCsvPath))
			{
			Debug.LogWarning($"Players CSV file not found at {playersCsvPath}. Creating new file.");
			SavePlayersToCsv(players);
			return players;
			}

		try
			{
			using StreamReader reader = new(playersCsvPath, System.Text.Encoding.UTF8);
			string line;
			while ((line = reader.ReadLine()) != null)
				{
				line = line.Trim();
				if (string.IsNullOrEmpty(line))
					continue;

				string[] values = line.Split(',');
				if (values.Length < 26) // Expecting at least 26 columns for our 21 parameters (depending on header)
					{
					Debug.LogWarning($"Skipping line with insufficient data: {line}");
					continue;
					}

				// Parse values – mapping (indices based on our CSV order):
				// 0: playerId (not used in constructor)
				// 1: PlayerName
				// 2: SkillLevel
				// 3: TeamId (as int)
				// 4: defensiveShotAvg -> currentSeasonDefensiveShotAverage
				// 5: CurrentSeasonMatchesPlayed
				// 6: CurrentSeasonMatchesWon
				// 7: CurrentSeasonBreakAndRun
				// 8: TeamName
				// 9: CurrentSeasonMiniSlams (not used in constructor)
				// 10: CurrentSeasonNineOnTheSnap (not used in constructor)
				// 11: CurrentSeasonShutouts
				// 12: CurrentSeasonPointsAwarded
				// 13: CurrentSeasonPointsPerMatch
				// 14: LifetimeMatchesPlayed (for later use)
				// 15: LifetimeDefensiveShotAverage (for later use)
				// 16: LifetimeGamesPlayed
				// 17: LifetimeGamesWon
				// 18: LifetimeMatchesWon
				// 19: LifetimeMiniSlams
				// 20: LifetimeNineOnTheSnap
				// 21: LifetimeBreakAndRun
				// 22: LifetimeShutouts
				// 23: CurrentSeasonSkillLevel
				// 24: CurrentSeasonTotalPoints
				// 25: LifetimeMatchesPlayedInLast2Years

				// We will map parameters to the Player constructor as follows:
				// 1. teamId: parsed from values[3]
				// 2. teamName: values[8]
				// 3. playerName: values[1]
				// 4. skillLevel: values[2]
				// 5. currentSeasonMatchesPlayed: values[5]
				// 6. currentSeasonMatchesWon: values[6]
				// 7. currentSeasonPointsAwarded: values[12]
				// 8. currentSeasonPointsPerMatch: values[13]
				// 9. currentSeasonBreakAndRun: values[7]
				// 10. currentSeasonDefensiveShotAverage: values[4] (parsed as float)
				// 11. currentSeasonShutouts: values[11]
				// 12. lifetimeGamesPlayed: values[16]
				// 13. lifetimeGamesWon: values[17]
				// 14. lifetimeMiniSlams: values[19]
				// 15. lifetimeNineOnTheSnap: values[20]
				// 16. lifetimeShutouts: values[22]
				// 17. lifetimeMatchesPlayedInLast2Years: values[25]
				// 18. lifetimeMatchesPlayed: values[14]
				// 19. lifetimeMatchesWon: values[18]
				// 20. lifetimeBreakAndRun: values[21]
				// 21. lifetimeDefensiveShotAverage: values[15]

				if (
					int.TryParse(values[3], out int teamIdCsv) &&
					int.TryParse(values[2], out int skillLevel) &&
					float.TryParse(values[4], out float currDefShotAvg) &&
					int.TryParse(values[5], out int currentSeasonMatchesPlayed) &&
					int.TryParse(values[6], out int currentSeasonMatchesWon) &&
					int.TryParse(values[12], out int currentSeasonPointsAwarded) &&
					float.TryParse(values[13], out float currentSeasonPointsPerMatch) &&
					int.TryParse(values[7], out int currentSeasonBreakAndRun) &&
					int.TryParse(values[11], out int currentSeasonShutouts) &&
					int.TryParse(values[16], out int lifetimeGamesPlayed) &&
					int.TryParse(values[17], out int lifetimeGamesWon) &&
					int.TryParse(values[19], out int lifetimeMiniSlams) &&
					int.TryParse(values[20], out int lifetimeNineOnTheSnap) &&
					int.TryParse(values[22], out int lifetimeShutouts) &&
					int.TryParse(values[25], out int lifetimeMatchesPlayedInLast2Years) &&
					int.TryParse(values[14], out int lifetimeMatchesPlayed) &&
					int.TryParse(values[18], out int lifetimeMatchesWon) &&
					int.TryParse(values[21], out int lifetimeBreakAndRun) &&
					float.TryParse(values[15], out float lifetimeDefensiveShotAverage)
					)
					{
					Player player = new(
						teamIdCsv,                       // teamId
						values[8],                       // teamName
						values[1],                       // playerName
						skillLevel,                      // skillLevel
						currentSeasonMatchesPlayed,      // currentSeasonMatchesPlayed
						currentSeasonMatchesWon,         // currentSeasonMatchesWon
						currentSeasonPointsAwarded,      // currentSeasonPointsAwarded
						currentSeasonPointsPerMatch,     // currentSeasonPointsPerMatch
						currentSeasonBreakAndRun,        // currentSeasonBreakAndRun
						currDefShotAvg,                  // currentSeasonDefensiveShotAverage
						currentSeasonShutouts,           // currentSeasonShutouts
						lifetimeGamesPlayed,             // lifetimeGamesPlayed
						lifetimeGamesWon,                // lifetimeGamesWon
						lifetimeMiniSlams,               // lifetimeMiniSlams
						lifetimeNineOnTheSnap,           // lifetimeNineOnTheSnap
						lifetimeShutouts,                // lifetimeShutouts
						lifetimeMatchesPlayedInLast2Years, // lifetimeMatchesPlayedInLast2Years
						lifetimeMatchesPlayed,           // lifetimeMatchesPlayed
						lifetimeMatchesWon,              // lifetimeMatchesWon
						lifetimeBreakAndRun,             // lifetimeBreakAndRun
						lifetimeDefensiveShotAverage     // lifetimeDefensiveShotAverage
					);

					if (player.TeamId == teamId) // Filter by teamId
						{
						players.Add(player);
						}
					}
				else
					{
					Debug.LogWarning($"Invalid data in CSV for player: {line}");
					}
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading players from CSV: {ex.Message}");
			}

		return players;
		}

	// --- Save Players to CSV using StringBuilder ---
	public void SavePlayersToCsv(List<Player> players)
		{
		try
			{
			StringBuilder csvBuilder = new();
			foreach (Player player in players)
				{
				csvBuilder.AppendLine(player.ToCsv());
				}
			File.WriteAllText(playersCsvPath, csvBuilder.ToString(), System.Text.Encoding.UTF8);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving players to CSV: {ex.Message}");
			}
		}

	// --- Load Teams from CSV ---
	public List<Team> LoadTeams()
		{
		List<Team> teams = new();
		try
			{
			if (File.Exists(teamsCsvPath))
				{
				using StreamReader reader = new(teamsCsvPath, System.Text.Encoding.UTF8);
				string line;
				while ((line = reader.ReadLine()) != null)
					{
					line = line.Trim();
					if (!string.IsNullOrEmpty(line))
						{
						Team team = Team.FromCsv(line);
						teams.Add(team);
						}
					}
				}
			else
				{
				Debug.LogWarning($"Teams CSV file not found at {teamsCsvPath}.");
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading teams from CSV: {ex.Message}");
			}
		return teams;
		}

	// --- Save Teams to CSV ---
	public void SaveTeams(List<Team> teams)
		{
		try
			{
			StringBuilder csvBuilder = new();
			foreach (Team team in teams)
				{
				csvBuilder.AppendLine(team.ToCsv());
				}
			File.WriteAllText(teamsCsvPath, csvBuilder.ToString(), System.Text.Encoding.UTF8);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving teams to CSV: {ex.Message}");
			}
		}

	// --- Load Matchups from CSV ---
	public List<Match> LoadMatchupsFromCsv()
		{
		List<Match> matchups = new();
		try
			{
			if (File.Exists(matchupsCsvPath))
				{
				using StreamReader reader = new(matchupsCsvPath, System.Text.Encoding.UTF8);
				string line;
				while ((line = reader.ReadLine()) != null)
					{
					string[] values = line.Trim().Split(',');
					if (values.Length == 6) // Expected format: MatchId, Player1Id, Player2Id, Player1Score, Player2Score, WinnerId
						{
						if (int.TryParse(values[0], out int matchId) &&
							int.TryParse(values[1], out int player1Id) &&
							int.TryParse(values[2], out int player2Id) &&
							int.TryParse(values[3], out int player1Score) &&
							int.TryParse(values[4], out int player2Score) &&
							int.TryParse(values[5], out int winnerId))
							{
							Match match = new(matchId, player1Id, player2Id, player1Score, player2Score, winnerId);
							matchups.Add(match);
							}
						else
							{
							Debug.LogWarning($"Invalid data in CSV for match with ID: {values[0]}");
							}
						}
					else
						{
						Debug.LogWarning($"Skipping line with insufficient data: {line}");
						}
					}
				}
			else
				{
				Debug.LogWarning($"Matchups CSV file not found at {matchupsCsvPath}.");
				}
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error loading matchups from CSV: {ex.Message}");
			}
		return matchups;
		}

	// --- Save Matchups to CSV ---
	public void SaveMatchupsToCsv(List<Match> matchups)
		{
		try
			{
			StringBuilder csvBuilder = new();
			foreach (Match match in matchups)
				{
				csvBuilder.AppendLine($"{match.MatchId},{match.Player1Id},{match.Player2Id},{match.Player1Score},{match.Player2Score},{match.WinnerId}");
				}
			File.WriteAllText(matchupsCsvPath, csvBuilder.ToString(), System.Text.Encoding.UTF8);
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving matchups to CSV: {ex.Message}");
			}
		}
	}
