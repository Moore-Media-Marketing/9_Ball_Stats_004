using UnityEngine;
using TMPro; // Use TMPro for TextMeshPro support
using UnityEngine.UI; // For Dropdowns and Buttons

public class ComparisonSetupPanel:MonoBehaviour
	{
	// --- Panel Elements --- //
	public TextMeshProUGUI headerText; // Header text for the panel
	public TextMeshProUGUI selectHomeTeamText; // Text for selecting home team
	public TextMeshProUGUI selectHomePlayerText; // Text for selecting home player
	public TMP_Dropdown homeTeamDropdown; // Dropdown for home team selection
	public Transform homeTeamPlayerScrollView; // Container for home team players (scrollable list)
	public TextMeshProUGUI selectAwayTeamText; // Text for selecting away team
	public TextMeshProUGUI selectAwayPlayerText; // Text for selecting away player
	public TMP_Dropdown awayTeamDropdown; // Dropdown for away team selection
	public Transform awayTeamPlayerScrollView; // Container for away team players (scrollable list)
	public Button compareButton; // Button to start comparison
	public Button backButton; // Back button to go back to the previous screen

	// --- Initialize the Panel with Available Teams and Players --- //
	public void InitializePanel()
		{
		// Clear dropdown options and populate with available teams
		homeTeamDropdown.ClearOptions();
		awayTeamDropdown.ClearOptions();

		// Assuming GetTeamNames() returns a list of team names (you can replace this with your actual method)
		homeTeamDropdown.AddOptions(GetTeamNames());
		awayTeamDropdown.AddOptions(GetTeamNames());

		// Clear previous player selections
		ClearPlayerSelections(homeTeamPlayerScrollView);
		ClearPlayerSelections(awayTeamPlayerScrollView);
		}

	// --- Get Team Names (Example) --- //
	private System.Collections.Generic.List<string> GetTeamNames()
		{
		// Replace this with your actual logic to get team names
		return new System.Collections.Generic.List<string> { "Team A", "Team B", "Team C" }; // Example teams
		}

	// --- Update Player List for Selected Team --- //
	public void UpdatePlayerList(TMP_Dropdown teamDropdown, Transform playerScrollView)
		{
		// Get the selected team based on the dropdown
		string selectedTeam = teamDropdown.options[teamDropdown.value].text;

		// Clear previous player list
		ClearPlayerSelections(playerScrollView);

		// Add players to the scroll view based on the selected team
		// Assuming GetPlayersForTeam() returns a list of player names for the selected team
		var players = GetPlayersForTeam(selectedTeam);

		foreach (string playerName in players)
			{
			// Create a new button for each player
			GameObject playerButton = new(playerName);
			playerButton.transform.SetParent(playerScrollView);

			// Add a button component to the player button
			Button button = playerButton.AddComponent<Button>();

			// Add text to the button using TextMeshPro
			TextMeshProUGUI buttonText = playerButton.AddComponent<TextMeshProUGUI>();
			buttonText.text = playerName;
			buttonText.fontSize = 14;

			// Add listener to the button for player selection
			button.onClick.AddListener(() => OnPlayerSelected(playerName, teamDropdown));
			}
		}

	// --- Get Players for Selected Team (Example) --- //
	private System.Collections.Generic.List<string> GetPlayersForTeam(string teamName)
		{
		// Replace this with actual logic to fetch players for the selected team
		return new System.Collections.Generic.List<string> { "Player 1", "Player 2", "Player 3" }; // Example players
		}

	// --- Clear Player Selections --- //
	private void ClearPlayerSelections(Transform playerScrollView)
		{
		// Destroy all existing player buttons in the scroll view
		foreach (Transform child in playerScrollView)
			{
			Destroy(child.gameObject);
			}
		}

	// --- Handle Player Selection --- //
	private void OnPlayerSelected(string playerName, TMP_Dropdown teamDropdown)
		{
		// Logic to handle player selection from the list
		Debug.Log($"{playerName} selected from {teamDropdown.options[teamDropdown.value].text}");
		}

	// --- Start Method for Initializing Button Listeners --- //
	private void Start()
		{
		// Initialize panel with teams and players
		InitializePanel();

		// Add listener for home team dropdown
		homeTeamDropdown.onValueChanged.AddListener((index) =>
		{
			UpdatePlayerList(homeTeamDropdown, homeTeamPlayerScrollView);
		});

		// Add listener for away team dropdown
		awayTeamDropdown.onValueChanged.AddListener((index) =>
		{
			UpdatePlayerList(awayTeamDropdown, awayTeamPlayerScrollView);
		});

		// Add listener for compare button (for starting the comparison)
		if (compareButton != null)
			{
			compareButton.onClick.AddListener(OnCompareButtonClicked);
			}

		// Add listener for back button
		if (backButton != null)
			{
			backButton.onClick.AddListener(OnBackButtonClicked);
			}
		}

	// --- Compare Button Logic --- //
	private void OnCompareButtonClicked()
		{
		// Logic to start comparison (could transition to results panel or trigger comparison logic)
		Debug.Log("Comparison started between teams.");
		}

	// --- Back Button Logic --- //
	private void OnBackButtonClicked()
		{
		// Navigate back to the previous panel (e.g., Main Menu or Team Management)
		UIManager.Instance.GoBackToPreviousPanel();
		}
	}
