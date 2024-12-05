using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject manager;
    private bool on = false;
    //public GameObject Player;
    //public GameObject Monster;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (on == false)
        {
            if (other.gameObject.tag == "Player")
            {
                on = true;
                manager.GetComponent<GameManager_R>().event_count--;
            }
        }
    }
}
