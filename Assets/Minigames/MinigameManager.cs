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

    List<MinigameInstance> ActiveMinigames = new List<MinigameInstance>();

    private void Awake()
    {
        Instance = this;
    }

    //TODO do we really even need the relevant stations
    public void StartMinigameForStations(IEnumerable<Station> stations, Station.Minigame type, Action<MinigameStatus> onFinish)
    {
        //TODO set up the scene by adding an a GameObject with a MinigameController

        string sceneName = type switch
        {
            Station.Minigame.CleanupTrack => "RemoveFromTrack",
            Station.Minigame.FixEngine => "FixEngine",
            Station.Minigame.CatchPassenger => "CatchPassenger",
            _ => "UNKNOWN need to implement"
        };

        var transponderGO = new GameObject("MinigameTransponder");
        transponderGO.SetActive(false); //prevent Awake() from triggering too early
        var transponder = transponderGO.AddComponent<MinigameTransponder>();
        transponder.MinigameManager = this;

        //scene should be loaded AFTER we make the GO
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        UnityEngine.SceneManagement.Scene minigame = SceneManager.GetSceneByName(sceneName);

        SceneManager.MoveGameObjectToScene(transponderGO, minigame);
        transponderGO.SetActive(true);

        ActiveMinigames.Add(new MinigameInstance(transponder, stations, onFinish)); //TODO
    }

    public void MinigameFinished(MinigameTransponder minigameTransponder, MinigameStatus status)
    {
        //find the MinigameInstance corresponding to the MinigameController
        var minigameInstance = ActiveMinigames.Single(x => x.MinigameTransponder == minigameTransponder);

        minigameInstance.OnFinishAction(status);

        ActiveMinigames.Remove(minigameInstance);
    }

    class MinigameInstance
    {
        public MinigameTransponder MinigameTransponder;
        public IEnumerable<Station> RelevantStations;
        public Action<MinigameStatus> OnFinishAction;

        public MinigameInstance(MinigameTransponder minigameController, IEnumerable<Station> relevantStations, Action<MinigameStatus> onFinishAction)
        {
            MinigameTransponder = minigameController;
            RelevantStations = relevantStations;
            OnFinishAction = onFinishAction;
        }
    }
}
public enum MinigameStatus
{
    Win,
    Loss
}
