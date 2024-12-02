using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Initializes the initial stats for the player that have to be carried over between planet and space
        PlayerPrefs.SetInt("credits", 0);
        PlayerPrefs.SetInt("charge", 0);

        PlayerPrefs.SetInt("laserLevel", 1);
        PlayerPrefs.SetInt("shieldLevel", 1);
        PlayerPrefs.SetInt("speedLevel", 1);
        PlayerPrefs.SetInt("specialLevel", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
