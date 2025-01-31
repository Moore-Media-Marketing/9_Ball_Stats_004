using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerManagementPanel:MonoBehaviour
	{
	public TMP_Text headerText;
	public TMP_Dropdown teamNameDropdown;
	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField playerNameInputField;
	public Button addPlayerButton;
	public Button deletePlayerButton;
	public Button addPlayerDetailsButton;
	public Button backButton;

	private void Start()
		{
		backButton.onClick.AddListener(OnBackButtonClicked);
		addPlayerButton.onClick.AddListener(OnAddPlayerClicked);
		deletePlayerButton.onClick.AddListener(OnDeletePlayerClicked);
		addPlayerDetailsButton.onClick.AddListener(OnAddPlayerDetailsClicked);
		}

	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel); // Use UIManager to handle panel switching
		}

	private void OnAddPlayerClicked()
		{
		// Add player logic
		}

	private void OnDeletePlayerClicked()
		{
		// Delete player logic
		}

	private void OnAddPlayerDetailsClicked()
		{
		// Add player details logic
		}
	}