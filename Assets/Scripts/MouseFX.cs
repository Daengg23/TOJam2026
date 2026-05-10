using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class MouseFX : MonoBehaviour
{
    public AudioClip OnMouseDownAudio;
    public AudioClip OnMouseOverAudio;

    public bool ExpandOnHover = true;
    public Vector3 ExtraScaleOnMouseOver = new Vector3(0.2f, 0.2f, 0.2f);

    public AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }


    bool hovered = false;
    private void OnMouseOver()
    {
        
    }

    private void OnMouseDown()
    {
        if (OnMouseDownAudio)
        {
            this.AudioSource.PlayOneShot(OnMouseDownAudio);
        }
    }

    private void OnMouseEnter()
    {
        if (OnMouseOverAudio)
        {
            this.AudioSource.PlayOneShot(OnMouseOverAudio);
        }

        hovered = true;
        if (ExpandOnHover)
        {
            transform.localScale += ExtraScaleOnMouseOver;
        }
    }

    private void OnMouseExit()
    {
        hovered = false;
        if (ExpandOnHover)
        {
            transform.localScale -= ExtraScaleOnMouseOver;
        }
    }

    private void Update()
    {
        
    }
}
