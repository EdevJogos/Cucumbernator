using UnityEngine;
using static TimerManager;
using static PoolDatabase;

public class Stars : MonoBehaviour
{
    public Range spawnTime;
    public Range quantity;

    private void Start()
    {
        CreatePool(Prefabs.STAR, 1);
        CreatePool(Prefabs.STAR_SMALL, 1);

        AddTimer(new RandomTimer(spawnTime, true, true, true, spawnTime.Value, Spawn));
        AddTimer(new RandomTimer(new Range(0.1f, 0.3f), true, true, true, 0.1f, SpawnLayer2));
    }

    private void Spawn()
    {
        int __total = (int)quantity.Value;

        for (int __i = 0; __i < __total; __i++)
        {
            GetPooledObject<Star>(Prefabs.STAR).Initialize(CameraManager.RandomRight(), Random.Range(0.3f, 1.0f), 5.0f);
        }
    }

    private void SpawnLayer2()
    {
        int __total = (int)quantity.Value;

        for (int __i = 0; __i < __total; __i++)
        {
            GetPooledObject<Star>(Prefabs.STAR_SMALL).Initialize(CameraManager.RandomRight(), 0.1f, 2.0f);
        }
    }
}