using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// How the Minigame system works: each minigame scene contains a MinigameTransponder
/// script which is used to hail back here when the minigame is finished
/// </summary>
public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    List<MinigameInstance> ActiveMinigames;

    private void Awake()
    {
        Instance = this;
    }

    public void StartMinigameForStations(IEnumerable<Station> stations, Station.Minigame type)
    {
        //TODO set up the scene by adding an a GameObject with a MinigameController

        string sceneName = type switch
        {
            Station.Minigame.CleanupTrack => "RemoveFromTrack",
            Station.Minigame.FixEngine => "FixEngine",
            Station.Minigame.CatchPassenger => "CatchPassenger",
            _ => "UNKNOWN need to implement"
        };

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        UnityEngine.SceneManagement.Scene minigame = SceneManager.GetSceneByName(sceneName);

        var transponderGO = new GameObject("MinigameTransponder");
        var transponder = transponderGO.AddComponent<MinigameTransponder>();
        transponder.MinigameManager = this;

        SceneManager.MoveGameObjectToScene(transponderGO, minigame);

        //ActiveMinigames.Add(new MinigameInstance()); //TODO
    }

    public void MinigameFinished(MinigameTransponder minigameController, MinigameStatus status)
    {
        //find the MinigameInstance corresponding to the MinigameController
        var minigameInstance = ActiveMinigames.Single(x => x.MinigameController == minigameController);

        //TODO act on relevant stations acording to status
    }

    class MinigameInstance
    {
        public MinigameTransponder MinigameController;
        public IEnumerable<Station> RelevantStations;

        public MinigameInstance(MinigameTransponder minigameController, IEnumerable<Station> relevantStations)
        {
            MinigameController = minigameController;
            RelevantStations = relevantStations;
        }
    }
}
public enum MinigameStatus
{
    Win,
    Loss
}
