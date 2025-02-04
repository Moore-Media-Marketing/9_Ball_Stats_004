using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

public class JSONEditorWindow:EditorWindow
	{
	// --- Region: UI Elements --- //
	[Header("UI Elements")]
	private string jsonData;
	private string schemaJsonData;
	private JObject jsonObject;
	private JSchema schema;
	private Vector2 scrollPosition;
	private bool isSchemaValid = true;

	// --- Region: Menu Item for Window --- //
	[MenuItem("Tools/JSON Editor Window")]
	public static void ShowWindow()
		{
		GetWindow<JSONEditorWindow>("JSON Editor Window");
		}

	// --- Region: OnEnable --- //
	private void OnEnable()
		{
		// Load any previously saved JSON data
		jsonData = LoadJsonData();
		}

	// --- Region: OnGUI --- //
	private void OnGUI()
		{
		// --- Region: JSON Display --- //
		GUILayout.Label("JSON Editor Window", EditorStyles.boldLabel);

		// Display the JSON data text area
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200)); // Added scroll view for better text management
		jsonData = EditorGUILayout.TextArea(jsonData, GUILayout.Height(200));
		EditorGUILayout.EndScrollView();

		// --- Region: Schema Validation --- //
		GUILayout.Space(10);
		GUILayout.Label("Schema Validation", EditorStyles.boldLabel);
		if (GUILayout.Button("Validate Schema"))
			{
			ValidateSchema();
			}

		// --- Region: Buttons for Actions --- //
		GUILayout.Space(10);
		if (GUILayout.Button("Load JSON"))
			{
			jsonData = LoadJsonData();
			}
		if (GUILayout.Button("Save JSON"))
			{
			SaveJsonData();
			}

		// --- Region: Undo/Redo --- //
		GUILayout.Space(10);
		if (GUILayout.Button("Undo"))
			{
			UndoJsonData();
			}
		if (GUILayout.Button("Redo"))
			{
			RedoJsonData();
			}

		// --- Region: Display Schema Validation Result --- //
		if (isSchemaValid)
			{
			GUILayout.Label("Schema is valid.", EditorStyles.boldLabel);
			}
		else
			{
			GUILayout.Label("Schema is invalid.", EditorStyles.boldLabel);
			}
		}
	// --- End Region --- //

	// --- Region: JSON Data Loading and Saving --- //
	private string LoadJsonData()
		{
		// Check for a saved JSON file in the project folder and load it
		string path = "Assets/JSONData.json";
		if (File.Exists(path))
			{
			return File.ReadAllText(path);
			}
		return "{}"; // Return empty JSON if file doesn't exist
		}

	private void SaveJsonData()
		{
		// Save the current JSON data to file
		string path = "Assets/JSONData.json";
		File.WriteAllText(path, jsonData);
		AssetDatabase.Refresh();
		}
	// --- End Region --- //

	// --- Region: Schema Validation --- //
	private void ValidateSchema()
		{
		try
			{
			// Attempt to parse the JSON data as JObject
			jsonObject = JObject.Parse(jsonData);

			// Load schema from the schema JSON data (you can modify this for your actual schema)
			schema = JSchema.Parse(schemaJsonData);

			// Validate the JSON data against the schema
			jsonObject.Validate(schema, out IList<string> errorMessages);
			isSchemaValid = errorMessages.Count == 0;
			}
		catch (JsonException ex)
			{
			Debug.LogError("JSON parsing error: " + ex.Message);
			isSchemaValid = false;
			}
		catch (JSchemaException ex)
			{
			Debug.LogError("Schema error: " + ex.Message);
			isSchemaValid = false;
			}
		}
	// --- End Region --- //

	// --- Region: Undo/Redo Logic (Basic Implementation) --- //
	private Stack<string> undoStack = new Stack<string>();
	private Stack<string> redoStack = new Stack<string>();

	private void UndoJsonData()
		{
		if (undoStack.Count > 0)
			{
			redoStack.Push(jsonData);
			jsonData = undoStack.Pop();
			}
		}

	private void RedoJsonData()
		{
		if (redoStack.Count > 0)
			{
			undoStack.Push(jsonData);
			jsonData = redoStack.Pop();
			}
		}
	// --- End Region --- //
	}
