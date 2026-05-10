using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class MetroCursor : MonoBehaviour
{
    private List<Station> selectedStations = new();
    private Metro metro;

    bool holdActive = false;

    bool minigameActive = false;

    void Start()
    {
        metro = GameManager.Instance.Metro; 
    }

    public void ResetStationChain()
    {
        minigameActive = false;
        if(metro == null) return;
        RemoveEmphasisAllStations();
        foreach(var station in metro.Stations)
        {
            station.HideMarked();
        }
        selectedStations.Clear();
    }

    void EmphasizeAllSimilarMinigames()
    {
        if(metro == null || selectedStations.Count == 0) return;

        foreach(var station in metro.Stations)
        {
            if(station.CurrentMinigame == selectedStations[0].CurrentMinigame)
            {
                station.EmphasizeStation();
            } else
            {
                station.DemphasizeStation();
            }
        }
    }

    void RemoveEmphasisAllStations()
    {
        if(metro == null) return;

        foreach(var station in metro.Stations)
        {
            station.RemoveEmphasisStation();
        }
    }

    void Update() {
        // left click hold
        if(Input.GetMouseButtonDown(0))
        {
            holdActive = true;
            EmphasizeAllSimilarMinigames();
        }
        //left click release
        if(Input.GetMouseButtonUp(0))
        {
            holdActive = false;
            StopStationChain();
        }
    }

    public void StartStationChain(Station firstStation)
    {
        if(selectedStations.Count == 0)
        {
            MarkStation(firstStation);
        }
    }

    public void MarkStation(Station station)
    {
        selectedStations.Add(station);
        station.ShowMarked();
    }

    public void StopStationChain()
    {

        if (minigameActive) return;
        if(selectedStations.Count == 0) return;

        // Start the minigame here
        _ = MinigameManager.Instance.StartMinigameForStations( //no need to await
            selectedStations,
            selectedStations[0].CurrentMinigame, //TODO replace with the selected one
            onFinish
            );

        minigameActive = true;

        // Code after the minigame

        void onFinish(MinigameStatus status) {

            minigameActive = false;

            if (status == MinigameStatus.Win)
            {
                Debug.Log("MetroCursor: WON THE MINIGAME");
                foreach (var station in selectedStations)
                {
                    station.ResetStation();
                }
                ResetStationChain();
            }
            else
            {
                Debug.Log("MetroCursor: LOST THE MINIGAME");
                ReputationManager r = GameManager.Instance.Reputation;
                float reputationLoss = 0f;
                foreach(var station in selectedStations)
                {
                    reputationLoss += 3 * station.BasePopulation;
                }
                r.SetReputation(r.CurrentReputation - reputationLoss);
                foreach (var station in selectedStations)
                {
                    station.ResetStation();
                }
                ResetStationChain();
            }
        }
        
    }

    public void PointerEnter(PointerEventData eventData, GameObject go)
    {
        if(!holdActive) return;

        Station station;
        if(station = go.GetComponent<Station>())
        {
            if(selectedStations.Count > 0 && station.CurrentMinigame == selectedStations[0].CurrentMinigame && !selectedStations.Contains(station))
            {
                MarkStation(station);
            }
        }
    }

    public void ClickStation(Station station)
    {
        if(selectedStations.Count == 0 && (station.CurrentState == Station.StationState.Troubled || station.CurrentState == Station.StationState.Delayed))
        {
            StartStationChain(station);
        }
    }

    public void PointerExit(PointerEventData eventData, GameObject go)
    {
        
    }
}