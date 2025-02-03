using TMPro;  // Import TextMeshPro namespace

using UnityEngine;
using UnityEngine.UI;

public class MatchupResultsPanel:MonoBehaviour
	{
	// --- Region: UI Elements ---
	[Header("UI Elements")]
	[Tooltip("The header text at the top of the panel")]
	public TMP_Text headerText;

	[Tooltip("Header text for Team A")]
	public TMP_Text teamAHeaderText;

	[Tooltip("Header text for Team B")]
	public TMP_Text teamBHeaderText;

	[Tooltip("Scrollable area for matchup results")]
	public ScrollRect matchupsScrollView;

	[Tooltip("Header text for best matchups")]
	public TMP_Text bestMatchupHeaderText;

	[Tooltip("Scrollable area for best matchups")]
	public ScrollRect bestMatchupsScrollView;

	[Tooltip("Button to navigate back to the home panel")]
	public Button backButton;

	[Tooltip("Prefab for displaying matchup results")]
	public TMP_Text matchupResultsTextPrefab;  // Ensure this is assigned in the Inspector!

	// --- End Region ---

	// --- Region: Initialization ---
	private void Start()
		{
		if (backButton != null)
			{
			backButton.onClick.AddListener(OnBackButtonClicked);
			}
		else
			{
			Debug.LogError("BackButton is not assigned in the Inspector!");
			}
		}
	// --- End Region ---

	// --- Region: Button Logic ---
	private void OnBackButtonClicked()
		{
		// Use UIManager to switch back to the home panel
		if (UIManager.Instance != null)
			{
			UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
			}
		else
			{
			Debug.LogError("UIManager instance is null! Cannot navigate back.");
			}
		}
	// --- End Region ---

	// --- Region: Update UI ---
	public void UpdateMatchupResults(string header, string teamAName, string teamBName, string matchupResults)
		{
		// Update the header text
		headerText.text = header;

		// Set the team names for Team A and Team B
		teamAHeaderText.text = teamAName;
		teamBHeaderText.text = teamBName;

		// Ensure content exists before attempting to update
		if (matchupsScrollView.content != null)
			{
			// Clear previous matchup results before adding new ones
			foreach (Transform child in matchupsScrollView.content)
				{
				Destroy(child.gameObject);
				}

			// Ensure prefab is assigned before instantiating
			if (matchupResultsTextPrefab != null)
				{
				TMP_Text resultTextInstance = Instantiate(matchupResultsTextPrefab, matchupsScrollView.content);
				resultTextInstance.text = matchupResults;
				}
			else
				{
				Debug.LogError("matchupResultsTextPrefab is not assigned in the Inspector!");
				}
			}
		else
			{
			Debug.LogError("No content found in matchupsScrollView.");
			}
		}
	// --- End Region ---

	// --- Region: Set Matchup Data ---
	public void SetMatchupData(MatchupResultData resultData, string teamAName, string teamBName)
		{
		if (resultData == null)
			{
			Debug.LogError("Received null MatchupResultData!");
			return;
			}

		// Set dynamic header
		string header = "Matchup Results";

		// Update team names
		teamAHeaderText.text = teamAName;
		teamBHeaderText.text = teamBName;

		// Validate matchup data
		float teamAWins = resultData.teamAWins;
		float teamBWins = resultData.teamBWins;
		float totalMatches = teamAWins + teamBWins;

		if (float.IsNaN(teamAWins) || float.IsNaN(teamBWins))
			{
			Debug.LogError("Win percentage contains NaN values. Check data source.");
			return;
			}

		if (teamAWins < 0 || teamBWins < 0)
			{
			Debug.LogError("Win counts cannot be negative.");
			return;
			}

		// Calculate and display win percentages safely
		string matchupResults;
		if (totalMatches > 0)
			{
			float teamAWinPercentage = (teamAWins / totalMatches) * 100f;
			float teamBWinPercentage = (teamBWins / totalMatches) * 100f;
			matchupResults = $"Team A Wins: {teamAWinPercentage:F2}%\nTeam B Wins: {teamBWinPercentage:F2}%";
			}
		else
			{
			matchupResults = "No valid matches played.";
			Debug.LogWarning("Total matches played is zero. Win percentages cannot be calculated.");
			}

		// Update the UI with calculated results
		UpdateMatchupResults(header, teamAName, teamBName, matchupResults);
		}
	// --- End Region ---
	}
