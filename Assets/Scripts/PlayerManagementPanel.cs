using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerManagementPanel:MonoBehaviour
	{
	// --- Region: Panel References --- //
	public TMP_Dropdown teamNameDropdown;

	public TMP_Dropdown playerNameDropdown;
	public TMP_InputField playerNameInputField;
	public Button addPlayerButton;
	public Button deletePlayerButton;
	public Button addPlayerDetailsButton;

	private void Start()
		{
		// Initialize buttons with listeners
		addPlayerButton.onClick.AddListener(OnAddPlayer);
		deletePlayerButton.onClick.AddListener(OnDeletePlayer);
		addPlayerDetailsButton.onClick.AddListener(OnAddPlayerDetails);
		}

	// --- Region: Button Actions --- //
	private void OnAddPlayer()
		{
		string playerName = playerNameInputField.text;
		// Logic to add the player to the selected team
		Debug.Log($"Adding player: {playerName}");
		}

	private void OnDeletePlayer()
		{
		string playerName = playerNameDropdown.options[playerNameDropdown.value].text;
		// Logic to delete the selected player
		Debug.Log($"Deleting player: {playerName}");
		}

	private void OnAddPlayerDetails()
		{
		string playerName = playerNameDropdown.options[playerNameDropdown.value].text;
		// Logic to show player details input panel
		Debug.Log($"Adding details for player: {playerName}");
		}
	}