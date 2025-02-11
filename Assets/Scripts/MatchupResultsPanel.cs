using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MatchupResultsPanel:MonoBehaviour
	{
	// Singleton instance
	public static MatchupResultsPanel Instance { get; private set; }

	[Header("UI Elements")]
	public TMP_Text team1WinProbabilityText;   // Team 1 win probability text
	public TMP_Text team2WinProbabilityText;   // Team 2 win probability text
	public Transform matchupListContainer;     // List container for matchups
	public Transform bestMatchupListContainer; // List container for best matchups
	public GameObject matchupEntryPrefab;      // Prefab for each matchup entry
	public Button backButton;                  // Button to close the panel

	// Ensures Singleton instance is assigned
	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			Debug.Log("✅ MatchupResultsPanel Singleton Initialized.");
			}
		else
			{
			Debug.LogWarning("⚠️ Duplicate MatchupResultsPanel detected! Destroying new one.");
			Destroy(gameObject);
			}
		}

	private void Start()
		{
		// Ensure the panel initializes properly
		if (Instance == null)
			{
			Debug.LogError("🚨 MatchupResultsPanel Instance was not initialized in Awake()! Forcing initialization.");
			Instance = this;
			}

		// If the panel was disabled at start, enable and disable to trigger Awake()
		if (!gameObject.activeInHierarchy)
			{
			gameObject.SetActive(true);
			gameObject.SetActive(false);
			}
		}

	/// <summary>
	/// Displays matchup results based on selected teams.
	/// </summary>
	public void DisplayMatchupResults(List<Player> team1Players, List<Player> team2Players)
		{
		// Ensure MatchupResultsPanel instance exists
		if (Instance == null)
			{
			Debug.LogError("🚨 MatchupResultsPanel.Instance is NULL! Cannot display matchups.");
			return;
			}

		// Validate player lists
		if (team1Players == null || team2Players == null || team1Players.Count == 0 || team2Players.Count == 0)
			{
			Debug.LogError("❌ Invalid player lists received in MatchupResultsPanel!");
			return;
			}

		Debug.Log($"🔹 Displaying matchup: Team 1 ({team1Players.Count} players) vs Team 2 ({team2Players.Count} players)");

		// Calculate win probabilities
		float winChanceTeam1 = HandicapSystem.CalculateWinProbability(team1Players, team2Players);
		float winChanceTeam2 = 1 - winChanceTeam1;

		// Update UI with win probabilities
		team1WinProbabilityText.text = $"Win Probability: {winChanceTeam1 * 100:F1}%";
		team2WinProbabilityText.text = $"Win Probability: {winChanceTeam2 * 100:F1}%";

		// Populate matchups and determine the best pairing
		PopulateMatchupList(team1Players, team2Players);

		// Make sure the panel is visible
		gameObject.SetActive(true);
		}

	/// <summary>
	/// Populates the list of matchups and identifies the best matchup.
	/// </summary>
	private void PopulateMatchupList(List<Player> team1Players, List<Player> team2Players)
		{
		// Clear previous matchup entries
		foreach (Transform child in matchupListContainer)
			{
			Destroy(child.gameObject);
			}

		foreach (Transform child in bestMatchupListContainer)
			{
			Destroy(child.gameObject);
			}

		GameObject bestMatchupEntry = null;
		float highestWinChance = 0;

		// Loop through all players in both teams to create matchups
		foreach (var player1 in team1Players)
			{
			foreach (var player2 in team2Players)
				{
				// Calculate win probability for the current matchup
				float matchupOdds = HandicapSystem.CalculateWinProbability(
					new List<Player> { player1 },
					new List<Player> { player2 });

				// Create a matchup entry in the UI
				GameObject matchupEntry = Instantiate(matchupEntryPrefab, matchupListContainer);
				matchupEntry.GetComponent<TMP_Text>().text = $"{player1.PlayerName} vs {player2.PlayerName}: {matchupOdds * 100:F1}%";

				// Identify the best matchup based on highest win probability
				if (matchupOdds > highestWinChance)
					{
					highestWinChance = matchupOdds;
					bestMatchupEntry = matchupEntry;
					}
				}
			}

		// Highlight the best matchup
		if (bestMatchupEntry != null)
			{
			Instantiate(bestMatchupEntry, bestMatchupListContainer);
			Debug.Log($"🏆 Best matchup identified: {bestMatchupEntry.GetComponent<TMP_Text>().text}");
			}
		}

	/// <summary>
	/// Hides the matchup results panel.
	/// </summary>
	public void HidePanel()
		{
		Debug.Log("📢 Hiding MatchupResultsPanel.");
		gameObject.SetActive(false);
		}
	}
