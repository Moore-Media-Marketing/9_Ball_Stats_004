using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MatchupComparisonPanel:MonoBehaviour
	{
	// --- Panel References --- //
	public TMP_Text headerText;
	public TMP_Text selectTeamAText;
	public TMP_Dropdown teamADropdown;
	public GameObject teamAPlayerScrollView;
	public TMP_Text teamBTeamText;
	public TMP_Dropdown teamBDropdown;
	public GameObject teamBPlayerScrollView;
	public TMP_Text compareButtonText;
	public TMP_Text backButtonText;

	// --- Player Toggle Prefab Reference --- //
	public GameObject playerTogglePrefab;

	// --- Matchup Result Panel --- //
	public GameObject matchupResultPanel;
	public TMP_Text matchupResultText;

	// --- Initialization --- //
	private void Start()
		{
		PopulateTeamDropdowns();
		compareButtonText.text = "Compare";
		backButtonText.text = "Back";
		}

	// --- Populate Team Dropdowns --- //
	private void PopulateTeamDropdowns()
		{
		List<Team> teams = DatabaseManager.Instance.LoadTeams();
		List<string> teamNames = new();

		foreach (var team in teams)
			{
			teamNames.Add(team.TeamName);
			}

		teamADropdown.ClearOptions();
		teamADropdown.AddOptions(teamNames);
		teamBDropdown.ClearOptions();
		teamBDropdown.AddOptions(teamNames);
		}

	// --- On Team Selection Changed --- //
	public void OnTeamADropdownChanged(int index)
		{
		Team selectedTeam = DatabaseManager.Instance.LoadTeams()[index];
		PopulateTeamPlayerScrollView(teamAPlayerScrollView, selectedTeam);
		}

	public void OnTeamBDropdownChanged(int index)
		{
		Team selectedTeam = DatabaseManager.Instance.LoadTeams()[index];
		PopulateTeamPlayerScrollView(teamBPlayerScrollView, selectedTeam);
		}

	// --- Populate Team Players ScrollView --- //
	private void PopulateTeamPlayerScrollView(GameObject scrollView, Team team)
		{
		foreach (Transform child in scrollView.transform)
			{
			Destroy(child.gameObject);
			}

		List<Player> teamPlayers = DatabaseManager.Instance.LoadPlayersFromCsv(team.TeamId);

		foreach (var player in teamPlayers)
			{
			GameObject playerToggle = Instantiate(playerTogglePrefab, scrollView.transform);
			TMP_Text playerNameText = playerToggle.GetComponentInChildren<TMP_Text>();
			playerNameText.text = player.PlayerName;
			Toggle playerToggleComponent = playerToggle.GetComponent<Toggle>();
			playerToggleComponent.onValueChanged.AddListener((bool value) => { OnPlayerToggleChanged(value, player); });
			}
		}

	// --- Handle Player Toggle Changes --- //
	private void OnPlayerToggleChanged(bool isSelected, Player player)
		{
		Debug.Log($"Player {player.PlayerName} selected: {isSelected}");
		}

	// --- On Compare Button Clicked --- //
	public void OnCompareButtonClicked()
		{
		string teamAName = teamADropdown.options[teamADropdown.value].text;
		string teamBName = teamBDropdown.options[teamBDropdown.value].text;

		// Load matchups directly from CSV
		List<string[]> matchups = DatabaseManager.Instance.LoadMatchupsFromCsv();
		string[] matchup = matchups.Find(m => m[0] == teamAName && m[1] == teamBName);

		if (matchup != null)
			{
			ShowMatchupResultsPanel(matchup);
			}
		else
			{
			matchupResultText.text = "No matchup data available for this pairing.";
			matchupResultPanel.SetActive(true);
			}
		}

	// --- Show Matchup Results Panel --- //
	private void ShowMatchupResultsPanel(string[] matchup)
		{
		matchupResultPanel.SetActive(true);
		matchupResultText.text = $"{matchup[0]} vs {matchup[1]}\n" +
								 $"Score: {matchup[2]} - {matchup[3]}\n" +
								 $"Winner: {matchup[4]}\n" +
								 $"Win Probabilities: {float.Parse(matchup[5]) * 100}% vs {float.Parse(matchup[6]) * 100}%";
		}

	// --- Back Button --- //
	public void OnBackButtonClicked()
		{
		UIManager.Instance.GoBackToPreviousPanel();
		}
	}
