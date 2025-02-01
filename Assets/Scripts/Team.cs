// --- Region: Using Directives --- //
using System;
using System.Collections.Generic;

using UnityEngine;

// --- End Region: Using Directives --- //

// --- Region: Class Definition --- //
[Serializable]
public class Team
	{
	// --- Region: Team Information --- //

	// --- Comment: Unique identifier for the team (assigned by DatabaseManager) --- //
	public int id; // Placeholder: target_variable

	// --- Comment: Name of the team --- //
	[Tooltip("Enter the team's name.")] // --- Tooltip: Enter the team's name (placeholder: target_variable) --- //
	public string name; // Placeholder: target_variable

	// --- End Region: Team Information --- //

	// --- Region: Players List --- //

	// --- Comment: List of players belonging to this team --- //
	[Tooltip("List of players in the team.")]
	public List<Player> players;

	// --- End Region: Players List --- //

	// --- Region: Constructor --- //

	// --- Comment: Constructor for creating a new team --- //
	public Team(string name)
		{
		this.name = name; // --- Comment: Assign the team's name (placeholder: target_variable) --- //
		players = new List<Player>(); // --- Comment: Initialize the players list --- //
		}

	// --- End Region: Constructor --- //

	// --- Region: Additional Functions --- //
	// --- Comment: Add any extra helper methods for the Team class here --- //
	// --- End Region: Additional Functions --- //
	}

// --- End Region: Class Definition --- //