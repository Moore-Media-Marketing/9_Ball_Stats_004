using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

[Serializable]
public class MatchupResultData
	{
	public string teamA;
	public string teamB;
	public float teamAWinProbability;
	public float teamBWinProbability;

	// --- Region: Constructor --- //
	public MatchupResultData(string teamA, string teamB, float teamAWinProbability, float teamBWinProbability)
		{
		this.teamA = teamA;
		this.teamB = teamB;
		this.teamAWinProbability = teamAWinProbability;
		this.teamBWinProbability = teamBWinProbability;
		}

	// --- Region: Default Constructor --- //
	public MatchupResultData() { }

	// --- Region: CSV Serialization --- //
	public static void SaveMatchupResultsToCSV(List<MatchupResultData> matchupResults, string filePath)
		{
		using StreamWriter writer = new(filePath);
		// Write header
		writer.WriteLine("TeamA,TeamB,TeamAWinProbability,TeamBWinProbability");

		// Write data
		foreach (var result in matchupResults)
			{
			writer.WriteLine($"{result.teamA},{result.teamB},{result.teamAWinProbability},{result.teamBWinProbability}");
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

				if (columns.Length == 4)
					{
					string teamA = columns[0];
					string teamB = columns[1];
					float teamAWinProbability = float.Parse(columns[2]);
					float teamBWinProbability = float.Parse(columns[3]);

					MatchupResultData result = new(teamA, teamB, teamAWinProbability, teamBWinProbability);
					matchupResults.Add(result);
					}
				}
			}
		return matchupResults;
		}
	}
