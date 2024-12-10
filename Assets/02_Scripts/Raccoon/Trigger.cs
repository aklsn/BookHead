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
    public bool doorControlEventOn = false;
    public bool LastEventOn = false;
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
                // 마네킹 움직이는 이벤트
                if (mannequinEventOn == true)
                {
                    manager.GetComponent<MannequinEvent>().mannequinEvent();
                    manager.GetComponent<MannequinEvent>().ControlDoor = ControlDoor;
                    ControlDoor.GetComponent<doorController>().CloseControl = true;
                    on = true;
                }
                // 방에서 피 흐르는 이벤트
                if (bloodflowEventOn == true)
                {
                    BloodEventObject.GetComponent<BloodFlowController>().BloodEvent();
                    on = true;
                }
                // 사진 바뀌는 이벤트
                if (Room1EventOn == true)
                {
                    if (manager.GetComponent<Room1Event>().Room1EventActive == true)
                    {
                        manager.GetComponent<Room1Event>().Room1_Event();
                        ControlDoor.GetComponent<doorController>().CloseControl = false;
                        on = true;
                    }
                }
                // 침대 방 문 닫는 이벤트
                if (doorControlEventOn == true)
                {
                    ControlDoor.GetComponent<doorController>().open = false;
                    ControlDoor.GetComponent<doorController>().CloseControl = true;
                    on = true;
                }
                //마지막 이벤트
                if (LastEventOn == true)
                {
                    if (manager.GetComponent<MirrorEvent>().MirrorEventActive)
                    { 
                        manager.GetComponent<LastEvent>().LastEventActive();
                        ControlDoor.GetComponent<doorController>().CloseControl = false;
                        on = true;
                    }
                }
            }
        }
    }
}
