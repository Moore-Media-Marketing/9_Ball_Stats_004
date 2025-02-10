using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import for TextMeshPro

/// <summary>
/// Manages the UI navigation, panel transitions, and feedback system.
/// </summary>
public class UIManager:MonoBehaviour
	{
	// Singleton instance
	public static UIManager Instance { get; private set; }

	// Panels
	[Header("Panels")]
	public GameObject homePanel; // Home panel
	public GameObject settingsPanel; // Settings panel
	public GameObject currentSeasonWeightSettingsPanel; // Current season weight settings panel
	public GameObject lifetimeWeightSettingsPanel; // Lifetime weight settings panel
	public GameObject teamManagementPanel; // Team management panel
	public GameObject playerManagementPanel; // Player management panel
	public GameObject comparisonPanel; // Comparison panel
	public GameObject matchupResultsPanel; // Matchup results panel

	// Feedback System
	[Header("Feedback System")]
	public GameObject feedbackPanel; // UI panel for feedback
	public TMP_Text feedbackText; // UI text for feedback

	// Panel History (for backtracking)
	private List<GameObject> panelHistory = new();

	// Awake method to initialize the instance
	private void Awake()
		{
		// Ensure Singleton pattern
		if (Instance == null)
			{
			Instance = this;
			DontDestroyOnLoad(gameObject); // Persist across scenes if needed
			}
		else
			{
			Destroy(gameObject);
			}
		}

	public void Start()
		{
		// Show the home panel at the start
		ShowHomePanel();
		}

	// --- Show Home Panel --- //
	public void ShowHomePanel()
		{
		homePanel.SetActive(true);
		panelHistory.Add(homePanel);
		DeactivateOtherPanels(homePanel);
		}

	// --- Show Settings Panel --- //
	public void ShowSettingsPanel()
		{
		settingsPanel.SetActive(true);
		panelHistory.Add(settingsPanel);
		DeactivateOtherPanels(settingsPanel);
		}

	// --- Show Current Season Weight Settings Panel --- //
	public void ShowCurrentSeasonWeightSettingsPanel()
		{
		currentSeasonWeightSettingsPanel.SetActive(true);
		panelHistory.Add(currentSeasonWeightSettingsPanel);
		DeactivateOtherPanels(currentSeasonWeightSettingsPanel);
		}

	// --- Show Lifetime Weight Settings Panel --- //
	public void ShowLifetimeWeightSettingsPanel()
		{
		lifetimeWeightSettingsPanel.SetActive(true);
		panelHistory.Add(lifetimeWeightSettingsPanel);
		DeactivateOtherPanels(lifetimeWeightSettingsPanel);
		}

	// --- Show Team Management Panel --- //
	public void ShowTeamManagementPanel()
		{
		teamManagementPanel.SetActive(true);
		panelHistory.Add(teamManagementPanel);
		DeactivateOtherPanels(teamManagementPanel);
		}

	// --- Show Player Management Panel --- //
	public void ShowPlayerManagementPanel()
		{
		playerManagementPanel.SetActive(true);
		panelHistory.Add(playerManagementPanel);
		DeactivateOtherPanels(playerManagementPanel);
		}

	// --- Show Comparison Panel --- //
	public void ShowComparisonPanel()
		{
		comparisonPanel.SetActive(true);
		panelHistory.Add(comparisonPanel);
		DeactivateOtherPanels(comparisonPanel);
		}

	// --- Show Matchup Results Panel --- //
	public void ShowMatchupResultsPanel()
		{
		matchupResultsPanel.SetActive(true);
		panelHistory.Add(matchupResultsPanel);
		DeactivateOtherPanels(matchupResultsPanel);
		}

	// --- Go Back to Previous Panel --- //
	public void GoBackToPreviousPanel()
		{
		if (panelHistory.Count > 1)
			{
			// Pop the current panel from history
			GameObject currentPanel = panelHistory[^1];
			panelHistory.RemoveAt(panelHistory.Count - 1);

			// Show the previous panel
			GameObject previousPanel = panelHistory[^1];
			previousPanel.SetActive(true);
			currentPanel.SetActive(false);
			}
		else
			{
			ShowFeedback("No previous panel to return to.");
			}
		}

	// --- Deactivate all panels except the given one --- //
	private void DeactivateOtherPanels(GameObject activePanel)
		{
		// Deactivate all panels except the active one
		if (homePanel != activePanel) homePanel.SetActive(false);
		if (settingsPanel != activePanel) settingsPanel.SetActive(false);
		if (currentSeasonWeightSettingsPanel != activePanel) currentSeasonWeightSettingsPanel.SetActive(false);
		if (lifetimeWeightSettingsPanel != activePanel) lifetimeWeightSettingsPanel.SetActive(false);
		if (teamManagementPanel != activePanel) teamManagementPanel.SetActive(false);
		if (playerManagementPanel != activePanel) playerManagementPanel.SetActive(false);
		if (comparisonPanel != activePanel) comparisonPanel.SetActive(false);
		if (matchupResultsPanel != activePanel) matchupResultsPanel.SetActive(false);
		}

	// --- Show Feedback Message --- //
	public void ShowFeedback(string message, System.Action callback = null)
		{
		if (feedbackPanel != null && feedbackText != null)
			{
			feedbackText.text = message;
			feedbackPanel.SetActive(true);

			// Auto-hide feedback after 3 seconds
			StartCoroutine(HideFeedbackAfterDelay(3f));

			// Invoke callback if provided
			callback?.Invoke();
			}
		else
			{
			Debug.LogWarning("Feedback panel or text is not assigned in UIManager!");
			}
		}

	// --- Hide Feedback Panel After Delay --- //
	private IEnumerator HideFeedbackAfterDelay(float delay)
		{
		yield return new WaitForSeconds(delay);
		feedbackPanel.SetActive(false);
		}
	}
