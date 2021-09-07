using UnityEngine;
using System;

public class AgentsManager : MonoBehaviour
{
    public event Action<int> onUpdateLifePointsRequested;

    public static AgentsManager Instance;

    public static Vector2 PlayerPos { get { return Instance.player.transform.localPosition; } }

    public Player player;

    private void Awake()
    {
        Instance = this;

        player.onLifePointsUpdated += Player_onLifePointsUpdated;
    }

    public void Restart()
    {
        player.Restart();
    }

    private void Player_onLifePointsUpdated(int p_life)
    {
        onUpdateLifePointsRequested?.Invoke(p_life);
    }
}
