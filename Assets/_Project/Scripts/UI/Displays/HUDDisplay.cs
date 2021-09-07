using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDDisplay : Display
{
    public GameObject[] myObjects;
    public LifeBar[] lifeBars;
    public TMPro.TextMeshProUGUI scoreText;
    public Slider exhaustionSlider;
    public Image sliderImage;

    public override void Initiate()
    {
        Player.onExhaustionUpdated += Player_onExhaustionUpdated;

        base.Initiate();
    }

    public override void Show(bool p_show, Action p_callback, float p_ratio)
    {
        for (int __i = 0; __i < myObjects.Length; __i++)
        {
            myObjects[__i].SetActive(p_show);
        }

        base.Show(p_show, p_callback, p_ratio);
    }

    public override void UpdateDisplay(int p_operation, float p_value, float p_data)
    {
        switch (p_operation)
        {
            case 0:
                UpdateLifePoints((int)p_value);
                break;
            case 1:
                scoreText.text = "Score: " + (int)p_value;
                break;
        }

        base.UpdateDisplay(p_operation, p_value, p_data);
    }

    private void UpdateLifePoints(int p_life)
    {
        int __lifeIndex = p_life - 1;

        for (int __i = 0; __i < lifeBars.Length; __i++)
        {
            lifeBars[__i].Active(__i <= __lifeIndex);
        }
    }

    private void Player_onExhaustionUpdated(float p_exhaustion)
    {
        exhaustionSlider.value = p_exhaustion;

        if (p_exhaustion >= 0.75f) sliderImage.color = Color.red;
        else if (p_exhaustion >= 0.5f) sliderImage.color = Color.yellow;
        else sliderImage.color = new Color(0, 250, 255, 255);
    }
}
