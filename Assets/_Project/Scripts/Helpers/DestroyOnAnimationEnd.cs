using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    private float _startTime, _total;

    private void Start()
    {
        _startTime = Time.time;

        float __duration = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        float __speed = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).speed;
        _total = __duration * __speed;
    }

    private void Update()
    {
        if( Time.time - _startTime > _total)
        {
            Destroy(gameObject);
        }
    }
}