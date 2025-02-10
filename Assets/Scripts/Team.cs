using System;

/// <summary>
/// Represents a team in the system.
/// </summary>
public class Team
	{
	public int TeamId { get; set; }
	public string TeamName { get; set; }

	public Team(int teamId, string teamName)
		{
		TeamId = teamId;
		TeamName = teamName;
		}
	}