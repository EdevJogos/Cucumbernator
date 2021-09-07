using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BackgroundLight : MonoBehaviour
{
    public float speed = 1f;

    private bool _changing;
    private int _rgbIndex = 0;
    private float _target;
    private float[] _rgb = new float[3] { 255, 145, 255 };
    private Light2D _light2D;

    private void Awake()
    {
        _light2D = GetComponent<Light2D>();
    }

    private void Update()
    {
        if(!_changing)
        {
            if (_rgb[_rgbIndex] == 255) { _target = 145; _changing = true; }
            else if (_rgb[_rgbIndex] == 145) { _target = 255; _changing = true; };
        }

        if(_changing)
        {
            _rgb[_rgbIndex] = Mathf.MoveTowards(_rgb[_rgbIndex], _target, speed * Time.deltaTime);

            _light2D.color = new Color32((byte)_rgb[0], (byte)_rgb[1], (byte)_rgb[2], 255);

            if(Mathf.Abs(_rgb[_rgbIndex] - _target) == 0)
            {
                _changing = false;
                _rgbIndex = HelpExtensions.ClampCircle(_rgbIndex + 1, 0, 2);
            }
        }
    }
}
