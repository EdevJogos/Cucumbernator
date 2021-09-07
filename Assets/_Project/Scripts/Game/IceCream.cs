using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCream : MonoBehaviour
{
    private int _hitPoints = 2;
    private Vector2 _direction;

    private void Start()
    {
        Vector2 __target = (Random.insideUnitCircle * 2) + AgentsManager.PlayerPos;
        _direction = (__target - (Vector2)transform.localPosition).normalized;
    }

    private void Update()
    {
        if (transform.position.x < CameraManager.HorizontalLimit.min)
        {
            Destroy(gameObject);
        }

        transform.Translate(_direction * 5f * StageManager.LowRatio * Time.deltaTime);
    }

    public void RequestDestroy()
    {
        CameraManager.ShakeScreen(0.2f, 0.2f);
        ParticlesDabatase.InstantiateParticle(Particles.ICECREAM_EXPLOSION, 0, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
