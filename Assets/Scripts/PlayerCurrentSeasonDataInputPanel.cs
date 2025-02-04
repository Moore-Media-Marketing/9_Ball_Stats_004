// --- Region: Using Directives --- //
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;  // --- SQLite Namespace ---
						 // --- End Region: Using Directives --- //

// --- Region: Class Definition --- //
public class PlayercurrentSeasonDataInputPanel:MonoBehaviour
	{
	#region Fields and UI Elements

	[Header("Player Data")]
	[Tooltip("Reference to the currently selected player.")]
	public Player selectedPlayer;  // --- The player whose data is displayed ---

	[Header("UI Elements")]
	[Tooltip("Input field for entering current season games won.")]
	public TMP_InputField GamesWonInputField;  // --- Input for games won ---

	[Tooltip("Input field for entering current season games played.")]
	public TMP_InputField GamesPlayedInputField;  // --- Input for games played ---

	[Tooltip("Input field for entering current season total points.")]
	public TMP_InputField TotalPointsInputField;  // --- Input for total points ---

	[Tooltip("Input field for entering current season points per match.")]
	public TMP_InputField PPMInputField;  // --- Input for points per match ---

	[Tooltip("Input field for entering current season accuracy percentage.")]
	public TMP_InputField PAPercentageInputField;  // --- Input for accuracy percentage ---

	[Tooltip("Input field for entering current season break and run stats.")]
	public TMP_InputField BreakAndRunInputField;  // --- Input for break and run stats ---

	[Tooltip("Input field for entering current season mini slams.")]
	public TMP_InputField MiniSlamsInputField;  // --- Input for mini slams ---

	[Tooltip("Input field for entering current season 9 on the snap.")]
	public TMP_InputField NineOnTheSnapInputField;  // --- Input for 9 on the snap ---

	[Tooltip("Input field for entering current season shutouts.")]
	public TMP_InputField ShutoutsInputField;  // --- Input for shutouts ---

	[Tooltip("Dropdown for selecting the player's current skill level.")]
	public TMP_Dropdown SkillLevelDropdown;  // --- Dropdown for skill level ---

	[Tooltip("Button to go back to the previous panel.")]
	public Button backButton;  // --- Back button for panel navigation ---

	#endregion Fields and UI Elements

	#region Unity Methods

	private void Start()
		{
		backButton.onClick.AddListener(OnBackButtonClicked);
		// --- Initialize the UI with the player's current season data on start ---
		UpdatePlayerData();

		// --- Set up listeners to update player data when fields change ---
		GamesWonInputField.onValueChanged.AddListener(OnGamesWonChanged);
		GamesPlayedInputField.onValueChanged.AddListener(OnGamesPlayedChanged);
		TotalPointsInputField.onValueChanged.AddListener(OnTotalPointsChanged);
		PPMInputField.onValueChanged.AddListener(OnPPMChanged);
		PAPercentageInputField.onValueChanged.AddListener(OnPAPercentageChanged);
		BreakAndRunInputField.onValueChanged.AddListener(OnBreakAndRunChanged);
		MiniSlamsInputField.onValueChanged.AddListener(OnMiniSlamsChanged);
		NineOnTheSnapInputField.onValueChanged.AddListener(OnNineOnTheSnapChanged);
		ShutoutsInputField.onValueChanged.AddListener(OnShutoutsChanged);
		SkillLevelDropdown.onValueChanged.AddListener(OnSkillLevelChanged);

		// --- Refresh dropdowns using DropdownManager ---
		DropdownManager.Instance.RefreshAllDropdowns();
		}

	private void OnBackButtonClicked()
		{
		// --- Return to the home panel using UIManager --- //
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		Debug.Log("MatchupComparisonPanel: Back button clicked, returning to home panel.");
		}

	#endregion Unity Methods

	#region Data Update Methods

	// --- Update the UI with the selected player's current season data ---
	private void UpdatePlayerData()
		{
		if (selectedPlayer != null)
			{
			GamesWonInputField.text = selectedPlayer.currentSeasonGamesWon.ToString();
			GamesPlayedInputField.text = selectedPlayer.currentSeasonGamesPlayed.ToString();
			TotalPointsInputField.text = selectedPlayer.currentSeasonTotalPoints.ToString();
			PPMInputField.text = selectedPlayer.currentSeasonPpm.ToString();
			PAPercentageInputField.text = selectedPlayer.currentSeasonPaPercentage.ToString("F2");  // Format to 2 decimal places
			BreakAndRunInputField.text = selectedPlayer.currentSeasonBreakAndRun.ToString();
			MiniSlamsInputField.text = selectedPlayer.currentSeasonMiniSlams.ToString();
			NineOnTheSnapInputField.text = selectedPlayer.currentSeasonNineOnTheSnap.ToString();
			ShutoutsInputField.text = selectedPlayer.currentSeasonShutouts.ToString();

			// Set the skill level dropdown to the current value
			SkillLevelDropdown.value = selectedPlayer.currentSeasonSkillLevel;
			}
		else
			{
			Debug.LogWarning("No player selected! Please select a player to display their data.");
			}
		}

	#endregion Data Update Methods

	#region Input Field Change Handlers

	// --- Handle the change in the "Games Won" input field ---
	private void OnGamesWonChanged(string value)
		{
		if (selectedPlayer != null && int.TryParse(value, out int gamesWon))
			{
			selectedPlayer.currentSeasonGamesWon = gamesWon;
			SavePlayerData();
			}
		}

	// --- Handle the change in the "Games Played" input field ---
	private void OnGamesPlayedChanged(string value)
		{
		if (selectedPlayer != null && int.TryParse(value, out int gamesPlayed))
			{
			selectedPlayer.currentSeasonGamesPlayed = gamesPlayed;
			SavePlayerData();
			}
		}

	// --- Handle the change in the "Total Points" input field ---
	private void OnTotalPointsChanged(string value)
		{
		if (selectedPlayer != null && int.TryParse(value, out int totalPoints))
			{
			selectedPlayer.currentSeasonTotalPoints = totalPoints;
			SavePlayerData();
			}
		}

	// --- Handle the change in the "Points Per Match" input field ---
	private void OnPPMChanged(string value)
		{
		if (selectedPlayer != null && float.TryParse(value, out float ppm))
			{
			selectedPlayer.currentSeasonPpm = (int) ppm;
			SavePlayerData();
			}
		}

	// --- Handle the change in the "PA Percentage" input field ---
	private void OnPAPercentageChanged(string value)
		{
		if (selectedPlayer != null && float.TryParse(value, out float paPercentage))
			{
			selectedPlayer.currentSeasonPaPercentage = paPercentage;
			SavePlayerData();
			}
		}

	// --- Handle the change in the "Break and Run" input field ---
	private void OnBreakAndRunChanged(string value)
		{
		if (selectedPlayer != null && int.TryParse(value, out int breakAndRun))
			{
			selectedPlayer.currentSeasonBreakAndRun = breakAndRun;
			SavePlayerData();
			}
		}

	// --- Handle the change in the "Mini Slams" input field ---
	private void OnMiniSlamsChanged(string value)
		{
		if (selectedPlayer != null && int.TryParse(value, out int miniSlams))
			{
			selectedPlayer.currentSeasonMiniSlams = miniSlams;
			SavePlayerData();
			}
		}

	// --- Handle the change in the "9 on the Snap" input field ---
	private void OnNineOnTheSnapChanged(string value)
		{
		if (selectedPlayer != null && int.TryParse(value, out int nineOnTheSnap))
			{
			selectedPlayer.currentSeasonNineOnTheSnap = nineOnTheSnap;
			SavePlayerData();
			}
		}

	// --- Handle the change in the "Shutouts" input field ---
	private void OnShutoutsChanged(string value)
		{
		if (selectedPlayer != null && int.TryParse(value, out int shutouts))
			{
			selectedPlayer.currentSeasonShutouts = shutouts;
			SavePlayerData();
			}
		}

	// --- Handle the change in the "Skill Level" dropdown ---
	private void OnSkillLevelChanged(int index)
		{
		if (selectedPlayer != null)
			{
			selectedPlayer.currentSeasonSkillLevel = index;  // Assuming skill level is stored as an integer (0-based index)
			SavePlayerData();
			}
		}

	#endregion Input Field Change Handlers

	#region SQLite Methods

	// --- Method to save player data to SQLite database ---
	private void SavePlayerData()
		{
		string conn = "URI=file:" + Application.persistentDataPath + "/gameDatabase.db";  // Path to SQLite database
		using (var dbConnection = new SqliteConnection(conn))
			{
			dbConnection.Open();
			string query = "UPDATE Players SET currentSeasonGamesWon = @gamesWon, currentSeasonGamesPlayed = @gamesPlayed, " +
						   "currentSeasonTotalPoints = @totalPoints, currentSeasonPpm = @ppm, currentSeasonPaPercentage = @paPercentage, " +
						   "currentSeasonBreakAndRun = @breakAndRun, currentSeasonMiniSlams = @miniSlams, currentSeasonNineOnTheSnap = @nineOnTheSnap, " +
						   "currentSeasonShutouts = @shutouts, currentSeasonSkillLevel = @skillLevel WHERE playerId = @playerId";
			using (var cmd = new SqliteCommand(query, dbConnection))
				{
				cmd.Parameters.AddWithValue("@gamesWon", selectedPlayer.currentSeasonGamesWon);
				cmd.Parameters.AddWithValue("@gamesPlayed", selectedPlayer.currentSeasonGamesPlayed);
				cmd.Parameters.AddWithValue("@totalPoints", selectedPlayer.currentSeasonTotalPoints);
				cmd.Parameters.AddWithValue("@ppm", selectedPlayer.currentSeasonPpm);
				cmd.Parameters.AddWithValue("@paPercentage", selectedPlayer.currentSeasonPaPercentage);
				cmd.Parameters.AddWithValue("@breakAndRun", selectedPlayer.currentSeasonBreakAndRun);
				cmd.Parameters.AddWithValue("@miniSlams", selectedPlayer.currentSeasonMiniSlams);
				cmd.Parameters.AddWithValue("@nineOnTheSnap", selectedPlayer.currentSeasonNineOnTheSnap);
				cmd.Parameters.AddWithValue("@shutouts", selectedPlayer.currentSeasonShutouts);
				cmd.Parameters.AddWithValue("@skillLevel", selectedPlayer.currentSeasonSkillLevel);
				cmd.Parameters.AddWithValue("@playerId", selectedPlayer.playerId);  // Assuming each player has a unique playerId

				cmd.ExecuteNonQuery();
				}
			dbConnection.Close();
			}
		}

	#endregion SQLite Methods

	#region Additional Functions

	// --- Additional custom functions for data input can be added here ---

	#endregion Additional Functions
	}

// --- End Region: Class Definition --- //
