using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class TeamManagementPanel:MonoBehaviour
	{
	// --- Region: UI References --- //
	public TMP_Dropdown teamDropdown;

	public TMP_InputField teamNameInputField;
	public Button addUpdateTeamButton;
	public Button deleteTeamButton;
	public Button backButton;

	// --- Region: Initialize Panel --- //
	private void Start()
		{
		addUpdateTeamButton.onClick.AddListener(OnAddUpdateTeam);
		deleteTeamButton.onClick.AddListener(OnDeleteTeam);
		backButton.onClick.AddListener(OnBackButton);
		}

	// --- Region: Add or Update Team --- //
	private void OnAddUpdateTeam()
		{
		string teamName = teamNameInputField.text;
		if (!string.IsNullOrEmpty(teamName))
			{
			Team team = new(teamName);
			// Add or update team logic here (e.g., saving to the database)
			Debug.Log("Team added or updated: " + teamName);
			}
		else
			{
			Debug.Log("Please enter a valid team name.");
			}
		}

	// --- Region: Delete Team --- //
	private void OnDeleteTeam()
		{
		string teamName = teamDropdown.options[teamDropdown.value].text;
		// Delete team logic here (e.g., removing from the database)
		Debug.Log("Team deleted: " + teamName);
		}

	// --- Region: Back Button --- //
	private void OnBackButton()
		{
		// Show the home panel or previous panel
		Debug.Log("Back button clicked.");
		}
	}