using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [System.Serializable]
    public class ScoreData
    {
        public string name;
        public int score;

        public ScoreData(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }

    public static ScoreManager Instance;

    public event System.Action<int> onScoreUpdated;
    public event System.Action onUpdateHighScoreRequested;

    public static List<ScoreData> HighScores = new List<ScoreData>(10);

    public int score;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //LB_Controller.OnUpdatedScores += OnLeaderboardUpdated;
        //DownloadScores();
    }

    public void Restart()
    {
        UpdateScore(-score);
    }

    public static void UpdateScore(int p_amount)
    {
        Instance.score = HelpExtensions.ClampMin0(Instance.score + p_amount);
        Instance.onScoreUpdated?.Invoke(Instance.score);
    }

    public static int IsTop10()
    {
        if (HighScores.Count == 0) return -1;

        for (int __i = 0; __i < HighScores.Count; __i++)
        {
            if (Instance.score > HighScores[__i].score)
            {
                return __i;
            }
        }

        return -2;
    }

    #region My Systems

    public void AddScore(string p_username, int p_score)
    {
        bool __updated = false;

        for (int __i = 0; __i < HighScores.Count; __i++)
        {
            if(HighScores[__i].name == p_username)
            {
                if(p_score > HighScores[__i].score)
                {
                    HighScores[__i].score = p_score;
                }

                __updated = true;
                break;
            }
        }

        if(!__updated)
        {
            HighScores.Add(new ScoreData(p_username, p_score));
            HighScores.Sort((a, b) => b.score.CompareTo(a.score));
        }

        LB_Controller.instance.StoreScore(p_score, p_username);

        onUpdateHighScoreRequested?.Invoke();
    }

    private void DownloadScores()
    {
        LB_Controller.instance.ReloadLeaderboard();
    }

    private void LB_StoreScore_onScoreStored()
    {
        DownloadScores();
    }

    private void OnLeaderboardUpdated(LB_Entry[] entries)
    {
        HighScores.Clear();

        foreach (LB_Entry entry in entries)
        {
            HighScores.Add(new ScoreData(entry.name, entry.points));
            //Debug.Log("Rank: " + entry.rank + "; Name: " + entry.name + "; Points: " + entry.points);
        }

        Debug.Log("Entries loaded");

        onUpdateHighScoreRequested?.Invoke();
    }

    private void OnDestroy()
    {
        LB_Controller.OnUpdatedScores -= OnLeaderboardUpdated;
    }

    #endregion
}
