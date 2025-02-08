using System;

public class Team
	{
	public int TeamId { get; set; }
	public string TeamName { get; set; }

	// Constructor to initialize TeamId and TeamName
	public Team(int teamId, string teamName)
		{
		if (string.IsNullOrEmpty(teamName))
			{
			throw new ArgumentException("Team name cannot be empty.");
			}
		if (teamName.Length > 50) // Max length for the team name
			{
			throw new ArgumentException("Team name cannot exceed 50 characters.");
			}

		TeamId = teamId;
		TeamName = teamName;
		}

	// Method to update the team name with validation
	public void UpdateTeamName(string newTeamName)
		{
		if (string.IsNullOrEmpty(newTeamName))
			{
			throw new ArgumentException("Team name cannot be empty.");
			}
		if (newTeamName.Length > 50) // Max length for the team name
			{
			throw new ArgumentException("Team name cannot exceed 50 characters.");
			}

		TeamName = newTeamName;
		}

	// Override Equals method to compare teams by their TeamId
	public override bool Equals(object obj)
		{
		return obj is Team team && TeamId == team.TeamId;
		}

	// Override GetHashCode for consistent behavior in collections
	public override int GetHashCode()
		{
		return TeamId.GetHashCode();
		}

	// Override ToString for a human-readable representation of the team
	public override string ToString()
		{
		return $"{TeamName} (ID: {TeamId})";
		}
	}
