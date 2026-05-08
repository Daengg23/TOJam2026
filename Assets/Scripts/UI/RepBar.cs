using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class RepBar : MonoBehaviour
{
    public float Percentage;
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

        if (prevPercentage == Percentage) return;
        prevPercentage = Percentage;
        
        //convenience

        float width = this.RectTransform.rect.width;
        float height = this.RectTransform.rect.height;
        float x = this.RectTransform.anchoredPosition.x;
        float y = this.RectTransform.anchoredPosition.y;


        float newWidth = Percentage * InitialWidth; //width of rep bar

        float newX = InitialPos.x - 0.5f*((1f - Percentage)*InitialWidth);

        this.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        this.RectTransform.anchoredPosition = new Vector2(newX, y);
    }
}
