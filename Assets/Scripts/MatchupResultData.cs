// --- Region: Matchup Result Data --- //
using UnityEngine;

[System.Serializable]
public class MatchupResultData
	{
	public float teamAWins;  // Percentage of wins for Team A
	public float teamBWins;  // Percentage of wins for Team B

	// --- Constructor for initializing the data --- //
	public MatchupResultData(float teamAWins, float teamBWins)
		{
		this.teamAWins = teamAWins;
		this.teamBWins = teamBWins;
		}

	// --- Method to save the matchup result data to a JSON file --- //
	public void SaveToJson(string filePath)
		{
		string json = JsonUtility.ToJson(this, true);  // Serialize to JSON
		System.IO.File.WriteAllText(filePath, json);   // Write the JSON to a file
		Debug.Log($"Matchup result data saved to {filePath}");
		}

	// --- Method to load the matchup result data from a JSON file --- //
	public static MatchupResultData LoadFromJson(string filePath)
		{
		if (System.IO.File.Exists(filePath))
			{
			string json = System.IO.File.ReadAllText(filePath);
			MatchupResultData resultData = JsonUtility.FromJson<MatchupResultData>(json);  // Deserialize from JSON
			Debug.Log($"Matchup result data loaded from {filePath}");
			return resultData;
			}
		else
			{
			Debug.LogError("File not found.");
			return null;
			}
		}
	}
// --- End Region --- //
