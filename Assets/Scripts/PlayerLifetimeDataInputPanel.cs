// --- Region: Using Directives --- //
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
// --- End Region: Using Directives --- //

public class PlayerLifetimeDataInputPanel:MonoBehaviour
	{
	#region UI Elements

	[Tooltip("Header text for the lifetime data panel.")]
	public TMP_Text headerText;  // --- Displays panel header ---

	[Tooltip("Dropdown for selecting a team (e.g., app.csv placeholder for team names).")]
	public TMP_Dropdown teamNameDropdown;  // --- Dropdown for team selection ---

	[Tooltip("Dropdown for selecting a player from the chosen team.")]
	public TMP_Dropdown playerNameDropdown;  // --- Dropdown for player selection ---

	[Tooltip("Input field for lifetime games won.")]
	public TMP_InputField lifetimeGamesWonInputField;  // --- Input for lifetime games won ---

	[Tooltip("Input field for lifetime games played.")]
	public TMP_InputField lifetimeGamesPlayedInputField;  // --- Input for lifetime games played ---

	[Tooltip("Input field for lifetime defensive shot average.")]
	public TMP_InputField lifetimeDefensiveShotAvgInputField;  // --- Input for defensive shot average ---

	[Tooltip("Input field for matches played in the last 2 years.")]
	public TMP_InputField matchesPlayedInLast2YearsInputField;  // --- Input for matches played in last 2 years ---

	[Tooltip("Input field for lifetime break and run statistic.")]
	public TMP_InputField lifetimeBreakAndRunInputField;  // --- Input for lifetime break and run ---

	[Tooltip("Input field for lifetime 9 on the snap statistic.")]
	public TMP_InputField nineOnTheSnapInputField;  // --- Input for lifetime 9 on the snap ---

	[Tooltip("Input field for lifetime mini slams.")]
	public TMP_InputField lifetimeMiniSlamsInputField;  // --- Input for lifetime mini slams ---

	[Tooltip("Input field for lifetime shutouts.")]
	public TMP_InputField lifetimeShutoutsInputField;  // --- Input for lifetime shutouts ---

	[Tooltip("Button to update the player's lifetime data.")]
	public Button updateLifetimeButton;  // --- Button to save lifetime data ---

	[Tooltip("Back button to return to the previous panel.")]
	public Button backButton;  // --- Button to go back to previous panel ---

	#endregion UI Elements

	#region Fields

	private Team selectedTeam;      // --- The currently selected team (placeholder: team object) ---
	private Player selectedPlayer;  // --- The currently selected player (placeholder: player object) ---

	// Singleton instance for global access
	public static PlayerLifetimeDataInputPanel Instance { get; private set; }

	#endregion Fields

	#region Unity Methods

	private void Awake()
		{
		// --- Ensure singleton instance ---
		if (Instance == null)
			{
			Instance = this;
			Debug.Log("PlayerLifetimeDataInputPanel Instance set.");
			}
		else
			{
			Destroy(gameObject);
			Debug.LogError("Duplicate PlayerLifetimeDataInputPanel instance found!");
			}
		}

	private void Start()
		{
		// --- Initialize dropdowns and set up button listeners ---
		InitializeDropdowns();
		updateLifetimeButton.onClick.AddListener(OnUpdateLifetimeData);
		backButton.onClick.AddListener(OnBackButtonPressed);
		}

	#endregion Unity Methods

	#region Dropdown Initialization and Event Handlers

	// --- Initialize the team dropdown using data from DatabaseManager ---
	private void InitializeDropdowns()
		{
		// --- Fetch teams from DatabaseManager (placeholder: "app.csv" or similar data source) ---
		List<Team> allTeams = DatabaseManager.Instance.GetAllTeams();
		if (allTeams == null || allTeams.Count == 0)
			{
			Debug.LogError("No teams found in the database!");
			return;
			}

		// --- Populate team dropdown with team names ---
		teamNameDropdown.ClearOptions();
		List<string> teamNames = allTeams.Select(team => team.name).ToList();
		teamNameDropdown.AddOptions(teamNames);

		// --- Set listener for team selection changes ---
		teamNameDropdown.onValueChanged.AddListener(OnTeamDropdownChanged);
		}

	// --- Handle team selection changes ---
	private void OnTeamDropdownChanged(int teamIndex)
		{
		List<Team> allTeams = DatabaseManager.Instance.GetAllTeams();
		if (allTeams == null || teamIndex < 0 || teamIndex >= allTeams.Count)
			{
			Debug.LogError("Invalid team index selected.");
			return;
			}

		// --- Set the selected team ---
		selectedTeam = allTeams[teamIndex];
		Debug.Log($"Team selected: {selectedTeam.name}");

		// --- Update the player dropdown based on the selected team ---
		UpdatePlayerDropdown();
		}

	// --- Update the player dropdown options based on the selected team ---
	private void UpdatePlayerDropdown()
		{
		playerNameDropdown.ClearOptions();

		if (selectedTeam != null)
			{
			// --- Fetch players for the selected team ---
			List<Player> players = DatabaseManager.Instance.GetPlayersByTeam(selectedTeam.id);
			if (players == null || players.Count == 0)
				{
				playerNameDropdown.AddOptions(new List<string> { "No players available" });
				Debug.LogWarning($"No players found for team: {selectedTeam.name}");
				}
			else
				{
				List<string> playerNames = players.Select(player => player.name).ToList();
				playerNameDropdown.AddOptions(playerNames);
				}

			// --- Set listener for player dropdown selection ---
			playerNameDropdown.onValueChanged.RemoveAllListeners();
			playerNameDropdown.onValueChanged.AddListener(OnPlayerDropdownChanged);
			}
		else
			{
			Debug.LogWarning("No team selected. Cannot update player dropdown.");
			}
		}

	// --- Handle player selection changes ---
	private void OnPlayerDropdownChanged(int playerIndex)
		{
		if (selectedTeam == null)
			{
			Debug.LogError("No team selected. Cannot select a player.");
			return;
			}

		// --- Get list of players from the selected team ---
		List<Player> players = DatabaseManager.Instance.GetPlayersByTeam(selectedTeam.id);
		if (players == null || players.Count == 0 || playerIndex < 0 || playerIndex >= players.Count)
			{
			Debug.LogError("Invalid player selection.");
			ResetLifetimeDataFields();
			return;
			}

		// --- Set the selected player ---
		selectedPlayer = players[playerIndex];
		Debug.Log($"Player selected: {selectedPlayer.name}");

		// --- Populate the lifetime data fields with the player's data ---
		PopulateLifetimeData(selectedPlayer);
		}

	#endregion Dropdown Initialization and Event Handlers

	#region Data Population Methods

	// --- Populate lifetime data fields for the selected player ---
	private void PopulateLifetimeData(Player player)
		{
		if (player == null)
			{
			Debug.LogError("Cannot populate lifetime data because player is null.");
			return;
			}

		lifetimeGamesWonInputField.text = player.lifetimeGamesWon.ToString();
		lifetimeGamesPlayedInputField.text = player.lifetimeGamesPlayed.ToString();
		lifetimeDefensiveShotAvgInputField.text = player.lifetimeDefensiveShotAvg.ToString("F2");
		matchesPlayedInLast2YearsInputField.text = player.matchesPlayedInLast2Years.ToString();
		lifetimeBreakAndRunInputField.text = player.lifetimeBreakAndRun.ToString();
		nineOnTheSnapInputField.text = player.lifetimeNineOnTheSnap.ToString();
		lifetimeMiniSlamsInputField.text = player.lifetimeMiniSlams.ToString();
		lifetimeShutoutsInputField.text = player.lifetimeShutouts.ToString();

		Debug.Log($"Populated lifetime data for player {player.name}");
		}

	// --- Reset all lifetime data input fields to empty ---
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
		Debug.Log("Lifetime data fields have been reset.");
		}

	#endregion Data Population Methods

	#region Panel Methods

	// --- Public method to set player data directly and update UI fields ---
	public void SetPlayerData(Player player)
		{
		if (player == null)
			{
			Debug.LogError("SetPlayerData called with null player.");
			return;
			}

		// --- Directly set the selected player ---
		selectedPlayer = player;
		Debug.Log($"SetPlayerData: {player.name} has been set as the selected player.");

		// --- Populate lifetime data fields for this player ---
		PopulateLifetimeData(player);
		}

	#endregion Panel Methods

	#region Button Event Handlers

	// --- Handle the update lifetime data button click ---
	private void OnUpdateLifetimeData()
		{
		if (selectedPlayer == null)
			{
			Debug.LogError("No player selected! Cannot update lifetime data.");
			return;
			}

		// --- Try parsing all input field values and update player lifetime data ---
		if (!int.TryParse(lifetimeGamesWonInputField.text, out int lifetimeGamesWon) ||
			!int.TryParse(lifetimeGamesPlayedInputField.text, out int lifetimeGamesPlayed) ||
			!float.TryParse(lifetimeDefensiveShotAvgInputField.text, out float lifetimeDefensiveShotAvg) ||
			!int.TryParse(matchesPlayedInLast2YearsInputField.text, out int matchesPlayedInLast2Years) ||
			!int.TryParse(lifetimeBreakAndRunInputField.text, out int lifetimeBreakAndRun) ||
			!int.TryParse(nineOnTheSnapInputField.text, out int lifetimeNineOnTheSnap) ||
			!int.TryParse(lifetimeMiniSlamsInputField.text, out int lifetimeMiniSlams) ||
			!int.TryParse(lifetimeShutoutsInputField.text, out int lifetimeShutouts))
			{
			Debug.LogError("Invalid input data! Please check the fields.");
			return;
			}

		// --- Update the selected player's lifetime stats ---
		selectedPlayer.lifetimeGamesWon = lifetimeGamesWon;
		selectedPlayer.lifetimeGamesPlayed = lifetimeGamesPlayed;
		selectedPlayer.lifetimeDefensiveShotAvg = lifetimeDefensiveShotAvg;
		selectedPlayer.matchesPlayedInLast2Years = matchesPlayedInLast2Years;
		selectedPlayer.lifetimeBreakAndRun = lifetimeBreakAndRun;
		selectedPlayer.lifetimeNineOnTheSnap = lifetimeNineOnTheSnap;
		selectedPlayer.lifetimeMiniSlams = lifetimeMiniSlams;
		selectedPlayer.lifetimeShutouts = lifetimeShutouts;

		// --- Save the updated data to the database (placeholder: DatabaseManager.SavePlayer) ---
		DatabaseManager.Instance.SavePlayer(selectedPlayer);

		Debug.Log($"Updated lifetime data for player {selectedPlayer.name}");
		}

	// --- Handle the back button click to return to the team management panel ---
	private void OnBackButtonPressed()
		{
		// --- Use ShowTeamManagementPanel() as defined in UIManager ---
		UIManager.Instance.ShowTeamManagementPanel();
		Debug.Log("Back button clicked, returning to team management panel.");
		}

	#endregion Button Event Handlers

	#region Additional Functions

	// --- Additional custom functions for lifetime data processing can be added here ---

	#endregion Additional Functions
	}
