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

	// --- Region: Undo/Redo Logic --- //
	private Stack<string> undoStack = new();
	private Stack<string> redoStack = new();

	// --- Region: Menu Item for Window --- //
	[MenuItem("Tools/JSON Editor Window")]
	public static void ShowWindow()
		{
		GetWindow<JSONEditorWindow>("JSON Editor Window");
		}

	// --- Region: OnEnable (Load Default JSON & Schema) --- //
	private void OnEnable()
		{
		// Load any previously saved JSON data
		jsonData = LoadJsonData();

		// Load a default schema (to prevent null reference issues)
		string schemaPath = "Assets/JSONSchema.json";
		if (File.Exists(schemaPath))
			{
			schemaJsonData = File.ReadAllText(schemaPath);
			}
		else
			{
			schemaJsonData = "{}"; // Assign empty JSON schema to avoid null reference
			}
		}

	// --- Region: OnGUI (Editor UI) --- //
	private void OnGUI()
		{
		// --- JSON Display --- //
		GUILayout.Label("JSON Editor Window", EditorStyles.boldLabel);

		// Scrollable text area for JSON input
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
		jsonData = EditorGUILayout.TextArea(jsonData, GUILayout.Height(200));
		EditorGUILayout.EndScrollView();

		// --- Schema Validation --- //
		GUILayout.Space(10);
		GUILayout.Label("Schema Validation", EditorStyles.boldLabel);
		if (GUILayout.Button("Validate Schema"))
			{
			ValidateSchema(GetJsonObject());
			}

		// --- Action Buttons --- //
		GUILayout.Space(10);
		if (GUILayout.Button("Load JSON"))
			{
			jsonData = LoadJsonData();
			}
		if (GUILayout.Button("Save JSON"))
			{
			SaveJsonData();
			}

		// --- Undo/Redo --- //
		GUILayout.Space(10);
		if (GUILayout.Button("Undo"))
			{
			UndoJsonData();
			}
		if (GUILayout.Button("Redo"))
			{
			RedoJsonData();
			}

		// --- Display Schema Validation Result --- //
		GUILayout.Space(10);
		GUILayout.Label(isSchemaValid ? "Schema is valid." : "Schema is invalid.", EditorStyles.boldLabel);
		}
	// --- End Region --- //

	// --- Region: JSON Data Loading and Saving --- //
	private string LoadJsonData()
		{
		string path = "Assets/JSONData.json";
		if (File.Exists(path))
			{
			return File.ReadAllText(path);
			}
		return "{}"; // Return default empty JSON if file doesn't exist
		}

	private void SaveJsonData()
		{
		string path = "Assets/JSONData.json";
		File.WriteAllText(path, jsonData);
		AssetDatabase.Refresh();
		}

	private JObject GetJsonObject()
		{
		return jsonObject;
		}

	// --- End Region --- //

	// --- Region: Schema Validation --- //
	private void ValidateSchema(JObject jsonObject)
		{
		try
			{
			// Parse JSON data
			jsonObject = JObject.Parse(jsonData);

			// Ensure schemaJsonData is not null or empty before parsing
			if (string.IsNullOrEmpty(schemaJsonData))
				{
				Debug.LogError("Schema JSON data is empty or null.");
				isSchemaValid = false;
				return;
				}

			// Load schema from JSON schema string
			schema = JSchema.Parse(schemaJsonData);

			// Validate the JSON object against the schema
			IList<ValidationError> errorMessages;
			bool valid = jsonObject.IsValid(schema, out errorMessages);

			isSchemaValid = valid;

			// Print validation errors to console
			if (!valid)
				{
				foreach (var error in errorMessages)
					{
					Debug.LogError($"Schema Validation Error: {error.Message}");
					}
				}
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
