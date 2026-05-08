using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class RepBar : MonoBehaviour
{
    public float MaxReputation = 1500;
    public float Reputation;
    public Vector3 InitialPos;
    public RectTransform RectTransform;
    public float InitialWidth;

    void Awake()
    {
        Reputation = MaxReputation;
        this.RectTransform = GetComponent<RectTransform>();
        InitialPos = this.RectTransform.anchoredPosition;
        InitialWidth = this.RectTransform.rect.width;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private float prevReputation;
    // Update is called once per frame
    void Update()
    {

        if (prevReputation == Reputation) return;
        prevReputation = Reputation;
        
        //convenience

        float width = this.RectTransform.rect.width;
        float height = this.RectTransform.rect.height;
        float x = this.RectTransform.anchoredPosition.x;
        float y = this.RectTransform.anchoredPosition.y;


        float newWidth = Reputation / MaxReputation * InitialWidth; //width of rep bar

        float newX = InitialPos.x + 0.5f*((1f - Reputation/MaxReputation)*InitialWidth);

        this.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        this.RectTransform.anchoredPosition = new Vector2(newX, y);
    }
}
