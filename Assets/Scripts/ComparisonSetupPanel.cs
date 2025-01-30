using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class ComparisonSetupPanel:MonoBehaviour
	{
	// --- UI Elements ---
	[Header("UI Elements")]
	[SerializeField] private TMP_Text headerText; // Header text for the panel

	[SerializeField] private TMP_Text selectHomeTeamText; // Text for selecting home team
	[SerializeField] private TMP_Text selectAwayTeamText; // Text for selecting away team
	[SerializeField] private TMP_Text selectHomePlayerText; // Text for selecting home player
	[SerializeField] private TMP_Text selectAwayPlayerText; // Text for selecting away player

	[SerializeField] private Toggle comparePlayersToggle; // Toggle to compare players or not
	[SerializeField] private Button compareButton; // Button to start comparison
	[SerializeField] private Button backButton; // Back button to go back to main menu

	// --- Team Dropdowns ---
	[Header("Team Dropdowns")]
	[SerializeField] private TMP_Dropdown homeTeamDropdown; // Home team dropdown

	[SerializeField] private TMP_Dropdown awayTeamDropdown; // Away team dropdown

	// --- Player List Scroll Views ---
	[Header("Player Selection")]
	[SerializeField] private GameObject homeTeamPlayerScrollView; // Scroll view for home team players

	[SerializeField] private GameObject awayTeamPlayerScrollView; // Scroll view for away team players
	[SerializeField] private GameObject playerTogglePrefab; // Prefab for player toggle in scroll views

	// --- Panel References ---
	[Header("Panels")]
	[SerializeField] private GameObject comparisonResultsPanel; // Comparison results panel

	// --- Initialization ---
	private void Start()
		{
		// Set the header text for the Comparison Setup Panel
		headerText.text = "Comparison Setup";

		// Initialize Dropdowns and Player List View
		homeTeamDropdown.onValueChanged.AddListener(OnHomeTeamSelected);
		awayTeamDropdown.onValueChanged.AddListener(OnAwayTeamSelected);
		compareButton.onClick.AddListener(OnCompareButtonClicked);
		backButton.onClick.AddListener(OnBackButtonClicked);

		// Initialize Home/Away Player Scroll Views (hidden initially)
		homeTeamPlayerScrollView.SetActive(false);
		awayTeamPlayerScrollView.SetActive(false);
		}

	// --- Button Handlers ---
	private void OnHomeTeamSelected(int index)
		{
		// Logic for selecting a home team and displaying the corresponding player list
		PopulatePlayerList(homeTeamDropdown.options[index].text, homeTeamPlayerScrollView);
		homeTeamPlayerScrollView.SetActive(true);
		}

	private void OnAwayTeamSelected(int index)
		{
		// Logic for selecting an away team and displaying the corresponding player list
		PopulatePlayerList(awayTeamDropdown.options[index].text, awayTeamPlayerScrollView);
		awayTeamPlayerScrollView.SetActive(true);
		}

	private void OnCompareButtonClicked()
		{
		// Check if both teams are selected and players are chosen
		if (homeTeamDropdown.value >= 0 && awayTeamDropdown.value >= 0)
			{
			// If comparing players is enabled, compare players
			if (comparePlayersToggle.isOn)
				{
				// Logic for comparing selected players in both teams
				ComparePlayers();
				}

			// Show the Comparison Results Panel
			comparisonResultsPanel.SetActive(true);
			gameObject.SetActive(false); // Hide the Comparison Setup Panel
			}
		else
			{
			// Show error or prompt to select teams
			Debug.LogWarning("Please select both teams before comparing.");
			}
		}

	private void OnBackButtonClicked()
		{
		// Go back to the main menu or the previous panel
		gameObject.SetActive(false); // Hide the current panel
									 // You can show the previous panel here based on your app flow
		}

	// --- Player List Population ---
	private void PopulatePlayerList(string teamName, GameObject scrollView)
		{
		// Clear previous player toggles in the list
		foreach (Transform child in scrollView.transform)
			{
			Destroy(child.gameObject);
			}

		// Fetch the players for the selected team (this is just a placeholder for your logic)
		var players = GetPlayersForTeam(teamName);

		// Instantiate player toggles for each player in the list
		foreach (var player in players)
			{
			GameObject toggleObj = Instantiate(playerTogglePrefab, scrollView.transform);
			toggleObj.GetComponentInChildren<TMP_Text>().text = player.PlayerName; // Changed to PlayerName
			toggleObj.GetComponent<Toggle>().onValueChanged.AddListener((isOn) => OnPlayerToggleChanged(player, isOn));
			}
		}

	// --- Player Toggle Handler ---
	private void OnPlayerToggleChanged(Player player, bool isOn)
		{
		// Handle the player toggle state change (e.g., select/unselect players)
		if (isOn)
			{
			Debug.Log(player.PlayerName + " selected."); // Changed to PlayerName
			}
		else
			{
			Debug.Log(player.PlayerName + " unselected."); // Changed to PlayerName
			}
		}

	// --- Compare Players (if enabled) ---
	private void ComparePlayers()
		{
		// Logic to compare selected players from both teams
		Debug.Log("Comparing players between teams...");
		}

	// --- Example Method to Get Players for a Team (to be replaced with actual data fetching logic) ---
	private Player[] GetPlayersForTeam(string teamName)
		{
		if (teamName is null)
			{
			throw new ArgumentNullException(nameof(teamName));
			}
		// Placeholder method to simulate fetching players for a team
		// Replace with actual data-fetching logic for your players
		return new Player[]
		{
			new("Player 1", 5),
			new("Player 2", 7),
			new("Player 3", 6),
			new("Player 4", 8),
			new("Player 5", 6)
		};
		}
	}