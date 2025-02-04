using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

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

		// Optionally load existing data if needed
		LoadMatchupResultsFromCsv();
		}

	// --- Region: Display Matchup Results --- //
	public void DisplayMatchupResults(string result, float teamAWinProbability, float teamBWinProbability)
		{
		// Update the header text for teams and matchup
		headerText.text = "Matchup Results";
		teamAHeaderText.text = "Team A";
		teamBHeaderText.text = "Team B";

		// Display the matchup result and probabilities
		matchupResultText.text = result;
		teamAWinProbabilityText.text = "Team A Win Probability: " + teamAWinProbability.ToString("0.00") + "%";
		teamBWinProbabilityText.text = "Team B Win Probability: " + teamBWinProbability.ToString("0.00") + "%";

		// Optionally store matchup results in the CSV
		SaveMatchupResult(result, teamAWinProbability, teamBWinProbability);
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

	// --- Region: Load Matchup Results from CSV --- //
	public void LoadMatchupResultsFromCsv()
		{
		var matchups = ReadMatchupsFromCsv();  // Read all matchup records from the CSV

		// Clear previous entries
		ClearMatchupResults();

		// Populate the UI with the retrieved data
		foreach (var matchup in matchups)
			{
			// Assuming MatchupEntry_Template is a prefab
			GameObject entry = Instantiate(Resources.Load("MatchupEntry_Template") as GameObject);
			entry.transform.SetParent(matchupListContent, false);

			TMP_Text matchupDetailsText = entry.transform.Find("MatchupDetailsText").GetComponent<TMP_Text>();
			matchupDetailsText.text = $"{matchup.Result}\nA Win Probability: {matchup.TeamAWinProbability}% | B Win Probability: {matchup.TeamBWinProbability}%";
			}
		}

	// --- Region: Read Matchups from CSV --- //
	private List<Matchup> ReadMatchupsFromCsv()
		{
		var matchups = new List<Matchup>();

		if (File.Exists(matchupFilePath))
			{
			var lines = File.ReadAllLines(matchupFilePath);
			for (int i = 1; i < lines.Length; i++)  // Skip the header line
				{
				var parts = lines[i].Split(',');

				if (parts.Length == 4)
					{
					int id = int.Parse(parts[0]);
					string result = parts[1];
					float teamAWinProbability = float.Parse(parts[2]);
					float teamBWinProbability = float.Parse(parts[3]);

					matchups.Add(new Matchup
						{
						Id = id,
						Result = result,
						TeamAWinProbability = teamAWinProbability,
						TeamBWinProbability = teamBWinProbability
						});
					}
				}
			}

		return matchups;
		}

	// --- Region: Clear Matchup Results --- //
	public void ClearMatchupResults()
		{
		matchupResultText.text = "";
		teamAWinProbabilityText.text = "";
		teamBWinProbabilityText.text = "";

		// Optionally clear the UI list content
		foreach (Transform child in matchupListContent)
			{
			Destroy(child.gameObject);
			}
		}

	// --- Region: Update Back Button Text --- //
	public void UpdateBackButtonText()
		{
		backButtonText.text = "Back to Main Menu";  // Example text, can be dynamic
		}

	// --- Region: Back Button Action --- //
	public void OnBackButtonClicked()
		{
		// Handle back button action, e.g., navigating to the previous panel
		Debug.Log("Back Button Clicked");
		// Optionally, close the current panel and show the previous one
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
				var lastLine = lines[lines.Length - 1];
				var lastId = int.Parse(lastLine.Split(',')[0]);  // Extract the ID from the last line
				nextId = lastId + 1;
				}
			}

		return nextId;
		}

	// --- Region: Matchup Data Model --- //
	public class Matchup
		{
		public int Id { get; set; }  // Unique ID for matchup record
		public string Result { get; set; }  // Result of the matchup
		public float TeamAWinProbability { get; set; }  // Team A's win probability
		public float TeamBWinProbability { get; set; }  // Team B's win probability
		}
	}
