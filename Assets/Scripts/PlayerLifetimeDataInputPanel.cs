using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;

public class PlayerLifetimeDataInputPanel:MonoBehaviour
	{
	// UI Elements
	[Header("UI Elements")]
	public TMP_Text headerText;
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;

	public TMP_InputField lifetimeGamesWonInputField;
	public TMP_InputField lifetimeGamesPlayedInputField;
	public TMP_InputField lifetimeDefensiveShotAverageInputField;
	public TMP_InputField matchesPlayedInLast2YearsInputField;
	public TMP_InputField lifetimeBreakAndRunInputField;
	public TMP_InputField nineOnTheSnapInputField;
	public TMP_InputField lifetimeMiniSlamsInputField;
	public TMP_InputField lifetimeShutoutsInputField;

	public TMP_Text updateLifetimeButtonText;
	public TMP_Text saveToCSVButtonText;
	public TMP_Text backButtonText;

	private List<Team> teams = new();
	private List<Player> players = new();

	// Initialization
	private void Start()
		{
		PopulateTeamDropdown();
		teamNameDropdown.onValueChanged.AddListener(OnTeamDropdownChanged);
		playerNameDropdown.onValueChanged.AddListener(OnPlayerDropdownChanged);

		// Initialize button text
		updateLifetimeButtonText.text = "Update Lifetime";
		saveToCSVButtonText.text = "Save to CSV";
		backButtonText.text = "Back";
		}

	// Populate the Team dropdown
	private void PopulateTeamDropdown()
		{
		teams = DatabaseManager.Instance.LoadTeams();

		if (teams == null || teams.Count == 0)
			{
			Debug.LogWarning("No teams found in the database!");
			teamNameDropdown.ClearOptions();
			return;
			}

		teamNameDropdown.ClearOptions();
		teamNameDropdown.AddOptions(teams.Select(t => t.TeamName).ToList());
		if (teams.Count > 0) OnTeamDropdownChanged(0);
		}

	// Populate the Player dropdown based on selected team
	private void PopulatePlayerDropdown()
		{
		playerNameDropdown.ClearOptions();
		if (players == null || players.Count == 0)
			{
			Debug.LogWarning("No players found for the selected team!");
			return;
			}

		playerNameDropdown.AddOptions(players.Select(p => p.PlayerName).ToList());
		if (players.Count > 0) OnPlayerDropdownChanged(0);
		}

	// Team dropdown change handler
	private void OnTeamDropdownChanged(int selectedIndex)
		{
		if (teams.Count == 0) return;

		Team selectedTeam = teams[selectedIndex];
		players = DatabaseManager.Instance.LoadPlayersByTeam(selectedTeam.TeamId); // Now passing TeamId
		PopulatePlayerDropdown();
		}

	// Player dropdown change handler
	private void OnPlayerDropdownChanged(int selectedIndex)
		{
		if (players.Count == 0) return;

		PopulatePlayerLifetimeData(players[selectedIndex]);
		}

	// Populate player lifetime data into the UI
	private void PopulatePlayerLifetimeData(Player player)
		{
		lifetimeGamesWonInputField.text = player.Stats.LifetimeGamesWon.ToString();
		lifetimeGamesPlayedInputField.text = player.Stats.LifetimeGamesPlayed.ToString();
		lifetimeDefensiveShotAverageInputField.text = player.Stats.LifetimeDefensiveShotAverage.ToString();
		matchesPlayedInLast2YearsInputField.text = player.Stats.LifetimeMatchesPlayedInLast2Years.ToString();
		lifetimeBreakAndRunInputField.text = player.Stats.LifetimeBreakAndRun.ToString();
		nineOnTheSnapInputField.text = player.Stats.LifetimeNineOnTheSnap.ToString();
		lifetimeMiniSlamsInputField.text = player.Stats.LifetimeMiniSlams.ToString();
		lifetimeShutoutsInputField.text = player.Stats.LifetimeShutouts.ToString();
		}

	// Optional: You can add button logic to save updated data to database or CSV
	public void UpdateLifetimeData()
		{
		if (playerNameDropdown.value < 0 || players.Count == 0) return;

		Player selectedPlayer = players[playerNameDropdown.value];
		PlayerStats updatedStats = new()
			{
			LifetimeGamesWon = int.Parse(lifetimeGamesWonInputField.text),
			LifetimeGamesPlayed = int.Parse(lifetimeGamesPlayedInputField.text),
			LifetimeDefensiveShotAverage = float.Parse(lifetimeDefensiveShotAverageInputField.text),
			LifetimeMatchesPlayedInLast2Years = int.Parse(matchesPlayedInLast2YearsInputField.text),
			LifetimeBreakAndRun = int.Parse(lifetimeBreakAndRunInputField.text),
			LifetimeNineOnTheSnap = int.Parse(nineOnTheSnapInputField.text),
			LifetimeMiniSlams = int.Parse(lifetimeMiniSlamsInputField.text),
			LifetimeShutouts = int.Parse(lifetimeShutoutsInputField.text)
			};

		selectedPlayer.Stats = updatedStats;
		DatabaseManager.Instance.SavePlayer(selectedPlayer);

		Debug.Log("Player lifetime data updated.");
		}

	// Optional: Add CSV save logic
	public void SaveToCSV()
		{
		// Logic to save player data to CSV
		}

	// Handle the back button
	public void HandleBackButton()
		{
		Debug.Log("Back button clicked. Returning to previous screen...");
		}
	}
