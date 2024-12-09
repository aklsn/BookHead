using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorEventCheck : MonoBehaviour
{
    public GameObject manager;
    public bool MirrorEventOn = false;
    public GameObject doorMirror;

    public bool ScuttleEventOn = false;
    public GameObject doorScuttle;

    public bool Room1EventOn = false;
    public GameObject doorRoom1;

    bool EventOn = false;

    private bool previousDoorMirrorState = false; 
    private bool previousDoorScuttleState = false;
    private bool previousDoorRoom1State = false;
    [System.NonSerialized]
    public bool mannequinEventEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MirrorEventOn == true)
        {
            bool currentDoorMirrorState = doorMirror.GetComponent<doorController>().open;

            if (currentDoorMirrorState != previousDoorMirrorState)
            {
                if (currentDoorMirrorState == true) 
                {
                    EventActive(doorMirror);
                    manager.GetComponent<MirrorEvent>().MirrorEventActive = true;
                }
            }
        }

        if (ScuttleEventOn == true)
        {
            bool currentDoorScuttleState = doorScuttle.GetComponent<doorController>().open;

            if (mannequinEventEnd)
            {
                if (currentDoorScuttleState != previousDoorScuttleState)
                {
                    if (currentDoorScuttleState == true)
                    {
                        EventActive(doorScuttle);
                        manager.GetComponent<ScuttleEvent>().ScuttleEventActive = true;
                    }
                }
            }
        }

        if (Room1EventOn == true)
        {
            bool currentDoorRoom1State = doorRoom1.GetComponent<doorController>().open;

            if (currentDoorRoom1State != previousDoorRoom1State)
            {
                if (currentDoorRoom1State == true)
                {
                    EventActive(doorRoom1);
                    manager.GetComponent<Room1Event>().Room1EventActive = true;
                }
            }
        }
    }

    void EventActive(GameObject e)
    {
        if (e.GetComponent<doorController>().EventOn == false)
        {
            manager.GetComponent<GameManager_R>().event_count--;
            e.GetComponent<doorController>().EventOn = true;
        }
    }
}
