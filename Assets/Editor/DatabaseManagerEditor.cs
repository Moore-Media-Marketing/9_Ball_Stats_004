//using UnityEditor;

//using UnityEngine;

//[CustomEditor(typeof(DatabaseManager))]
//public class DatabaseManagerEditor:Editor
//	{
//	public override void OnInspectorGUI()
//		{
//		serializedObject.Update();

//		// Null check for DatabaseManager instance
//		if (DatabaseManager.Instance == null)
//			{
//			EditorGUILayout.HelpBox("DatabaseManager instance not found.", MessageType.Error);
//			return;
//			}

//		// Null check for allTeams list
//		if (DatabaseManager.Instance.allTeams == null)
//			{
//			EditorGUILayout.HelpBox("Teams list is not initialized.", MessageType.Error);
//			return;
//			}

//		// Null check for allPlayers list
//		if (DatabaseManager.Instance.allPlayers == null)
//			{
//			EditorGUILayout.HelpBox("Players list is not initialized.", MessageType.Error);
//			return;
//			}

//		EditorGUILayout.LabelField("Teams", EditorStyles.boldLabel);
//		foreach (var team in DatabaseManager.Instance.allTeams)
//			{
//			EditorGUILayout.LabelField(team.Name);
//			}

//		serializedObject.ApplyModifiedProperties();
//		}
//	}
