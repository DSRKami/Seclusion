using UnityEngine;

/// <summary>
/// Stores player-related configuration data
/// Useful for stat-driven gameplay elements
/// </summary>
public class PlayerData : ScriptableObject
{
    [Tooltip("Base movement speed of the player")]
    public float moveSpeed = 5f;
}
