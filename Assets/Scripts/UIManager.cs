using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UIManager:MonoBehaviour
	{
	// --- Singleton Instance --- //
	public static UIManager Instance { get; private set; }

	// --- UI Elements --- //
	[Header("Panels")]
	[SerializeField] private GameObject[] panels; // Remove feedback panel from this array

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

	// --- Variables for home/away teams --- //
	private Team homeTeam;
	private Team awayTeam;

	private void Awake()
		{
		if (Instance == null)
			{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			Debug.Log("UIManager initialized and marked with DontDestroyOnLoad.");
			}
		else
			{
			Destroy(gameObject);
			Debug.Log("UIManager already exists, destroyed duplicate.");
			return;
			}

		InitializePanelLookup();
		InitializeBackButton();
		InitializeDropdowns();

		// Subscribe to the team and player data update events
		DataManager.Instance.OnPlayerDataUpdated += UpdatePlayerDropdowns;
		DataManager.Instance.OnTeamDataUpdated += UpdateTeamDropdowns;
		}

	private void OnDestroy()
		{
		// Unsubscribe from the events to avoid memory leaks
		if (DataManager.Instance != null)
			{
			DataManager.Instance.OnPlayerDataUpdated -= UpdatePlayerDropdowns;
			DataManager.Instance.OnTeamDataUpdated -= UpdateTeamDropdowns;
			}
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
			teamDropdown.AddOptions(ConvertToOptionDataList(DataManager.Instance.GetTeamNames()));
			teamDropdown.onValueChanged.AddListener(OnTeamDropdownValueChanged);
			}

		// Player Input Manager Dropdown Initialization
		if (playerNameDropdown != null)
			{
			playerNameDropdown.ClearOptions();
			playerNameDropdown.AddOptions(ConvertToOptionDataList(DataManager.Instance.GetPlayerNames()));
			playerNameDropdown.onValueChanged.AddListener(OnPlayerNameDropdownValueChanged);
			}

		if (skillLevelDropdown != null)
			{
			skillLevelDropdown.ClearOptions();
			skillLevelDropdown.AddOptions(ConvertToOptionDataList(DataManager.Instance.GetSkillLevelOptions()));
			skillLevelDropdown.onValueChanged.AddListener(OnSkillLevelDropdownValueChanged);
			}

		// Comparison Setup Dropdown Initialization
		if (homeDropdown != null)
			{
			homeDropdown.ClearOptions();
			homeDropdown.AddOptions(ConvertToOptionDataList(DataManager.Instance.GetTeamNames())); // All teams are neutral initially
			homeDropdown.onValueChanged.AddListener(OnHomeDropdownValueChanged);
			}

		if (awayDropdown != null)
			{
			awayDropdown.ClearOptions();
			awayDropdown.AddOptions(ConvertToOptionDataList(DataManager.Instance.GetTeamNames())); // All teams are neutral initially
			awayDropdown.onValueChanged.AddListener(OnAwayDropdownValueChanged);
			}
		}

	// --- Update Player Dropdowns --- //
	private void UpdatePlayerDropdowns()
		{
		if (playerNameDropdown != null)
			{
			playerNameDropdown.ClearOptions();
			playerNameDropdown.AddOptions(ConvertToOptionDataList(DataManager.Instance.GetPlayerNames()));
			Debug.Log("Updated player dropdown.");
			}
		}

	// --- Update Team Dropdowns --- //
	private void UpdateTeamDropdowns()
		{
		if (teamDropdown != null)
			{
			teamDropdown.ClearOptions();
			teamDropdown.AddOptions(ConvertToOptionDataList(DataManager.Instance.GetTeamNames()));
			}

		if (homeDropdown != null)
			{
			homeDropdown.ClearOptions();
			homeDropdown.AddOptions(ConvertToOptionDataList(DataManager.Instance.GetTeamNames()));
			}

		if (awayDropdown != null)
			{
			awayDropdown.ClearOptions();
			awayDropdown.AddOptions(ConvertToOptionDataList(DataManager.Instance.GetTeamNames()));
			}
		}

	// --- Convert a list of strings or integers to TMP_Dropdown.OptionData --- //
	private List<TMP_Dropdown.OptionData> ConvertToOptionDataList(List<int> items)
		{
		List<TMP_Dropdown.OptionData> optionDataList = new();
		foreach (int item in items)
			{
			optionDataList.Add(new TMP_Dropdown.OptionData(item.ToString())); // Convert integer to string
			}
		return optionDataList;
		}

	private List<TMP_Dropdown.OptionData> ConvertToOptionDataList(List<string> items)
		{
		List<TMP_Dropdown.OptionData> optionDataList = new();
		foreach (string item in items)
			{
			optionDataList.Add(new TMP_Dropdown.OptionData(item)); // Directly add string items
			}
		return optionDataList;
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

		// Enable or disable back button
		backButton.interactable = panels[panelIndex].name != "MainMenuPanel" && panels[panelIndex].name != "OverlayFeedbackPanel";
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
			ShowPanel("MainMenuPanel");
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
		homeTeam = DataManager.Instance.GetTeamByName(homeDropdown.options[index].text);
		Debug.Log("Selected Home Team: " + homeTeam.Name);
		}

	private void OnAwayDropdownValueChanged(int index)
		{
		awayTeam = DataManager.Instance.GetTeamByName(awayDropdown.options[index].text);
		Debug.Log("Selected Away Team: " + awayTeam.Name);
		}

	// --- Get Home and Away Teams --- //
	public Team GetHomeTeam() => homeTeam;
	public Team GetAwayTeam() => awayTeam;
	}
