using TMPro; // For TextMeshPro

using UnityEngine;
using UnityEngine.UI; // For Button and UI interactions

namespace NickWasHere
	{
	public class TeamPanel:MonoBehaviour
		{
		// References to UI elements
		public TextMeshProUGUI teamNameText;          // Team name text

		public TextMeshProUGUI gamesPlayedText;       // Games played text
		public TextMeshProUGUI gamesWonText;          // Games won text
		public TextMeshProUGUI winPercentageText;     // Win percentage text
		public TextMeshProUGUI totalSkillLevelText;   // Total skill level text
		public Transform playerListScrollView;       // Container for player list items
		public Button detailsButton;                 // Button to show details
		public Button compareButton;                 // Button to compare this team
		public Button backButton;                    // Button to go back

		// Team data
		private Team currentTeam;

		// Start is called before the first frame update
		private void Start()
			{
			// Check if references are set
			if (teamNameText == null || gamesPlayedText == null || gamesWonText == null ||
				winPercentageText == null || totalSkillLevelText == null || playerListScrollView == null ||
				detailsButton == null || compareButton == null || backButton == null)
				{
				Debug.LogError("Missing references in TeamPanel.");
				return;
				}

			// Hook up buttons to methods
			detailsButton.onClick.AddListener(OnDetailsButtonClicked);
			compareButton.onClick.AddListener(OnCompareButtonClicked);
			backButton.onClick.AddListener(OnBackButtonClicked);
			}

		// Method to initialize and display team data
		public void DisplayTeamInfo(Team team)
			{
			currentTeam = team;

			// Set the text fields with the team data
			teamNameText.text = team.teamName;
			gamesPlayedText.text = "Games Played: " + team.GetTotalGamesPlayed();
			gamesWonText.text = "Games Won: " + team.GetTotalGamesWon();
			winPercentageText.text = "Win %: " + team.GetWinPercentage().ToString("F2");
			totalSkillLevelText.text = "Total Skill Level: " + team.GetTotalSkillLevel();

			// Clear the player list and add each player to it
			foreach (Transform child in playerListScrollView)
				{
				Destroy(child.gameObject); // Clear the previous list
				}

			foreach (var player in team.players)
				{
				GameObject playerItem = new("PlayerItem");
				playerItem.transform.SetParent(playerListScrollView);

				// Add TextMeshPro component for displaying player name and skill level
				TextMeshProUGUI playerText = playerItem.AddComponent<TextMeshProUGUI>();
				playerText.text = player.name + " (Skill: " + player.skillLevel + ")";
				}
			}

		// Method called when the Details button is clicked
		private void OnDetailsButtonClicked()
			{
			// Implement logic to show team details (e.g., navigate to a new panel with more information)
			Debug.Log("Details for team: " + currentTeam.teamName);
			}

		// Method called when the Compare button is clicked
		private void OnCompareButtonClicked()
			{
			// Implement logic to compare teams (e.g., open comparison panel)
			Debug.Log("Comparing team: " + currentTeam.teamName);
			}

		// Method called when the Back button is clicked
		private void OnBackButtonClicked()
			{
			// Implement logic to go back to the previous panel (e.g., hide this panel)
			gameObject.SetActive(false); // Hides the team panel
			Debug.Log("Going back to previous panel.");
			}
		}
	}