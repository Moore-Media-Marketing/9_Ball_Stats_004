// --- Team Class --- //
public class Team
	{
	public int TeamId { get; set; }
	public string TeamName { get; set; }

	// Constructor
	public Team(int teamId, string teamName)
		{
		TeamId = teamId;
		TeamName = teamName;
		}
	}
