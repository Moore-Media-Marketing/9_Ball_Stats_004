using UnityEngine;
using TMPro;
using SQLite;  // Required for SQLite functionality
using UnityEngine.UI;  // Required for Scrollbars and Buttons

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

	// --- Region: SQLite References --- //
	private SQLiteConnection db;
	private string dbPath;  // SQLite database path

	// --- Region: Initialize SQLite and UI References --- //
	private void Start()
		{
		dbPath = System.IO.Path.Combine(Application.persistentDataPath, "matchups.db");
		db = new SQLiteConnection(dbPath);
		db.CreateTable<Matchup>();  // Ensure the Matchup table exists

		// Initialize the Back Button text
		UpdateBackButtonText();

		// Optionally load existing data if needed
		LoadMatchupResultsFromDatabase();
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

		// Optionally store matchup results in the database
		SaveMatchupResult(result, teamAWinProbability, teamBWinProbability);
		}

	// --- Region: Save Matchup Results to Database --- //
	public void SaveMatchupResult(string result, float teamAWinProbability, float teamBWinProbability)
		{
		Matchup newMatchup = new Matchup
			{
			Result = result,
			TeamAWinProbability = teamAWinProbability,
			TeamBWinProbability = teamBWinProbability
			};

		db.Insert(newMatchup);  // Save to SQLite
		Debug.Log("Matchup result saved to database.");
		}

	// --- Region: Load Matchup Results from Database --- //
	public void LoadMatchupResultsFromDatabase()
		{
		var matchups = db.Table<Matchup>().ToList();  // Retrieve all matchup records from database

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

	// --- Region: Matchup Data Model --- //
	public class Matchup
		{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }  // Unique ID for matchup record
		public string Result { get; set; }  // Result of the matchup
		public float TeamAWinProbability { get; set; }  // Team A's win probability
		public float TeamBWinProbability { get; set; }  // Team B's win probability
		}
	}
