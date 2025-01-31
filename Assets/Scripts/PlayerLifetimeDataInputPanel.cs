using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerLifetimeDataInputPanel:MonoBehaviour
	{
	// --- UI Elements --- //
	public TMP_Text headerText;
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField lifetimeGamesWonInputField;
	public TMP_InputField lifetimeGamesPlayedInputField;
	public TMP_InputField lifetimeDefensiveShotAvgInputField;
	public TMP_InputField matchesPlayedInLast2YearsInputField;
	public TMP_InputField lifetimeBreakAndRunInputField;
	public TMP_InputField nineOnTheSnapInputField;
	public TMP_InputField lifetimeMiniSlamsInputField;
	public TMP_InputField lifetimeShutoutsInputField;
	public Button updateLifetimeButton;
	public Button backButton;

	// Store player data
	private Team selectedTeam;
	private Player selectedPlayer;

	// Singleton Instance
	public static PlayerLifetimeDataInputPanel Instance { get; private set; }

	private void Awake()
		{
		// Ensure there is only one instance of PlayerLifetimeDataInputPanel
		if (Instance == null)
			{
			Instance = this;
			Debug.Log("PlayerLifetimeDataInputPanel Instance set.");
			}
		else
			{
			Destroy(gameObject); // Destroy duplicates
			Debug.LogError("Duplicate PlayerLifetimeDataInputPanel instance destroyed.");
			}
		}

	private void Start()
		{
		if (Instance == null)
			{
			Debug.LogError("PlayerLifetimeDataInputPanel instance is still null at Start!");
			return;
			}

		backButton.onClick.AddListener(OnBackButtonClicked);
		updateLifetimeButton.onClick.AddListener(OnUpdateLifetimeButtonClicked);

		InitializeDropdowns();
		}

	// Method to initialize and populate dropdowns
	private void InitializeDropdowns()
		{
		// Populate team dropdown
		var teams = DatabaseManager.Instance.GetAllTeams();
		teamNameDropdown.ClearOptions();
		teamNameDropdown.AddOptions(teams.Select(t => t.Name).ToList());

		// Populate player dropdown when a team is selected
		teamNameDropdown.onValueChanged.AddListener(OnTeamSelected);
		Debug.Log("Team dropdown initialized.");
		}

	// Method when team is selected from dropdown
	private void OnTeamSelected(int teamIndex)
		{
		if (teamIndex > 0) // Make sure "Select Team" is not selected
			{
			selectedTeam = DatabaseManager.Instance.GetAllTeams().ElementAt(teamIndex - 1); // Offset by 1 for "Select Team" option

			// Populate player dropdown
			var players = DatabaseManager.Instance.GetPlayersByTeam(selectedTeam.Id);
			playerNameDropdown.ClearOptions();
			playerNameDropdown.AddOptions(players.Select(p => p.Name).ToList());

			playerNameDropdown.onValueChanged.AddListener(OnPlayerSelected);
			Debug.Log($"Team selected: {selectedTeam.Name}");
			}
		else
			{
			playerNameDropdown.ClearOptions(); // Clear player dropdown if no team is selected
			Debug.Log("No team selected.");
			}
		}

	// When a player is selected from PlayerNameDropdown, populate data
	private void OnPlayerSelected(int playerIndex)
		{
		if (playerIndex > 0) // Make sure "Select Player" is not selected
			{
			selectedPlayer = DatabaseManager.Instance.GetPlayersByTeam(selectedTeam.Id)
				.FirstOrDefault(p => p.Name == playerNameDropdown.options[playerIndex].text);

			if (selectedPlayer != null)
				{
				PopulateLifetimeData(selectedPlayer);
				Debug.Log($"Player selected: {selectedPlayer.Name}");
				}
			else
				{
				Debug.LogError("Selected player not found.");
				}
			}
		else
			{
			// Clear player data if no player is selected
			ResetLifetimeDataFields();
			Debug.Log("No player selected.");
			}
		}

	private void PopulateLifetimeData(Player player)
		{
		// Fill input fields with saved data for the selected player
		lifetimeGamesWonInputField.text = player.LifetimeGamesWon.ToString();
		lifetimeGamesPlayedInputField.text = player.LifetimeGamesPlayed.ToString();
		lifetimeDefensiveShotAvgInputField.text = player.LifetimeDefensiveShotAvg.ToString();
		matchesPlayedInLast2YearsInputField.text = player.MatchesPlayedInLast2Years.ToString();
		lifetimeBreakAndRunInputField.text = player.LifetimeBreakAndRun.ToString();
		nineOnTheSnapInputField.text = player.NineOnTheSnap.ToString();
		lifetimeMiniSlamsInputField.text = player.LifetimeMiniSlams.ToString();
		lifetimeShutoutsInputField.text = player.LifetimeShutouts.ToString();

		Debug.Log($"Populated lifetime data for player {player.Name}");
		}

	private void ResetLifetimeDataFields()
		{
		lifetimeGamesWonInputField.text = "";
		lifetimeGamesPlayedInputField.text = "";
		lifetimeDefensiveShotAvgInputField.text = "";
		matchesPlayedInLast2YearsInputField.text = "";
		lifetimeBreakAndRunInputField.text = "";
		nineOnTheSnapInputField.text = "";
		lifetimeMiniSlamsInputField.text = "";
		lifetimeShutoutsInputField.text = "";

		Debug.Log("Reset lifetime data fields.");
		}

	// On Back button clicked
	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		Debug.Log("Back button clicked, showing home panel.");
		}

	// On Update Lifetime button clicked
	private void OnUpdateLifetimeButtonClicked()
		{
		// Check if player data exists, if not return early
		if (selectedPlayer == null)
			{
			Debug.LogError("No player selected!");
			return;
			}

		// Check if all input fields are valid
		if (!int.TryParse(lifetimeGamesWonInputField.text, out int lifetimeGamesWon) ||
			!int.TryParse(lifetimeGamesPlayedInputField.text, out int lifetimeGamesPlayed) ||
			!float.TryParse(lifetimeDefensiveShotAvgInputField.text, out float lifetimeDefensiveShotAvg) ||
			!int.TryParse(matchesPlayedInLast2YearsInputField.text, out int matchesPlayedInLast2Years) ||
			!int.TryParse(lifetimeBreakAndRunInputField.text, out int lifetimeBreakAndRun) ||
			!int.TryParse(nineOnTheSnapInputField.text, out int nineOnTheSnap) ||
			!int.TryParse(lifetimeMiniSlamsInputField.text, out int lifetimeMiniSlams) ||
			!int.TryParse(lifetimeShutoutsInputField.text, out int lifetimeShutouts))
			{
			Debug.LogError("Invalid input data! Please check the fields.");
			return;
			}

		// Update player lifetime data
		selectedPlayer.LifetimeGamesWon = lifetimeGamesWon;
		selectedPlayer.LifetimeGamesPlayed = lifetimeGamesPlayed;
		selectedPlayer.LifetimeDefensiveShotAvg = lifetimeDefensiveShotAvg;
		selectedPlayer.MatchesPlayedInLast2Years = matchesPlayedInLast2Years;
		selectedPlayer.LifetimeBreakAndRun = lifetimeBreakAndRun;
		selectedPlayer.NineOnTheSnap = nineOnTheSnap;
		selectedPlayer.LifetimeMiniSlams = lifetimeMiniSlams;
		selectedPlayer.LifetimeShutouts = lifetimeShutouts;

		// Save the updated player data to the database
		DatabaseManager.Instance.SavePlayer(selectedPlayer);

		Debug.Log($"Updated lifetime data for player {selectedPlayer.Name}");
		}

	// Methods to open the panel and populate data for a specific player
	public void OpenWithPlayerData(Player player)
		{
		selectedPlayer = player;
		PopulateLifetimeData(player);
		UIManager.Instance.ShowPanel(this.gameObject); // Ensure the panel is shown
		Debug.Log($"Opened panel with player data: {player.Name}");
		}

	// Open the panel without player data (if no player is selected)
	public void OpenWithNoData()
		{
		selectedPlayer = null;
		ResetLifetimeDataFields();
		UIManager.Instance.ShowPanel(this.gameObject); // Ensure the panel is shown
		Debug.Log("Opening without player data. Please select a player.");
		}
	}
