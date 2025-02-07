using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeightSettings", menuName = "Player/WeightSettings", order = 1)]
public class PlayerWeightSettings:ScriptableObject
	{
	// --- Region: Current Season Stats Weights ---
	public float weightCurrentSeasonPointsAwarded = 1.0f;
	public float weightCurrentSeasonMatchesWon = 1.0f;
	public float weightCurrentSeasonDefensiveShotAverage = 1.0f;
	public float weightCurrentSeasonSkillLevel = 1.0f;
	public float weightCurrentSeasonPpm = 1.0f;
	public float weightCurrentSeasonShutouts = 1.0f;
	public float weightCurrentSeasonMiniSlams = 1.0f;
	public float weightCurrentSeasonNineOnTheSnap = 1.0f;
	public float weightCurrentSeasonPaPercentage = 1.0f;
	public float weightCurrentSeasonBreakAndRun = 1.0f;

	// --- Region: Lifetime Stats Weights ---
	public float weightLifetimeGamesWon = 1.0f;
	public float weightLifetimeMiniSlams = 1.0f;
	public float weightLifetimeNineOnTheSnap = 1.0f;
	public float weightLifetimeShutouts = 1.0f;
	public float weightLifetimeBreakAndRun = 1.0f;
	public float weightLifetimeDefensiveShotAverage = 1.0f;
	public float weightLifetimeMatchesPlayed = 1.0f;
	public float weightLifetimeMatchesWon = 1.0f;
	}
