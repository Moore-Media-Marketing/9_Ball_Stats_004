using System;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class MatchupResultData
	{
	public string teamA;  // Changed from TeamAName to teamA
	public string teamB;  // Changed from TeamBName to teamB
	public int TeamAScore;
	public int TeamBScore;
	public float teamAWinProbability;
	public float teamBWinProbability;
	public string WinningTeamName;

	// --- Region: Constructor --- //
	public MatchupResultData(string teamA, string teamB, int teamAScore, int teamBScore,
							  float teamAWinProbability, float teamBWinProbability, string winningTeamName)
		{
		this.teamA = teamA;
		this.teamB = teamB;
		this.TeamAScore = teamAScore;
		this.TeamBScore = teamBScore;
		this.teamAWinProbability = teamAWinProbability;
		this.teamBWinProbability = teamBWinProbability;
		this.WinningTeamName = winningTeamName;
		}

	// --- Region: Default Constructor --- //
	public MatchupResultData() { }

	// --- Region: CSV Serialization --- //
	public static void SaveMatchupResultsToCSV(List<MatchupResultData> matchupResults, string filePath)
		{
		try
			{
			using StreamWriter writer = new(filePath);
			// Write header
			writer.WriteLine("teamA,teamB,TeamAScore,TeamBScore,teamAWinProbability,teamBWinProbability,WinningTeamName");

			// Write data
			foreach (var result in matchupResults)
				{
				writer.WriteLine($"{result.teamA},{result.teamB},{result.TeamAScore},{result.TeamBScore}," +
								 $"{result.teamAWinProbability},{result.teamBWinProbability},{result.WinningTeamName}");
				}
			}
		catch (IOException ex)
			{
			Console.WriteLine($"Error saving matchup results: {ex.Message}");
			}
		}

	// --- Region: CSV Deserialization --- //
	public static List<MatchupResultData> LoadMatchupResultsFromCSV(string filePath)
		{
		List<MatchupResultData> matchupResults = new();

		try
			{
			if (File.Exists(filePath))
				{
				using StreamReader reader = new(filePath);
				// Skip header
				reader.ReadLine();

				// Read each line
				string line;
				while ((line = reader.ReadLine()) != null)
					{
					string[] columns = line.Split(',');

					if (columns.Length == 7)
						{
						string teamA = columns[0];
						string teamB = columns[1];
						int teamAScore = int.Parse(columns[2]);
						int teamBScore = int.Parse(columns[3]);
						float teamAWinProbability = float.Parse(columns[4]);
						float teamBWinProbability = float.Parse(columns[5]);
						string winningTeam = columns[6];

						MatchupResultData result = new(teamA, teamB, teamAScore, teamBScore, teamAWinProbability, teamBWinProbability, winningTeam);
						matchupResults.Add(result);
						}
					}
				}
			}
		catch (IOException ex)
			{
			Console.WriteLine($"Error loading matchup results: {ex.Message}");
			}

		return matchupResults;
		}
	}
