using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController:MonoBehaviour
	{
	[Header("UI References")]
	public Toggle sampleDataGeneratorToggle;
	public Button currentSeasonWeightSettingsButton;
	public Button lifetimeWeightSettingsButton;
	public Button backButton;

	[Header("Panels")]
	public GameObject currentSeasonWeightSettingsPanel;
	public GameObject lifetimeWeightSettingsPanel;

	[Header("Sample Data Generator")]
	public GameObject sampleDataGenerator;

	private void Start()
		{
		// Ensure UI references are assigned
		if (sampleDataGeneratorToggle == null || sampleDataGenerator == null)
			{
			Debug.LogError("SettingsPanelController: Missing Sample Data Toggle or Generator reference!");
			return;
			}
		if (currentSeasonWeightSettingsButton == null || lifetimeWeightSettingsButton == null || backButton == null)
			{
			Debug.LogError("SettingsPanelController: Missing button references!");
			return;
			}

		// Initialize toggle state
		sampleDataGeneratorToggle.isOn = sampleDataGenerator.activeSelf;

		// Add listeners
		sampleDataGeneratorToggle.onValueChanged.AddListener(OnSampleDataToggleChanged);
		currentSeasonWeightSettingsButton.onClick.AddListener(OpenCurrentSeasonWeightSettings);
		lifetimeWeightSettingsButton.onClick.AddListener(OpenLifetimeWeightSettings);
		backButton.onClick.AddListener(GoBack);
		}

	private void OnEnable()
		{
		// Ensure toggle reflects current state when panel is reopened
		if (sampleDataGeneratorToggle != null && sampleDataGenerator != null)
			{
			sampleDataGeneratorToggle.isOn = sampleDataGenerator.activeSelf;
			}
		}

	private void OnDestroy()
		{
		// Cleanup listeners to prevent memory leaks
		if (sampleDataGeneratorToggle != null)
			{
			sampleDataGeneratorToggle.onValueChanged.RemoveListener(OnSampleDataToggleChanged);
			}
		if (currentSeasonWeightSettingsButton != null)
			{
			currentSeasonWeightSettingsButton.onClick.RemoveListener(OpenCurrentSeasonWeightSettings);
			}
		if (lifetimeWeightSettingsButton != null)
			{
			lifetimeWeightSettingsButton.onClick.RemoveListener(OpenLifetimeWeightSettings);
			}
		if (backButton != null)
			{
			backButton.onClick.RemoveListener(GoBack);
			}
		}

	// --- Toggle Sample Data Generation --- //
	private void OnSampleDataToggleChanged(bool isOn)
		{
		if (sampleDataGenerator != null)
			{
			sampleDataGenerator.SetActive(isOn);
			}

		if (isOn)
			{
			GenerateSampleData();
			}
		else
			{
			ClearSampleData();
			}

		Debug.Log("Sample Data Generator is now " + (isOn ? "ENABLED" : "DISABLED"));
		}

	// --- Generate Sample Data using DatabaseManager --- //
	private void GenerateSampleData()
		{
		if (DatabaseManager.Instance != null)
			{
			Debug.Log("Generating sample data...");
			DatabaseManager.Instance.GenerateSampleData(); // Call the method in DatabaseManager
			}
		else
			{
			Debug.LogError("DatabaseManager instance is null! Cannot generate sample data.");
			}
		}

	// --- Clear Sample Data --- //
	private void ClearSampleData()
		{
		if (DatabaseManager.Instance != null)
			{
			Debug.Log("Clearing sample data...");
			DatabaseManager.Instance.ClearSampleData(); // Call method in DatabaseManager
			}
		else
			{
			Debug.LogError("DatabaseManager instance is null! Cannot clear sample data.");
			}
		}

	// --- Open Current Season Weight Settings Panel --- //
	private void OpenCurrentSeasonWeightSettings()
		{
		if (UIManager.Instance != null)
			{
			UIManager.Instance.panelHistory.Add(UIManager.Instance.settingsPanel);
			UIManager.Instance.settingsPanel.SetActive(false);
			if (currentSeasonWeightSettingsPanel != null)
				{
				currentSeasonWeightSettingsPanel.SetActive(true);
				}
			}
		else
			{
			Debug.LogError("SettingsPanelController: UIManager instance is null!");
			}
		}

	// --- Open Lifetime Weight Settings Panel --- //
	private void OpenLifetimeWeightSettings()
		{
		if (UIManager.Instance != null)
			{
			UIManager.Instance.panelHistory.Add(UIManager.Instance.settingsPanel);
			UIManager.Instance.settingsPanel.SetActive(false);
			if (lifetimeWeightSettingsPanel != null)
				{
				lifetimeWeightSettingsPanel.SetActive(true);
				}
			}
		else
			{
			Debug.LogError("SettingsPanelController: UIManager instance is null!");
			}
		}

	// --- Go Back using UIManager --- //
	private void GoBack()
		{
		if (UIManager.Instance != null)
			{
			UIManager.Instance.GoBackToPreviousPanel();
			}
		else
			{
			Debug.LogError("SettingsPanelController: UIManager instance is null!");
			}
		}
	}
