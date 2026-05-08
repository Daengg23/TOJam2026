using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class RepArrowController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    public GameObject OneArrow;
    public GameObject TwoArrows;
    public GameObject ThreeArrows;
    public GameObject Checkmark;
    /*
     * 0 = checkmark
     * 1 = 1 arrow
     * 2 = 2 arrows
     * 3 = 3 arrows
     */
    public int Appearance = 0;

    public float GlowSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        //it's a sine wave that alternates between 0 and 0.5
        float glow = 0.25f*(Mathf.Sin(Time.time * GlowSpeed * Mathf.PI) + 1); 

        if (Appearance > 3 || Appearance < 0) Debug.LogError("Appearance can only be in [0, 3].");
        if(Appearance == 0)
        {
            OneArrow.SetActive(false);
            TwoArrows.SetActive(false);
            ThreeArrows.SetActive(false);
            Checkmark.SetActive(true);
        }
        if (Appearance == 1)
        {
            OneArrow.SetActive(true);
            TwoArrows.SetActive(false);
            ThreeArrows.SetActive(false);
            Checkmark.SetActive(false);
            GlowSpeed = 1.5f;
            SetColorRecursively(new Color(1f, 0.8f - glow, 0.8f - glow), OneArrow);
        }
        if (Appearance == 2)
        {
            OneArrow.SetActive(false);
            TwoArrows.SetActive(true);
            ThreeArrows.SetActive(false);
            Checkmark.SetActive(false);
            GlowSpeed = 2.5f;
            SetColorRecursively(new Color(1f, 0.8f - glow, 0.8f - glow), TwoArrows);
        }
        if (Appearance == 3)
        {
            OneArrow.SetActive(false);
            TwoArrows.SetActive(false);
            ThreeArrows.SetActive(true);
            Checkmark.SetActive(false);
            GlowSpeed = 4f;
            SetColorRecursively(new Color(1f, 0.8f - glow, 0.8f - glow), ThreeArrows);
        }
    }

    void SetColorRecursively(Color color, GameObject g)
    {
        var ImageRenderers = g.GetComponentsInChildren<Image>();
        foreach (var renderer in ImageRenderers)
        {
            renderer.color = color;
        }
    }
}
