using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace NickWasHere
	{
	public class PlayerInputManagerPanel:MonoBehaviour
		{
		// --- References to UI Elements --- //
		[Header("Player Input Fields")]
		public TMP_InputField playerNameInput;

		public TMP_Dropdown skillLevelDropdown;
		public TMP_InputField gamesPlayedInput;
		public TMP_InputField gamesWonInput;
		public TMP_Dropdown teamDropdown;
		public Button addPlayerButton;
		public Button removePlayerButton;
		public Button backButton;

		private DataManager dataManager;

		private void Start()
			{
			// Validate references
			if (playerNameInput == null || skillLevelDropdown == null || gamesPlayedInput == null ||
				gamesWonInput == null || teamDropdown == null || addPlayerButton == null ||
				removePlayerButton == null || backButton == null)
				{
				Debug.LogError("One or more UI references are missing in PlayerInputManagerPanel.");
				return;
				}

			// Hook up button listeners
			addPlayerButton.onClick.AddListener(OnAddPlayerButtonClicked);
			removePlayerButton.onClick.AddListener(OnRemovePlayerButtonClicked);
			backButton.onClick.AddListener(OnBackButtonClicked);

			// Initially disable the add player button
			addPlayerButton.interactable = false;

			// Get the reference to DataManager
			dataManager = DataManager.Instance;
			if (dataManager == null)
				{
				Debug.LogError("DataManager instance not found in the scene.");
				}

			// Subscribe to the OnTeamListChanged event
			dataManager.OnTeamListChanged += RefreshTeamDropdown;

			// Refresh the dropdown initially
			RefreshTeamDropdown();
			}

		// Refresh the team dropdown with updated team list
		private void RefreshTeamDropdown()
			{
			teamDropdown.ClearOptions();
			var teamOptions = dataManager.GetTeamNames(); // Assuming GetTeamNames returns a list of team names
			teamOptions.Insert(0, "Select Team"); // Add default option
			teamDropdown.AddOptions(teamOptions);
			teamDropdown.value = 0; // Reset to "Select Team"
			}

		// --- Button Event Handlers --- //
		private void OnAddPlayerButtonClicked()
			{
			string playerName = playerNameInput.text.Trim();
			int skillLevel = skillLevelDropdown.value;
			if (skillLevel == 0) return;

			if (!int.TryParse(gamesPlayedInput.text, out int gamesPlayed) || gamesPlayed < 0)
				{
				Debug.LogError("Invalid number of games played entered.");
				return;
				}

			if (!int.TryParse(gamesWonInput.text, out int gamesWon) || gamesWon < 0)
				{
				Debug.LogError("Invalid number of games won entered.");
				return;
				}

			if (gamesWon > gamesPlayed)
				{
				Debug.LogError("Games won cannot exceed games played.");
				return;
				}

			string teamName = teamDropdown.options[teamDropdown.value].text;
			if (teamDropdown.value == 0) return;

			if (dataManager.PlayerExists(playerName))
				{
				Debug.LogError($"Player {playerName} already exists.");
				return;
				}

			Player newPlayer = new(playerName, skillLevel, gamesPlayed, gamesWon, teamName);
			dataManager.AddPlayer(newPlayer);
			dataManager.SaveData();

			ClearInputFields();
			}

		private void OnRemovePlayerButtonClicked()
			{
			string playerName = playerNameInput.text.Trim();

			if (string.IsNullOrEmpty(playerName))
				{
				Debug.LogError("Player name is required to remove a player.");
				return;
				}

			// Find the player by name
			Player playerToRemove = DataManager.Instance.players.FirstOrDefault(p => p.name == playerName);
			if (playerToRemove != null)
				{
				// Pass the Player object to the RemovePlayer method
				DataManager.Instance.RemovePlayer(playerToRemove);
				}
			else
				{
				Debug.LogError($"Player {playerName} not found in DataManager.");
				}

			dataManager.SaveData();
			ClearInputFields();
			}


		private void OnBackButtonClicked()
			{
			UIManager.Instance.GoBackToPreviousPanel();
			}

		private void ClearInputFields()
			{
			playerNameInput.text = string.Empty;
			skillLevelDropdown.value = 0;
			gamesPlayedInput.text = string.Empty;
			gamesWonInput.text = string.Empty;
			}
		}
	}