using UnityEngine;
using UnityEngine.UI; // For Button functionality
using TMPro; // For TextMeshPro

namespace NickWasHere
	{
	public class MainMenuPanel:MonoBehaviour
		{
		// --- References to UI Buttons --- //
		[Header("Main Menu Buttons")]
		[Tooltip("Button to navigate to the Team Management Panel.")]
		public Button teamManagementButton;

		[Tooltip("Button to navigate to the Player Management Panel.")]
		public Button playerManagementButton;

		[Tooltip("Button to navigate to the Comparison Setup Panel.")]
		public Button comparisonSetupButton;

		[Tooltip("Button to navigate to the Game Analysis Panel.")]
		public Button gameAnalysisButton;

		[Tooltip("Button to exit the application.")]
		public Button exitButton;

		// --- Unity Methods --- //
		private void Start()
			{
			// Validate UI references
			if (teamManagementButton == null || playerManagementButton == null ||
				comparisonSetupButton == null || gameAnalysisButton == null || exitButton == null)
				{
				Debug.LogError("One or more button references are missing in MainMenuPanel.");
				return;
				}

			// Hook up button listeners
			teamManagementButton.onClick.AddListener(OnTeamManagementButtonClicked);
			playerManagementButton.onClick.AddListener(OnPlayerManagementButtonClicked);
			comparisonSetupButton.onClick.AddListener(OnComparisonSetupButtonClicked);
			gameAnalysisButton.onClick.AddListener(OnGameAnalysisButtonClicked);
			exitButton.onClick.AddListener(OnExitButtonClicked);
			}

		// --- Button Event Handlers --- //

		// Handles the Team Management button click
		private void OnTeamManagementButtonClicked()
			{
			Debug.Log("Navigating to Team Management Panel...");
			UIManager.Instance.ShowPanel("TeamManagementPanel"); // Replace with your panel name
			}

		// Handles the Player Management button click
		private void OnPlayerManagementButtonClicked()
			{
			Debug.Log("Navigating to Player Management Panel...");
			UIManager.Instance.ShowPanel("PlayerManagementPanel"); // Replace with your panel name
			}

		// Handles the Comparison Setup button click
		private void OnComparisonSetupButtonClicked()
			{
			Debug.Log("Navigating to Comparison Setup Panel...");
			UIManager.Instance.ShowPanel("ComparisonSetupPanel"); // Replace with your panel name
			}

		// Handles the Game Analysis button click
		private void OnGameAnalysisButtonClicked()
			{
			Debug.Log("Navigating to Game Analysis Panel...");
			UIManager.Instance.ShowPanel("GameAnalysisPanel"); // Replace with your panel name
			}

		// Handles the Exit button click
		private void OnExitButtonClicked()
			{
			Debug.Log("Exiting the application...");
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false; // Exit Play mode in the Unity Editor
#else
            Application.Quit(); // Exit the application in a built version
#endif
			}
		}
	}
