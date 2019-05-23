using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Player Owner { get; private set; }

    void Start()
    {
        Destroy(gameObject, 300f);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, .4f);
    }

    public void InitializeBullet(Player owner, Vector3 originalDirection)
    {
        
        Owner = owner;
        transform.forward = originalDirection;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = originalDirection * 30f;

    }
}
