using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class GameManager_R : MonoBehaviour
{
    public Transform player;

    public GameObject bed;

    public int event_count;

    void Start()
    {

    }

    void Update()
    {
        if (event_count <= 0)
        {
            bed.GetComponent<BedScript_Raccoon>().IsEventOn = true;
        }


    }
}