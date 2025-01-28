using UnityEngine;
using UnityEngine.UI;
using TMPro; // Use TMPro for TextMeshPro support

public class ComparisonResultsPanel:MonoBehaviour
	{
	// --- Panel Elements --- //
	public TextMeshProUGUI headerText; // Header text for the panel
	public TextMeshProUGUI homeTeamNameText; // Text displaying the home team's name
	public TextMeshProUGUI awayTeamNameText; // Text displaying the away team's name
	public GameObject playerComparisonContainer; // Container for displaying player comparison results
	public TextMeshProUGUI winnerPredictionText; // Text displaying the winner prediction
	public Button backButton; // Back button to navigate back to the previous screen

	// --- Initialize the Panel --- //
	public void InitializePanel(string homeTeamName, string awayTeamName, string winnerPrediction)
		{
		homeTeamNameText.text = homeTeamName;
		awayTeamNameText.text = awayTeamName;
		winnerPredictionText.text = winnerPrediction;
		}

	// --- Clear All Results --- //
	public void ClearResults()
		{
		// Clear the player comparison container (removes all children)
		foreach (Transform child in playerComparisonContainer.transform)
			{
			Destroy(child.gameObject);
			}

		// Reset the winner prediction text
		winnerPredictionText.text = "Prediction will appear here";
		}

	// --- Add Result to Comparison --- //
	public void AddResult(string resultText)
		{
		// Create a new GameObject to hold the result text
		GameObject resultObject = new("ResultText");
		resultObject.transform.SetParent(playerComparisonContainer.transform);

		// Add a TextMeshProUGUI component to display the result
		TextMeshProUGUI resultTextComponent = resultObject.AddComponent<TextMeshProUGUI>();
		resultTextComponent.text = resultText;

		// Optionally adjust the text formatting (size, color)
		resultTextComponent.fontSize = 14;
		resultTextComponent.color = Color.white;
		}

	// --- Display the Winner --- //
	public void DisplayWinner(string winner)
		{
		winnerPredictionText.text = "Winner: " + winner;
		}

	// --- Back Button Functionality --- //
	public void OnBackButtonClicked()
		{
		// Call the UIManager to handle the back button logic
		UIManager.Instance.GoBackToPreviousPanel();
		}

	// --- Start Method for Initializing Button Listeners --- //
	private void Start()
		{
		// If backButton is assigned, set up its listener
		if (backButton != null)
			{
			backButton.onClick.AddListener(OnBackButtonClicked);
			}
		}
	}
