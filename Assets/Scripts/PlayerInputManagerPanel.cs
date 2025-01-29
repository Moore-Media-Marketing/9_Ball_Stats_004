using UnityEngine;
using UnityEngine.UI; // For UI components
using TMPro; // For TextMeshPro components

namespace NickWasHere
	{
	public class PlayerInputManagerPanel:MonoBehaviour
		{
		// --- References to UI Elements --- //
		[Header("Player Input Fields")]
		[Tooltip("Input field for the player's name.")]
		public TMP_InputField playerNameInput;

		[Tooltip("Dropdown for selecting the player's skill level.")]
		public TMP_Dropdown skillLevelDropdown;

		[Tooltip("Input field for the games played.")]
		public TMP_InputField gamesPlayedInput;

		[Tooltip("Input field for the games won.")]
		public TMP_InputField gamesWonInput;

		[Tooltip("Dropdown for selecting the team.")]
		public TMP_Dropdown teamDropdown;

		[Tooltip("Button to add the player.")]
		public Button addPlayerButton;

		[Tooltip("Button to remove the player.")]
		public Button removePlayerButton;

		[Tooltip("Button to return to the previous panel.")]
		public Button backButton;

		// Reference to DataManager
		private DataManager dataManager;

		// --- Unity Methods --- //
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

			// Add validation listeners
			playerNameInput.onValueChanged.AddListener(ValidateInput);
			skillLevelDropdown.onValueChanged.AddListener(ValidateInput);
			gamesPlayedInput.onValueChanged.AddListener(ValidateInput);
			gamesWonInput.onValueChanged.AddListener(ValidateInput);

			// Initially disable the add player button
			addPlayerButton.interactable = false;

			// Get the reference to DataManager
			dataManager = Object.FindAnyObjectByType<DataManager>();
			if (dataManager == null)
				{
				Debug.LogError("DataManager instance not found in the scene.");
				}
			}

		// --- Button Event Handlers --- //

		// Handles the Add Player button click
		private void OnAddPlayerButtonClicked()
			{
			string playerName = playerNameInput.text.Trim();
			int skillLevel = skillLevelDropdown.value + 1;  // Assuming skill level dropdown values start from 0
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

			// Assuming DataManager exists to handle player creation and storage
			if (dataManager != null)
				{
				// Add the player to the DataManager (team name is assumed to be managed internally)
				dataManager.AddPlayer(playerName, skillLevel, gamesPlayed, gamesWon);
				}
			else
				{
				Debug.LogError("DataManager is not initialized.");
				}

			// Clear fields after adding the player
			ClearInputFields();
			}

		// Handles the Remove Player button click
		private void OnRemovePlayerButtonClicked()
			{
			string playerName = playerNameInput.text.Trim();

			if (string.IsNullOrEmpty(playerName))
				{
				Debug.LogError("Player name is required to remove a player.");
				return;
				}

			// Assuming DataManager has a method to remove players
			if (dataManager != null)
				{
				// Remove the player from the DataManager
				dataManager.RemovePlayer(playerName);
				}
			else
				{
				Debug.LogError("DataManager is not initialized.");
				}

			// Clear fields after removing the player
			ClearInputFields();
			}

		// Handles the Back button click
		private void OnBackButtonClicked()
			{
			Debug.Log("Navigating to the previous panel...");
			UIManager.Instance.GoBackToPreviousPanel();
			}

		// --- Helper Methods --- //

		// Clears the input fields
		private void ClearInputFields()
			{
			playerNameInput.text = string.Empty;
			skillLevelDropdown.value = 0;  // Reset to first option (or adjust according to default)
			gamesPlayedInput.text = string.Empty;
			gamesWonInput.text = string.Empty;
			addPlayerButton.interactable = false; // Disable add player button
			}

		// Validates the input fields
		private void ValidateInput(string _)
			{
			bool isNameValid = !string.IsNullOrWhiteSpace(playerNameInput.text);
			bool isSkillLevelValid = skillLevelDropdown.value >= 0;  // Ensure the skill level is valid (dropdown index)
			bool areGamesValid = int.TryParse(gamesPlayedInput.text, out int gamesPlayed) && gamesPlayed >= 0 &&
								 int.TryParse(gamesWonInput.text, out int gamesWon) && gamesWon >= 0;

			// Enable the add player button only if all inputs are valid
			addPlayerButton.interactable = isNameValid && isSkillLevelValid && areGamesValid;
			}

		// Overloaded validation for skill level dropdown
		private void ValidateInput(int _)
			{
			bool isNameValid = !string.IsNullOrWhiteSpace(playerNameInput.text);
			bool isSkillLevelValid = skillLevelDropdown.value >= 0;  // Ensure the skill level is valid (dropdown index)
			bool areGamesValid = int.TryParse(gamesPlayedInput.text, out int gamesPlayed) && gamesPlayed >= 0 &&
								 int.TryParse(gamesWonInput.text, out int gamesWon) && gamesWon >= 0;

			// Enable the add player button only if all inputs are valid
			addPlayerButton.interactable = isNameValid && isSkillLevelValid && areGamesValid;
			}
		}
	}
