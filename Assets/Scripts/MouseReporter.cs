using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MouseReporter : MonoBehaviour
{
    public Action<MouseReporter> OnMouseEnterEvent;
    public Action<MouseReporter> OnMouseExitEvent;
    public Action<MouseReporter> OnMouseDownEvent;
    public Action<MouseReporter> OnMouseUpEvent;
    public bool IsMouseOver = false;
    public bool IsBeingClicked = false;

    private void OnMouseEnter()
    {
        IsMouseOver = true;
        this.OnMouseEnterEvent?.Invoke(this);
    }
    private void OnMouseExit()
    {
        IsMouseOver = false;
        this.OnMouseExitEvent?.Invoke(this);
    }
    private void OnMouseDown()
    {
        IsBeingClicked = true;
        this.OnMouseDownEvent?.Invoke(this);
    }
    private void OnMouseUp()
    {
        IsBeingClicked = false;
        this.OnMouseUpEvent?.Invoke(this);
    }

}
