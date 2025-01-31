using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TeamManagementPanel:MonoBehaviour
	{
	public TMP_Text headerText;
	public TMP_InputField teamNameInputField;
	public Button addUpdateTeamButton;
	public Button clearTeamNameButton;
	public TMP_Dropdown teamDropdown;
	public Button modifyTeamNameButton;
	public Button deleteButton;
	public Button backButton;

	private void Start()
		{
		backButton.onClick.AddListener(OnBackButtonClicked);
		addUpdateTeamButton.onClick.AddListener(OnAddUpdateTeamClicked);
		clearTeamNameButton.onClick.AddListener(OnClearTeamNameClicked);
		modifyTeamNameButton.onClick.AddListener(OnModifyTeamNameClicked);
		deleteButton.onClick.AddListener(OnDeleteButtonClicked);
		}

	private void OnBackButtonClicked()
		{
		UIManager.Instance.ShowPanel(UIManager.Instance.homePanel);
		}

	private void OnAddUpdateTeamClicked()
		{
		// Logic to add or update a team
		}

	private void OnClearTeamNameClicked()
		{
		teamNameInputField.text = "";
		}

	private void OnModifyTeamNameClicked()
		{
		// Modify team name logic
		}

	private void OnDeleteButtonClicked()
		{
		// Delete team logic
		}
	}
