using System.Collections;
using UnityEngine;
using static PoolDatabase;

public class StageManager : MonoBehaviour
{
    public static float LowRatio = 0.5f, HighRatio = 1.5f;

    public Range spawnTime;
    public Range quantity;

    [SerializeField] private int _scoreTarget = 0;
    [SerializeField] private int _scoreMark = 500;
    private float _spawnTimer;

    private void Start()
    {
        CreatePool(Prefabs.CUCUMBER, 5);
    }

    private void Update()
    {
        if (GameCEO.State != GameState.PLAY)
            return;

        if((ScoreManager.Instance.score - _scoreTarget) == _scoreMark)
        {
            LowRatio += 0.1f;
            HighRatio = Mathf.Clamp(HighRatio - 0.1f, 0.3f, 1.5f);
            
            _scoreTarget = ScoreManager.Instance.score;
            _scoreMark = _scoreTarget < 2000 ? 500 : _scoreTarget < 4000 ? 1000 : 2000;

            PrefabsDatabase.InstantiatePrefab(Prefabs.ICE_CREAM, 0, CameraManager.RandomRight(0.8f), Quaternion.identity);
        }

        _spawnTimer -= Time.deltaTime;

        if(_spawnTimer <= 0)
        {
            Spawn();

            _spawnTimer = spawnTime.Value * HighRatio;
        }
    }

    private void Spawn()
    {
        int __total = (int)quantity.Value;

        for (int __i = 0; __i < __total; __i++)
        {
            Cucumber __cucumber = GetPooledObject<Cucumber>(Prefabs.CUCUMBER);
            
            if (HighRatio >= 1f)
            {
                __cucumber.Initialize(CameraManager.RandomRight(0.85f), Random.Range(1f, HighRatio));
            }
            else
            {
                __cucumber.Initialize(CameraManager.RandomRight(0.85f), Random.Range(HighRatio, 1f));
            }
        }
    }

    public void Restart()
    {
        LowRatio = 0.5f;
        HighRatio = 1.5f;
        _scoreTarget = 0;
        _scoreMark = 500;
        _spawnTimer = 0f;
}
}
