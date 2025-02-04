using TMPro; // Import TextMeshPro namespace

using UnityEngine;
using UnityEngine.UI;

public class MatchupResultsPanel:MonoBehaviour
	{
	// --- Region: UI Elements --- //
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
											   // --- End Region --- //

	// --- Region: Initialization --- //
	private void Start()
		{
		// Check if backButton is assigned and attach the event listener
		if (backButton != null)
			{
			backButton.onClick.AddListener(OnBackButtonClicked);
			}
		else
			{
			Debug.LogError("BackButton is not assigned in the Inspector!");
			}
		}
	// --- End Region --- //

	// --- Region: Button Logic --- //
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
	// --- End Region --- //

	// --- Region: Update UI --- //
	public void UpdateMatchupResults(string header, string teamAName, string teamBName, string matchupResults)
		{
		// --- Comment --- // Update the header text
		headerText.text = header;

		// --- Comment --- // Set the team names for Team A and Team B
		teamAHeaderText.text = teamAName;
		teamBHeaderText.text = teamBName;

		// --- Comment --- // Ensure content exists before attempting to update
		if (matchupsScrollView.content != null)
			{
			// --- Comment --- // Clear previous matchup results before adding new ones
			foreach (Transform child in matchupsScrollView.content)
				{
				Destroy(child.gameObject);
				}

			// --- Comment --- // Ensure prefab is assigned before instantiating
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
	// --- End Region --- //

	// --- Region: Set Matchup Data --- //
	public void SetMatchupData(MatchupResultData resultData, string teamAName, string teamBName)
		{
		// --- Comment --- // Check if resultData is valid
		if (resultData == null)
			{
			Debug.LogError("Received null MatchupResultData!");
			return;
			}

		// --- Comment --- // Set dynamic header
		string header = "Matchup Results";

		// --- Comment --- // Update team names
		teamAHeaderText.text = teamAName;
		teamBHeaderText.text = teamBName;

		// --- Comment --- // Validate matchup data and ensure there are no NaN values
		float teamAWins = resultData.teamAWins;
		float teamBWins = resultData.teamBWins;

		// --- Comment --- // Debug logs to track the values of teamAWins and teamBWins
		Debug.Log($"Team A Wins: {teamAWins}");
		Debug.Log($"Team B Wins: {teamBWins}");

		if (float.IsNaN(teamAWins) || float.IsNaN(teamBWins))
			{
			Debug.LogError("Win percentages contain NaN values. Check data source.");
			Debug.Log($"Calculated Team A Wins: {teamAWins}, Team B Wins: {teamBWins}");

			Debug.LogError($"Team A Wins: {teamAWins}, Team B Wins: {teamBWins}");
			}
		else
			{
			// --- Comment --- // Create and update results text dynamically
			string matchupResults = $"Team A: {teamAWins}%\nTeam B: {teamBWins}%";
			UpdateMatchupResults(header, teamAName, teamBName, matchupResults);
			}
		}
	// --- End Region --- //
	}
