using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
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

        //TODO make the optimal percentage shrink based on difficulty
        GenerateOptimalPercentage(Random.Range(0.3f, 0.7f));
    }

    /* state 
     * 0 = waiting to start screw 1
     * 1 = doing screw 1
     * 2 = waiting to start screw 2
     * 3 = doing screw 2
     * 4 = waiting to start screw 3
     * 5 = doing screw 3
     * 6 = done
     * 7 = nothing
     */
    int state = 0;
    void Update()
    {

        if (state == 0)
        {
            if (PowerBarBgMR.IsBeingClicked)
            {
                state = 1;
            }
        }
        if(state == 1)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="percentage"></param>
    /// <returns>float1: min optimal %, float2: max optimal %</returns>
    /// <exception cref="ArgumentException"></exception>
    (float, float) GenerateOptimalPercentage(float percentage)
    {
        if (percentage <= 0f || percentage > 1f) throw new ArgumentException("percentage must be in (0 1]");

        float barHeight = powerBarBgHeight * percentage;

        float randomLimit = percentage / 2;

        float randomCenterPercentage = Random.Range(randomLimit, 1-randomLimit);

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

        return (minOptimalPercentage, maxOptimalPercentage);
    }
}
