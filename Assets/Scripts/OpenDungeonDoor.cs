using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDungeonDoor : MonoBehaviour
{
    [SerializeField, Range(1, 10)]
    private uint doorSpeed = 1;
    [SerializeField]
    private bool isOpen = false;
    [SerializeField, Tooltip("The range from which the door can be opened.")]
    private float range = 5.0f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("open", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.transform == this.transform && Input.GetMouseButtonDown(0))
            {
                
            }
        }
    }
}
