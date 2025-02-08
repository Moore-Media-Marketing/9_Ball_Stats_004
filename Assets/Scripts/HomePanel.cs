using UnityEngine;
using UnityEngine.UI;

public class HomePanel:MonoBehaviour
	{
	// UI references for buttons
	public Button manageTeamsButton;
	public Button createPlayerButton;
	public Button comparisonButton;
	public Button settingsButton;
	public Button exitButton;

	private void Start()
		{
		manageTeamsButton.onClick.AddListener(OnManageTeamsClicked);
		createPlayerButton.onClick.AddListener(OnCreatePlayerClicked);
		comparisonButton.onClick.AddListener(OnComparisonClicked);
		settingsButton.onClick.AddListener(OnSettingsClicked);
		exitButton.onClick.AddListener(OnExitClicked);
		}

	// Navigate to Team Management Panel
	private void OnManageTeamsClicked()
		{
		UIManager.Instance.ShowTeamManagementPanel();
		}

	// Navigate to Player Management Panel
	private void OnCreatePlayerClicked()
		{
		UIManager.Instance.ShowPlayerManagementPanel();
		}

	// Navigate to Comparison Panel
	private void OnComparisonClicked()
		{
		UIManager.Instance.ShowComparisonPanel();
		}

	// Navigate to Settings Panel
	private void OnSettingsClicked()
		{
		UIManager.Instance.ShowSettingsPanel();
		}

	// Exit the app
	private void OnExitClicked()
		{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false; // For editor
#elif UNITY_ANDROID
        Application.Quit(); // For Android
#else
        Application.Quit(); // For other platforms
#endif
		}
	}
