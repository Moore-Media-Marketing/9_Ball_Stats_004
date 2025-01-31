using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrentSeasonDataInputPanel:MonoBehaviour
	{
	public TMP_Text headerText;
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField gamesWonInputField;
	public TMP_InputField gamesPlayedInputField;
	public TMP_InputField totalPointsInputField;
	public TMP_InputField ppmInputField;
	public TMP_InputField paPercentageInputField;
	public TMP_InputField breakAndRunInputField;
	public TMP_InputField miniSlamsInputField;
	public TMP_InputField nineOnTheSnapInputField;
	public TMP_InputField shutoutsInputField;
	public TMP_Dropdown skillLevelDropdown;
	public Button addPlayerButton;
	public Button removePlayerButton;
	public Button backButton;

	private void Start()
		{
		backButton.onClick.AddListener(OnBackButtonClicked);
		addPlayerButton.onClick.AddListener(OnAddPlayerClicked);
		removePlayerButton.onClick.AddListener(OnRemovePlayerClicked);
		}

	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel); // Use UIManager to handle panel switching
		}

	private void OnAddPlayerClicked()
		{
		// Add player current season data logic
		}

	private void OnRemovePlayerClicked()
		{
		// Remove player current season data logic
		}
	}