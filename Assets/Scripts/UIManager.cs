using System.Collections.Generic;

using NickWasHere;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UIManager:MonoBehaviour
	{
	// --- Singleton Instance --- //
	public static UIManager Instance { get; private set; }

	// --- UI Elements --- //
	[Header("Panels")]
	[SerializeField] private GameObject[] panels;

	[Header("Back Button")]
	[SerializeField] private Button backButton;

	// --- Dropdowns for different panels --- //
	[Header("Team Management Panel Dropdowns")]
	public TMP_Dropdown teamDropdown;

	[Header("Player Input Manager Panel Dropdowns")]

	public TMP_Dropdown teamNameDropdown;

	public TMP_Dropdown playerNameDropdown;

	public TMP_Dropdown skillLevelDropdown;

	[Header("Comparison Setup Panel Dropdowns")]
	public TMP_Dropdown homeDropdown;

	public TMP_Dropdown awayDropdown;
	
	// --- Panel Tracking --- //
	private int currentPanelIndex = -1;

	private Stack<int> panelHistory = new();
	private Dictionary<string, int> panelLookup = new();

	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			}
		else
			{
			Destroy(gameObject);
			return;
			}

		InitializePanelLookup();
		InitializeBackButton();
		InitializeDropdowns();
		}

	// --- Initialize Panel Lookup --- //
	private void InitializePanelLookup()
		{
		for (int i = 0; i < panels.Length; i++)
			{
			if (panels[i] != null)
				{
				panelLookup[panels[i].name] = i;
				}
			else
				{
				Debug.LogError($"Panel at index {i} is null in UIManager.");
				}
			}
		}

	// --- Initialize Back Button --- //
	private void InitializeBackButton()
		{
		if (backButton != null)
			{
			backButton.onClick.AddListener(GoBackToPreviousPanel);
			}
		else
			{
			Debug.LogError("Back button is not assigned in UIManager.");
			}
		}

	// --- Initialize Dropdowns --- //
	private void InitializeDropdowns()
		{
		// Main Menu Dropdown Initialization
		if (teamDropdown != null)
			{
			teamDropdown.ClearOptions();
			teamDropdown.AddOptions(DataManager.Instance.GetTeamNames());
			teamDropdown.onValueChanged.AddListener(OnTeamDropdownValueChanged);
			}

		// Player Input Manager Dropdown Initialization
		if (playerNameDropdown != null)
			{
			playerNameDropdown.ClearOptions();
			playerNameDropdown.AddOptions(DataManager.Instance.GetPlayerNames());
			playerNameDropdown.onValueChanged.AddListener(OnPlayerNameDropdownValueChanged);
			}

		if (skillLevelDropdown != null)
			{
			skillLevelDropdown.ClearOptions();
			skillLevelDropdown.AddOptions(DataManager.Instance.GetSkillLevelOptions());
			skillLevelDropdown.onValueChanged.AddListener(OnSkillLevelDropdownValueChanged);
			}

		// Comparison Setup Dropdown Initialization
		if (homeDropdown != null)
			{
			homeDropdown.ClearOptions();
			homeDropdown.AddOptions(DataManager.Instance.GetHomeTeamNames());
			homeDropdown.onValueChanged.AddListener(OnHomeDropdownValueChanged);
			}

		if (awayDropdown != null)
			{
			awayDropdown.ClearOptions();
			awayDropdown.AddOptions(DataManager.Instance.GetAwayTeamNames());
			awayDropdown.onValueChanged.AddListener(OnAwayDropdownValueChanged);
			}
		}

	// --- Show Panel by Index --- //
	public void ShowPanel(int panelIndex)
		{
		if (panelIndex < 0 || panelIndex >= panels.Length)
			{
			Debug.LogError($"Invalid panel index: {panelIndex}");
			return;
			}

		HideAllPanels();

		if (currentPanelIndex != -1)
			{
			panelHistory.Push(currentPanelIndex);
			}

		panels[panelIndex].SetActive(true);
		currentPanelIndex = panelIndex;
		Debug.Log($"Switched to panel {panelIndex}");
		}

	// --- Show Panel by Name --- //
	public void ShowPanel(string panelName)
		{
		if (panelLookup.TryGetValue(panelName, out int panelIndex))
			{
			ShowPanel(panelIndex);
			}
		else
			{
			Debug.LogError($"Panel with name '{panelName}' not found in UIManager.");
			}
		}

	// --- Hide All Panels --- //
	private void HideAllPanels()
		{
		foreach (GameObject panel in panels)
			{
			if (panel != null)
				{
				panel.SetActive(false);
				}
			}
		}

	// --- Go Back to Previous Panel --- //
	public void GoBackToPreviousPanel()
		{
		if (panelHistory.Count > 0)
			{
			ShowPanel(panelHistory.Pop());
			}
		else
			{
			Debug.Log("No previous panels in history.");
			}
		}

	// --- Dropdown Value Changed Handlers --- //
	private void OnTeamDropdownValueChanged(int index)
		{
		Debug.Log("Selected Team: " + teamDropdown.options[index].text);
		}

	private void OnPlayerNameDropdownValueChanged(int index)
		{
		Debug.Log("Selected Player: " + playerNameDropdown.options[index].text);
		}

	private void OnSkillLevelDropdownValueChanged(int index)
		{
		Debug.Log("Selected Skill Level: " + skillLevelDropdown.options[index].text);
		}

	private void OnHomeDropdownValueChanged(int index)
		{
		Debug.Log("Selected Home Team: " + homeDropdown.options[index].text);
		}

	private void OnAwayDropdownValueChanged(int index)
		{
		Debug.Log("Selected Away Team: " + awayDropdown.options[index].text);
		}
	}