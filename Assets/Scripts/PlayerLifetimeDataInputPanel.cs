using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerLifetimeDataInputPanel:MonoBehaviour
	{
	[Header("UI Elements")]
	public TMP_Text headerText;

	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;

	// Lifetime data input fields
	public TMP_InputField lifetimeGamesWonInputField;

	public TMP_InputField lifetimeGamesPlayedInputField;
	public TMP_InputField lifetimeDefensiveShotAvgInputField;
	public TMP_InputField matchesPlayedInLast2YearsInputField;
	public TMP_InputField lifetimeBreakAndRunInputField;
	public TMP_InputField nineOnTheSnapInputField;
	public TMP_InputField lifetimeMiniSlamsInputField;
	public TMP_InputField lifetimeShutoutsInputField;

	// Buttons
	public Button updateLifetimeButton;

	public Button saveToCSVButton;
	public Button backButton;
	public TMP_Text backButtonText;

	// Lists to store team and player data
	private List<Team> teams = new();

	private List<Player> players = new();

	// Currently selected IDs
	private int selectedTeamId = -1;

	private int selectedPlayerId = -1;

	// Start is called before the first frame update
	public void Start()
		{
		PopulateTeamDropdown();
		SetupButtonListeners();
		}

	// Set up listeners for button actions
	public void SetupButtonListeners()
		{
		if (updateLifetimeButton != null)
			updateLifetimeButton.onClick.AddListener(UpdateLifetimeData);
		if (saveToCSVButton != null)
			saveToCSVButton.onClick.AddListener(SaveToCSV);
		if (backButton != null)
			backButton.onClick.AddListener(HandleBackButton);
		if (teamNameDropdown != null)
			teamNameDropdown.onValueChanged.AddListener(OnTeamSelected);
		if (playerNameDropdown != null)
			playerNameDropdown.onValueChanged.AddListener(OnPlayerSelected);
		}

	// Populate team dropdown with available teams
	public void PopulateTeamDropdown()
		{
		if (teamNameDropdown == null)
			{
			Debug.LogError("TeamNameDropdown reference is missing! Assign it in the Unity Inspector.");
			return;
			}

		if (PlayersAndTeamsManager.Instance == null)
			{
			Debug.LogError("PlayersAndTeamsManager instance is missing!");
			return;
			}

		// Reload teams from PlayersAndTeamsManager
		teams = PlayersAndTeamsManager.Instance.GetAllTeams();

		// Clear dropdown before adding new items
		teamNameDropdown.ClearOptions();

		// Add team names to the dropdown
		List<string> teamNames = new();
		foreach (var team in teams)
			{
			teamNames.Add(team.TeamName);
			}

		teamNameDropdown.AddOptions(teamNames);
		teamNameDropdown.RefreshShownValue(); // Ensure UI updates properly
		}

	// Populate player dropdown based on selected team
	public void PopulatePlayerDropdown(int teamId)
		{
		if (playerNameDropdown == null)
			{
			Debug.LogError("PlayerNameDropdown reference is missing! Assign it in the Unity Inspector.");
			return;
			}

		// Get players from the selected team
		players = PlayersAndTeamsManager.Instance.GetPlayersByTeamId(teamId);

		// Clear dropdown before adding new items
		playerNameDropdown.ClearOptions();

		// Add player names to the dropdown
		List<string> playerNames = new();
		foreach (var player in players)
			{
			playerNames.Add(player.PlayerName);
			}

		playerNameDropdown.AddOptions(playerNames);
		playerNameDropdown.RefreshShownValue(); // Ensure UI updates properly
		}

	// Handle team selection change
	public void OnTeamSelected(int index)
		{
		if (index < 0 || index >= teams.Count)
			{
			return;
			}

		selectedTeamId = teams[index].TeamId;
		PopulatePlayerDropdown(selectedTeamId); // Update player dropdown
		}

	// Handle player selection change
	public void OnPlayerSelected(int index)
		{
		if (index < 0 || index >= players.Count)
			{
			return;
			}

		selectedPlayerId = players[index].PlayerId;
		}

	// Update lifetime data (placeholder functionality)
	public void UpdateLifetimeData()
		{
		if (selectedPlayerId == -1)
			{
			Debug.LogWarning("No player selected to update lifetime data.");
			return;
			}

		// Gather the input data

		// Try to parse the input fields and log errors if invalid
		if (!int.TryParse(lifetimeGamesWonInputField.text, out int gamesWon))
			{
			Debug.LogWarning("Invalid input for Lifetime Games Won.");
			return;
			}

		if (!int.TryParse(lifetimeGamesPlayedInputField.text, out int gamesPlayed))
			{
			Debug.LogWarning("Invalid input for Lifetime Games Played.");
			return;
			}

		if (!float.TryParse(lifetimeDefensiveShotAvgInputField.text, out float defensiveShotAvg))
			{
			Debug.LogWarning("Invalid input for Lifetime Defensive Shot Average.");
			return;
			}

		if (!int.TryParse(matchesPlayedInLast2YearsInputField.text, out int matchesPlayedInLast2Years))
			{
			Debug.LogWarning("Invalid input for Matches Played in Last 2 Years.");
			return;
			}

		if (!int.TryParse(lifetimeBreakAndRunInputField.text, out int breakAndRun))
			{
			Debug.LogWarning("Invalid input for Lifetime Break and Run.");
			return;
			}

		if (!int.TryParse(nineOnTheSnapInputField.text, out int nineOnTheSnap))
			{
			Debug.LogWarning("Invalid input for Nine on the Snap.");
			return;
			}

		if (!int.TryParse(lifetimeMiniSlamsInputField.text, out int miniSlams))
			{
			Debug.LogWarning("Invalid input for Lifetime Mini Slams.");
			return;
			}

		if (!int.TryParse(lifetimeShutoutsInputField.text, out int shutouts))
			{
			Debug.LogWarning("Invalid input for Lifetime Shutouts.");
			return;
			}

		// Update the player data (using the PlayerStats object for lifetime data)
		PlayersAndTeamsManager.Instance.UpdatePlayerLifetimeData(selectedPlayerId, gamesWon, gamesPlayed, defensiveShotAvg,
																  matchesPlayedInLast2Years, breakAndRun, nineOnTheSnap,
																  miniSlams, shutouts);

		Debug.Log($"Updated lifetime data for player {players[selectedPlayerId].PlayerName}");
		}

	// Save lifetime data to CSV (placeholder functionality)
	public void SaveToCSV()
		{
		if (selectedPlayerId == -1)
			{
			Debug.LogWarning("No player selected to save lifetime data.");
			return;
			}

		// Save data to CSV (implement your CSV save logic here)
		Debug.Log($"Saving lifetime data for player {players[selectedPlayerId].PlayerName} to CSV...");
		}

	// Handle back button click
	public void HandleBackButton()
		{
		UIManager.Instance.GoBackToPreviousPanel();
		}

	// Update back button text
	public void UpdateBackButtonText(string newText)
		{
		if (backButtonText != null)
			{
			backButtonText.text = newText;
			}
		}
	}