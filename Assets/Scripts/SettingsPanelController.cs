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

	private SampleDataGenerator generatorScript;

	private void Start()
		{
		// Ensure UI references are assigned
		ValidateUIReferences();

		// Cache the SampleDataGenerator script reference for efficiency
		if (sampleDataGenerator != null)
			{
			generatorScript = sampleDataGenerator.GetComponent<SampleDataGenerator>();
			if (generatorScript == null)
				{
				Debug.LogError("SettingsPanelController: SampleDataGenerator script not found!");
				}
			}

		// Initialize toggle state
		sampleDataGeneratorToggle.isOn = sampleDataGenerator.activeSelf;

		// Add listeners
		sampleDataGeneratorToggle.onValueChanged.AddListener(OnSampleDataToggleChanged);
		currentSeasonWeightSettingsButton.onClick.AddListener(() => OpenSettingsPanel(currentSeasonWeightSettingsPanel));
		lifetimeWeightSettingsButton.onClick.AddListener(() => OpenSettingsPanel(lifetimeWeightSettingsPanel));
		backButton.onClick.AddListener(GoBack);
		}

	private void ValidateUIReferences()
		{
		if (sampleDataGeneratorToggle == null || sampleDataGenerator == null)
			{
			Debug.LogError("SettingsPanelController: Missing Sample Data Toggle or Generator reference!");
			}
		if (currentSeasonWeightSettingsButton == null || lifetimeWeightSettingsButton == null || backButton == null)
			{
			Debug.LogError("SettingsPanelController: Missing button references!");
			}
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
			currentSeasonWeightSettingsButton.onClick.RemoveListener(() => OpenSettingsPanel(currentSeasonWeightSettingsPanel));
			}
		if (lifetimeWeightSettingsButton != null)
			{
			lifetimeWeightSettingsButton.onClick.RemoveListener(() => OpenSettingsPanel(lifetimeWeightSettingsPanel));
			}
		if (backButton != null)
			{
			backButton.onClick.RemoveListener(GoBack);
			}
		}

	private void OnSampleDataToggleChanged(bool isOn)
		{
		if (sampleDataGenerator != null)
			{
			sampleDataGenerator.SetActive(isOn);
			}

		if (generatorScript != null)
			{
			if (isOn)
				{
				ActivateSampleDataGenerator();
				}
			else
				{
				DeactivateSampleDataGenerator();
				}
			}

		Debug.Log("Sample Data Generator is now " + (isOn ? "ENABLED" : "DISABLED"));
		}

	private void ActivateSampleDataGenerator()
		{
		if (generatorScript != null)
			{
			generatorScript.enabled = true;
			Debug.Log("Sample Data Generator script has been enabled.");
			}
		}

	private void DeactivateSampleDataGenerator()
		{
		if (generatorScript != null)
			{
			generatorScript.enabled = false;
			Debug.Log("Sample Data Generator script has been disabled.");
			}
		}

	private void OpenSettingsPanel(GameObject settingsPanel)
		{
		if (UIManager.Instance != null)
			{
			UIManager.Instance.panelHistory.Add(UIManager.Instance.settingsPanel);
			UIManager.Instance.settingsPanel.SetActive(false);
			if (settingsPanel != null)
				{
				settingsPanel.SetActive(true);
				}
			}
		else
			{
			Debug.LogError("SettingsPanelController: UIManager instance is null!");
			}
		}

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
