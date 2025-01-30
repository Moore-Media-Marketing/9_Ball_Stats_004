using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel:MonoBehaviour
	{
	// --- UI Elements ---
	[Header("UI Elements")]
	[SerializeField] private Button manageTeamsButton; // Manage teams button

	[SerializeField] private Button managePlayersButton; // Manage players button
	
	[SerializeField] private Button comparisonButton; // Comparison button
	[SerializeField] private Button exitButton; // Exit button

	[SerializeField] private TMP_Text headerText; // Header text for the panel

	// --- Panel References ---
	[Header("Panels")]
	[SerializeField] private GameObject teamManagementPanel;

	[SerializeField] private GameObject playerManagementPanel;
	[SerializeField] private GameObject comparisonSetupPanel;
	[SerializeField] private GameObject mainMenuPanel; // The main menu panel itself

	// --- Initialization ---
	private void Start()
		{
		// Set the header text for the Main Menu
		headerText.text = "Main Menu";

		// --- Button Click Listeners ---
		manageTeamsButton.onClick.AddListener(OnManageTeamsButtonClicked);
		managePlayersButton.onClick.AddListener(OnManagePlayersButtonClicked);

		comparisonButton.onClick.AddListener(OnComparisonButtonClicked);
		exitButton.onClick.AddListener(OnExitButtonClicked);
		}

	// --- Button Handlers ---
	private void OnManageTeamsButtonClicked()
		{
		// Open the Team Management Panel
		ShowPanel(teamManagementPanel);
		}

	private void OnManagePlayersButtonClicked()
		{
		// Open the Player Management Panel
		ShowPanel(playerManagementPanel);
		}

	private void OnComparisonButtonClicked()
		{
		// Open the Comparison Setup Panel
		ShowPanel(comparisonSetupPanel);
		}

	private void OnExitButtonClicked()
		{
		// Exit the application
		Application.Quit();
		}

	// --- Helper Method ---
	private void ShowPanel(GameObject panelToShow)
		{
		// Hide all panels
		teamManagementPanel.SetActive(false);
		playerManagementPanel.SetActive(false);
		comparisonSetupPanel.SetActive(false);

		mainMenuPanel.SetActive(false); // Hide the Main Menu

		// Show the selected panel
		panelToShow.SetActive(true);
		}
	}