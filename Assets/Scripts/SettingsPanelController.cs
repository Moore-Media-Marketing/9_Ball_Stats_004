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
		// --- Ensure UI references are assigned --- //
		if (!sampleDataGeneratorToggle || !sampleDataGenerator)
			{
			Debug.LogError("SettingsPanelController: Missing Sample Data Toggle or Generator reference!");
			return;
			}
		if (!currentSeasonWeightSettingsButton || !lifetimeWeightSettingsButton || !backButton)
			{
			Debug.LogError("SettingsPanelController: Missing button references!");
			return;
			}

		// --- Initialize toggle state --- //
		sampleDataGeneratorToggle.isOn = sampleDataGenerator.activeSelf;

		// --- Add listeners --- //
		sampleDataGeneratorToggle.onValueChanged.AddListener(OnSampleDataToggleChanged);
		currentSeasonWeightSettingsButton.onClick.AddListener(OpenCurrentSeasonWeightSettings);
		lifetimeWeightSettingsButton.onClick.AddListener(OpenLifetimeWeightSettings);
		backButton.onClick.AddListener(GoBack);
		}

	// --- Toggle SampleDataGenerator on/off --- //
	private void OnSampleDataToggleChanged(bool isOn)
		{
		sampleDataGenerator.SetActive(isOn);
		Debug.Log("SampleDataGenerator is now " + (isOn ? "ENABLED" : "DISABLED"));
		}

	// --- Open Current Season Weight Settings Panel --- //
	private void OpenCurrentSeasonWeightSettings()
		{
		if (UIManager.Instance != null)
			{
			UIManager.Instance.panelHistory.Push(UIManager.Instance.settingsPanel);
			UIManager.Instance.settingsPanel.SetActive(false);
			currentSeasonWeightSettingsPanel.SetActive(true);
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
			UIManager.Instance.panelHistory.Push(UIManager.Instance.settingsPanel);
			UIManager.Instance.settingsPanel.SetActive(false);
			lifetimeWeightSettingsPanel.SetActive(true);
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