using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Metro : MonoBehaviour
{
    public Action<StationContext> OnStationStateChanged;
    public List<Station> Stations = new();

    void Awake()
    {
        Stations = GetComponentsInChildren<Station>().ToList();

        foreach(var station in Stations)
        {
            station.OnStationStateChanged += (station) =>
            {
                OnStationStateChanged?.Invoke(station.GenerateStationContext());
            };
        }
    }

    public void ResetAllStations()
    {
        foreach(var station in Stations)
        {
            station.ResetStation();
        }
    }

}
