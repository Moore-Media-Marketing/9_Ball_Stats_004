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
			}
		else
			{
			Destroy(gameObject); // Destroy duplicates
			}
		}

	private void Start()
		{
		backButton.onClick.AddListener(OnBackButtonClicked);
		updateLifetimeButton.onClick.AddListener(OnUpdateLifetimeButtonClicked);

		// Initialize Dropdowns
		InitializeDropdowns();
		}

	// Method to initialize and populate dropdowns
	private void InitializeDropdowns()
		{
		// Populate team dropdown
		var teams = DatabaseManager.Instance.GetAllTeams();
		teamNameDropdown.ClearOptions();
		teamNameDropdown.AddOptions(teams.Select(t => t.Name).ToList());

		// Populate player dropdown
		var players = DatabaseManager.Instance.GetAllPlayers();
		playerNameDropdown.ClearOptions();
		playerNameDropdown.AddOptions(players.Select(p => p.Name).ToList());
		}

	// When a player is selected from PlayerNameDropdown, populate data
	public void OnPlayerSelected(int playerIndex)
		{
		if (playerIndex > 0) // Make sure "Select Player" is not selected
			{
			selectedPlayer = DatabaseManager.Instance.GetPlayersByTeam(selectedTeam.Id)
				.FirstOrDefault(p => p.Name == playerNameDropdown.options[playerIndex].text);

			if (selectedPlayer != null)
				{
				// Populate lifetime data if available
				PopulateLifetimeData(selectedPlayer);
				}
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

	// On Back button clicked
	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel); // Use UIManager to handle panel switching
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

		// Update player lifetime data
		selectedPlayer.LifetimeGamesWon = int.Parse(lifetimeGamesWonInputField.text);
		selectedPlayer.LifetimeGamesPlayed = int.Parse(lifetimeGamesPlayedInputField.text);
		selectedPlayer.LifetimeDefensiveShotAvg = float.Parse(lifetimeDefensiveShotAvgInputField.text);
		selectedPlayer.MatchesPlayedInLast2Years = int.Parse(matchesPlayedInLast2YearsInputField.text);
		selectedPlayer.LifetimeBreakAndRun = int.Parse(lifetimeBreakAndRunInputField.text);
		selectedPlayer.NineOnTheSnap = int.Parse(nineOnTheSnapInputField.text);
		selectedPlayer.LifetimeMiniSlams = int.Parse(lifetimeMiniSlamsInputField.text);
		selectedPlayer.LifetimeShutouts = int.Parse(lifetimeShutoutsInputField.text);

		// Save the updated player data to the database
		DatabaseManager.Instance.SavePlayer(selectedPlayer);

		Debug.Log($"Updated lifetime data for player {selectedPlayer.Name}");
		}

	// Set selected team and player (called when player is selected)
	public void SetSelectedTeamAndPlayer(int teamIndex, int playerIndex)
		{
		// Get selected team
		selectedTeam = DatabaseManager.Instance.GetAllTeams().ElementAt(teamIndex);

		// Get selected player from the player dropdown
		if (playerIndex > 0) // Ensure a valid player is selected
			{
			selectedPlayer = DatabaseManager.Instance.GetPlayersByTeam(selectedTeam.Id)
				.FirstOrDefault(p => p.Name == playerNameDropdown.options[playerIndex].text);
			}
		}

	// Methods to open the panel and populate data for a specific player
	public void OpenWithPlayerData(Player player)
		{
		selectedPlayer = player;
		PopulateLifetimeData(player);
		// Any additional setup when opening with player data
		}

	public void OpenWithoutData()
		{
		selectedPlayer = null;
		// Optionally reset fields to empty or default values
		lifetimeGamesWonInputField.text = "";
		lifetimeGamesPlayedInputField.text = "";
		lifetimeDefensiveShotAvgInputField.text = "";
		matchesPlayedInLast2YearsInputField.text = "";
		lifetimeBreakAndRunInputField.text = "";
		nineOnTheSnapInputField.text = "";
		lifetimeMiniSlamsInputField.text = "";
		lifetimeShutoutsInputField.text = "";
		}
	}
