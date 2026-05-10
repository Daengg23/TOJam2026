using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Glow : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Color Max = new Color(1, 1, 1);
    public Color Min = new Color(0, 0, 0);
    public float PeriodSecs = 0.5f;

    public float LifeTime = 0f;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        LifeTime += Time.deltaTime;
        float lerp = 0.5f * (Mathf.Sin(LifeTime * Mathf.PI / PeriodSecs) + 1);
        
        Color newColor = Color.Lerp(Min, Max, lerp);

        SpriteRenderer.color = newColor;
    }
}
