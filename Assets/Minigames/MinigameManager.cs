using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public async Task StartMinigameForStations(IEnumerable<Station> stations, Station.Minigame type, Action<MinigameStatus> onFinish)
    {
        //TODO set up the scene by adding an a GameObject with a MinigameController

        string sceneName = type switch
        {
            //Station.Minigame.CleanupTrack => "RemoveFromTrack",
            Station.Minigame.FixEngine => "FixEngine",
            Station.Minigame.FixElevator => "BrokenElevator",
            //Station.Minigame.CatchPassenger => "CatchPassenger",
            _ => "UNKNOWN need to implement"
        };

        var transponderGO = new GameObject("MinigameTransponder");
        transponderGO.SetActive(false); //prevent Awake() from triggering too early
        var transponder = transponderGO.AddComponent<MinigameTransponder>();
        transponder.MinigameManager = this;

        //scene should be loaded AFTER we make the GO
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        UnityEngine.SceneManagement.Scene minigame = SceneManager.GetSceneByName(sceneName);

        SceneManager.MoveGameObjectToScene(transponderGO, minigame);
        transponderGO.SetActive(true);

        ActiveMinigames.Add(new MinigameInstance(transponder, stations, onFinish, sceneName)); //TODO

        await loadScene;
        SceneManager.SetActiveScene(minigame);
    }

    public void UnloadAllScenes()
    {
        for (int i = ActiveMinigames.Count-1; i >= 0; i--)
        {
            var s = ActiveMinigames[i];

            SceneManager.UnloadSceneAsync(s.SceneName);
            ActiveMinigames.Remove(s);
        }
    }

    public void MinigameFinished(MinigameTransponder minigameTransponder, MinigameStatus status)
    {
        //find the MinigameInstance corresponding to the MinigameController
        var minigameInstance = ActiveMinigames.Single(x => x.MinigameTransponder == minigameTransponder);

        minigameInstance.OnFinishAction(status);

        SceneManager.UnloadSceneAsync(minigameInstance.SceneName);

        ActiveMinigames.Remove(minigameInstance);
    }

    class MinigameInstance
    {
        public MinigameTransponder MinigameTransponder;
        public IEnumerable<Station> RelevantStations;
        public Action<MinigameStatus> OnFinishAction;
        public string SceneName;

        public MinigameInstance(MinigameTransponder minigameController, IEnumerable<Station> relevantStations, Action<MinigameStatus> onFinishAction, String sceneName)
        {
            MinigameTransponder = minigameController;
            RelevantStations = relevantStations;
            OnFinishAction = onFinishAction;
            SceneName = sceneName;
        }
    }
}
public enum MinigameStatus
{
    Win,
    Loss
}
