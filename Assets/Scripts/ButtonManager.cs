using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonManager : MonoBehaviour
{
    public string optionName { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        optionName = GetComponentInParent<Canvas>().name;
    }

    public void OptionOnClick()
    {
        //Debug.Log("Picked Question"+);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
