using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{

    public Action<string> OnNotificationMessage;
    public Action<Sprite> OnSpriteChange;

    private Metro metro;

    [Header("Dialogue System")]
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject skipButton;

    public DialogueSO TutorialDialogue;

    private int currentDialogueIndex = 0;
    private DialogueSO currentDialogueSO;

    [SerializeField] private Sprite mascot_normal;
    [SerializeField] private Sprite mascot_cry;
    [SerializeField] private Sprite mascot_smile;


    void Start()
    {
        metro = GameManager.Instance.Metro;

        metro.OnStationStateChanged += CreateNotification;
        nextButton.SetActive(false);
        skipButton.SetActive(false);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            EndDialogue();
        }
    }

    public void ResetNotificationManager()
    {
        OnNotificationMessage?.Invoke("");
        OnSpriteChange?.Invoke(TutorialDialogue.defaultSprite);
        currentDialogueIndex = 0;
        currentDialogueSO = null;
        nextButton.SetActive(false);
        skipButton.SetActive(false);
    }

    void CreateNotification(StationContext stationContext)
    {
        string message = "";

        switch(stationContext.stationState)
        {
            case Station.StationState.Troubled: 
                OnSpriteChange?.Invoke(mascot_normal);
                message = GenerateTroubleMessage(stationContext);
                break;
            case Station.StationState.Delayed:
                OnSpriteChange?.Invoke(mascot_cry);
                message = GenerateDelayMessage(stationContext);
                break;
            case Station.StationState.Cleared:
                OnSpriteChange?.Invoke(mascot_smile);
                message = GenerateClearmessage(stationContext);
                break;
            default:
                break;
        }
        
        OnNotificationMessage?.Invoke(message);
    }

    public string GenerateTroubleMessage(StationContext stationContext)
    {
        return $"There is {GetTroubleName(stationContext.currentMinigame)} at {stationContext.station.StationName}";
    }
    public string GenerateDelayMessage(StationContext stationContext)
    {
        return $"Get your ass over to {stationContext.station.StationName} there's a fricking DELAY!";
    }
    public string GenerateClearmessage(StationContext stationContext)
    {
        return $"{stationContext.station.StationName} cleared!";
    }

    string GetTroubleName(Station.Minigame minigame)
    {
        if(minigame == Station.Minigame.FixElevator)
        {
            return "a <b>Broken Elevator</b>";
        }
        if(minigame == Station.Minigame.FixEngine)
        {
            return "an <b>Engine Failure</b>";
        }

        return "";
    }

    public void StartDialogue(DialogueSO dialogueSO)
    {
        if(dialogueSO == null) {
            Debug.LogError("DialogueSO is null");
            return;
        }

        GameManager.Instance.CurrentGameState = GameManager.GameState.MetroInactive;

        currentDialogueSO = dialogueSO;
        currentDialogueIndex = 0;
        nextButton.SetActive(true);
        skipButton.SetActive(true);
        OnSpriteChange?.Invoke(TutorialDialogue.defaultSprite);
        NextLine();
    }

    public void NextLine()
    {
        if(currentDialogueSO == null) return;

        if(currentDialogueIndex < currentDialogueSO.Lines.Count) {
            OnNotificationMessage?.Invoke(currentDialogueSO.Lines[currentDialogueIndex].text);
            OnSpriteChange?.Invoke(currentDialogueSO.Lines[currentDialogueIndex].expression);
        } else
        {
            EndDialogue();
        }

        currentDialogueIndex++;
    }

    public void EndDialogue()
    {
        GameManager.Instance.CurrentGameState = GameManager.GameState.MetroActive;

        currentDialogueSO = null;
        currentDialogueIndex = 0;
        OnSpriteChange?.Invoke(TutorialDialogue.defaultSprite);
        nextButton.SetActive(false);
        skipButton.SetActive(false);
    }
}
