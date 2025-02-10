using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the selection of teams and players for comparison.
/// </summary>
public class MatchupComparisonPanel:MonoBehaviour
	{
	[Header("UI Elements")]
	public TMP_Dropdown teamADropdown;
	public TMP_Dropdown teamBDropdown;
	public GameObject playerTogglePrefab; // Prefab for individual player toggle
	public Button compareButton;
	public Button backButton;

	[Header("UI Containers")]
	public Transform teamAPlayerScrollViewContent; // Content container for Team A player toggles
	public Transform teamBPlayerScrollViewContent; // Content container for Team B player toggles

	private List<Player> selectedTeamAPlayers = new();
	private List<Player> selectedTeamBPlayers = new();

	private void Start()
		{
		PopulateTeamDropdowns();
		compareButton.onClick.AddListener(CompareMatchup);
		backButton.onClick.AddListener(BackToPreviousPanel);
		}

	/// <summary>
	/// Populates team dropdowns with team names.
	/// </summary>
	private void PopulateTeamDropdowns()
		{
		teamADropdown.ClearOptions();
		teamBDropdown.ClearOptions();

		List<Team> teams = DatabaseManager.Instance.GetAllTeams();
		Debug.Log("Teams loaded: " + teams.Count);
		List<string> teamNames = teams.Select(t => t.TeamName).ToList();

		teamADropdown.AddOptions(teamNames);
		teamBDropdown.AddOptions(teamNames);

		teamADropdown.onValueChanged.AddListener(delegate { LoadTeamPlayers(true); });
		teamBDropdown.onValueChanged.AddListener(delegate { LoadTeamPlayers(false); });

		LoadTeamPlayers(true);
		LoadTeamPlayers(false);
		}

	/// <summary>
	/// Loads players for the selected team into the player scroll view.
	/// </summary>
	private void LoadTeamPlayers(bool isTeamA)
		{
		TMP_Dropdown teamDropdown = isTeamA ? teamADropdown : teamBDropdown;
		Transform contentContainer = isTeamA ? teamAPlayerScrollViewContent : teamBPlayerScrollViewContent;

		// Clear previous player list
		foreach (Transform child in contentContainer)
			{
			Destroy(child.gameObject);
			}

		string selectedTeamName = teamDropdown.options[teamDropdown.value].text;
		Team selectedTeam = DatabaseManager.Instance.GetAllTeams().FirstOrDefault(t => t.TeamName == selectedTeamName);

		if (selectedTeam == null)
			{
			Debug.LogError($"Team not found: {selectedTeamName}");
			return;
			}

		List<Player> teamPlayers = DatabaseManager.Instance.GetAllPlayers().Where(p => p.TeamId == selectedTeam.TeamId).ToList();

		foreach (Player player in teamPlayers)
			{
			// Instantiate the player toggle inside the scroll view content container (teamAPlayerScrollViewContent or teamBPlayerScrollViewContent)
			GameObject toggleObj = Instantiate(playerTogglePrefab, contentContainer);
			Toggle playerToggle = toggleObj.GetComponent<Toggle>();
			TMP_Text playerText = toggleObj.GetComponentInChildren<TMP_Text>();

			playerText.text = $"{player.PlayerName} (Skill {player.Stats.CurrentSeasonSkillLevel})";

			// Add listener to add/remove the player from the selected list
			playerToggle.onValueChanged.AddListener(delegate
				{
					if (playerToggle.isOn)
						{
						if (isTeamA) selectedTeamAPlayers.Add(player);
						else selectedTeamBPlayers.Add(player);
						}
					else
						{
						if (isTeamA) selectedTeamAPlayers.Remove(player);
						else selectedTeamBPlayers.Remove(player);
						}
					});
			}
		}

	/// <summary>
	/// Compares the selected teams and players.
	/// </summary>
	private void CompareMatchup()
		{
		if (!HandicapSystem.IsValidTeamSelection(selectedTeamAPlayers) || !HandicapSystem.IsValidTeamSelection(selectedTeamBPlayers))
			{
			Debug.LogError("Invalid team selection based on the 23-Rule.");
			return;
			}

		MatchupResultsPanel.Instance.DisplayMatchupResults(selectedTeamAPlayers, selectedTeamBPlayers);
		}

	/// <summary>
	/// Returns to the previous panel.
	/// </summary>
	private void BackToPreviousPanel()
		{
		gameObject.SetActive(false);
		}
	}
