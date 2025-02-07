using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;

public class PlayerLifetimeDataInputPanel:MonoBehaviour
	{
	// --- Region: UI Elements --- //
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

	// --- Region: Initialization --- //
	private void Start()
		{
		PopulateTeamDropdown();
		teamNameDropdown.onValueChanged.AddListener(OnTeamDropdownChanged);
		playerNameDropdown.onValueChanged.AddListener(OnPlayerDropdownChanged);

		updateLifetimeButtonText.text = "Update Lifetime";
		saveToCSVButtonText.text = "Save to CSV";
		backButtonText.text = "Back";
		}

	// --- Region: Dropdown Population --- //
	private void PopulateTeamDropdown()
		{
		teams = DatabaseManager.Instance.LoadTeams();
		teamNameDropdown.ClearOptions();
		teamNameDropdown.AddOptions(teams.Select(t => t.TeamName).ToList());
		if (teams.Count > 0) OnTeamDropdownChanged(0);
		}

	private void PopulatePlayerDropdown()
		{
		playerNameDropdown.ClearOptions();
		playerNameDropdown.AddOptions(players.Select(p => p.PlayerName).ToList());
		if (players.Count > 0) OnPlayerDropdownChanged(0);
		}

	// --- Region: Dropdown Change Handlers --- //
	private void OnTeamDropdownChanged(int selectedIndex)
		{
		Team selectedTeam = teams[selectedIndex];
		players = DatabaseManager.Instance.LoadPlayersFromCsv(selectedTeam.TeamId);
		PopulatePlayerDropdown();
		}

	private void OnPlayerDropdownChanged(int selectedIndex)
		{
		if (players.Count > 0) PopulatePlayerLifetimeData(players[selectedIndex]);
		}

	// --- Region: Player Data Population --- //
	private void PopulatePlayerLifetimeData(Player player)
		{
		lifetimeGamesWonInputField.text = player.LifetimeGamesWon.ToString();
		lifetimeGamesPlayedInputField.text = player.LifetimeGamesPlayed.ToString();
		lifetimeDefensiveShotAverageInputField.text = player.LifetimeDefensiveShotAverage.ToString("F2");
		matchesPlayedInLast2YearsInputField.text = player.LifetimeMatchesPlayedInLast2Years.ToString();
		lifetimeBreakAndRunInputField.text = player.LifetimeBreakAndRun.ToString();
		nineOnTheSnapInputField.text = player.LifetimeNineOnTheSnap.ToString();
		lifetimeMiniSlamsInputField.text = player.LifetimeMiniSlams.ToString();
		lifetimeShutoutsInputField.text = player.LifetimeShutouts.ToString();
		}

	// --- Region: Button Handlers --- //
	public void OnUpdateLifetimeButtonClicked()
		{
		int selectedPlayerIndex = playerNameDropdown.value;
		if (players.Count == 0 || selectedPlayerIndex >= players.Count) return;

		Player selectedPlayer = players[selectedPlayerIndex];

		if (int.TryParse(lifetimeGamesWonInputField.text, out int gamesWon))
			selectedPlayer.LifetimeGamesWon = gamesWon;
		if (int.TryParse(lifetimeGamesPlayedInputField.text, out int gamesPlayed))
			selectedPlayer.LifetimeGamesPlayed = gamesPlayed;
		if (float.TryParse(lifetimeDefensiveShotAverageInputField.text, out float defensiveAvg))
			selectedPlayer.LifetimeDefensiveShotAverage = defensiveAvg;
		if (int.TryParse(matchesPlayedInLast2YearsInputField.text, out int matchesPlayed))
			selectedPlayer.LifetimeMatchesPlayedInLast2Years = matchesPlayed;
		if (int.TryParse(lifetimeBreakAndRunInputField.text, out int breakAndRun))
			selectedPlayer.LifetimeBreakAndRun = breakAndRun;
		if (int.TryParse(nineOnTheSnapInputField.text, out int nineOnSnap))
			selectedPlayer.LifetimeNineOnTheSnap = nineOnSnap;
		if (int.TryParse(lifetimeMiniSlamsInputField.text, out int miniSlams))
			selectedPlayer.LifetimeMiniSlams = miniSlams;
		if (int.TryParse(lifetimeShutoutsInputField.text, out int shutouts))
			selectedPlayer.LifetimeShutouts = shutouts;

		Debug.Log($"Updated lifetime data for player: {selectedPlayer.PlayerName}");
		}

	public void OnSaveToCSVButtonClicked()
		{
		DatabaseManager.Instance.SavePlayersToCsv(players);
		Debug.Log("Player data saved to CSV.");
		}

	public void OnBackButtonClicked()
		{
		UIManager.Instance.GoBackToPreviousPanel();
		Debug.Log("Back button clicked. Navigating back...");
		}
	}
