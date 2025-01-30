using UnityEngine;

public class UIManager:MonoBehaviour
	{
	[Header("UI Panels")]
	[Tooltip("The main menu panel")]
	public GameObject mainMenuPanel;

	[Tooltip("The team management panel")]
	public GameObject teamManagementPanel;

	[Tooltip("The player input manager panel")]
	public GameObject playerInputManagerPanel;

	[Tooltip("The comparison setup panel")]
	public GameObject comparisonSetupPanel;

	[Tooltip("The overlay panel for feedback")]
	public GameObject overlayPanel;

	// --- Show Main Menu Method ---
	public void ShowMainMenu()
		{
		SwitchPanel(mainMenuPanel);
		}

	// --- Show Team Management Panel Method ---
	public void ShowTeamManagementPanel()
		{
		SwitchPanel(teamManagementPanel);
		}

	// --- Show Player Input Manager Panel Method ---
	public void ShowPlayerInputManagerPanel()
		{
		SwitchPanel(playerInputManagerPanel);
		}

	// --- Show Comparison Setup Panel Method ---
	public void ShowComparisonSetupPanel()
		{
		SwitchPanel(comparisonSetupPanel);
		}

	// --- Switch Panel Method ---
	private void SwitchPanel(GameObject panelToActivate)
		{
		// Deactivate all panels
		mainMenuPanel.SetActive(false);
		teamManagementPanel.SetActive(false);
		playerInputManagerPanel.SetActive(false);
		comparisonSetupPanel.SetActive(false);
		overlayPanel.SetActive(false);

		// Activate the selected panel
		panelToActivate.SetActive(true);
		}
	}