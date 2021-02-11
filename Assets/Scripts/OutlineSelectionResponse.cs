using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class OutlineSelectionResponse : MonoBehaviour, ISelectionResponse
{
    [SerializeField, Tooltip("Overrides the outline width of the gameObject while the object is selected. Make sure the gameObject has an attached Outline script component.")]
    private float selectionOutlineWidth = 10.0f;

    public void onDeselect(Transform selection)
    {
        Outline outline = selection.GetComponent<Outline>();
        if (outline) outline.OutlineWidth = 0;
    }

    public void onSelect(Transform selection)
    {
        Outline outline = selection.GetComponent<Outline>();
        if (outline) outline.OutlineWidth = selectionOutlineWidth;
    }
}
