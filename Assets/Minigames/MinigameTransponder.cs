using System;
using UnityEngine;

/// <summary>
/// Each Minigame scene should contain one of these; it's used to call MinigameManager
/// to declare that the minigame is finished
/// </summary>
public class MinigameTransponder : MonoBehaviour
{
    public MinigameManager MinigameManager;

    private void Awake()
    {
        if (MinigameManager == null) Debug.LogError("MinigameTransponder needs a parent MinigameManager reference");
    }

    public void Finish(MinigameStatus status)
    {
        this.MinigameManager.MinigameFinished(this, status);
    }

}
