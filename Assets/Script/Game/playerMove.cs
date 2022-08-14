using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class playerMove : MonoBehaviour
{
    PhotonView view;
    Rigidbody rb;
    public float Speed = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (view.IsMine)
        {
            rb.velocity = new Vector3(Input.GetAxis("Horizontal") * Speed, 0 , Input.GetAxis("Vertical") * Speed);
        }
    }
}
