using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class BrokenElevatorController : MonoBehaviour
{
    public List<MouseReporter> MouseReporters = new List<MouseReporter>();
    public MinigameTransponder minigameTransponder;

    public MouseReporter PowerBarBgMR;

    public GameObject PowerBarBg;
    public GameObject PowerBarOptimal;
    public GameObject PowerBar;

    private void Awake()
    {
        //minigameTransponder = FindObjectsByType<MinigameTransponder>().Single();
    }

    Vector3 powerBarBgPos;
    float powerBarBgHeight;
    float powerBarBgBottomY;
    void Start()
    {
        powerBarBgPos = PowerBarBg.transform.position;
        powerBarBgHeight = PowerBarBg.transform.localScale.y;
        powerBarBgBottomY = PowerBarBg.transform.position.y - (powerBarBgHeight / 2f);
        MouseReporters.AddRange(FindObjectsByType<MouseReporter>());
    }

    //bar goes up faster at higher %
    void IncreasePBPercentage()
    {
        //first float controls how fast it starts off, second how much it accelerates
        pbPercentage += Time.deltaTime * 0.1f * (1f+pbPercentage*25);
    }

    float minPercentage;
    float maxPercentage;
    [SerializeField]
    float pbPercentage = 0f;
    [SerializeField]
    int stage = -1; //-2 specifically is loss
    void Update()
    {
        if (stage == -1)
        {
            //TODO make the optimal percentage shrink based on difficulty
            GenerateOptimalPercentage(Random.Range(0.3f, 0.7f));
        }

        if (stage == 0) //wait for pb click
        {
            if (PowerBarBgMR.IsBeingClicked)
            {
                pbPercentage = 0f;
                SetBarPercentage(0.02f);
                stage++;
            }
        }
        if(stage == 1) //pb being clicked
        {
            if (PowerBarBgMR.IsBeingClicked)
            {
                IncreasePBPercentage();
                SetBarPercentage(pbPercentage);
            } else
            {
                stage++;
            }
        }
        if (stage == 2) //evaluate result 1
        {
            if(pbPercentage > minPercentage && pbPercentage < maxPercentage)
            {
                GenerateOptimalPercentage(Random.Range(0.3f, 0.7f));
                pbPercentage = 0f;
                SetBarPercentage(0.02f);
                stage++;
            } else
            {
                stage = -2;
            }
        }
        if(stage == 3) //wait for pb click 
        {

        }
        if(stage == 4) //pb being clicked 
        {

        }
        if(stage == 5) //evaluate result 2
        {

        }
        if(stage == 6) //
        {

        }


            if (stage == 1)
        {

        }
    }

    void SetBarPercentage(float percentage)
    {
        if (percentage < 0f || percentage > 1f) throw new ArgumentException("percentage must be in [0 1]");

        float pbBgHeight = powerBarBgHeight;

        Vector3 PBScale = PowerBar.transform.localScale;
        Vector3 PBPos = PowerBar.transform.position;

        Vector3 PBScaleNew = new Vector3(PBScale.x, pbBgHeight * percentage, PBScale.z);

        float PBYNew = powerBarBgBottomY + PBScaleNew.y/2;

        Vector3 PBPosNew = new Vector3(PBPos.x, PBYNew, PBPos.z);

        PowerBar.transform.position = PBPosNew;
        PowerBar.transform.localScale = PBScaleNew;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="percentage"></param>
    /// <returns>float1: min optimal %, float2: max optimal %</returns>
    /// <exception cref="ArgumentException"></exception>
    void GenerateOptimalPercentage(float percentage)
    {
        if (percentage <= 0f || percentage > 1f) throw new ArgumentException("percentage must be in (0 1]");

        float barHeight = powerBarBgHeight * percentage;

        float randomLimit = percentage / 2;

        //picked 0.9f instead of 1f because it's very annoying if it's at the end
        float randomCenterPercentage = Random.Range(randomLimit, 0.9f-randomLimit);

        float randomY = randomCenterPercentage * powerBarBgHeight + powerBarBgBottomY;

        Vector3 PowerBarOptimalNewPos = new Vector3(
            PowerBarOptimal.transform.position.x,
            randomY,
            PowerBarOptimal.transform.position.z
            );

        Vector3 PowerBarOptimalNewScale = new Vector3(
            PowerBarOptimal.transform.localScale.x,
            barHeight,
            PowerBarOptimal.transform.localScale.z
            );

        PowerBarOptimal.transform.position = PowerBarOptimalNewPos;
        PowerBarOptimal.transform.localScale = PowerBarOptimalNewScale;

        float minOptimalPercentage = randomCenterPercentage - percentage / 2;
        float maxOptimalPercentage = randomCenterPercentage + percentage / 2;

        minPercentage = minOptimalPercentage;
        maxPercentage = maxOptimalPercentage;
    }
}
