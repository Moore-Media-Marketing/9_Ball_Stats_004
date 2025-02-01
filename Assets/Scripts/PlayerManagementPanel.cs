using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerManagementPanel:MonoBehaviour
	{
	[Header("UI Elements")]
	public TMP_Text headerText;
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField playerNameInputField;
	public Button addPlayerButton;
	public Button deletePlayerButton;
	public Button addPlayerDetailsButton;
	public Button backButton;

	private List<Team> teamList = new List<Team>();
	private List<Player> currentTeamPlayers = new List<Player>();
	private Team selectedTeam;
	private Player selectedPlayer;

	private void Start()
		{
		backButton.onClick.AddListener(OnBackButtonClicked);
		addPlayerButton.onClick.AddListener(OnAddPlayerClicked);
		deletePlayerButton.onClick.AddListener(OnDeletePlayerClicked);
		playerNameDropdown.onValueChanged.AddListener(OnPlayerDropdownValueChanged);

		LoadTeams();
		UpdateTeamDropdown();
		teamNameDropdown.onValueChanged.AddListener(OnTeamDropdownValueChanged);
		}

	private void LoadTeams()
		{
		if (DatabaseManager.Instance != null)
			{
			teamList = DatabaseManager.Instance.GetAllTeams();
			Debug.Log("Loaded " + teamList.Count + " teams.");
			}
		else
			{
			Debug.LogError("DatabaseManager.Instance is null! Cannot load teams.");
			}
		}

	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		Debug.Log("Back button clicked, showing home panel.");
		}

	private void OnAddPlayerClicked()
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

		if (currentTeamPlayers.Any(p => string.Equals(p.Name, playerName, System.StringComparison.OrdinalIgnoreCase)))
			{
			ShowFeedback("Player already exists in the team.");
			return;
			}

		// Create a new player with a valid team association.
		Player newPlayer = new Player(playerName, 5, selectedTeam.Id); // 5 is a placeholder skill level.
		currentTeamPlayers.Add(newPlayer);
		DatabaseManager.Instance.AddPlayer(newPlayer);

		ShowFeedback($"Player '{playerName}' added to team '{selectedTeam.Name}'.");
		playerNameInputField.text = "";
		UpdatePlayerDropdown();
		}

	private void OnDeletePlayerClicked()
		{
		if (playerNameDropdown.value > 0)
			{
			string selectedPlayerName = playerNameDropdown.options[playerNameDropdown.value].text;
			Player playerToDelete = currentTeamPlayers.FirstOrDefault(p => p.Name == selectedPlayerName);
			if (playerToDelete != null)
				{
				currentTeamPlayers.Remove(playerToDelete);
				// (Optional) Implement deletion from database if needed.
				ShowFeedback($"Player '{playerToDelete.Name}' removed from team '{selectedTeam.Name}'.");
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

	private void OnTeamDropdownValueChanged(int index)
		{
		if (index > 0)
			{
			string selectedTeamName = teamNameDropdown.options[index].text;
			selectedTeam = teamList.FirstOrDefault(team => team.Name == selectedTeamName);
			if (selectedTeam != null)
				{
				currentTeamPlayers = DatabaseManager.Instance.GetPlayersByTeam(selectedTeam.Id);
				UpdatePlayerDropdown();
				Debug.Log($"Team selected: {selectedTeam.Name}");
				}
			else
				{
				ShowFeedback("Selected team is not available.");
				}
			}
		else
			{
			playerNameDropdown.ClearOptions();
			selectedTeam = null;
			}
		}

	private void UpdateTeamDropdown()
		{
		teamNameDropdown.ClearOptions();
		List<string> teamNames = new List<string> { "Select Team" };
		if (teamList != null && teamList.Any())
			{
			teamNames.AddRange(teamList.Select(team => team.Name));
			}
		else
			{
			ShowFeedback("No teams available to select.");
			}
		teamNameDropdown.AddOptions(teamNames);
		}

	private void UpdatePlayerDropdown()
		{
		playerNameDropdown.ClearOptions();
		List<string> playerNames = new List<string> { "Select Player" };
		if (currentTeamPlayers != null && currentTeamPlayers.Any())
			{
			playerNames.AddRange(currentTeamPlayers.Select(player => player.Name));
			}
		else
			{
			playerNames.Add("No players available");
			}
		playerNameDropdown.AddOptions(playerNames);
		selectedPlayer = null;
		}

	private void OnPlayerDropdownValueChanged(int index)
		{
		if (index > 0)
			{
			string selectedPlayerName = playerNameDropdown.options[index].text;
			selectedPlayer = currentTeamPlayers.FirstOrDefault(player => player.Name == selectedPlayerName);
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
