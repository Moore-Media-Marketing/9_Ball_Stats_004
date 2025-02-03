using TMPro;  // Import TMP namespace

using UnityEngine;
using UnityEngine.UI;

public class MatchupResultsPanel:MonoBehaviour
	{
	// --- Region: UI Elements ---
	public TMP_Text headerText;  // The header text at the top of the panel
	public TMP_Text teamAHeaderText;  // Header text for Team A
	public TMP_Text teamBHeaderText;  // Header text for Team B
	public ScrollRect matchupsScrollView;  // Scrollable area for matchup results
	public TMP_Text bestMatchupHeaderText;  // Header text for best matchups
	public ScrollRect bestMatchupsScrollView;  // Scrollable area for best matchups
	public Button backButton;  // Button to navigate back to the home panel
							   // --- End Region ---

	// --- Region: Initialization ---
	private void Start()
		{
		// Add listener for the back button click
		backButton.onClick.AddListener(OnBackButtonClicked);
		}
	// --- End Region ---

	// --- Region: Button Logic ---
	private void OnBackButtonClicked()
		{
		// Use UIManager to switch back to the home panel
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		}
	// --- End Region ---

	// --- Region: Update UI ---
	// This method will be called to update the matchup results
	public void UpdateMatchupResults(string header, string teamAName, string teamBName, string matchupResults)
		{
		// Update the header text
		headerText.text = header;

		// Set the team names for Team A and Team B
		teamAHeaderText.text = teamAName;
		teamBHeaderText.text = teamBName;

		// Safely update the results in the scroll view
		if (matchupsScrollView.content != null)
			{
			// Clear previous matchup results before adding new ones
			foreach (Transform child in matchupsScrollView.content)
				{
				Destroy(child.gameObject);
				}

			// Instantiate a new entry from the prefab and set its text
			TMP_Text resultTextInstance = Instantiate(matchupResultsTextPrefab, matchupsScrollView.content);
			resultTextInstance.text = matchupResults;
			}
		else
			{
			Debug.LogError("No content found in matchupsScrollView.");
			}

		// You can also add other relevant UI updates for best matchups, etc.
		}
	// --- End Region ---

	// --- Region: Set Matchup Data ---
	// This method is added to match the expected call in MatchupComparisonPanel
	public void SetMatchupData(MatchupResultData resultData, string teamAName, string teamBName)
		{
		// Use the data from the resultData to update the UI as needed
		string header = "Matchup Results";

		// Update the team names dynamically based on the selected teams
		teamAHeaderText.text = teamAName;
		teamBHeaderText.text = teamBName;

		// Safely calculate the win percentages
		float teamAWins = resultData.teamAWins;
		float teamBWins = resultData.teamBWins;

		if (teamAWins < 0 || teamBWins < 0)
			{
			Debug.LogError("Invalid win percentages: teamAWins or teamBWins are less than zero.");
			}

		// Check if the win percentages are valid before calculating the results
		string matchupResults = string.Empty;

		if (teamAWins >= 0 && teamBWins >= 0)
			{
			// Safely calculate and format the results
			matchupResults = $"Team A Wins: {teamAWins}%\nTeam B Wins: {teamBWins}%";
			}
		else
			{
			// Log error if calculations are invalid
			Debug.LogError("Invalid win percentage calculation.");
			matchupResults = "Invalid matchup results.";
			}

		// Update the results in the UI
		UpdateMatchupResults(header, teamAName, teamBName, matchupResults);
		}

	// --- End Region ---

	// --- Region: Prefab References ---
	public TMP_Text matchupResultsTextPrefab;  // The prefab for the result text (assign this in the Inspector)
											   // --- End Region ---
	}
