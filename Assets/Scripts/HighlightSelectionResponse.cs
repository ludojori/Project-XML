using UnityEngine;

[DisallowMultipleComponent]

internal class HighlightSelectionResponse : MonoBehaviour, ISelectionResponse
{
    private Material _selectionMaterial;
    // keep track of the original material
    private Material _originalMaterial;

    public void onSelect(Transform selection)
    {
        Renderer selectionRenderer = selection.GetComponent<Renderer>();
        if (selectionRenderer)
        {
            _originalMaterial = selectionRenderer.materials[0];
            _selectionMaterial = selectionRenderer.materials[1];
            selectionRenderer.material = _selectionMaterial;
        }
    }

    public void onDeselect(Transform selection)
    {
        Renderer selectionRenderer = selection.GetComponent<Renderer>();
        if (selectionRenderer)
        {
            selectionRenderer.material = _originalMaterial;
        }
    }
}