using UnityEngine;

public class Cucumber : MonoBehaviour, IDestructable
{
    private int _hitPoints = 2;
    private Vector2 _direction;

    public void Initialize(Vector3 p_position, float p_scale)
    {
        _hitPoints = 2;

        transform.position = p_position;
        transform.localScale = new Vector2(p_scale, p_scale);

        gameObject.SetActive(true);

        Vector2 __target = (Random.insideUnitCircle * 2.0f) + AgentsManager.PlayerPos;
        _direction = (__target - (Vector2)transform.localPosition).normalized;
    }

    private void Update()
    {
        if (transform.position.x < CameraManager.HorizontalLimit.min)
        {
            gameObject.SetActive(false);
        }

        transform.Translate(_direction * 5f * StageManager.LowRatio * Time.deltaTime);
    }

    public void RequestDestroy()
    {
        CameraManager.ShakeScreen(0.2f, 0.2f);
        ParticlesDabatase.InstantiateParticle(Particles.CUCUMBER_EXPLOSION, 0, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public void DestroyImmediately()
    {
        gameObject.SetActive(false);
    }

    public bool UpdateHitPoints(int p_amount)
    {
        _hitPoints += p_amount;

        if (_hitPoints <= 0)
        {
            RequestDestroy();
        }

        return _hitPoints <= 0;
    }
}
