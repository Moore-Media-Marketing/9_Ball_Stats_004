using UnityEngine;
using TMPro; // Add TextMeshPro namespace
using System.Collections.Generic;

namespace NickWasHere
	{
	// --- DropdownManager class ---
	// This class manages the creation and configuration of dropdown menus for team and player selection.
	// It handles the setup, updates, and clears dropdowns dynamically at runtime.

	public class DropdownManager:MonoBehaviour
		{
		// --- Variables ---
		// Reference to the CustomDropdown prefab (Drag in the prefab in the inspector)
		[Tooltip("Drag the CustomDropdown prefab here")]
		public GameObject customDropdownPrefab;

		// List to keep track of all dropdowns created
		private List<GameObject> dropdowns = new List<GameObject>();

		// Dropdown options for teams and players
		[Header("Dropdown Options")]
		[Tooltip("List of team names to populate in dropdown")]
		public List<string> teamOptions = new List<string>();

		[Tooltip("List of player names to populate in dropdown")]
		public List<string> playerOptions = new List<string>();

		// --- Methods ---
		#region Dropdown Creation and Setup

		// Method to create a dropdown for a specific panel
		// Takes a position, list of options, and parent to instantiate the dropdown
		public void CreateDropdown(Vector3 position, List<string> options, Transform parent)
			{
			// Ensure the custom dropdown prefab is assigned
			if (customDropdownPrefab == null)
				{
				Debug.LogError("CustomDropdown prefab is not assigned in the DropdownManager.");
				return;
				}

			// Instantiate the dropdown prefab
			GameObject newDropdown = Instantiate(customDropdownPrefab, position, Quaternion.identity, parent);

			// Use TryGetComponent to avoid unnecessary allocations if the component is not found
			if (newDropdown.TryGetComponent(out TMP_Dropdown dropdownComponent))
				{
				// Clear existing options and add new ones
				dropdownComponent.ClearOptions();

				// Prepare the options for TMP_Dropdown
				List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();
				foreach (string option in options)
					{
					optionDataList.Add(new TMP_Dropdown.OptionData(option));
					}

				// Add the options to the dropdown
				dropdownComponent.AddOptions(optionDataList);

				// Optionally add an on-value change listener for dropdown selection
				dropdownComponent.onValueChanged.AddListener(delegate {
					OnDropdownValueChanged(dropdownComponent);
					});
				}
			else
				{
				Debug.LogError("TMP_Dropdown component not found on the instantiated prefab.");
				}

			// Add the newly created dropdown to the list
			dropdowns.Add(newDropdown);
			}

		#endregion

		#region Dropdown Value Change Handling

		// Method to handle the dropdown value change event
		// Logs the selected option and triggers relevant logic
		private void OnDropdownValueChanged(TMP_Dropdown dropdown)
			{
			string selectedOption = dropdown.options[dropdown.value].text;
			Debug.Log("Selected option: " + selectedOption);

			// Handle your logic based on the selected option
			// For example, updating text fields, showing relevant panels, etc.
			}

		#endregion

		#region Dropdown Management

		// Method to clear all dynamically created dropdowns
		public void ClearAllDropdowns()
			{
			// Iterate through each dropdown in the list and destroy it
			foreach (GameObject dropdown in dropdowns)
				{
				Destroy(dropdown);
				}

			// Clear the list after destroying all dropdowns
			dropdowns.Clear();
			}

		#endregion

		#region Setup Dropdowns

		// Method to set up the team dropdown with options
		public void SetupTeamDropdown(Transform parent)
			{
			CreateDropdown(Vector3.zero, teamOptions, parent);
			}

		// Method to set up the player dropdown with options
		public void SetupPlayerDropdown(Transform parent)
			{
			CreateDropdown(Vector3.zero, playerOptions, parent);
			}

		#endregion

		#region Initialization

		// Example usage: Call this when setting up the panel
		// This method initializes both team and player dropdowns
		public void InitializeDropdowns(Transform teamParent, Transform playerParent)
			{
			// Ensure to set teamOptions and playerOptions before calling this
			SetupTeamDropdown(teamParent);
			SetupPlayerDropdown(playerParent);
			}

		#endregion
		}
	}
