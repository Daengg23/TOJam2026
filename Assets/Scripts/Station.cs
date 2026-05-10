using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.EventSystems;
using TMPro;

public class Station : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{   
    public Action<Station> OnCleared;
    public Action<Station> OnTroubled;
    public Action<Station> OnDelayed;
    public Action<Station> OnMinigamePicked;

    public Action<Station> OnStationStateChanged;

    public enum Minigame
    {
        None,
        //CleanupTrack,
        FixEngine,
        FixElevator
        //CatchPassenger
    }

    public enum StationState {
        Cleared,
        Troubled,
        Delayed
    }

    public Minigame CurrentMinigame;

    [SerializeField] private Image stationIcon;
    [SerializeField] private Image markedIcon;
    [SerializeField] private TextMeshProUGUI minigameText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float emphScale = 1.25f;
    [SerializeField] private float demphAlpha = 0.1f;

    [Header("Unique Properties")]

    public string StationName;

    [Range(0f, 1f)]
    public float BasePopulation = 0.1f;

    private float troubleTimer;
    private float delayTimer;
    private RectTransform rectTransform;
    
    [HideInInspector] public StationState CurrentState {get; private set;}

    void Awake()
    {
        OnCleared += (s) => OnStationStateChanged?.Invoke(s);
        OnTroubled += (s) => OnStationStateChanged?.Invoke(s);
        OnDelayed += (s) => OnStationStateChanged?.Invoke(s);

        rectTransform = GetComponent<RectTransform>();

        ResetStation();
        markedIcon.gameObject.SetActive(false);
    }

    public void ResetStation()
    {
        StopAllCoroutines();
        SetMinigame(Minigame.None);
        SetStationState(StationState.Cleared);
        troubleTimer = 0f;
        delayTimer = 0f;
    }

    public void SetStationState(StationState state)
    {
        StationState stateToCheck = CurrentState;
        CurrentState = state;
        
        switch(state)
        {
            case StationState.Cleared:
                stationIcon.color = Color.white;
                if(stateToCheck != state) OnCleared?.Invoke(this);
                break;
            case StationState.Troubled:
                stationIcon.color = Color.yellow;
                if(stateToCheck != state) OnTroubled?.Invoke(this);
                break;
            case StationState.Delayed:
                stationIcon.color = Color.red;
                if(stateToCheck != state) OnDelayed?.Invoke(this);
                break;
        }
        
    }

    public void SetMinigame(Minigame minigame)
    {
        Minigame minigameToCheck = minigame;
        CurrentMinigame = minigame;

        switch(minigameToCheck)
        {
            case Minigame.None:
                minigameText.text = "";
                break;
            //case Minigame.CleanupTrack:
            //    minigameText.text = "CL";
            //    break;
            case Minigame.FixEngine:
                minigameText.text = "T";
                break;
            //case Minigame.CatchPassenger:
            //    minigameText.text = "C";
            //    break;
            case Minigame.FixElevator:
                minigameText.text = "E";
                break;
        }
    }

    public void StartTrouble()
    {
        if(CurrentState != StationState.Troubled)
        {
            PickMinigame();
            SetStationState(StationState.Troubled);
            StartCoroutine(WaitForStationDelay(GameManager.Instance.CalculateTimeToDelay(this)));
        }
    }

    public void PickMinigame()
    {
        int ranInt = Random.Range(1, 3);
        SetMinigame((Minigame) ranInt);
        OnMinigamePicked?.Invoke(this);
    }

    public IEnumerator WaitForStationDelay(float time)
    {   

        while(CurrentState == StationState.Troubled)
        {
            troubleTimer += Time.deltaTime;
            yield return null;

            if(troubleTimer >= time) {
                troubleTimer = 0f;
                break;
            }

            stationIcon.color = Color.Lerp(Color.yellow, Color.red, troubleTimer/time);
        }

        if(CurrentState == StationState.Troubled)
        {
            SetStationState(StationState.Delayed);
        }
        
    }

    public float EvaluateReputationLoss(float multiplier) {
        if(CurrentState == StationState.Delayed)
        {
            return multiplier * BasePopulation;
        } else
        {
            return 0f;
        }
    }

    public StationContext GenerateStationContext() {
        var context = new StationContext();
        context.station = this;
        context.stationState = CurrentState;
        context.currentMinigame = CurrentMinigame;
        context.troubleTime = troubleTimer;
        context.delayTime = delayTimer;
        return context;
    }

    public void EmphasizeStation()
    {
        SetRectScale(emphScale);
        canvasGroup.alpha = 1f;
    }

    public void RemoveEmphasisStation()
    {
        SetRectScale(normalScale);
        canvasGroup.alpha = 1f;
    }

    public void DemphasizeStation()
    {
        SetRectScale(normalScale);
        canvasGroup.alpha = demphAlpha;
    }

    public void ShowMarked()
    {
        if(markedIcon.gameObject.activeInHierarchy) return;

        markedIcon.gameObject.SetActive(true);
    }

    public void HideMarked()
    {
        if(!markedIcon.gameObject.activeInHierarchy) return;

        markedIcon.gameObject.SetActive(false);
    }

    void SetRectScale(float amount)
    {
        rectTransform.localScale = Vector3.one * amount;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.MetroCursor.PointerExit(eventData, gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.MetroCursor.PointerEnter(eventData, gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.MetroCursor.ClickStation(this);
    }
}



public class StationContext {
    public Station station;
    public Station.StationState stationState;
    public Station.Minigame currentMinigame;
    public float troubleTime;
    public float delayTime;
}