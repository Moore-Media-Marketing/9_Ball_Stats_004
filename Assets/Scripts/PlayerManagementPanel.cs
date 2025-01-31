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
		addPlayerDetailsButton.onClick.AddListener(OnAddPlayerDetailsClicked);

		// --- Populate Team Dropdown --- //
		UpdateTeamDropdown();
		teamNameDropdown.onValueChanged.AddListener(OnTeamDropdownValueChanged);
		}

	// --- Button Click Handlers --- //
	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		}

	private void OnAddPlayerClicked()
		{
		string playerName = playerNameInputField.text;

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
				DatabaseManager.Instance.UpdateTeam(selectedTeam);
				ShowFeedback($"Player '{playerName}' added to team '{selectedTeam.Name}'.");

				playerNameInputField.text = "";
				UpdatePlayerDropdown();
				}
			}
		}

	private void OnDeletePlayerClicked()
		{
		if (playerNameDropdown.value > 0)
			{
			string selectedPlayerName = playerNameDropdown.options[playerNameDropdown.value].text;
			Player selectedPlayer = currentTeamPlayers.FirstOrDefault(p => p.Name == selectedPlayerName);

			if (selectedPlayer != null)
				{
				currentTeamPlayers.Remove(selectedPlayer);
				if (selectedTeam != null)
					{
					DatabaseManager.Instance.UpdateTeam(selectedTeam);
					ShowFeedback($"Player '{selectedPlayer.Name}' deleted from team '{selectedTeam.Name}'.");

					UpdatePlayerDropdown();
					}
				}
			}
		else
			{
			ShowFeedback("Please select a player to delete.");
			}
		}

	private void OnAddPlayerDetailsClicked()
		{
		Debug.Log("OnAddPlayerDetailsClicked called");

		if (PlayerLifetimeDataInputPanel.Instance == null)
			{
			Debug.LogError("PlayerLifetimeDataInputPanel.Instance is null! Ensure it is in the scene.");
			return;
			}

		if (teamNameDropdown == null)
			{
			Debug.LogError("teamNameDropdown is null!");
			return;
			}
		if (playerNameDropdown == null)
			{
			Debug.LogError("playerNameDropdown is null!");
			return;
			}

		if (selectedTeam == null)
			{
			Debug.LogWarning("No team selected! Opening PlayerLifetimeDataInputPanel without player data.");
			PlayerLifetimeDataInputPanel.Instance.OpenWithNoData();
			return;
			}

		if (selectedPlayer == null)
			{
			Debug.LogWarning("No player selected! Opening PlayerLifetimeDataInputPanel without player data.");
			PlayerLifetimeDataInputPanel.Instance.OpenWithNoData();
			return;
			}

		Debug.Log($"Opening PlayerLifetimeDataInputPanel with player: {selectedPlayer.Name}");
		PlayerLifetimeDataInputPanel.Instance.OpenWithPlayerData(selectedPlayer);
		}

	private void OnTeamDropdownValueChanged(int index)
		{
		if (index > 0)
			{
			string selectedTeamName = teamNameDropdown.options[index].text;
			selectedTeam = teamList.FirstOrDefault(team => team.Name == selectedTeamName);

			if (selectedTeam != null)
				{
				Debug.Log($"Team selected: {selectedTeam.Name}");
				currentTeamPlayers = selectedTeam.GetPlayers();
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

	// --- Updates the Team Dropdown --- //
	private void UpdateTeamDropdown()
		{
		// --- Check for Available Teams --- //
		if (teamList != null && teamList.Any())
			{
			// Clear any existing options
			teamNameDropdown.ClearOptions();

			// Add a default "Select Team" option
			List<string> teamNames = new() { "Select Team" };

			// Add each team's name to the dropdown
			teamNames.AddRange(teamList.Select(team => team.Name));

			// Populate the dropdown with team names
			teamNameDropdown.AddOptions(teamNames);
			}
		else
			{
			Debug.LogWarning("No teams available to populate the dropdown.");
			}
		}

	// --- Updates the Player Dropdown --- //
	private void UpdatePlayerDropdown()
		{
		List<string> playerNames = currentTeamPlayers.Select(player => player.Name).ToList();
		playerNames.Insert(0, "Select Player");

		playerNameDropdown.ClearOptions();
		playerNameDropdown.AddOptions(playerNames);

		selectedPlayer = null;
		playerNameDropdown.onValueChanged.AddListener(OnPlayerDropdownValueChanged);
		}

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
