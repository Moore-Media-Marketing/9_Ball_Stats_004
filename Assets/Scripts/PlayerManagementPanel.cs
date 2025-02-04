using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO; // --- Added for File I/O --- //
using Mono.Data.Sqlite; // --- Added for SQLite support --- //

public class PlayerManagementPanel:MonoBehaviour
	{
	#region UI Elements

	[Header("UI Elements")]
	public TMP_Text headerText;
	public Button addPlayerButton;
	public Button deletePlayerButton;
	public Button addPlayerDetailsButton;
	public Button backButton;
	public TMP_InputField playerNameInputField;

	#endregion UI Elements

	#region Private Fields

	private List<Team> teamList = new();
	private List<Player> currentTeamPlayers = new();
	private Team selectedTeam;
	private Player selectedPlayer;

	#endregion Private Fields

	#region Unity Methods

	private void Start()
		{
		backButton.onClick.AddListener(OnBackButtonClicked);
		addPlayerButton.onClick.AddListener(OnAddPlayerClicked);
		deletePlayerButton.onClick.AddListener(OnDeletePlayerClicked);
		addPlayerDetailsButton.onClick.AddListener(OnAddPlayerDetailsButtonClicked);

		DropdownManager.Instance.teamDropdown.onValueChanged.AddListener(OnTeamDropdownValueChanged);
		DropdownManager.Instance.playerNameDropdown.onValueChanged.AddListener(OnPlayerDropdownValueChanged);

		LoadTeams();  // Load teams from SQLite
		DropdownManager.Instance.PopulateTeamsDropdown();
		}

	#endregion Unity Methods

	#region Data Loading and UI Update

	private void LoadTeams()
		{
		teamList = DatabaseManager.Instance.GetAllTeams(); // Load teams from the database
		Debug.Log("Loaded " + teamList.Count + " teams.");
		}

	private void UpdatePlayerDropdown()
		{
		List<string> playerNames = new() { "Select Player" };
		if (currentTeamPlayers != null && currentTeamPlayers.Any())
			{
			playerNames.AddRange(currentTeamPlayers.Select(player => player.name));
			}
		else
			{
			playerNames.Add("No players available");
			}
		DropdownManager.Instance.UpdateDropdown(DropdownManager.Instance.playerNameDropdown, playerNames);
		selectedPlayer = null;
		}

	#endregion Data Loading and UI Update

	#region Button Event Handlers

	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		Debug.Log("Back button clicked, showing home panel.");
		}

	public void OnAddPlayerClicked()
		{
		string playerName = playerNameInputField.text.Trim();
		if (string.IsNullOrEmpty(playerName))
			{
			ShowFeedback("Player name cannot be empty.");
			return;
			}
		if (selectedTeam == null)
			{
			ShowFeedback("Please select a team first.");
			return;
			}
		if (currentTeamPlayers.Any(p => string.Equals(p.name, playerName, System.StringComparison.OrdinalIgnoreCase)))
			{
			ShowFeedback("Player already exists in the team.");
			return;
			}

		Player newPlayer = new(playerName, 5, selectedTeam.id);
		currentTeamPlayers.Add(newPlayer);
		DatabaseManager.Instance.SavePlayersToDatabase(currentTeamPlayers); // Save to SQLite
		ShowFeedback($"Player '{playerName}' added to team '{selectedTeam.name}'.");
		playerNameInputField.text = "";
		UpdatePlayerDropdown();
		}

	private void OnDeletePlayerClicked()
		{
		if (DropdownManager.Instance.playerNameDropdown.value > 0)
			{
			string selectedPlayerName = DropdownManager.Instance.playerNameDropdown.options[DropdownManager.Instance.playerNameDropdown.value].text;
			Player playerToDelete = currentTeamPlayers.FirstOrDefault(p => p.name == selectedPlayerName);
			if (playerToDelete != null)
				{
				currentTeamPlayers.Remove(playerToDelete);
				DatabaseManager.Instance.SavePlayersToDatabase(currentTeamPlayers); // Save to SQLite
				ShowFeedback($"Player '{playerToDelete.name}' removed from team '{selectedTeam.name}'.");
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

	private void OnPlayerDropdownValueChanged(int index)
		{
		if (index > 0)
			{
			string selectedPlayerName = DropdownManager.Instance.playerNameDropdown.options[index].text;
			selectedPlayer = currentTeamPlayers.FirstOrDefault(player => player.name == selectedPlayerName);
			if (selectedPlayer != null)
				{
				Debug.Log($"Player selected: {selectedPlayer.name}");
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

	private void OnTeamDropdownValueChanged(int index)
		{
		if (index > 0)
			{
			string selectedTeamName = DropdownManager.Instance.teamDropdown.options[index].text;
			selectedTeam = teamList.FirstOrDefault(team => team.name == selectedTeamName);
			if (selectedTeam != null)
				{
				currentTeamPlayers = DatabaseManager.Instance.GetPlayersByTeam(selectedTeam.id); // Load players for selected team
				UpdatePlayerDropdown();
				Debug.Log($"Team selected: {selectedTeam.name}");
				}
			else
				{
				ShowFeedback("Selected team is not available.");
				}
			}
		else
			{
			DropdownManager.Instance.playerNameDropdown.ClearOptions();
			selectedTeam = null;
			}
		}

	private void OnAddPlayerDetailsButtonClicked()
		{
		if (selectedTeam == null || selectedPlayer == null)
			{
			ShowFeedback("Please select a team and a player first.");
			return;
			}
		if (UIManager.Instance.playerLifetimeDataInputPanel.TryGetComponent(out PlayerLifetimeDataInputPanel lifetimeDataPanel))
			{
			lifetimeDataPanel.SetPlayerData(selectedPlayer);
			UIManager.Instance.ShowPanel(UIManager.Instance.playerLifetimeDataInputPanel);
			Debug.Log($"Opening Lifetime Data panel for player: {selectedPlayer.name}");
			}
		else
			{
			ShowFeedback("Lifetime Data panel is not available.");
			}
		}

	#endregion Button Event Handlers

	#region Feedback Functions

	private void ShowFeedback(string message)
		{
		if (FeedbackOverlay.Instance != null)
			{
			FeedbackOverlay.Instance.ShowFeedback(message, 2f);
			}
		else
			{
			Debug.LogWarning("FeedbackOverlay.Instance is null, cannot show feedback.");
			}
		}

	#endregion Feedback Functions
	}
