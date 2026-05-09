using System;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{

    public Action<string> OnNotificationMessage;

    private Metro metro;

    void Start()
    {
        metro = GameManager.Instance.Metro;

        metro.OnStationStateChanged += CreateNotification;
    }

    public void ResetNotificationManager()
    {
        OnNotificationMessage?.Invoke("");
    }

    void CreateNotification(StationContext stationContext)
    {
        string message = "";
        
        if(stationContext.stationState == Station.StationState.Troubled)
        {   
            message = $"There is a {stationContext.currentMinigame.ToString()} in {stationContext.station.StationName}";
        }
        else if(stationContext.stationState == Station.StationState.Delayed)
        {
            message = $"Delay in {stationContext.station.StationName}";
        }


        OnNotificationMessage?.Invoke(message);

    }
}
