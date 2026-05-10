using UnityEngine;
using System;

public class ReputationManager : MonoBehaviour
{   
    public Action OnReputationChanged;

    private Metro metro;
    [SerializeField] private float maxReputation;

    public float CurrentReputation {get; private set;}
    public float BaseMultiplier = 1f;

    void Start() {
        metro = GameManager.Instance.Metro;

        ResetReputation();
    }

    public void ResetReputation()
    {
        CurrentReputation = maxReputation;
        OnReputationChanged?.Invoke();
    }

    public void UpdateReputationLoss() {
        bool repLossed = false;
        float totalRepLossed = 0f;

        foreach(var station in metro.Stations)
        {
            float repLoss = station.EvaluateReputationLoss(BaseMultiplier);
            CurrentReputation -= repLoss * Time.deltaTime;
            repLossed = true;
            totalRepLossed += repLoss;
        }

        if(repLossed) {
            OnReputationChanged?.Invoke();
        }

        if(CurrentReputation <= 0)
        {
            GameManager.Instance.LoseGame();
        }
    }

    public void SetReputation(float reputation)
    {
        this.CurrentReputation = reputation;
    }
}
