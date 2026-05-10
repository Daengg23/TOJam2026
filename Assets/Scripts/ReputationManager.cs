using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections;

public class ReputationManager : MonoBehaviour
{
    public RepUIController RepUIController;

    public Action OnReputationChanged;

    private Metro metro;
    [SerializeField] private float maxReputation;

    public float CurrentReputation {
        get => currentReputation; 
        private set
        {
            RepUIController.RepBar.SetPercentage(currentReputation / maxReputation);
            currentReputation = value;
        }
    }
    private float currentReputation;


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

    public void SetReputationWithAnimation(float reputation)
    {
        float curRep = this.CurrentReputation;
        float endRep = curRep - reputation;

        RepUIController.AnimateRemoveRepChunk(curRep, endRep);

        CurrentReputation = reputation;
    }

    //public void SetReputationWithAnimation(float reputation)
    //{
    //    float curRep = this.CurrentReputation;
    //    float endRep = curRep - reputation;

    //    StartCoroutine(AnimateRemoveRepChunk(curRep, endRep));
    //}

    //private IEnumerator AnimateRemoveRepChunk(float curRep, float endRep)
    //{
    //    yield return new WaitForSeconds(1f);
    //    RepUIController.AnimateRemoveRepChunk(curRep, endRep);
    //}

    //void animateRemoveRepChunk(float curRep, float endRep)
    //{
    //    RepUIController.AnimateRemoveRepChunk(curRep, endRep);
    //}
}
