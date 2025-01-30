using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ComparisonSetupPanel:MonoBehaviour
	{
	// --- Panel Elements --- //
	public TextMeshProUGUI HeaderText;

	public TextMeshProUGUI SelectHomeTeamText;
	public TextMeshProUGUI SelectHomePlayerText;
	public TMP_Dropdown HomeTeamDropdown;
	public Transform HomeTeamPlayerScrollView;
	public TextMeshProUGUI SelectAwayTeamText;
	public TextMeshProUGUI SelectAwayPlayerText;
	public TMP_Dropdown AwayTeamDropdown;
	public Transform AwayTeamPlayerScrollView;
	public Button CompareButton;
	public Button BackButton;

	// --- Initialize the Panel with Available Teams and Players --- //
	public void InitializePanel()
		{
		// Clear dropdown options and populate with available teams
		HomeTeamDropdown.ClearOptions();
		AwayTeamDropdown.ClearOptions();

		// Add available team names to both dropdowns
		List<string> teamNames = GetTeamNames();
		HomeTeamDropdown.AddOptions(teamNames);
		AwayTeamDropdown.AddOptions(teamNames);

		// Clear previous player selections
		ClearPlayerSelections(HomeTeamPlayerScrollView);
		ClearPlayerSelections(AwayTeamPlayerScrollView);
		}

	// --- Get Team Names from DataManager --- //
	private List<string> GetTeamNames()
		{
		return DataManager.Instance.GetTeamNames();
		}

	// --- Update Player List for Selected Team --- //
	public void UpdatePlayerList(TMP_Dropdown teamDropdown, Transform playerScrollView)
		{
		// Get the selected team based on the dropdown
		string selectedTeam = teamDropdown.options[teamDropdown.value].text;

		// Clear previous player list
		ClearPlayerSelections(playerScrollView);

		// Get the list of player names
		List<string> playerNames = DataManager.Instance.GetPlayersForTeam(selectedTeam)
													   .Select(p => p.name)
													   .ToList();

		foreach (string playerName in playerNames)
			{
			// Create a new button for each player
			GameObject playerButton = new(playerName);
			playerButton.transform.SetParent(playerScrollView, false);

			// Add a button component to the player button
			Button button = playerButton.AddComponent<Button>();

			// Create a TextMeshProUGUI component for the button text
			GameObject textObject = new("Text");
			textObject.transform.SetParent(playerButton.transform, false);
			TextMeshProUGUI buttonText = textObject.AddComponent<TextMeshProUGUI>();
			buttonText.text = playerName;
			buttonText.fontSize = 14;
			buttonText.alignment = TextAlignmentOptions.Center;

			// Add listener to the button for player selection
			button.onClick.AddListener(() => OnPlayerSelected(playerName, teamDropdown));
			}
		}

	// --- Clear Player Selections --- //
	private void ClearPlayerSelections(Transform playerScrollView)
		{
		foreach (Transform child in playerScrollView)
			{
			Destroy(child.gameObject);
			}
		}

	// --- Handle Player Selection --- //
	private void OnPlayerSelected(string playerName, TMP_Dropdown teamDropdown)
		{
		Debug.Log($"{playerName} selected from {teamDropdown.options[teamDropdown.value].text}");
		}

	// --- Start Method for Initializing Button Listeners --- //
	private void Start()
		{
		InitializePanel();

		// Add listener for home team dropdown
		HomeTeamDropdown.onValueChanged.AddListener((index) =>
		{
			UpdatePlayerList(HomeTeamDropdown, HomeTeamPlayerScrollView);
		});

		// Add listener for away team dropdown
		AwayTeamDropdown.onValueChanged.AddListener((index) =>
		{
			UpdatePlayerList(AwayTeamDropdown, AwayTeamPlayerScrollView);
		});

		// Add listener for compare button
		if (CompareButton != null)
			{
			CompareButton.onClick.AddListener(OnCompareButtonClicked);
			}

		// Add listener for back button
		if (BackButton != null)
			{
			BackButton.onClick.AddListener(OnBackButtonClicked);
			}
		}

	// --- Compare Button Logic using ComparisonManager --- //
	private void OnCompareButtonClicked()
		{
		string homeTeamName = HomeTeamDropdown.options[HomeTeamDropdown.value].text;
		string awayTeamName = AwayTeamDropdown.options[AwayTeamDropdown.value].text;

		Team homeTeam = DataManager.Instance.GetTeamByName(homeTeamName);
		Team awayTeam = DataManager.Instance.GetTeamByName(awayTeamName);

		if (homeTeam == null || awayTeam == null)
			{
			Debug.LogError("One or both selected teams could not be found!");
			return;
			}

		ComparisonManager.Instance.CompareTeams(homeTeam, awayTeam);
		Debug.Log($"Comparison started between {homeTeamName} and {awayTeamName}");
		}

	// --- Back Button Logic --- //
	private void OnBackButtonClicked()
		{
		UIManager.Instance.GoBackToPreviousPanel();
		}
	}
