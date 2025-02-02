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

		// Update the results in the scroll views (example of text setting)
		matchupsScrollView.content.GetComponentInChildren<TMP_Text>().text = matchupResults;

		// You can also add other relevant UI updates for best matchups, etc.
		}

	// --- End Region ---
	}