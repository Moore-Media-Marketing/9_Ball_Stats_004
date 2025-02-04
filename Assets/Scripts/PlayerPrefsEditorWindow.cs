using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class PlayerPrefsEditorWindow:EditorWindow
	{
	private Vector2 scrollPos;
	private Dictionary<int, Team> teams = new();
	private Dictionary<int, Player> players = new();

	[MenuItem("Tools/PlayerPrefs Editor")]
	public static void ShowWindow()
		{
		GetWindow<PlayerPrefsEditorWindow>("PlayerPrefs Editor");
		}

	private void OnEnable()
		{
		LoadData();
		}

	private void LoadData()
		{
		teams.Clear();
		players.Clear();

		// Load Teams from JSON
		for (int i = 1; i <= 100; i++)
			{
			if (PlayerPrefs.HasKey($"Team_{i}"))
				{
				string json = PlayerPrefs.GetString($"Team_{i}");
				Team team = JsonUtility.FromJson<Team>(json);
				teams[i] = team;
				}
			}

		// Load Players from JSON
		for (int i = 1; i <= 500; i++)
			{
			if (PlayerPrefs.HasKey($"Player_{i}"))
				{
				string json = PlayerPrefs.GetString($"Player_{i}");
				Player player = JsonUtility.FromJson<Player>(json);
				players[i] = player;
				}
			}
		}

	private void OnGUI()
		{
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		EditorGUILayout.LabelField("Teams", EditorStyles.boldLabel);
		foreach (var kvp in teams)
			{
			Team team = kvp.Value;
			EditorGUILayout.BeginVertical("box");
			team.name = EditorGUILayout.TextField($"Team {team.id} Name", team.name);
			if (GUILayout.Button("Save Team"))
				{
				PlayerPrefs.SetString($"Team_{team.id}", JsonUtility.ToJson(team));
				PlayerPrefs.Save();
				}
			EditorGUILayout.EndVertical();
			}

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Players", EditorStyles.boldLabel);
		foreach (var kvp in players)
			{
			Player player = kvp.Value;
			EditorGUILayout.BeginVertical("box");
			player.name = EditorGUILayout.TextField($"Player {player.id} Name", player.name);
			player.teamId = EditorGUILayout.IntField("Team ID", player.teamId);
			if (GUILayout.Button("Save Player"))
				{
				PlayerPrefs.SetString($"Player_{player.id}", JsonUtility.ToJson(player));
				PlayerPrefs.Save();
				}
			EditorGUILayout.EndVertical();
			}

		EditorGUILayout.EndScrollView();

		if (GUILayout.Button("Refresh"))
			{
			LoadData();
			}
		}
	}
