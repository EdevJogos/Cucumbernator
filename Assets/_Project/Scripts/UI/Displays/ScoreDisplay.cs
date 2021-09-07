using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplay : Display
{
    public TMPro.TextMeshProUGUI[] scoresTexts;

    public override void Show(bool p_show, Action p_callback, float p_ratio)
    {
        if(p_show)
        {
            if (scoresTexts[0].text == "") scoresTexts[0].text = "Loading...";
        }

        base.Show(p_show, p_callback, p_ratio);
    }

    public override void UpdateDisplay(int p_operation, float p_value, float p_data)
    {
        switch(p_operation)
        {
            case 0:
                UpdateHighScoreTable();
                break;
        }

        base.UpdateDisplay(p_operation, p_value, p_data);
    }

    private void UpdateHighScoreTable()
    {
        int __total = ScoreManager.HighScores.Count;

        for (int __i = 0; __i < __total && __i < 10; __i++)
        {
            ScoreManager.ScoreData __scoreData = ScoreManager.HighScores[__i];
            scoresTexts[__i].text = __scoreData.name + " - " + __scoreData.score;
        }
    }
}
