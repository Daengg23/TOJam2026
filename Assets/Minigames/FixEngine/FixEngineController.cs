using Mono.Cecil;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FixEngineController : MonoBehaviour
{

    public List<MouseReporter> MouseReporters = new List<MouseReporter>();
    public MouseReporter StartMouseReporter;
    public MouseReporter EndMouseReporter;
    public GameObject SegmentPrefab;
    public GameObject DebugPointPrefab;
    public MinigameTransponder minigameTransponder;

    public GameObject BigCheckmark;
    public GameObject BigX;
    public TextMeshPro InstructionText;

    public GameObject Screwdriver;

    public GameObject Boom;

    private void Awake()
    {
        minigameTransponder = FindObjectsByType<MinigameTransponder>().Single();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMaze();

        MouseReporters.AddRange(FindObjectsByType<MouseReporter>());
    }

    public float EndAnimationTimer = 1f;
    int state = 0; //-2 means loss
    void Update()
    {
        bool isTouchingStart = StartMouseReporter.IsMouseOver;
        bool isTouchingEnd = EndMouseReporter.IsMouseOver;
        bool isTouchingAnyMouseReporter = false;
        bool isMouseDownOnAnyMR = false;
        foreach(var mr in MouseReporters) {
            if(mr.IsMouseOver)
            {
                isTouchingAnyMouseReporter = true;
                break;
            }
        }
        foreach (var mr in MouseReporters)
        {
            if (mr.IsBeingClicked)
            {
                isMouseDownOnAnyMR = true;
            }
        }

        var mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        if (state == 2 || state == -2)
        {
            //dont follow mouse
        } else
        {
            //follow mouse
            Screwdriver.transform.position = new Vector3(mousePosition.x, mousePosition.y, -5);
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Screwdriver.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        } else
        {
            Screwdriver.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if (state == 0)
        {
            if (isTouchingStart && StartMouseReporter.IsBeingClicked)
                {
                    //TODO: MOUSE MAZE GO!!
                    InstructionText.SetText("Keep holding and drag the cursor to END without leaving the highlighted path.");
                    //Debug.Log("MOUSE MAZE GO");
                state = 1;
            }
        }
        if(state == 1)
        {
            //mouse was let go but we're not at the end yet
            if(!isTouchingEnd && !isMouseDownOnAnyMR)
            {
                var rb = Screwdriver.AddComponent<Rigidbody2D>();
                rb.gravityScale = 1.3f;
                rb.AddTorque(120);
                state = -2;
            }
            //we left the path
            if (!isTouchingAnyMouseReporter)
            {
                BoomAtMousePos();
                state = -2;
            }
            //we got to the end!!
            if (isTouchingEnd)
            {
                state = 2;
            }

        }

        if(state == 2) //WIN!!
        {
            GameManager.Instance.CurrentGameState = GameManager.GameState.MetroInactive;
            BigCheckmark.SetActive(true);
            InstructionText.SetText("SUCCESS");
            EndAnimationTimer -= Time.deltaTime;
            if (EndAnimationTimer < 0)
            {
                GameManager.Instance.CurrentGameState = GameManager.GameState.MetroActive;
                minigameTransponder.Finish(MinigameStatus.Win);
            }
        }
        if(state == -2)
        {
            GameManager.Instance.CurrentGameState = GameManager.GameState.MetroInactive;
            BigX.SetActive(true);
            InstructionText.SetText("FAILURE");
            EndAnimationTimer -= Time.deltaTime;
            if(EndAnimationTimer < 0)
            {
                GameManager.Instance.CurrentGameState = GameManager.GameState.MetroActive;
                minigameTransponder.Finish(MinigameStatus.Loss);
            }
        }

        void BoomAtMousePos()
        {
            Boom.transform.position = new Vector3(mousePosition.x, mousePosition.y, Boom.transform.position.z);
            Boom.SetActive(true);
        }
    }

    void GenerateMaze()
    {

        const float xDistanceFromMazeEnd = 12f;

        int segments = Random.Range(2, 4);

        float startRandomY = Random.Range(-1f, 1f) * 4f;
        //the start object is shifted up or down the screen a random amount
        StartMouseReporter.transform.position = new Vector3(StartMouseReporter.transform.position.x, startRandomY, StartMouseReporter.transform.position.z);

        //the end object is shifted up or down the screen a random amount
        float endRandomY = Random.Range(-1f, 1f) * 4f;
        EndMouseReporter.transform.position = new Vector3(EndMouseReporter.transform.position.x, endRandomY, EndMouseReporter.transform.position.z);

        Vector2 startPos = StartMouseReporter.transform.position;
        Vector2 endPos = EndMouseReporter.transform.position;

        Vector2 prevPoint = startPos; //used to track the end pos of the last segment
        for (int i = 0; i < segments; i++)
        {

            float width = Random.Range(0.7f, 1.1f);

            float randomXToTheLeft = xDistanceFromMazeEnd / segments * Random.Range(0f, 1.5f);
            Vector2 nextPoint = new Vector2(prevPoint.x - randomXToTheLeft, Random.Range(-1f, 1f) * 4f); //TODO bell curve distribute the Y offset

            if (i == segments - 1) //last segment should go to the end segment
            {
                nextPoint = EndMouseReporter.transform.position;
            }

            //TODO generate the rectangle that connects from previous point to next point
            Vector2 midpoint = prevPoint + (nextPoint - prevPoint) / 2;
            var segmentGO = Instantiate(SegmentPrefab);
            segmentGO.transform.position = midpoint;
            segmentGO.transform.localScale = new Vector3((prevPoint - nextPoint).magnitude, width, segmentGO.transform.localScale.z);

            //point at the next point
            Vector2 diff = nextPoint - prevPoint;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            segmentGO.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 180); //point at the next point

            segmentGO.SetActive(true);

            ////DEBUG
            //var dbg = Instantiate(DebugPointPrefab);
            //dbg.transform.position = nextPoint;

            prevPoint = nextPoint; //put this at the end 
        }
    }
}
