using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class RepChunkAnimation : MonoBehaviour
{
    public float AnimationDurationSecs = 1.0f;
    public float AnimationFallDistance = 90f;
    public Vector2 InitialPosition;
    Image image;
    RectTransform rectTransform;
    public Color InitialColor = new Color(1f, 0f, 0f, 1f);
    float animationStartTime;

    void Start()
    {
        if (InitialPosition == null) Debug.LogError("Initial position is required for this RepChunkAnimation");
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        animationStartTime = Time.time;
    }

    void Update()
    {
        //e.g. if AnimationDuration is 4s, and this existed for 3s we'd get 75% / 0.75
        float lifespanPercentage = (Time.time - animationStartTime) / AnimationDurationSecs;
        float alpha = 1 - lifespanPercentage;

        rectTransform.anchoredPosition = new Vector2(InitialPosition.x, InitialPosition.y - (lifespanPercentage * AnimationFallDistance));
        image.color = new Color(InitialColor.r, InitialColor.g, InitialColor.b, alpha);

        if (lifespanPercentage > 1f) Destroy(gameObject);
    }
}
