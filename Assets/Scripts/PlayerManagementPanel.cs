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
			Team selectedTeam = teamList.FirstOrDefault(team => team.Name == selectedTeamName);

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
			}
		}

	private void UpdatePlayerDropdown()
		{
		List<string> playerNames = currentTeamPlayers.Select(player => player.Name).ToList();
		playerNames.Insert(0, "Select Player"); // Add "Select Player" option

		playerNameDropdown.ClearOptions();
		playerNameDropdown.AddOptions(playerNames);
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
			Team selectedTeam = teamList.FirstOrDefault(t => t.Name == teamNameDropdown.options[teamNameDropdown.value].text);
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
				Team selectedTeam = teamList.FirstOrDefault(t => t.Name == teamNameDropdown.options[teamNameDropdown.value].text);
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
		if (playerNameDropdown.value > 0) // Check if a player is selected
			{
			string selectedPlayerName = playerNameDropdown.options[playerNameDropdown.value].text;
			Player selectedPlayer = currentTeamPlayers.FirstOrDefault(p => p.Name == selectedPlayerName);

			if (selectedPlayer != null)
				{
				// Open PlayerLifetimeDataInputPanel and populate with selected player data
				PlayerLifetimeDataInputPanel.Instance.OpenWithPlayerData(selectedPlayer);
				}
			}
		else
			{
			// Open PlayerLifetimeDataInputPanel without any player data
			PlayerLifetimeDataInputPanel.Instance.OpenWithoutData();
			}
		}

	private void ShowFeedback(string message)
		{
		// Show feedback (assumed to be on the overlay panel)
		Debug.Log(message); // Replace with actual feedback display logic
		}
	}
