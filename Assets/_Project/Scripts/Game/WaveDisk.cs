using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class WaveDisk : MonoBehaviour
{
    public float speed = 1f;
    public Light2D light2D;

    private bool _changing;
    private int _rgbIndex = 0;
    private float _target;
    private float[] _rgb = new float[3] { 255, 0, 255 };

    private void Update()
    {
        if (!_changing)
        {
            if (_rgb[_rgbIndex] == 255) { _target = 0; _changing = true; }
            else if (_rgb[_rgbIndex] == 0) { _target = 255; _changing = true; };
        }

        if (_changing)
        {
            _rgb[_rgbIndex] = Mathf.MoveTowards(_rgb[_rgbIndex], _target, speed * Time.deltaTime);

            light2D.color = new Color32((byte)_rgb[0], (byte)_rgb[1], (byte)_rgb[2], 255);

            if (Mathf.Abs(_rgb[_rgbIndex] - _target) == 0)
            {
                _changing = false;
                _rgbIndex = HelpExtensions.ClampCircle(_rgbIndex + 1, 0, 2);
            }
        }

        transform.Rotate(Vector3.forward * -90f * Time.deltaTime);
    }

    public void Active(bool p_active)
    {
        gameObject.SetActive(p_active);
    }
}
