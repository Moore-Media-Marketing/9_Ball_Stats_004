using TMPro; // Use TextMeshPro for text support

using UnityEngine;
using UnityEngine.UI; // For Button component

public class GameAnalysisPanel:MonoBehaviour
	{
	// --- Panel Elements --- //
	public TextMeshProUGUI headerText; // Header text for the panel

	public TextMeshProUGUI gameStatsText; // Text to display game stats
	public Transform oddsScrollView; // ScrollView to display the odds for various player combinations
	public Button analyzeGameButton; // Button to analyze the game
	public Button backButton; // Back button to go back to the previous panel

	// --- Initialize the Panel with Game Stats and Odds --- //
	public void InitializePanel(string gameStats, System.Collections.Generic.List<string> odds)
		{
		// Set game stats text
		gameStatsText.text = gameStats;

		// Populate odds in the scroll view
		PopulateOdds(odds);
		}

	// --- Populate Odds for Different Player Combinations --- //
	private void PopulateOdds(System.Collections.Generic.List<string> odds)
		{
		// Clear previous odds
		foreach (Transform child in oddsScrollView)
			{
			Destroy(child.gameObject);
			}

		// Add each odds combination to the scroll view
		foreach (string odd in odds)
			{
			// Create a new GameObject to hold the odds text
			GameObject oddsObject = new("OddsText");
			oddsObject.transform.SetParent(oddsScrollView);

			// Add a TextMeshPro component to display the odds
			TextMeshProUGUI oddsText = oddsObject.AddComponent<TextMeshProUGUI>();
			oddsText.text = odd;
			oddsText.fontSize = 14;
			oddsText.color = Color.white;
			}
		}

	// --- Analyze Game Button Logic --- //
	private void OnAnalyzeGameButtonClicked()
		{
		// Trigger the game analysis logic
		Debug.Log("Game analysis started.");
		// You can add your actual game analysis logic here (e.g., comparing teams, calculating probabilities)
		}

	// --- Back Button Logic --- //
	private void OnBackButtonClicked()
		{
		// Navigate back to the previous panel
		UIManager.Instance.GoBackToPreviousPanel();
		}

	// --- Start Method for Initializing Button Listeners --- //
	private void Start()
		{
		// Add listener for analyze game button
		if (analyzeGameButton != null)
			{
			analyzeGameButton.onClick.AddListener(OnAnalyzeGameButtonClicked);
			}

		// Add listener for back button
		if (backButton != null)
			{
			backButton.onClick.AddListener(OnBackButtonClicked);
			}
		}
	}