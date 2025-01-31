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
	public GameObject exitButton; // New Exit Button

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

		// Ensure HomePanel is displayed when the app starts
		UIManager.Instance.ShowHomePanel();
		}

	#endregion Singleton

	#region Button Handlers

	// Handler for Team Management button
	public void OnTeamManagementButtonClicked()
		{
		UIManager.Instance.ShowTeamManagementPanel();
		}

	// Handler for Player Management button
	public void OnPlayerManagementButtonClicked()
		{
		UIManager.Instance.ShowPlayerManagementPanel();
		}

	// Handler for Matchup Comparison button
	public void OnMatchupComparisonButtonClicked()
		{
		UIManager.Instance.ShowMatchupComparisonPanel();
		}

	// Handler for Settings button
	public void OnSettingsButtonClicked()
		{
		UIManager.Instance.ShowSettingsPanel();
		}

	// Handler for Exit button
	public void OnExitButtonClicked()
		{
		// Quit the application
		Debug.Log("Exiting application...");
		Application.Quit();

		// If running in the editor, stop playmode
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		}

	#endregion Button Handlers
	}