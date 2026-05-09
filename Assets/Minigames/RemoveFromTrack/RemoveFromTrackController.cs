using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFromTrackController : MonoBehaviour
{

    public List<MouseReporter> MouseReporters = new List<MouseReporter>();
    public MouseReporter StartObject;
    public MouseReporter EndObject;
    public GameObject SegmentPrefab;
    public GameObject DebugPointPrefab;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {

        const float xDistanceFromMazeEnd = 12f;

        int segments = 3;

        float perlinX = 0f;

        float startRandomY = Random.Range(-1f, 1f) * 4f;
        //the start object is shifted up or down the screen a random amount
        StartObject.transform.position = new Vector3(StartObject.transform.position.x, startRandomY, StartObject.transform.position.z);

        //the end object is shifted up or down the screen a random amount
        float endRandomY = Random.Range(-1f, 1f) * 4f;
        EndObject.transform.position = new Vector3(EndObject.transform.position.x, endRandomY, EndObject.transform.position.z);

        Vector2 startPos = StartObject.transform.position;
        Vector2 endPos = EndObject.transform.position;

        Vector2 prevPoint = startPos; //used to track the end pos of the last segment
        for (int i = 0; i < segments; i++)
        {

            float width = 0.5f; //TODO randomize this

            float randomXToTheLeft = xDistanceFromMazeEnd / segments * Random.Range(0f, 1.5f);
            Vector2 nextPoint = new Vector2(prevPoint.x - randomXToTheLeft, Random.Range(-1f, 1f) * 4f); //TODO bell curve distribute the Y offset

            if (i == segments - 1) //last segment should go to the end segment
            {
                nextPoint = EndObject.transform.position;
            }

            //TODO generate the rectangle that connects from previous point to next point
            Vector2 midpoint = prevPoint + (nextPoint - prevPoint)/2;
            var segmentGO = Instantiate(SegmentPrefab);
            segmentGO.transform.position = midpoint;
            segmentGO.transform.localScale = new Vector3((prevPoint - nextPoint).magnitude, segmentGO.transform.localScale.y, segmentGO.transform.localScale.z);

            //point at the next point
            Vector2 diff = nextPoint - prevPoint;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            segmentGO.transform.rotation = Quaternion.Euler(0f, 0f, rot_z-180); //point at the next point

            segmentGO.SetActive(true);

            ////DEBUG
            //var dbg = Instantiate(DebugPointPrefab);
            //dbg.transform.position = nextPoint;

            prevPoint = nextPoint; //put this at the end 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
