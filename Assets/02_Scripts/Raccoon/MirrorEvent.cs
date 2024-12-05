using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MirrorEvent : MonoBehaviour
{
    public Transform player;

    public Transform mirrorObject;
    public Transform monsterObject;

    public Transform revealLocation;

    // MirrorEvent
    public bool MirrorEventActive = false;

    private bool isLookingAtMirror = false;
    private bool eventTriggered = false;
    private bool isMonsterVisible = false;

    public float activationAngle = 20f;
    public float mirror_revealDelay = 0.1f;
    public float disappearAngle = 45f;


    // Start is called before the first frame update
    void Start()
    {
        monsterObject.position = new Vector3(0, -100, 0);
        isMonsterVisible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (MirrorEventActive == true)
        {
            CheckMirrorLook();
            CheckPlayerTurnAway();

            if (isMonsterVisible)
            {
                Vector3 directionToPlayer = player.position - monsterObject.position;
                directionToPlayer.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                monsterObject.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            }
        }
    }

    void CheckMirrorLook()
    {
    Vector3 toMirror = mirrorObject.position - player.position;
    float angleToMirror = Vector3.Angle(player.forward, toMirror);

        if (angleToMirror < activationAngle)
        {
            if (!isLookingAtMirror && !eventTriggered)
            {
                isLookingAtMirror = true;
                StartCoroutine(TriggerMirrorMonsterEvent());
            }
        }
        else
        {
            isLookingAtMirror = false;
        }
    }

    IEnumerator TriggerMirrorMonsterEvent()
    {
        yield return new WaitForSeconds(mirror_revealDelay);

        if (isLookingAtMirror)
        {
            PositionMonsterBehindPlayer();
            isMonsterVisible = true;
            eventTriggered = true;
        }
    }

    void CheckPlayerTurnAway()
    {
        if (isMonsterVisible)
        {
            Vector3 toMonster = monsterObject.position - player.position;
            float angleToMonster = Vector3.Angle(player.forward, toMonster);

            if (angleToMonster < disappearAngle)
            {
                monsterObject.position = new Vector3(0, -100, 0);
                isMonsterVisible = false;
            }
        }
    }

    void PositionMonsterBehindPlayer()
    {
        Vector3 behindPlayer = revealLocation.position;
        behindPlayer.y = 0.1f;

        monsterObject.position = behindPlayer;
    }
}