[System.Serializable]
public class Player
	{
	// Player properties
	public string PlayerName;  // This property holds the player's name

	public int SkillLevel;     // This property holds the player's skill level

	// Constructor to initialize a player with a name and skill level
	public Player(string playerName, int skillLevel)
		{
		PlayerName = playerName;
		SkillLevel = skillLevel;
		}
	}