using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCEO : MonoBehaviour
{
    private static GameCEO Instance;

    public static event System.Action onGameStateChanged;

    public static bool Tutorial { get; private set; } = true;
    public static GameState State { get; private set; }

    #if UNITY_EDITOR
    public GameState state;
    #endif

    public GUIManager guiManager;
    public InputManager inputManager;
    public CameraManager cameraManager;
    public AudioManager audioManager;
    public AgentsManager agentsManager;
    public StageManager stageManager;
    public ScoreManager scoreManager;
    public GPServicesManager gpServicesManager;

    public ParticlesDabatase particlesDabatase;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Initiate();
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initiate()
    {
        guiManager.onStartGameRequested += GuiManager_onStartGameRequested;
        guiManager.onPauseGameRequested += InputManager_onPauseRequested;
        guiManager.onQuitMatchRequested += GuiManager_onQuitMatchRequested;
        guiManager.onSaveScoreRequested += GuiManager_onSaveScoreRequested;
        guiManager.onScoreRequested += GuiManager_onScoreRequested;

        inputManager.onPauseRequested += InputManager_onPauseRequested;

        scoreManager.onScoreUpdated += ScoreManager_onScoreUpdated;
        scoreManager.onUpdateHighScoreRequested += ScoreManager_onUpdateHighScoreRequested;

        agentsManager.onUpdateLifePointsRequested += AgentsManager_onUpdateLifePointsRequested;

        

        cameraManager.Initiate();
        audioManager.Initate();
        guiManager.Initiate();
    }

    public void Initialize()
    {
        audioManager.Initialize();
        guiManager.Initialize();

        ChangeGameState(GameState.INTRO);
        guiManager.ShowDisplay(Displays.INTRO);
    }

    //-----------------CEO------------------

    private void StartGame()
    {
        guiManager.ShowDisplay(Displays.HUD, 1f, () => { ChangeGameState(GameState.PLAY); });
    }

    private void ChangeGameState(GameState p_state)
    {
        #if UNITY_EDITOR
            state = p_state;
        #endif

        State = p_state;
        onGameStateChanged?.Invoke();
    }

    private IEnumerator RoutineLoadScene(int p_scene, float p_delay = 0f)
    {
        if (p_delay > 0) yield return new WaitForSeconds(p_delay);

        var sceneLoader = SceneManager.LoadSceneAsync(p_scene);

        while (sceneLoader.progress <= 1)
        {
            yield return null;
        }
    }

    //-----------------INPUT MANAGER------------------

    private void InputManager_onPauseRequested()
    {
        if(State == GameState.PAUSE)
        {
            guiManager.ShowDisplay(Displays.HUD, 1f, () => { Time.timeScale = 1f; ChangeGameState(GameState.PLAY); });
        }
        else
        {
            Time.timeScale = 0f;
            ChangeGameState(GameState.PAUSE);
            guiManager.ShowDisplay(Displays.PAUSE);
        }
    }

    //-----------------GUI MANAGER------------------

    private void GuiManager_onStartGameRequested()
    {
        StartGame();
    }

    private void GuiManager_onQuitMatchRequested()
    {
        Time.timeScale = 1f;

        ChangeGameState(GameState.GAME_OVER);

        stageManager.Restart();
        scoreManager.Restart();
        agentsManager.Restart();

        ChangeGameState(GameState.INTRO);
        guiManager.ShowDisplay(Displays.INTRO);
    }

    private void GuiManager_onSaveScoreRequested()
    {
        scoreManager.AddScore(guiManager.GetData(Displays.GAME_OVER, 0).ToString(), scoreManager.score);
    }

    private void GuiManager_onScoreRequested()
    {
#if UNITY_ANDROID
        gpServicesManager.ShowLeaderboard();
#else
        guiManager.ShowDisplay(Displays.SCORE);
#endif
    }

    //-----------------SCORE MANAGER----------------

    private void ScoreManager_onScoreUpdated(int p_score)
    {
        guiManager.UpdateDisplay(Displays.HUD, 1, p_score);
    }

    private void ScoreManager_onUpdateHighScoreRequested()
    {
        guiManager.UpdateDisplay(Displays.SCORE, 0);
    }

    //-----------------STAGE MANAGER----------------

    //-----------------AGENTS MANAGER----------------

    private void AgentsManager_onUpdateLifePointsRequested(int p_life)
    {
        guiManager.UpdateDisplay(Displays.HUD, 0, p_life);

        if(p_life <= 0)
        {
#if UNITY_ANDROID
            gpServicesManager.PostScore(scoreManager.score);
#endif
            ChangeGameState(GameState.GAME_OVER);
            guiManager.ShowDisplay(Displays.GAME_OVER);
        }
    }
}
