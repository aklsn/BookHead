using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public bool open = false;
    public float doorOpenAngle = 0f;
    public float doorCloseAngle = -90f;
    public float smoot = 2f;

    void Start()
    {

    }

    public void ChangeDoorState()
    {
        Debug.Log("��");
        open = !open;
    }

    void Update()
    {
        if (open==true)
        {
            Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoot);
        }
        else
        {
            Quaternion targetRotation2 = Quaternion.Euler(0, doorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, Time.deltaTime * smoot);
        }
    }
}