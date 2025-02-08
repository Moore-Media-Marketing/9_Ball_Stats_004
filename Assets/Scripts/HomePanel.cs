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
		// Add listeners to buttons if they are not null
		if (manageTeamsButton != null)
			manageTeamsButton.onClick.AddListener(OnManageTeamsClicked);

		if (createPlayerButton != null)
			createPlayerButton.onClick.AddListener(OnCreatePlayerClicked);

		if (comparisonButton != null)
			comparisonButton.onClick.AddListener(OnComparisonClicked);

		if (settingsButton != null)
			settingsButton.onClick.AddListener(OnSettingsClicked);

		if (exitButton != null)
			exitButton.onClick.AddListener(OnExitClicked);
		}

	// Navigate to Team Management Panel
	private void OnManageTeamsClicked()
		{
		if (UIManager.Instance != null)
			UIManager.Instance.ShowTeamManagementPanel();
		}

	// Navigate to Player Management Panel
	private void OnCreatePlayerClicked()
		{
		if (UIManager.Instance != null)
			UIManager.Instance.ShowPlayerManagementPanel();
		}

	// Navigate to Comparison Panel
	private void OnComparisonClicked()
		{
		if (UIManager.Instance != null)
			UIManager.Instance.ShowComparisonPanel();
		}

	// Navigate to Settings Panel
	private void OnSettingsClicked()
		{
		if (UIManager.Instance != null)
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
