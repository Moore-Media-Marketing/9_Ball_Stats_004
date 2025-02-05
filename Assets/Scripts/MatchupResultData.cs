using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

[Serializable]
public class MatchupResultData
	{
	// Update to match the expected names
	public string TeamAName;
	public string TeamBName;
	public int TeamAScore;
	public int TeamBScore;
	public string WinningTeamName;

	public float teamAWinProbability;
	public float teamBWinProbability;

	// --- Region: Constructor --- //
	public MatchupResultData(string teamA, string teamB, int teamAScore, int teamBScore,
							  float teamAWinProbability, float teamBWinProbability, string winningTeam)
		{
		this.TeamAName = teamA;
		this.TeamBName = teamB;
		this.TeamAScore = teamAScore;
		this.TeamBScore = teamBScore;
		this.teamAWinProbability = teamAWinProbability;
		this.teamBWinProbability = teamBWinProbability;
		this.WinningTeamName = winningTeam;
		}

	// --- Region: Default Constructor --- //
	public MatchupResultData() { }

	// --- Region: CSV Serialization --- //
	public static void SaveMatchupResultsToCSV(List<MatchupResultData> matchupResults, string filePath)
		{
		using StreamWriter writer = new(filePath);
		// Write header
		writer.WriteLine("TeamAName,TeamBName,TeamAScore,TeamBScore,TeamAWinProbability,TeamBWinProbability,WinningTeamName");

		// Write data
		foreach (var result in matchupResults)
			{
			writer.WriteLine($"{result.TeamAName},{result.TeamBName},{result.TeamAScore},{result.TeamBScore}," +
							 $"{result.teamAWinProbability},{result.teamBWinProbability},{result.WinningTeamName}");
			}
		}

	// --- Region: CSV Deserialization --- //
	public static List<MatchupResultData> LoadMatchupResultsFromCSV(string filePath)
		{
		List<MatchupResultData> matchupResults = new();

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
		return matchupResults;
		}
	}
