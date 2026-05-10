using UnityEngine;
using System;

public class ReputationManager : MonoBehaviour
{   
    public Action<float> OnReputationLoss;

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
            OnReputationLoss?.Invoke(totalRepLossed);
        }
    }

    public void SetReputation(float reputation)
    {
        this.CurrentReputation = reputation;
    }
}
