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
	public GameObject playerLifetimeDataInputPanel;
	public GameObject playercurrentSeasonDataInputPanel;

	[Header("Buttons")]
	public GameObject teamManagementButton;

	public GameObject playerManagementButton;
	public GameObject matchupComparisonButton;
	public GameObject settingsButton;
	public GameObject exitButton; // Exit Button

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
			return;
			}
		}

	private void Start()
		{
		// Ensure HomePanel is displayed when the app starts
		if (UIManager.Instance != null)
			{
			UIManager.Instance.ShowHomePanel();
			}
		else
			{
			Debug.LogError("UIManager Instance is null! HomePanel cannot be shown.");
			}
		}

	#endregion Singleton

	#region Button Handlers

	public void OnTeamManagementButtonClicked()
		{
		UIManager.Instance.ShowTeamManagementPanel();
		}

	public void OnPlayerManagementButtonClicked()
		{
		UIManager.Instance.ShowPlayerManagementPanel();
		}

	public void OnPlayerLifetimeDataInputPanelClicked()
		{
		UIManager.Instance.ShowPlayerLifetimeDataInputPanel();
		}

	public void OnPlayercurrentSeasonDataInputPanel()
		{
		UIManager.Instance.ShowPlayercurrentSeasonDataInputPanel();
		}

	public void OnMatchupComparisonButtonClicked()
		{
		UIManager.Instance.ShowMatchupComparisonPanel();
		}

	public void OnSettingsButtonClicked()
		{
		UIManager.Instance.ShowSettingsPanel();
		}

	public void OnExitButtonClicked()
		{
		Debug.Log("Exiting application...");
		Application.Quit();

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		}

	#endregion Button Handlers
	}