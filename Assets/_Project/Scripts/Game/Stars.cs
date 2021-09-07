using UnityEngine;
using static TimerManager;
using static PrefabsDatabase;

public class Stars : MonoBehaviour
{
    public Range spawnTime;
    public Range quantity;

    private void Start()
    {
        AddTimer(new RandomTimer(spawnTime, true, true, true, spawnTime.Value, Spawn));
        AddTimer(new RandomTimer(new Range(0.1f, 0.3f), true, true, true, 0.1f, SpawnLayer2));
    }

    private void Spawn()
    {
        int __total = (int)quantity.Value;

        for (int __i = 0; __i < __total; __i++)
        {
            InstantiatePrefab<Star>(Prefabs.STAR, 0, CameraManager.RandomRight(), Quaternion.identity).Initialize(Random.Range(0.3f, 1.0f), 5.0f);
        }
    }

    private void SpawnLayer2()
    {
        int __total = (int)quantity.Value;

        for (int __i = 0; __i < __total; __i++)
        {
            InstantiatePrefab<Star>(Prefabs.STAR, 1, CameraManager.RandomRight(), Quaternion.identity).Initialize(0.1f, 2.0f);
        }
    }
}