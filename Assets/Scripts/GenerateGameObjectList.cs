using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateGameObjectList:MonoBehaviour
	{
	// Path to save the file in the Assets folder
	private string filePath = "Assets/TextFiles/GameObjectHierarchy.txt";

	private void Awake()
		{
		SaveEntireSceneHierarchyToFile();
		}

	// Method to generate and save the entire scene hierarchy to a text file
	private void SaveEntireSceneHierarchyToFile()
		{
		string listText = "";

		// Get all root GameObjects in the current scene
		GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

		// Loop through each root GameObject in the scene
		foreach (GameObject rootObject in rootObjects)
			{
			listText += GetHierarchyString(rootObject.transform, 0);
			}

		// Save the hierarchy structure to a text file in the Assets folder
		File.WriteAllText(filePath, listText); // File class is now accessible
		Debug.Log($"GameObject hierarchy saved to {filePath}");

		// Print the hierarchy to the console for debugging
		Debug.Log("Generated Hierarchy:\n" + listText);
		}

	// Recursive method to build the hierarchy string with indentation
	private string GetHierarchyString(Transform child, int indentLevel)
		{
		string indent = new(' ', indentLevel * 2); // Indentation for each level
		string line = $"{indent}{child.name}\n";

		// Loop through all children of the current child and call this method recursively
		foreach (Transform grandchild in child)
			{
			line += GetHierarchyString(grandchild, indentLevel + 1);
			}

		return line;
		}
	}