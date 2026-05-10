using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Breathe : MonoBehaviour
{
    public Vector3 OriginalScale;
    public Vector3 Max = new Vector3(0.2f, 0.2f, 0.2f);
    public Vector3 Min = new Vector3(0, 0, 0);
    public float PeriodSecs = 0.4f;

    public float LifeTime = 0f;

    public bool Stop = false;

    void Start()
    {
        OriginalScale = transform.localScale;
    }

    void Update()
    {

        if(Stop)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            return;
        }

        LifeTime += Time.deltaTime;
        float lerp = 0.5f * (Mathf.Sin(LifeTime * Mathf.PI / PeriodSecs) + 1);

        Vector3 scaleToAdd = Vector3.Lerp(Min, Max, lerp);

        transform.localScale = OriginalScale + scaleToAdd;
    }
}
