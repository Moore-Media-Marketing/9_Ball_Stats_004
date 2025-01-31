using UnityEngine;

public class HomePanel:MonoBehaviour
	{
	#region UI Elements

	[Header("Panels")]
	public GameObject teamManagementPanel;

	public GameObject playerManagementPanel;
	public GameObject matchupComparisonPanel;
	public GameObject settingsPanel;
	public GameObject overlayFeedbackPanel;

	[Header("Buttons")]
	public GameObject teamManagementButton;

	public GameObject playerManagementButton;
	public GameObject matchupComparisonButton;
	public GameObject settingsButton;

	#endregion UI Elements

	#region Singleton

	public static HomePanel Instance;

	private void Awake()
		{
		// Singleton pattern for HomePanel
		if (Instance == null)
			{
			Instance = this;
			}
		else
			{
			Destroy(gameObject);
			}

		// Make sure all panels are hidden initially
		HideAllPanels();
		}

	#endregion Singleton

	#region Panel Navigation Methods

	// Show Team Management Panel
	public void ShowTeamManagementPanel()
		{
		HideAllPanels();  // Hide all panels
		teamManagementPanel.SetActive(true);  // Show team management panel
		}

	// Show Player Management Panel
	public void ShowPlayerManagementPanel()
		{
		HideAllPanels();
		playerManagementPanel.SetActive(true);
		}

	// Show Matchup Comparison Panel
	public void ShowMatchupComparisonPanel()
		{
		HideAllPanels();
		matchupComparisonPanel.SetActive(true);
		}

	// Show Settings Panel
	public void ShowSettingsPanel()
		{
		HideAllPanels();
		settingsPanel.SetActive(true);
		}

	// Show Overlay Feedback Panel
	public void ShowFeedbackPanel()
		{
		overlayFeedbackPanel.SetActive(true);
		}

	// Hide all panels
	private void HideAllPanels()
		{
		teamManagementPanel.SetActive(false);
		playerManagementPanel.SetActive(false);
		matchupComparisonPanel.SetActive(false);
		settingsPanel.SetActive(false);
		overlayFeedbackPanel.SetActive(false);
		}

	#endregion Panel Navigation Methods

	#region Button Handlers

	// Handler for Team Management button
	public void OnTeamManagementButtonClicked()
		{
		ShowTeamManagementPanel();
		}

	// Handler for Player Management button
	public void OnPlayerManagementButtonClicked()
		{
		ShowPlayerManagementPanel();
		}

	// Handler for Matchup Comparison button
	public void OnMatchupComparisonButtonClicked()
		{
		ShowMatchupComparisonPanel();
		}

	// Handler for Settings button
	public void OnSettingsButtonClicked()
		{
		ShowSettingsPanel();
		}

	#endregion Button Handlers
	}