using SQLite;

using System;

public class Match
	{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	public int Team1Id { get; set; } // Foreign key to Team 1
	public int Team2Id { get; set; } // Foreign key to Team 2

	public int Team1Score { get; set; }
	public int Team2Score { get; set; }

	public DateTime MatchDate { get; set; }
	}
