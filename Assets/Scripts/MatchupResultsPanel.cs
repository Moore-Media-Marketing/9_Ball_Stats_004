using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchupResultsPanel:MonoBehaviour
	{
	[Header("UI Elements")]
	public TextMeshProUGUI matchupHeaderText;
	public TextMeshProUGUI team1NameText;
	public TextMeshProUGUI team2NameText;
	public TextMeshProUGUI team1WinProbabilityText;
	public TextMeshProUGUI team2WinProbabilityText;
	public Button backButton;
	public Button confirmButton;

	// Panels for odds and best matchups
	public GameObject winOddsPanel;
	public GameObject bestMatchupPanel;
	public GameObject matchupListScrollView;
	public GameObject bestMatchupListScrollView;

	// This list will store the players of the two teams
	private List<Player> team1Players;
	private List<Player> team2Players;

	// Singleton instance for this panel
	public static MatchupResultsPanel Instance;

	private void Awake()
		{
		// Singleton pattern to ensure only one instance exists
		if (Instance == null)
			{
			Instance = this;
			}
		else
			{
			Destroy(gameObject);
			}
		}

	private void Start()
		{
		// Add listeners for buttons
		backButton.onClick.AddListener(BackToComparison);
		confirmButton.onClick.AddListener(ConfirmMatchup);
		}

	/// <summary>
	/// Displays the result of a matchup by calculating the win probabilities.
	/// </summary>
	public void DisplayMatchupResults(List<Player> team1, List<Player> team2)
		{
		if (team1 == null || team2 == null || team1.Count == 0 || team2.Count == 0)
			{
			Debug.LogError("Invalid teams. Cannot display matchup results.");
			return;
			}

		team1Players = team1;
		team2Players = team2;

		// Display team names
		team1NameText.text = "Team 1: " + team1[0].TeamName;
		team2NameText.text = "Team 2: " + team2[0].TeamName;

		// Calculate the win probabilities for both teams using the HandicapSystem with weighted values
		float team1WinProbability = HandicapSystem.CalculateAdjustedWinProbability(team1, team2);
		float team2WinProbability = 1f - team1WinProbability;

		// Display win probabilities in the panel
		team1WinProbabilityText.text = $"Win Probability: {team1WinProbability * 100f:F2}%";
		team2WinProbabilityText.text = $"Win Probability: {team2WinProbability * 100f:F2}%";

		// Set the header
		matchupHeaderText.text = "Matchup Results";

		// Show Win Odds Panel
		DisplayWinOddsPanel(team1, team2);

		// Show Best Matchup Panel
		DisplayBestMatchupPanel(team1, team2);
		}

	/// <summary>
	/// Displays all matchup odds for team 1 and team 2.
	/// </summary>
	private void DisplayWinOddsPanel(List<Player> team1, List<Player> team2)
		{
		// Clear previous entries
		foreach (Transform child in matchupListScrollView.transform)
			{
			Destroy(child.gameObject);
			}

		// Set the layout group for the content (optional but recommended for automatic spacing)
		VerticalLayoutGroup verticalLayoutGroup = matchupListScrollView.GetComponentInChildren<VerticalLayoutGroup>();
		if (verticalLayoutGroup != null)
			{
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;  // Ensures elements don't stretch vertically
			}

		// Generate Win Odds Entries based on players from both teams
		foreach (var player1 in team1)
			{
			foreach (var player2 in team2)
				{
				// Create WinOddsEntry GameObject
				GameObject winOddsEntry = new GameObject("WinOddsEntry", typeof(RectTransform), typeof(TextMeshProUGUI));
				winOddsEntry.transform.SetParent(matchupListScrollView.transform);

				// Set RectTransform to stretch within the parent (the scroll view's content area)
				RectTransform entryRect = winOddsEntry.GetComponent<RectTransform>();
				entryRect.anchorMin = new Vector2(0f, 0f);   // Start from the left-bottom of the parent
				entryRect.anchorMax = new Vector2(1f, 0f);   // Stretch to the right edge
				entryRect.pivot = new Vector2(0.5f, 0f);     // Align the pivot in the middle of the width, bottom vertically
				entryRect.sizeDelta = new Vector2(0, 50);    // Set fixed height for each entry (e.g., 50px)

				// Calculate the win probability for this matchup based on weighted stats
				float winProbability = HandicapSystem.CalculateWinProbability(player1, player2);

				// Set up the display for the entry
				TextMeshProUGUI text = winOddsEntry.GetComponent<TextMeshProUGUI>();
				text.text = $"{player1.PlayerName} vs {player2.PlayerName}: {winProbability * 100f:F2}%";

				// Optional: Customize the layout of the entry here (e.g., font size based on screen width)
				text.fontSize = Mathf.Lerp(20, 30, Screen.width / 1080f);  // Example of responsive font size
				text.alignment = TextAlignmentOptions.Center;
				}
			}
		}


	/// <summary>
	/// Displays the optimal matchup and the players to be selected for the best team combinations.
	/// </summary>
	private void DisplayBestMatchupPanel(List<Player> team1, List<Player> team2)
		{
		foreach (Transform child in bestMatchupListScrollView.transform)
			{
			Destroy(child.gameObject);  // Clear previous entries
			}

		// Find the optimal matchup (you can adjust the criteria based on your system)
		var bestTeam1Players = HandicapSystem.FindOptimalTeamSelection(team1);
		var bestTeam2Players = HandicapSystem.FindOptimalTeamSelection(team2);

		// Generate Best Matchup Entries
		for (int i = 0; i < bestTeam1Players.Count; i++)
			{
			GameObject bestMatchupEntry = new GameObject("BestMatchupEntry", typeof(RectTransform), typeof(TextMeshProUGUI));
			bestMatchupEntry.transform.SetParent(bestMatchupListScrollView.transform);

			// Calculate win probability for the optimal matchup
			float winProbability = HandicapSystem.CalculateWinProbability(bestTeam1Players[i], bestTeam2Players[i]);

			// Set up the display for the entry (you can modify it further as needed)
			var text = bestMatchupEntry.GetComponent<TextMeshProUGUI>();
			text.text = $"Best Matchup: {bestTeam1Players[i].PlayerName} vs {bestTeam2Players[i].PlayerName} with {winProbability * 100f:F2}% win chance";

			// Optional: Customize the layout of the entry here (e.g., font size based on screen width)
			text.fontSize = Mathf.Lerp(18, 28, Screen.width / 1080f);  // Example of responsive font size
			}
		}

	/// <summary>
	/// Returns to the Matchup Comparison panel when the back button is clicked.
	/// </summary>
	private void BackToComparison()
		{
		Debug.Log("Returning to Matchup Comparison panel.");
		UIManager.Instance.GoBackToPreviousPanel(); // Go back using UIManager
		}

	/// <summary>
	/// Confirms the matchup and does any necessary follow-up logic.
	/// </summary>
	private void ConfirmMatchup()
		{
		Debug.Log("Matchup confirmed between Team 1 and Team 2.");
		// Additional confirmation logic can be added here, e.g., saving matchup results or moving to the next screen.
		}
	}
