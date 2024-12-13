using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrigger : MonoBehaviour
{
    public doorController doorController;
    public Transform PlayerTransform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(!doorController.isOpen)
            PlayerTransform.position = new Vector3(5, 1.1f, 2);
        }
    }
}
