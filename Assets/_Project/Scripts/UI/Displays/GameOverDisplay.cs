using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverDisplay : Display
{
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI top10Text;
    public TMPro.TextMeshProUGUI warningText;
    public TMPro.TMP_InputField inputField;
    public Button saveButton;

    public GameObject[] defaultComponents;
    public GameObject[] top10Components;

    public override void Show(bool p_show, Action p_callback, float p_ratio)
    {
        if(p_show)
        {
            saveButton.interactable = true;
            scoreText.text = "Total score: " + ScoreManager.Instance.score;

            int __position = ScoreManager.IsTop10();
            bool __isTop10 = __position >= -1;

#if UNITY_ANDROID
            __isTop10 = false;
#endif

            if (__isTop10)
            {
                if(__position == -1)
                {
                    top10Text.text = "Do you wish to save your score?";
                }
                else
                {
                    __position = __position == -1 ? 0 : __position;
                    top10Text.text = "You've got " + (__position + 1) + "º place!\nsave your score?";
                }
            }

            for (int __i = 0; __i < top10Components.Length; __i++)
            {
                top10Components[__i].SetActive(__isTop10);
            }

            for (int __i = 0; __i < defaultComponents.Length; __i++)
            {
                defaultComponents[__i].SetActive(!__isTop10);
            }
        }

        base.Show(p_show, p_callback, p_ratio);
    }

    public override void RequestAction(int p_action)
    {
        if(p_action == 1)
        {
            if(inputField.text == "")
            {
                warningText.text = "The name field cannot be empty.";
            }
            else if(inputField.text.Contains(" "))
            {
                warningText.text = "The name field cannot have space.";
            }
            else
            {

                saveButton.interactable = false;
                base.RequestAction(p_action);
                warningText.text = "Saved!";
            }
        }
        else base.RequestAction(p_action);
    }

    public override object GetData(int p_data)
    {
        return inputField.text;
    }
}
