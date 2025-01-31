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

	private List<Team> teamList = new(); // Assuming a list of teams
	private List<Player> currentTeamPlayers = new(); // Players of the selected team
	private Team selectedTeam;
	private Player selectedPlayer;

	private void Start()
		{
		// Add listeners for buttons
		backButton.onClick.AddListener(OnBackButtonClicked);
		addPlayerButton.onClick.AddListener(OnAddPlayerClicked);
		deletePlayerButton.onClick.AddListener(OnDeletePlayerClicked);
		addPlayerDetailsButton.onClick.AddListener(OnAddPlayerDetailsClicked);

		// Populate the team dropdown with teams
		UpdateTeamDropdown();
		teamNameDropdown.onValueChanged.AddListener(OnTeamDropdownValueChanged);
		}

	private void OnBackButtonClicked()
		{
		// Switch to home panel using UIManager
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		}

	private void UpdateTeamDropdown()
		{
		// Get teams from the database
		teamList = DatabaseManager.Instance.GetAllTeams(); // Replace with actual method to get teams

		List<string> teamNames = teamList.Select(team => team.Name).ToList();
		teamNames.Insert(0, "Select Team"); // Add "Select Team" option

		// Clear and populate dropdown
		teamNameDropdown.ClearOptions();
		teamNameDropdown.AddOptions(teamNames);

		// Reset player dropdown
		playerNameDropdown.ClearOptions();
		}

	private void OnTeamDropdownValueChanged(int index)
		{
		// If a team is selected (index > 0), populate player dropdown
		if (index > 0)
			{
			string selectedTeamName = teamNameDropdown.options[index].text;
			selectedTeam = teamList.FirstOrDefault(team => team.Name == selectedTeamName);

			if (selectedTeam != null)
				{
				// Use the GetPlayers() method from Team class
				currentTeamPlayers = selectedTeam.GetPlayers();
				UpdatePlayerDropdown(); // Update the player dropdown
				}
			}
		else
			{
			// Clear player dropdown if no team is selected
			playerNameDropdown.ClearOptions();
			selectedTeam = null; // Ensure selectedTeam is null
			}
		}

	private void UpdatePlayerDropdown()
		{
		List<string> playerNames = currentTeamPlayers.Select(player => player.Name).ToList();
		playerNames.Insert(0, "Select Player"); // Add "Select Player" option

		playerNameDropdown.ClearOptions();
		playerNameDropdown.AddOptions(playerNames);

		// Ensure selectedPlayer is null initially
		selectedPlayer = null;
		playerNameDropdown.onValueChanged.AddListener(OnPlayerDropdownValueChanged);
		}

	// When a player is selected, set selectedPlayer
	private void OnPlayerDropdownValueChanged(int playerIndex)
		{
		if (playerIndex > 0) // Make sure "Select Player" is not selected
			{
			selectedPlayer = currentTeamPlayers.FirstOrDefault(player => player.Name == playerNameDropdown.options[playerIndex].text);
			}
		else
			{
			selectedPlayer = null;
			}
		}

	private void OnAddPlayerClicked()
		{
		string playerName = playerNameInputField.text;

		if (string.IsNullOrEmpty(playerName))
			{
			ShowFeedback("Player name cannot be empty.");
			return;
			}

		// Check if player already exists (case-insensitive)
		if (currentTeamPlayers.Any(p => string.Equals(p.Name, playerName, StringComparison.OrdinalIgnoreCase)))
			{
			ShowFeedback("Player already exists in the team.");
			}
		else
			{
			// Add new player
			Player newPlayer = new() { Name = playerName };
			currentTeamPlayers.Add(newPlayer);

			// Assuming Team has an UpdatePlayer method
			if (selectedTeam != null)
				{
				DatabaseManager.Instance.UpdateTeam(selectedTeam); // Update team in the database
				ShowFeedback($"Player '{playerName}' added to team '{selectedTeam.Name}'.");

				// Clear input field and refresh dropdowns
				playerNameInputField.text = "";
				UpdatePlayerDropdown();
				}
			}
		}

	private void OnDeletePlayerClicked()
		{
		// Ensure a player is selected
		if (playerNameDropdown.value > 0)
			{
			string selectedPlayerName = playerNameDropdown.options[playerNameDropdown.value].text;
			Player selectedPlayer = currentTeamPlayers.FirstOrDefault(p => p.Name == selectedPlayerName);

			if (selectedPlayer != null)
				{
				currentTeamPlayers.Remove(selectedPlayer);

				// Assuming Team has an UpdatePlayer method
				if (selectedTeam != null)
					{
					DatabaseManager.Instance.UpdateTeam(selectedTeam); // Update team in the database
					ShowFeedback($"Player '{selectedPlayer.Name}' deleted from team '{selectedTeam.Name}'.");

					// Refresh player dropdown
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

		// Ensure selectedTeam and selectedPlayer are set
		if (selectedTeam == null)
			{
			Debug.LogError("selectedTeam is null! Please select a team.");
			return;
			}
		if (selectedPlayer == null)
			{
			Debug.LogError("selectedPlayer is null! Please select a player.");
			return;
			}

		// Open PlayerLifetimeDataInputPanel with selected player data
		PlayerLifetimeDataInputPanel.Instance.OpenWithPlayerData(selectedPlayer);
		}

	private void ShowFeedback(string message)
		{
		// Show feedback (assumed to be on the overlay panel)
		Debug.Log(message); // Replace with actual feedback display logic
		}
	}
