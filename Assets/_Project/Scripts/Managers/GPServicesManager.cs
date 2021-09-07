using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine;

public class GPServicesManager : MonoBehaviour
{
    //public TMPro.TextMeshProUGUI debugText;

    private void Start()
    {
        //debugText.text += "Connecting to google play...\n";

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (result) => {
            //debugText.text += "Connection result: " + result + "\n";
        });
    }

    public void ShowLeaderboard()
    {
        //debugText.text += "ShowLeaderboard\n";
        PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIxKu5q5UdEAIQCA");
    }

    public void PostScore(int p_score)
    {
        Social.ReportScore(p_score, "CgkIxKu5q5UdEAIQCA", (bool success) => {
            // handle success or failure
        });
    }
}
