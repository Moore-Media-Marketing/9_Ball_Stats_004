using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class UIManager:MonoBehaviour
	{
	// Singleton instance
	public static UIManager Instance { get; private set; }

	// Panels
	[Header("Panels")]
	public GameObject homePanel;
	public GameObject settingsPanel;
	public GameObject playerManagementPanel;
	public GameObject teamManagementPanel;
	public GameObject comparisonPanel;

	// Panel History (a list of panels for backtracking or history)
	public List<GameObject> panelHistory = new();

	// Awake method to initialize the instance
	private void Awake()
		{
		// Check if there's already an instance
		if (Instance == null)
			{
			Instance = this; // Assign the current instance to the static Instance
			DontDestroyOnLoad(gameObject); // Keep the instance between scenes if needed
			}
		else
			{
			Destroy(gameObject); // Destroy duplicate instances
			}
		}

	private void Start()
		{
		// Show the home panel at the start
		ShowHomePanel();
		}

	// --- Show Home Panel --- //
	public void ShowHomePanel()
		{
		homePanel.SetActive(true);
		panelHistory.Add(homePanel); // Add to panel history
		DeactivateOtherPanels(homePanel); // Hide other panels
		}

	// --- Show Settings Panel --- //
	public void ShowSettingsPanel()
		{
		settingsPanel.SetActive(true);
		panelHistory.Add(settingsPanel); // Add to panel history
		DeactivateOtherPanels(settingsPanel); // Hide other panels
		}

	// --- Show Player Management Panel --- //
	public void ShowPlayerManagementPanel()
		{
		playerManagementPanel.SetActive(true);
		panelHistory.Add(playerManagementPanel); // Add to panel history
		DeactivateOtherPanels(playerManagementPanel); // Hide other panels
		}

	// --- Show Team Management Panel --- //
	public void ShowTeamManagementPanel()
		{
		teamManagementPanel.SetActive(true);
		panelHistory.Add(teamManagementPanel); // Add to panel history
		DeactivateOtherPanels(teamManagementPanel); // Hide other panels
		}

	// --- Show Comparison Panel --- //
	public void ShowComparisonPanel()
		{
		comparisonPanel.SetActive(true);
		panelHistory.Add(comparisonPanel); // Add to panel history
		DeactivateOtherPanels(comparisonPanel); // Hide other panels
		}

	// --- Go Back to Previous Panel --- //
	public void GoBackToPreviousPanel()
		{
		if (panelHistory.Count > 1)
			{
			// Pop the current panel from history
			GameObject currentPanel = panelHistory.Last();
			panelHistory.RemoveAt(panelHistory.Count - 1);

			// Show the previous panel
			GameObject previousPanel = panelHistory.Last();
			previousPanel.SetActive(true);
			currentPanel.SetActive(false); // Hide the current panel
			}
		else
			{
			Debug.LogWarning("No previous panel in history.");
			}
		}

	// --- Deactivate all panels except the given one --- //
	private void DeactivateOtherPanels(GameObject activePanel)
		{
		// Deactivate all panels except the active one
		if (homePanel != activePanel) homePanel.SetActive(false);
		if (settingsPanel != activePanel) settingsPanel.SetActive(false);
		if (playerManagementPanel != activePanel) playerManagementPanel.SetActive(false);
		if (teamManagementPanel != activePanel) teamManagementPanel.SetActive(false);
		if (comparisonPanel != activePanel) comparisonPanel.SetActive(false);
		}
	}
