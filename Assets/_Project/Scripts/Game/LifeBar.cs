using UnityEngine;
using UnityEngine.UI;


public class LifeBar : MonoBehaviour
{
    public Image life;
    public ParticleSystem explosion;

    public void Active(bool p_active)
    {
        if (!p_active && life.enabled) explosion.Play();
        life.enabled = p_active;
    }
}
