using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{

    private PlayerMovement playerMovement;
    private PhotonView PV;


    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        PV = GetComponentInParent<PhotonView>();
        
    }



}
