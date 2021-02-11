using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondMulticastScript : MonoBehaviour, ISelectionResponse
{
    delegate void MultiDelegate();
    MultiDelegate multiDelegate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onDeselect(Transform selection)
    {
        
    }

    public void onSelect(Transform selection)
    {
        
    }
}
