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

    void Start()
    {
        metro = GameManager.Instance.Metro;

        metro.OnStationStateChanged += CreateNotification;
        nextButton.SetActive(false);
        skipButton.SetActive(false);
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
                message = GenerateTroubleMessage(stationContext);
                break;
            case Station.StationState.Delayed:
                message = GenerateDelayMessage(stationContext);
                break;
            case Station.StationState.Cleared:
                message = GenerateClearmessage(stationContext);
                break;
            default:
                break;
        }
        
        OnNotificationMessage?.Invoke(message);
    }

    public string GenerateTroubleMessage(StationContext stationContext)
    {
        return $"There is at {stationContext.currentMinigame.ToString()} in {stationContext.station.StationName}";;
    }
    public string GenerateDelayMessage(StationContext stationContext)
    {
        return $"Delay at {stationContext.station.StationName}";
    }
    public string GenerateClearmessage(StationContext stationContext)
    {
        return $"{stationContext.station.StationName} cleared!";
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
