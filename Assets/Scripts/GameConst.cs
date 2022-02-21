using UnityEngine;

public static class GameConst
{
    #region Tag
    public static string PLAYER_TAG = "Player";
    public static string ENEMY_TAG = "Enemy";
    public static string DEAD_TAG = "Dead";
    #endregion

    #region LayerMask
    public static LayerMask PLAYER = LayerMask.NameToLayer("Player");
    public static LayerMask IGNORE_RAYCAST = LayerMask.NameToLayer("Ignore Raycast");
    #endregion
}
