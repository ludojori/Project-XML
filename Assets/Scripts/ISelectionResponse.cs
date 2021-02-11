using UnityEngine;

internal interface ISelectionResponse
{
    void onDeselect(Transform selection);
    void onSelect(Transform selection);
}
