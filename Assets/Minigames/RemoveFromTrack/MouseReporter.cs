using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MouseReporter : MonoBehaviour
{
    public Action<MouseReporter> OnMouseEnterEvent;
    public Action<MouseReporter> OnMouseExitEvent;
    public Action<MouseReporter> OnMouseDownEvent;
    public Action<MouseReporter> OnMouseUpEvent;

    private void OnMouseEnter()
    {
        this.OnMouseEnterEvent?.Invoke(this);
    }
    private void OnMouseExit()
    {
        this.OnMouseExitEvent?.Invoke(this);
    }
    private void OnMouseDown()
    {
        this.OnMouseDownEvent?.Invoke(this);
    }
    private void OnMouseUp()
    {
        this.OnMouseUpEvent?.Invoke(this);
    }

}
