using UnityEngine;

public class Star : MonoBehaviour
{
    public float speed;

    public void Initialize(float p_scale, float p_speed)
    {
        speed = p_speed;
        transform.localScale = new Vector2(p_scale, p_scale);
    }

    private void Update()
    {
        if (transform.position.x < CameraManager.HorizontalLimit.min)
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}