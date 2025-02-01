using System;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerManagementPanel:MonoBehaviour
	{
	// --- UI Elements --- //
	public TMP_Text headerText;
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField playerNameInputField;
	public Button addPlayerButton;
	public Button deletePlayerButton;
	public Button addPlayerDetailsButton;
	public Button backButton;

	private List<Team> teamList = new();
	private List<Player> currentTeamPlayers = new();
	private Team selectedTeam;
	private Player selectedPlayer;

	private void Start()
		{
		// --- Button Listeners --- //
		backButton.onClick.AddListener(OnBackButtonClicked);
		addPlayerButton.onClick.AddListener(OnAddPlayerClicked);
		deletePlayerButton.onClick.AddListener(OnDeletePlayerClicked);
		playerNameDropdown.onValueChanged.AddListener(OnPlayerDropdownValueChanged);

		// --- Load Teams from Database --- //
		LoadTeams();

		// --- Populate Team Dropdown --- //
		UpdateTeamDropdown();
		teamNameDropdown.onValueChanged.AddListener(OnTeamDropdownValueChanged);
		}

	// --- Loads Teams from Database --- //
	private void LoadTeams()
		{
		if (DatabaseManager.Instance != null)
			{
			teamList = DatabaseManager.Instance.GetTeams();
			}
		else
			{
			Debug.LogError("DatabaseManager.Instance is null! Cannot load teams.");
			}
		}

	// --- Back Button Click Handler --- //
	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		Debug.Log("Back button clicked, showing home panel.");
		}

	// --- Add Player Handler --- //
	private void OnAddPlayerClicked()
		{
		string playerName = playerNameInputField.text.Trim();

		if (string.IsNullOrEmpty(playerName))
			{
			ShowFeedback("Player name cannot be empty.");
			return;
			}

		if (currentTeamPlayers.Any(p => string.Equals(p.Name, playerName, StringComparison.OrdinalIgnoreCase)))
			{
			ShowFeedback("Player already exists in the team.");
			}
		else
			{
			Player newPlayer = new() { Name = playerName };
			currentTeamPlayers.Add(newPlayer);

			if (selectedTeam != null)
				{
				selectedTeam.SetPlayers(currentTeamPlayers);
				DatabaseManager.Instance.UpdateTeam(selectedTeam);
				ShowFeedback($"Player '{playerName}' added to team '{selectedTeam.Name}'.");

				playerNameInputField.text = "";
				UpdatePlayerDropdown();
				}
			else
				{
				Debug.LogError("Selected team is null when adding player.");
				}
			}
		}

	// --- Delete Player Handler --- //
	private void OnDeletePlayerClicked()
		{
		if (playerNameDropdown.value > 0)
			{
			string selectedPlayerName = playerNameDropdown.options[playerNameDropdown.value].text;
			Player selectedPlayer = currentTeamPlayers.FirstOrDefault(p => p.Name == selectedPlayerName);

			if (selectedPlayer != null)
				{
				currentTeamPlayers.Remove(selectedPlayer);
				selectedTeam.SetPlayers(currentTeamPlayers);
				DatabaseManager.Instance.UpdateTeam(selectedTeam);
				ShowFeedback($"Player '{selectedPlayer.Name}' deleted from team '{selectedTeam.Name}'.");

				UpdatePlayerDropdown();
				}
			else
				{
				ShowFeedback("Player not found in the current team.");
				}
			}
		else
			{
			ShowFeedback("Please select a player to delete.");
			}
		}

	// --- Open Player Lifetime Data Panel --- //
	public void OpenPlayerLifetimeDataPanel()
		{
		Debug.Log("OpenPlayerLifetimeDataPanel clicked");

		if (UIManager.Instance.playerLifetimeDataInputPanel == null)
			{
			Debug.LogError("PlayerLifetimeDataInputPanel is null! Ensure it is assigned in UIManager.");
			return;
			}

		if (selectedTeam == null || selectedPlayer == null)
			{
			Debug.LogWarning("No team or player selected! Opening panel without player data.");
			}

		UIManager.Instance.ShowPanel(UIManager.Instance.playerLifetimeDataInputPanel);
		}

	// --- Team Dropdown Value Changed --- //
	private void OnTeamDropdownValueChanged(int index)
		{
		if (index > 0)
			{
			string selectedTeamName = teamNameDropdown.options[index].text;
			selectedTeam = teamList.FirstOrDefault(team => team.Name == selectedTeamName);

			if (selectedTeam != null)
				{
				Debug.Log($"Team selected: {selectedTeam.Name}");
				currentTeamPlayers = selectedTeam.GetPlayers() ?? new List<Player>();
				UpdatePlayerDropdown();
				}
			else
				{
				Debug.LogWarning("Selected team not found in the list.");
				ShowFeedback("Selected team is not available.");
				}
			}
		else
			{
			playerNameDropdown.ClearOptions();
			selectedTeam = null;
			}
		}

	// --- Updates the Team Dropdown --- //
	private void UpdateTeamDropdown()
		{
		teamNameDropdown.ClearOptions();
		List<string> teamNames = new() { "Select Team" };

		if (teamList != null && teamList.Any())
			{
			teamNames.AddRange(teamList.Select(team => team.Name));
			}
		else
			{
			Debug.LogWarning("No teams available to populate the dropdown.");
			ShowFeedback("No teams available to select.");
			}

		teamNameDropdown.AddOptions(teamNames);
		}

	// --- Updates the Player Dropdown --- //
	private void UpdatePlayerDropdown()
		{
		List<string> playerNames = new() { "Select Player" };
		playerNames.AddRange(currentTeamPlayers.Select(player => player.Name));

		playerNameDropdown.ClearOptions();
		playerNameDropdown.AddOptions(playerNames);
		selectedPlayer = null;
		}

	// --- Player Dropdown Value Changed --- //
	private void OnPlayerDropdownValueChanged(int playerIndex)
		{
		if (playerIndex > 0)
			{
			selectedPlayer = currentTeamPlayers.FirstOrDefault(player => player.Name == playerNameDropdown.options[playerIndex].text);
			if (selectedPlayer != null)
				{
				Debug.Log($"Player selected: {selectedPlayer.Name}");
				}
			else
				{
				Debug.LogError("Selected player not found in the list!");
				}
			}
		else
			{
			selectedPlayer = null;
			}
		}

	// --- Show Feedback --- //
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
	}
