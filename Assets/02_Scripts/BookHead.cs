using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BookHead : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Trigger()
    {
        Debug.Log("¹ßµ¿");
        rb.AddForce(-1 * Vector3.forward * speed* speed , ForceMode.Impulse);
    }
}
