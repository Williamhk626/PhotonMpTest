using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public bool IsOpen { get; set; } = false;

    public void Interact()
    {
        if (IsOpen)
            transform.eulerAngles += new Vector3(0, 90, 0);
        else
            transform.eulerAngles += new Vector3(0, -90, 0);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
