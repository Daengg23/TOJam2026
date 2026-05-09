using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.EventSystems;

public class Station : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{   
    public Action<Station> OnCleared;
    public Action<Station> OnTroubled;
    public Action<Station> OnDelayed;
    public Action<Station> OnMinigamePicked;

    public Action<Station> OnStationStateChanged;

    public enum Minigame
    {
        None,
        CleanupTrack,
        FixEngine,
        CatchPassenger
    }

    public enum StationState {
        Cleared,
        Troubled,
        Delayed
    }

    public Minigame CurrentMinigame;

    [SerializeField] private Image stationIcon;
    [SerializeField] private Image markedIcon;
    [SerializeField] private Slider troubleSlider;

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
        CurrentMinigame = Minigame.None;
        SetStationState(StationState.Cleared);
        troubleTimer = 0f;
        delayTimer = 0f;
        troubleSlider.value = 1f;
        troubleSlider.gameObject.SetActive(false); 
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
        CurrentMinigame = (Minigame) ranInt;
        OnMinigamePicked?.Invoke(this);
    }

    public IEnumerator WaitForStationDelay(float time)
    {   
        troubleSlider.gameObject.SetActive(true);
        while(CurrentState == StationState.Troubled)
        {
            troubleTimer += Time.deltaTime;
            yield return null;

            if(troubleTimer >= time) {
                troubleTimer = 0f;
                break;
            }

            troubleSlider.value = 1 - troubleTimer/time;
        }
        troubleSlider.gameObject.SetActive(false);
        troubleSlider.value = 0f;

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

    void OnDrawGizmos() {
        if(CurrentMinigame != Minigame.None)
        {
            Handles.color = Color.cyan;
            Handles.Label(transform.position + Vector3.up, CurrentMinigame.ToString());

        }
    }

    public void EmphasizeStation()
    {
        SetRectScale(1.5f);
    }

    public void DemphasizeStation()
    {
        SetRectScale(1f);
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
        GameManager.Instance.MetroCursor.PointerEnter(eventData, gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.MetroCursor.PointerExit(eventData, gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.MetroCursor.PointerClick(eventData, gameObject);
    }
}



public class StationContext {
    public Station station;
    public Station.StationState stationState;
    public Station.Minigame currentMinigame;
    public float troubleTime;
    public float delayTime;
}