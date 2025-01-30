using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class ComparisonSetupPanel:MonoBehaviour
	{
	// --- Panel Elements --- //
	public TextMeshProUGUI headerText;

	public TextMeshProUGUI selectHomeTeamText;
	public TextMeshProUGUI selectHomePlayerText;
	public TMP_Dropdown homeTeamDropdown;
	public Transform homeTeamPlayerScrollView;
	public TextMeshProUGUI selectAwayTeamText;
	public TextMeshProUGUI selectAwayPlayerText;
	public TMP_Dropdown awayTeamDropdown;
	public Transform awayTeamPlayerScrollView;
	public Button compareButton;
	public Button backButton;

	// --- Initialize the Panel with Available Teams and Players --- //
	public void InitializePanel()
		{
		// Clear dropdown options and populate with available teams
		homeTeamDropdown.ClearOptions();
		awayTeamDropdown.ClearOptions();

		// Add available team names to both dropdowns
		homeTeamDropdown.AddOptions(GetTeamNames());
		awayTeamDropdown.AddOptions(GetTeamNames());

		// Clear previous player selections
		ClearPlayerSelections(homeTeamPlayerScrollView);
		ClearPlayerSelections(awayTeamPlayerScrollView);
		}

	// --- Get Team Names from DataManager --- //
	private System.Collections.Generic.List<string> GetTeamNames()
		{
		// Fetch team names from DataManager (retrieved from PlayerPrefs)
		return DataManager.Instance.GetTeamNames();
		}

	// --- Update Player List for Selected Team --- //
	public void UpdatePlayerList(TMP_Dropdown teamDropdown, Transform playerScrollView)
		{
		// Get the selected team based on the dropdown
		string selectedTeam = teamDropdown.options[teamDropdown.value].text;

		// Clear previous player list
		ClearPlayerSelections(playerScrollView);

		// Add players to the scroll view based on the selected team
		var players = GetPlayersForTeam(selectedTeam);

		foreach (string playerName in players)
			{
			// Create a new button for each player
			GameObject playerButton = new(playerName);
			playerButton.transform.SetParent(playerScrollView, false); // Ensure correct hierarchy

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

	// --- Get Players for Selected Team from DataManager --- //
	private System.Collections.Generic.List<string> GetPlayersForTeam(string teamName)
		{
		// Retrieve player names for the selected team from DataManager (stored in PlayerPrefs)
		return DataManager.Instance.GetPlayersForTeam(teamName);
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

	// --- Compare Button Logic using ComparisonManager --- //
	private void OnCompareButtonClicked()
		{
		// Get the selected team names from the dropdown
		string homeTeamName = homeTeamDropdown.options[homeTeamDropdown.value].text;
		string awayTeamName = awayTeamDropdown.options[awayTeamDropdown.value].text;

		// Retrieve the Team objects from DataManager
		Team homeTeam = DataManager.Instance.GetTeamByName(homeTeamName);
		Team awayTeam = DataManager.Instance.GetTeamByName(awayTeamName);

		// Ensure both teams exist before proceeding
		if (homeTeam == null || awayTeam == null)
			{
			Debug.LogError("One or both selected teams could not be found!");
			return;
			}

		// Use ComparisonManager to compare the teams
		ComparisonManager.Instance.CompareTeams(homeTeam, awayTeam);

		Debug.Log($"Comparison started between {homeTeamName} and {awayTeamName}");
		}

	// --- Back Button Logic --- //
	private void OnBackButtonClicked()
		{
		// Navigate back to the previous panel (e.g., Main Menu or Team Management)
		UIManager.Instance.GoBackToPreviousPanel();
		}
	}