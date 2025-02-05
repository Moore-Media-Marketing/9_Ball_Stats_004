using System;

public class Team
	{
	public int TeamId { get; set; }
	public string TeamName { get; set; }

	// --- Constructor --- //
	public Team(int teamId, string teamName)
		{
		TeamId = teamId;
		TeamName = teamName;
		}

	// --- From CSV --- //
	public static Team FromCsv(string csvLine)
		{
		string[] values = csvLine.Split(',');
		if (values.Length != 2)
			{
			throw new ArgumentException("CSV line does not contain enough values.");
			}

		return new Team(int.Parse(values[0]), values[1]);
		}

	// --- To CSV --- //
	public string ToCsv()
		{
		return $"{TeamId},{TeamName}";
		}
	}
