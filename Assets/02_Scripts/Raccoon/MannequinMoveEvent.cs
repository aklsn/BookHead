using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinMoveEvent : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToPlayer = player.position - this.transform.position;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        this.transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
    }
}
