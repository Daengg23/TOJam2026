using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class MetroCursor : MonoBehaviour
{
    private Station.Minigame selectedMinigame;
    private List<Station> selectedStations = new();
    private Metro metro;

    bool holdActive = false;

    void Start()
    {
        metro = GameManager.Instance.Metro; 
    }

    public void ResetStationChain()
    {
        if(metro == null) return;
        DemphasizeAllSimilarMinigames();
        foreach(var station in metro.Stations)
        {
            station.HideMarked();
        }
        selectedStations.Clear();
    }

    void EmphasizeAllSimilarMinigames()
    {
        if(metro == null) return;

        foreach(var station in metro.Stations)
        {
            if(station.CurrentMinigame == selectedStations[0].CurrentMinigame)
            {
                station.EmphasizeStation();
            }
        }
    }

    void DemphasizeAllSimilarMinigames()
    {
        if(metro == null) return;

        foreach(var station in metro.Stations)
        {
            station.DemphasizeStation();
        }
    }

    void Update() {
        // left click hold
        if(Input.GetMouseButtonDown(0))
        {
            holdActive = true;
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
            EmphasizeAllSimilarMinigames();
        }
        Debug.Log("STATION CHAIN START");
    }

    public void MarkStation(Station station)
    {
        selectedStations.Add(station);
        station.ShowMarked();
    }

    public void StopStationChain()
    {
        if(selectedStations.Count == 0) return;

        // Start the minigame here

        // Code after the minigame
        foreach(var station in selectedStations)
        {
            station.ResetStation();
        }

        ResetStationChain();
        Debug.Log("STATION CHAIN STOP");
    }

    public void PointerEnter(PointerEventData eventData, GameObject go)
    {
        Debug.Log(eventData.pointerEnter);

        Station station;
        if(station = go.GetComponent<Station>())
        {
            if(selectedStations.Count > 0 && station.CurrentMinigame == selectedStations[0].CurrentMinigame && !selectedStations.Contains(station))
            {
                MarkStation(station);
            }
        }
    }

    public void PointerClick(PointerEventData eventData, GameObject go)
    {
        Station station;
        if(station = go.GetComponent<Station>())
        {
            if(selectedStations.Count == 0)
            {
                StartStationChain(station);
            }
        }
    }

    public void PointerExit(PointerEventData eventData, GameObject go)
    {
        
    }
}