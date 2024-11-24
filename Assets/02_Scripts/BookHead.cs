using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class BookHead : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 10f;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(player.transform.position);
    }

    public void Trigger()
    {
        rb.AddForce(-1 * Vector3.forward * speed* speed , ForceMode.Impulse);
    }
}
