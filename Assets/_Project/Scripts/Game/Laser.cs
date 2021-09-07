using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform impactPoint;

    private bool _destroying;
    private float _speed;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        GameCEO.onGameStateChanged += GameCEO_onGameStateChanged;
    }

    private void OnDestroy()
    {
        GameCEO.onGameStateChanged -= GameCEO_onGameStateChanged;
    }

    private void GameCEO_onGameStateChanged()
    {
        if (GameCEO.State == GameState.GAME_OVER)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!CameraManager.InsideCamera(transform.position))
        {
            Destroy(gameObject);
        }

        if (GameCEO.State != GameState.PLAY)
            return;

        if (_destroying)
        {
            _rigidbody2D.velocity = Vector2.MoveTowards(_rigidbody2D.velocity, Vector2.zero, 80f * Time.deltaTime);
        }
    }

    public void Initialize(float p_speed)
    {
        _speed = p_speed;
        _rigidbody2D.velocity = transform.right * _speed;
    }

    public void RequestDestroy()
    {
        PrefabsDatabase.InstantiatePrefab(Prefabs.LASER_IMPACT, 0, impactPoint.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if(p_other.tag == "Cucumber")
        {
            Cucumber __cucumber = p_other.GetComponent<Cucumber>();

            bool _destroyed = __cucumber.UpdateHitPoints(-1);

            if (_destroyed)
            {
                AudioManager.PlaySFX(SFXOccurrence.CUCUMBER_EXPLOSION);
                PrefabsDatabase.InstantiatePrefab(Prefabs.POINTS, 0, p_other.transform.localPosition, Quaternion.identity);
                ScoreManager.UpdateScore(100);
                RequestDestroy();
            }
            else Destroy(gameObject);
        }
        else if(p_other.tag == "IceCream")
        {
            IceCream __iceCream = p_other.GetComponentInParent<IceCream>();

            bool _destroyed = __iceCream.UpdateHitPoints(-1);

            if (_destroyed)
            {
                AudioManager.PlaySFX(SFXOccurrence.CUCUMBER_EXPLOSION);
                PrefabsDatabase.InstantiatePrefab(Prefabs.POINTS, 1, p_other.transform.position, Quaternion.identity);
                ScoreManager.UpdateScore(100);
                RequestDestroy();
            }
            else Destroy(gameObject);
        }
    }
}
