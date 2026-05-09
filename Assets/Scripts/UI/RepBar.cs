using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class RepBar : MonoBehaviour
{
    [SerializeField] //we should set this using SetPercentage
    private float percentage;
    public float Percentage { get => percentage; }
    public Vector3 InitialPos;
    public RectTransform RectTransform;
    public float InitialWidth;

    void Awake()
    {
        this.RectTransform = GetComponent<RectTransform>();
        InitialPos = this.RectTransform.anchoredPosition;
        InitialWidth = this.RectTransform.rect.width;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private float prevPercentage;
    // Update is called once per frame
    void Update()
    {

        if (prevPercentage == percentage) return;
        prevPercentage = percentage;
        
        //convenience

        float width = this.RectTransform.rect.width;
        float height = this.RectTransform.rect.height;
        float x = this.RectTransform.anchoredPosition.x;
        float y = this.RectTransform.anchoredPosition.y;


        float newWidth = percentage * InitialWidth; //width of rep bar

        float newX = InitialPos.x - 0.5f*((1f - percentage)*InitialWidth);

        this.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        this.RectTransform.anchoredPosition = new Vector2(newX, y);
    }

    public void SetPercentage(float percentage)
    {
        this.percentage = percentage;
    }
}
