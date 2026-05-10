using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assemblies;

public class TroubleMaker : MonoBehaviour
{
    [SerializeField] private Vector2 baseTroubleRange = new Vector2(3, 5);

    Dictionary<Station, bool> troubledStations = new();

    private Metro metro;
    private float currentTroubleTime;
    
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

        if(timer >= currentTroubleTime)
        {
            CreateTrouble();
            timer = 0f;
            Vector2 rangeToUse = GetTroubleRange(); 
            currentTroubleTime = Random.Range(rangeToUse.x, rangeToUse.y);
        }
    }

    Vector2 GetTroubleRange()
    {
        var rangeToUse = baseTroubleRange;
        var difficulty = GameManager.Instance.CurrentDifficulty;

        if(difficulty == 2)
        {
            rangeToUse = rangeToUse * 0.8f;
        } else if (difficulty == 3)
        {
            rangeToUse = rangeToUse * 0.7f;
        } else if (difficulty == 4)
        {
            rangeToUse = rangeToUse * 0.6f;
        } else if (difficulty == 5)
        {
            rangeToUse = rangeToUse * 0.5f;
        } else if (difficulty == 6)
        {
            rangeToUse = rangeToUse * 0.4f;
        } else if (difficulty == 7)
        {
            rangeToUse = rangeToUse * 0.3f;
        } else if (difficulty >= 8)
        {
            rangeToUse = rangeToUse * 0.25f;
        }
        
        return rangeToUse;
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
