using UnityEngine;

public class DataPersistence:MonoBehaviour
	{
	[Header("Data Persistence")]
	[Tooltip("Key for storing teams data in PlayerPrefs")]
	private string teamsKey = "TeamsData";

	[Tooltip("Key for storing players data in PlayerPrefs")]
	private string playersKey = "PlayersData";

	// Reference to PlayerManager (ensure it's attached in the Inspector or use FindFirstObjectByType)
	public PlayerManager playerManager;

	// Reference to TeamManager (ensure it's attached in the Inspector or use FindFirstObjectByType)
	public TeamManager teamManager;

	private void Awake()
		{
		// If PlayerManager is not assigned in the Inspector, find it in the scene
		if (playerManager == null)
			{
			playerManager = Object.FindFirstObjectByType<PlayerManager>(); // Replacing deprecated FindObjectOfType
			}

		// If TeamManager is not assigned in the Inspector, find it in the scene
		if (teamManager == null)
			{
			teamManager = Object.FindFirstObjectByType<TeamManager>(); // Replacing deprecated FindObjectOfType
			}
		}

	// --- Save Teams Data ---
	public void SaveTeamsData()
		{
		if (teamManager != null)
			{
			string jsonData = JsonUtility.ToJson(teamManager);
			PlayerPrefs.SetString(teamsKey, jsonData);
			PlayerPrefs.Save();
			}
		else
			{
			Debug.LogWarning("TeamManager is not assigned or found in the scene!");
			}
		}

	// --- Load Teams Data ---
	public TeamManager LoadTeamsData()
		{
		if (PlayerPrefs.HasKey(teamsKey))
			{
			string jsonData = PlayerPrefs.GetString(teamsKey);
			try
				{
				// Assuming the teamManager is already assigned or found in the scene
				if (teamManager == null)
					{
					teamManager = Object.FindFirstObjectByType<TeamManager>(); // Replacing deprecated FindObjectOfType
					}

				if (teamManager != null)
					{
					JsonUtility.FromJsonOverwrite(jsonData, teamManager);
					}
				else
					{
					Debug.LogWarning("TeamManager is not found in the scene.");
					}
				}
			catch (System.Exception ex)
				{
				Debug.LogError($"Error loading teams data: {ex.Message}");
				}
			}
		else
			{
			Debug.LogWarning("No teams data found in PlayerPrefs");
			}
		return teamManager;
		}

	// --- Save Players Data ---
	public void SavePlayersData()
		{
		if (playerManager != null)
			{
			string jsonData = JsonUtility.ToJson(playerManager);
			PlayerPrefs.SetString(playersKey, jsonData);
			PlayerPrefs.Save();
			}
		else
			{
			Debug.LogWarning("PlayerManager is not assigned or found in the scene!");
			}
		}

	// --- Load Players Data ---
	public void LoadPlayersData()
		{
		if (PlayerPrefs.HasKey(playersKey))
			{
			string jsonData = PlayerPrefs.GetString(playersKey);
			try
				{
				// Here we assume PlayerManager is attached to a GameObject, no need to instantiate
				if (playerManager == null)
					{
					playerManager = Object.FindFirstObjectByType<PlayerManager>(); // Replacing deprecated FindObjectOfType
					}

				if (playerManager != null)
					{
					JsonUtility.FromJsonOverwrite(jsonData, playerManager);
					}
				else
					{
					Debug.LogWarning("PlayerManager is not found in the scene.");
					}
				}
			catch (System.Exception ex)
				{
				Debug.LogError($"Error loading players data: {ex.Message}");
				}
			}
		else
			{
			Debug.LogWarning("No players data found in PlayerPrefs");
			}
		}
	}