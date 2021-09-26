using UnityEngine;

public class Star : MonoBehaviour
{
    public float speed;

    public void Initialize(Vector2 p_position, float p_scale, float p_speed)
    {
        speed = p_speed;
        transform.localScale = new Vector2(p_scale, p_scale);
        transform.localPosition = p_position;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (transform.position.x < CameraManager.HorizontalLimit.min)
        {
            gameObject.SetActive(false);
        }

        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}