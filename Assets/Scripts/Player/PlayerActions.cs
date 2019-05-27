using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private float reach = 4;

    private PlayerMovement playerMovement;
    private PhotonView PV;


    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        PV = GetComponentInParent<PhotonView>();
        
    }

    void Update()
    {
        if (!PV.IsMine)
            return;


        if (Input.GetButton("Interact"))
        {
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, reach);

            
            if (hit.collider != null && hit.collider.tag == "Interactable")
            {
                IInteractable interactable = hit.transform.GetComponent<Door>();

                interactable.Interact();
                Debug.Log("Interacted with " + hit.collider.name);
            }
        }
    }


}
