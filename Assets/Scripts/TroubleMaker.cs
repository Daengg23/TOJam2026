using System.Collections.Generic;
using UnityEngine;

public class TroubleMaker : MonoBehaviour
{
    [SerializeField] float baseTroubleRate = 2f;

    Dictionary<Station, bool> troubledStations = new();

    private Metro metro;
    
    void Start()
    {
        metro = GameManager.Instance.Metro;
        foreach(var station in metro.Stations)
        {
            troubledStations.Add(station, false);
            station.OnTroubled += (station) => 
            {
                troubledStations[station] = true;
            };

            station.OnCleared += (station) =>
            {
                troubledStations[station] = false;
            };
        }
    }

    float timer;
    public void UpdateTroubleMaker()
    {
        timer += Time.deltaTime;
        if(timer >= baseTroubleRate)
        {
            CreateTrouble();
            timer = 0f;
        }
    }

    void CreateTrouble()
    {
        if(metro.Stations == null) return;

        List<Station> validStations = new();
        foreach(var kvp in troubledStations)
        {
            // only add non troubled stations
            if(!kvp.Value)
            {
                validStations.Add(kvp.Key);
            }
        }
        if(validStations.Count > 0)
        {
            int ranStation = (int) Random.Range(0, validStations.Count);
            Station station = validStations[ranStation];

            if(station.CurrentState == Station.StationState.Cleared)
            {   
                station.StartTrouble(); 
            }
        }
        
    }
}
