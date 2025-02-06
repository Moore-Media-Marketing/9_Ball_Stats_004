using System.Collections.Generic;
using System.IO;

using TMPro;

using UnityEngine;

public class MatchupResultsPanel:MonoBehaviour
	{
	// --- Region: UI References --- //
	public TMP_Text headerText;
	public TMP_Text teamAHeaderText;
	public TMP_Text teamBHeaderText;

	// --- Matchup Results Section --- //
	public TMP_Text matchupResultText;
	public TMP_Text teamAWinProbabilityText;
	public TMP_Text teamBWinProbabilityText;

	// --- Region: ScrollView References --- //
	public Transform matchupListContent;
	public Transform bestMatchupListContent;

	// --- Region: Back Button --- //
	public TMP_Text backButtonText;

	// --- Region: Dependencies --- //
	public ComparisonManager comparisonManager;
	public PlayerWeightSettings weightSettings;

	private string matchupFilePath;  // Path to the CSV file where matchup results are stored

	// --- Region: Initialize --- //
	private void Start()
		{
		matchupFilePath = Path.Combine(Application.persistentDataPath, "matchups.csv");

		// Ensure the CSV file exists (create it if it doesn't)
		if (!File.Exists(matchupFilePath))
			{
			File.WriteAllText(matchupFilePath, "Id,Result,TeamAWinProbability,TeamBWinProbability\n");  // Add headers if file is new
			}

		// Initialize the Back Button text
		UpdateBackButtonText();
		}

	// --- Region: Simulate and Display Matchup --- //
	public void SimulateAndDisplayMatchup(Player player1, Player player2)
		{
		if (player1 == null || player2 == null)
			{
			Debug.LogError("MatchupResultsPanel: One or both players are null!");
			return;
			}

		// Simulate matchup using ComparisonManager
		Player winner = comparisonManager.SimulateMatchup(player1, player2);

		// Calculate probability based on overall score
		float score1 = player1.CalculateOverallScore(weightSettings);
		float score2 = player2.CalculateOverallScore(weightSettings);
		float totalScore = score1 + score2;
		float teamAWinProbability = (totalScore > 0) ? (score1 / totalScore) * 100f : 50f;
		float teamBWinProbability = (totalScore > 0) ? (score2 / totalScore) * 100f : 50f;

		// Update UI elements
		DisplayMatchupResults(winner?.PlayerName ?? "Tie", teamAWinProbability, teamBWinProbability);

		// Save the results
		SaveMatchupResult(winner?.PlayerName ?? "Tie", teamAWinProbability, teamBWinProbability);
		}

	// --- Region: Display Matchup Results --- //
	public void DisplayMatchupResults(string result, float teamAWinProbability, float teamBWinProbability)
		{
		// Update the header text for teams and matchup
		headerText.text = "Matchup Results";
		teamAHeaderText.text = "Team A";
		teamBHeaderText.text = "Team B";

		// Display the matchup result and probabilities
		matchupResultText.text = $"Winner: {result}";
		teamAWinProbabilityText.text = $"Team A Win Probability: {teamAWinProbability:0.00}%";
		teamBWinProbabilityText.text = $"Team B Win Probability: {teamBWinProbability:0.00}%";
		}

	// --- Region: Save Matchup Results to CSV --- //
	public void SaveMatchupResult(string result, float teamAWinProbability, float teamBWinProbability)
		{
		try
			{
			int newId = GetNextMatchupId();
			var line = $"{newId},{result},{teamAWinProbability},{teamBWinProbability}";
			File.AppendAllLines(matchupFilePath, new[] { line });  // Append new matchup to the CSV
			Debug.Log("Matchup result saved to CSV.");
			}
		catch (System.Exception ex)
			{
			Debug.LogError($"Error saving matchup to CSV: {ex.Message}");
			}
		}

	// --- Region: Get Next Matchup ID --- //
	private int GetNextMatchupId()
		{
		int nextId = 1;

		if (File.Exists(matchupFilePath))
			{
			var lines = File.ReadAllLines(matchupFilePath);
			if (lines.Length > 1)  // If there are already entries (skipping header)
				{
				var lastLine = lines[^1];
				var lastId = int.Parse(lastLine.Split(',')[0]);  // Extract the ID from the last line
				nextId = lastId + 1;
				}
			}

		return nextId;
		}

	// --- Region: Update Back Button Text --- //
	public void UpdateBackButtonText()
		{
		backButtonText.text = "Back to Main Menu";  // Example text, can be dynamic
		}

	// --- Region: Back Button Action --- //
	public void OnBackButtonClicked()
		{
		Debug.Log("Back Button Clicked");
		}
	}
