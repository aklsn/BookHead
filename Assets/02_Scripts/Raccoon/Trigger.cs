using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject manager;
    private bool on = false;
    public GameObject Player;
    public bool mannequinEventOn = false;
    public bool bloodflowEventOn = false;
    public bool Room1EventOn = false;
    public GameObject BloodEventObject;
    public GameObject ControlDoor;


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
            if (other.gameObject.tag == "Player" || other == Player)
            {
                if (mannequinEventOn == true )
                {
                    manager.GetComponent<MannequinEvent>().mannequinEvent();
                    manager.GetComponent<MannequinEvent>().ControlDoor = ControlDoor;
                    ControlDoor.GetComponent<doorController>().CloseControl = true;
                    on = true;
                }
                if (bloodflowEventOn == true)
                {
                    BloodEventObject.GetComponent<BloodFlowController>().BloodEvent();
                    on = true;
                }
                if (Room1EventOn == true)
                {
                    if (manager.GetComponent<Room1Event>().Room1EventActive == true)
                    {
                        manager.GetComponent<Room1Event>().Room1_Event();
                        ControlDoor.GetComponent<doorController>().CloseControl = false;
                        on = true;
                    }
                }
                manager.GetComponent<GameManager_R>().event_count--;
            }
        }
    }
}
