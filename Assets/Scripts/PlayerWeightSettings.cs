using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeightSettings", menuName = "Player/WeightSettings", order = 1)]
public class PlayerWeightSettings:ScriptableObject
	{
	// --- Region: Current Season Stats Weights ---
	[Header("Current Season Stats Weights")]
	[Tooltip("Weight for points awarded in the current season.")]
	public float weightCurrentSeasonPointsAwarded = 1.0f;

	[Tooltip("Weight for matches won in the current season.")]
	public float weightCurrentSeasonMatchesWon = 1.0f;

	[Tooltip("Weight for defensive shot average in the current season.")]
	public float weightCurrentSeasonDefensiveShotAverage = 1.0f;

	[Tooltip("Weight for skill level in the current season.")]
	public float weightCurrentSeasonSkillLevel = 1.0f;

	[Tooltip("Weight for PPM (Points per Match) in the current season.")]
	public float weightCurrentSeasonPpm = 1.0f;

	[Tooltip("Weight for shutouts in the current season.")]
	public float weightCurrentSeasonShutouts = 1.0f;

	[Tooltip("Weight for mini slams in the current season.")]
	public float weightCurrentSeasonMiniSlams = 1.0f;

	[Tooltip("Weight for 'Nine on the Snap' occurrences in the current season.")]
	public float weightCurrentSeasonNineOnTheSnap = 1.0f;

	[Tooltip("Weight for PA (Percentage Accuracy) in the current season.")]
	public float weightCurrentSeasonPaPercentage = 1.0f;

	[Tooltip("Weight for break and run occurrences in the current season.")]
	public float weightCurrentSeasonBreakAndRun = 1.0f;

	// --- Region: Lifetime Stats Weights ---
	[Header("Lifetime Stats Weights")]
	[Tooltip("Weight for games won in the player's lifetime.")]
	public float weightLifetimeGamesWon = 1.0f;

	[Tooltip("Weight for mini slams in the player's lifetime.")]
	public float weightLifetimeMiniSlams = 1.0f;

	[Tooltip("Weight for 'Nine on the Snap' occurrences in the player's lifetime.")]
	public float weightLifetimeNineOnTheSnap = 1.0f;

	[Tooltip("Weight for shutouts in the player's lifetime.")]
	public float weightLifetimeShutouts = 1.0f;

	[Tooltip("Weight for break and run occurrences in the player's lifetime.")]
	public float weightLifetimeBreakAndRun = 1.0f;

	[Tooltip("Weight for defensive shot average in the player's lifetime.")]
	public float weightLifetimeDefensiveShotAverage = 1.0f;

	[Tooltip("Weight for matches played in the player's lifetime.")]
	public float weightLifetimeMatchesPlayed = 1.0f;

	[Tooltip("Weight for matches won in the player's lifetime.")]
	public float weightLifetimeMatchesWon = 1.0f;

	// --- Region: Additional Weights ---
	[Header("Additional Weights")]
	[Tooltip("General weight for points.")]
	public float PointsWeight = 1.0f;

	[Tooltip("General weight for defensive shots.")]
	public float DefensiveShotWeight = 1.0f;

	[Tooltip("General weight for match wins.")]
	public float MatchWinWeight = 1.0f;

	[Tooltip("General weight for 'Nine on the Snap' occurrences.")]
	public float NineOnSnapWeight = 1.0f;

	[Tooltip("General weight for break and run occurrences.")]
	public float BreakAndRunWeight = 1.0f;
	}
