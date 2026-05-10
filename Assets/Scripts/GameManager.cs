using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

// Acts as the manger of the game and the contains all references to important game components
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Action<int> OnScoreChanged;
    public Action OnStartGame;

    public enum GameState
    {
        MetroInactive,
        MetroActive
    }

    [SerializeField] private int baseDifficulty;
    [SerializeField] private float baseMaxTimeToDelay;
    [SerializeField] private float baseMinTimeToDelay;

    public int CurrentDifficulty {get; private set;}
    float elapsedTime = 0f;

    public Canvas LoseCanvas;
    public Metro Metro;
    public ReputationManager Reputation;
    public TroubleMaker TroubleMaker;
    public NotificationManager NotificationManager;
    public MetroCursor MetroCursor;

    public GameState CurrentGameState;
    public int CurrentScore {get; private set;}
    public int Highscore {get; private set;}

    public float CalculateTimeToDelay(Station station)
    {
        return Mathf.Lerp(baseMaxTimeToDelay, baseMinTimeToDelay, station.BasePopulation);
    }

    public void UpdateScore()
    {
        elapsedTime += Time.deltaTime;

        var prevScore = CurrentScore;
        CurrentScore = (int)elapsedTime;

        if(prevScore != CurrentScore)
        {
            OnScoreChanged?.Invoke(CurrentScore);
        }

        CurrentDifficulty = baseDifficulty + (int)CurrentScore/12;

        
    }

    public void ResetScore()
    {
        elapsedTime = 0;
        CurrentScore = 0;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void StartGame()
    {
        CurrentGameState = GameState.MetroActive;

        NotificationManager.StartDialogue(NotificationManager.TutorialDialogue);
        
        OnStartGame?.Invoke();
    
    }

    public void LoseGame()
    {
        CurrentGameState = GameState.MetroInactive;
        LoseCanvas.gameObject.SetActive(true);
        if(CurrentScore > Highscore)
        {
            Highscore = CurrentScore;
            OnScoreChanged?.Invoke(CurrentScore);
        }
    }

    public void ResetGame()
    {
        LoseCanvas.gameObject.SetActive(false);
        CurrentDifficulty = baseDifficulty;
        
        ResetScore();
        NotificationManager.ResetNotificationManager();
        Reputation.ResetReputation();
        Metro.ResetAllStations();
        MetroCursor.ResetStationChain();

        MinigameManager.Instance.UnloadAllScenes();
    }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }

        LoseCanvas.gameObject.SetActive(false);
    }

    bool gameStarted = false;

    void Update()
    {
        if(!gameStarted)
        {
            gameStarted = true;
            ResetGame();
            StartGame();

        }

        if(CurrentGameState == GameState.MetroActive)
        {
            Reputation.UpdateReputationLoss();
            TroubleMaker.UpdateTroubleMaker();
            UpdateScore();
        }
        
    }
}
