using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrentSeasonDataInputPanel:MonoBehaviour
	{
	// --- UI Elements --- //
	public TMP_Text headerText;
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField gamesWonInputField;
	public TMP_InputField gamesPlayedInputField;
	public TMP_InputField totalPointsInputField;
	public TMP_InputField ppmInputField;
	public TMP_InputField paPercentageInputField;
	public TMP_InputField breakAndRunInputField;
	public TMP_InputField miniSlamsInputField;
	public TMP_InputField nineOnTheSnapInputField;
	public TMP_InputField shutoutsInputField;
	public TMP_Dropdown skillLevelDropdown;
	public Button addPlayerButton;
	public Button removePlayerButton;
	public Button backButton;

	private Team selectedTeam;
	private Player selectedPlayer;

	private void Start()
		{
		// --- Button Listeners --- //
		backButton.onClick.AddListener(OnBackButtonClicked);
		addPlayerButton.onClick.AddListener(OnAddPlayerClicked);
		removePlayerButton.onClick.AddListener(OnRemovePlayerClicked);

		// --- Populate Dropdowns --- //
		teamNameDropdown.onValueChanged.AddListener(OnTeamDropdownValueChanged);
		playerNameDropdown.onValueChanged.AddListener(OnPlayerDropdownValueChanged);
		}

	// --- Handle Back Button Click --- //
	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel); // Switch to the Home Panel
		}

	// --- Handle Add Player Data Click --- //
	private void OnAddPlayerClicked()
		{
		if (selectedPlayer == null)
			{
			ShowFeedback("Please select a player.");
			return;
			}

		// Parse the input fields
		if (!int.TryParse(gamesWonInputField.text, out int gamesWon) ||
			!int.TryParse(gamesPlayedInputField.text, out int gamesPlayed) ||
			!int.TryParse(totalPointsInputField.text, out int totalPoints) ||
			!float.TryParse(ppmInputField.text, out float ppm) ||
			!float.TryParse(paPercentageInputField.text, out float paPercentage) ||
			!float.TryParse(breakAndRunInputField.text, out float breakAndRun) ||
			!float.TryParse(miniSlamsInputField.text, out float miniSlams) ||
			!float.TryParse(nineOnTheSnapInputField.text, out float nineOnTheSnap) ||
			!float.TryParse(shutoutsInputField.text, out float shutouts))
			{
			ShowFeedback("Please enter valid values for all fields.");
			return;
			}

		// Assign the current season data directly to the player's properties
		selectedPlayer.CurrentSeasonMatchesWon = gamesWon;
		selectedPlayer.CurrentSeasonMatchesPlayed = gamesPlayed;
		selectedPlayer.CurrentSeasonTotalPoints = totalPoints;
		selectedPlayer.CurrentSeasonPpm = (int) ppm;
		selectedPlayer.CurrentSeasonPaPercentage = paPercentage;
		selectedPlayer.CurrentSeasonBreakAndRun = (int) breakAndRun;
		selectedPlayer.CurrentSeasonMiniSlams = (int) miniSlams;
		selectedPlayer.CurrentSeasonNineOnTheSnap = (int) nineOnTheSnap;
		selectedPlayer.CurrentSeasonShutouts = (int) shutouts;

		// FIX: Use the dropdown index for skill level
		selectedPlayer.CurrentSeasonSkillLevel = skillLevelDropdown.value; // Store index as int instead of string

		ShowFeedback($"Current season data added for {selectedPlayer.Name}.");
		}

	// --- Handle Remove Player Data Click --- //
	private void OnRemovePlayerClicked()
		{
		if (selectedPlayer == null)
			{
			ShowFeedback("Please select a player.");
			return;
			}

		// Clear the current season data for the selected player
		selectedPlayer.CurrentSeasonMatchesWon = 0;
		selectedPlayer.CurrentSeasonMatchesPlayed = 0;
		selectedPlayer.CurrentSeasonTotalPoints = 0;
		selectedPlayer.CurrentSeasonPpm = 0;
		selectedPlayer.CurrentSeasonPaPercentage = 0;
		selectedPlayer.CurrentSeasonBreakAndRun = 0;
		selectedPlayer.CurrentSeasonMiniSlams = 0;
		selectedPlayer.CurrentSeasonNineOnTheSnap = 0;
		selectedPlayer.CurrentSeasonShutouts = 0;
		selectedPlayer.CurrentSeasonSkillLevel = -1; // Reset skill level to a default value

		// Clear the input fields after removal
		ClearInputFields();

		ShowFeedback($"Current season data removed for {selectedPlayer.Name}.");
		}

	// --- Handle Team Dropdown Selection --- //
	private void OnTeamDropdownValueChanged(int index)
		{
		if (index > 0)
			{
			string selectedTeamName = teamNameDropdown.options[index].text;

			// Use the new method to retrieve the team
			selectedTeam = DatabaseManager.Instance.GetTeamByName(selectedTeamName);

			if (selectedTeam != null)
				{
				Debug.Log($"Team selected: {selectedTeam.Name}");
				UpdatePlayerDropdown();
				}
			else
				{
				Debug.LogWarning("Team not found!");
				}
			}
		else
			{
			playerNameDropdown.ClearOptions();
			selectedTeam = null;
			}
		}


	// --- Handle Player Dropdown Selection --- //
	private void OnPlayerDropdownValueChanged(int playerIndex)
		{
		if (playerIndex > 0)
			{
			string selectedPlayerName = playerNameDropdown.options[playerIndex].text;

			selectedPlayer = selectedTeam.GetPlayers().FirstOrDefault(player => player.Name == selectedPlayerName);

			if (selectedPlayer != null)
				{
				Debug.Log($"Player selected: {selectedPlayer.Name}");
				PopulateInputFieldsWithCurrentSeasonData(selectedPlayer);
				}
			else
				{
				Debug.LogError("Selected player not found in the team!");
				}
			}
		else
			{
			selectedPlayer = null;
			ClearInputFields();
			}
		}

	// --- Populate Input Fields with Existing Data --- //
	private void PopulateInputFieldsWithCurrentSeasonData(Player player)
		{
		if (player != null)
			{
			gamesWonInputField.text = player.CurrentSeasonMatchesWon.ToString();
			gamesPlayedInputField.text = player.CurrentSeasonMatchesPlayed.ToString();
			totalPointsInputField.text = player.CurrentSeasonTotalPoints.ToString();
			ppmInputField.text = player.CurrentSeasonPpm.ToString();
			paPercentageInputField.text = player.CurrentSeasonPaPercentage.ToString();
			breakAndRunInputField.text = player.CurrentSeasonBreakAndRun.ToString();
			miniSlamsInputField.text = player.CurrentSeasonMiniSlams.ToString();
			nineOnTheSnapInputField.text = player.CurrentSeasonNineOnTheSnap.ToString();
			shutoutsInputField.text = player.CurrentSeasonShutouts.ToString();

			// FIX: Use index for skill level
			skillLevelDropdown.value = player.CurrentSeasonSkillLevel;
			}
		else
			{
			ClearInputFields();
			}
		}

	// --- Clear All Input Fields --- //
	private void ClearInputFields()
		{
		gamesWonInputField.text = "";
		gamesPlayedInputField.text = "";
		totalPointsInputField.text = "";
		ppmInputField.text = "";
		paPercentageInputField.text = "";
		breakAndRunInputField.text = "";
		miniSlamsInputField.text = "";
		nineOnTheSnapInputField.text = "";
		shutoutsInputField.text = "";
		skillLevelDropdown.value = 0; // Set to default value
		}

	// --- Show Feedback Message --- //
	private void ShowFeedback(string message)
		{
		if (FeedbackOverlay.Instance != null)
			{
			FeedbackOverlay.Instance.ShowFeedback(message, 2f);
			}
		else
			{
			Debug.LogError("FeedbackOverlay.Instance is null!");
			}
		}

	// --- Update Player Dropdown --- //
	private void UpdatePlayerDropdown()
		{
		if (selectedTeam != null)
			{
			var playerOptions = selectedTeam.GetPlayers()
				.Select(player => new TMP_Dropdown.OptionData(player.Name))
				.ToList();

			playerNameDropdown.ClearOptions();
			playerNameDropdown.AddOptions(playerOptions);
			}
		}
	}
