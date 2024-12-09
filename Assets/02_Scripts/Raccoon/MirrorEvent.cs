using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorEvent : MonoBehaviour
{
    public Transform player;              
    public Transform mirrorObject;        
    public GameObject monsterObject;       
    public Transform revealLocation;
    public Light _light;

    [System.NonSerialized]
    public bool MirrorEventActive = false; 

    public float activationAngle = 10f;    
    public float disappearAngle = 120f;      
    public float mirrorRevealDelay = 0.1f; 

    private bool isLookingAtMirror = false; 
    private bool isMonsterVisible = false;
    private bool isEventTriggered = false;

    void Start()
    {
        isMonsterVisible = false;
        HideMonster();
    }

    void Update()
    {
        if (MirrorEventActive)
        {
            CheckMirrorLook();    
            CheckPlayerTurnAway();  

            if (isMonsterVisible)
            {
                FacePlayer();
            }
        }
        else
        {
            _light.intensity = 0.0f;
        }
    }

    void CheckMirrorLook()
    {
        Vector3 toMirror = mirrorObject.position - player.position;
        float angleToMirror = Vector3.Angle(player.forward, toMirror);

        if (angleToMirror < activationAngle)
        {
            if (!isLookingAtMirror && !isEventTriggered)
            {
                _light.intensity = 0.6f;
                isLookingAtMirror = true;
                StartCoroutine(TriggerMirrorMonsterEvent());
            }
        }
        else
        {
            isLookingAtMirror = false;
            _light.intensity = 0.0f;
        }
    }

    IEnumerator TriggerMirrorMonsterEvent()
    {
        isEventTriggered = true;
        yield return new WaitForSeconds(mirrorRevealDelay);

        if (isLookingAtMirror) 
        {
            PositionMonsterBehindPlayer();
            ShowMonster();
            isMonsterVisible = true;
        }

        isEventTriggered = false;
    }
    void CheckPlayerTurnAway()
    {
        if (isMonsterVisible)
        {
            Vector3 toMonster = monsterObject.transform.position - player.position;
            float angleToMonster = Vector3.Angle(player.forward, toMonster);

            if (angleToMonster < disappearAngle) 
            {
                HideMonster();
                isMonsterVisible = false;
            }
        }
    }

    void PositionMonsterBehindPlayer()
    {
        Vector3 behindPlayer = revealLocation.position;
        behindPlayer.y = 0.0f; 
        monsterObject.transform.position = behindPlayer;
    }

    void FacePlayer()
    {
        Vector3 directionToPlayer = player.position - monsterObject.transform.position;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        monsterObject.transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
    }

    // 몬스터 숨기기
    void HideMonster()
    {
        monsterObject.transform.position = new Vector3(0.0f, -100.0f, 0.0f);
    }

    // 몬스터 보이기
    void ShowMonster()
    {
        monsterObject.SetActive(true);
    }
}
