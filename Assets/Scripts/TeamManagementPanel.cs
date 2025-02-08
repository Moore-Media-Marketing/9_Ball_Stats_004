using UnityEngine;
using CsvHelper;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

public class TeamManagementPanel:MonoBehaviour
	{
	private DatabaseManager databaseManager;

	// UI references
	public Button loadTeamsButton;
	public Button saveTeamsButton;

	private void Start()
		{
		databaseManager = new DatabaseManager();

		loadTeamsButton.onClick.AddListener(LoadTeams);
		saveTeamsButton.onClick.AddListener(SaveTeams);
		}

	public void LoadTeams()
		{
		var teams = databaseManager.LoadTeams();
		Debug.Log("Teams loaded: " + teams.Count);
		// Update UI with teams data if needed
		}

	public void SaveTeams()
		{
		var teams = new List<Team>(); // Get teams data from UI or wherever it is stored
		databaseManager.SaveTeams(teams);
		Debug.Log("Teams saved");
		}
	}
