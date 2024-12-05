using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BedScript_Raccoon : MonoBehaviour
{
    public bool IsEventOn = false;
    public bool IsClick = false;
    public String NextScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( IsEventOn == true && IsClick == true )
        {
           SceneManager.LoadScene(NextScene);
        }
    }
}
