using System;
using UnityEngine;

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

    private int currentDifficulty;
    float elapsedTime = 0f;

    public Metro Metro;
    public ReputationManager Reputation;
    public TroubleMaker TroubleMaker;
    public NotificationManager NotificationManager;
    public MetroCursor MetroCursor;

    public GameState CurrentGameState;
    public int CurrentScore {get; private set;}

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
    }

    public void ResetScore()
    {
        elapsedTime = 0;
        CurrentScore = 0;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void StartGame()
    {
        if(CurrentGameState == GameState.MetroActive) return;
        
        CurrentGameState = GameState.MetroActive;

        NotificationManager.StartDialogue(NotificationManager.TutorialDialogue);
        
        OnStartGame?.Invoke();
    
    }

    public void ResetGame()
    {
        currentDifficulty = baseDifficulty;
        
        ResetScore();
        NotificationManager.ResetNotificationManager();
        Reputation.ResetReputation();
        Metro.ResetAllStations();
        MetroCursor.ResetStationChain();

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
    }

    void Update()
    {
        if(CurrentGameState == GameState.MetroActive)
        {
            Reputation.UpdateReputationLoss();
            TroubleMaker.UpdateTroubleMaker();
            UpdateScore();
        }
        
    }

    // tojam2026-06@georgebrowncollege.onmicrosoft.com
    // ToJ@m2026!
}
