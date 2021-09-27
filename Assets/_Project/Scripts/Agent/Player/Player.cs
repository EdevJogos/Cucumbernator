using System.Collections;
using UnityEngine;
using static PrefabsDatabase;
using static PoolDatabase;

public class Player : Agent
{
    public static event System.Action<float> onExhaustionUpdated;

    public event System.Action<int> onLifePointsUpdated;

    public Sprite defaultSprite;
    public Sprite exhaustedSprite;
    public Sprite frozenSprite;
    public SpriteRenderer spriteRenderer;
    public Transform rightPoint, leftPoint;
    public Transform rightEye, leftEye;
    public WaveDisk waveDisk;

    private bool _exhausted, _frozen;
    private int _lifes = 7;
    private float _exhaustion = 0.0f, _frozenTimer;

    private void Awake()
    {
        GameCEO.onGameStateChanged += GameCEO_onGameStateChanged;
    }

    private void OnDestroy()
    {
        GameCEO.onGameStateChanged -= GameCEO_onGameStateChanged;
    }

    private void GameCEO_onGameStateChanged()
    {
        if(GameCEO.State == GameState.GAME_OVER)
        {
            StopWaveDisk();
        }
    }

    private void Start()
    {
        CreatePool(Prefabs.LASER, 8);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!_exhausted)
            {
                if (InputManager.MouseWorld.x > 5.85f && InputManager.MouseWorld.y < - 3.75f)
                    return;

                Shoot();
            }
        }

        if(_exhaustion > 0)
        {
            UpdateExhaustion(-(0.5f * Time.deltaTime));
        }

        if(_frozen)
        {
            _frozenTimer -= Time.deltaTime;

            if(_frozenTimer <= 0f)
            {
                StopWaveDisk();
            }
        }
    }

    public void Restart()
    {
        StopWaveDisk();
        UpdateExhaustion(-_exhaustion);
        UpdateLifePoints(7);
    }

    private void Shoot()
    {
        AudioManager.PlaySFX(SFXOccurrence.LASER);

        Vector2 __rightDirection = (InputManager.MouseWorld - (Vector2)rightPoint.position).normalized;
        Vector2 __leftDirection = (InputManager.MouseWorld - (Vector2)leftPoint.position).normalized;

        rightEye.right = __rightDirection;
        leftEye.right = __leftDirection;

        GetPooledObject<Laser>(Prefabs.LASER).Initialize(rightPoint.position, rightEye.rotation, 20f);
        GetPooledObject<Laser>(Prefabs.LASER).Initialize(leftPoint.position, leftEye.rotation, 20f);

        //InstantiatePrefab<Laser>(Prefabs.LASER, 0, rightPoint.position, rightEye.rotation).Initialize(20f);
        //InstantiatePrefab<Laser>(Prefabs.LASER, 0, leftPoint.position, leftEye.rotation).Initialize(20f);

        GetComponent<Animator>().SetTrigger("Muzzle");
        UpdateExhaustion(0.2f);
    }

    private void UpdateLifePoints(int p_amount)
    {
        _lifes = Mathf.Clamp(_lifes + p_amount, 0, 7);

        onLifePointsUpdated?.Invoke(_lifes);
    }

    private void UpdateExhaustion(float p_amount)
    {
        _exhaustion = Mathf.Clamp(_exhaustion + p_amount, 0.0f, 1.1f);

        if(!_exhausted)
        {
            onExhaustionUpdated?.Invoke(_exhaustion);
        }

        if(_exhaustion >= 1f)
        {
            spriteRenderer.sprite = exhaustedSprite;
            onExhaustionUpdated?.Invoke(1f);
            _exhausted = true;
        }

        if(_exhausted && _exhaustion <= 0f)
        {
            spriteRenderer.sprite = defaultSprite;
            onExhaustionUpdated?.Invoke(_exhaustion);
            _exhausted = false;
        }
    }

    private void StopWaveDisk()
    {
        _frozen = false;
        _frozenTimer = 5f;
        waveDisk.Active(false);
        spriteRenderer.sprite = defaultSprite;
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (!gameObject.activeSelf)
            return;

        if (p_other.tag == "Cucumber")
        {
            AudioManager.PlaySFX(SFXOccurrence.PLAYER_HIT);
            UpdateLifePoints(-1);
            p_other.GetComponent<Cucumber>().RequestDestroy();
        }
        else if(p_other.tag == "IceCream")
        {
            AudioManager.PlaySFX(SFXOccurrence.FROZEN);
            AudioManager.PlaySFX(SFXOccurrence.PLAYER_HIT);
            spriteRenderer.sprite = frozenSprite;
            p_other.GetComponentInParent<IceCream>().RequestDestroy();
            waveDisk.Active(true);
            _frozenTimer = 5f;
            _frozen = true;
        }
    }
}
