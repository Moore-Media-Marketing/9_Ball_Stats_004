public class Team
	{
	public int TeamId { get; set; }
	public string TeamName { get; set; }

	public Team(int teamId, string teamName)
		{
		TeamId = teamId;
		TeamName = teamName;
		}

	// Method to update the team name
	public void UpdateTeamName(string newTeamName)
		{
		TeamName = newTeamName;
		}
	}
