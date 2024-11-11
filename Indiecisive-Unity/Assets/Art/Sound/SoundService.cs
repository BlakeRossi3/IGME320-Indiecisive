using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundService : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip1;

    //private bool isPlayingFootsteps = false;

    // Update is called once per frame
    void Start()
    {
        //source = GetComponent<AudioSource>();
        //source.clip = clip;
    }
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            // Debug.Log("space key was pressed");
            //source.Play();
            isPlayingFootsteps = true;
        }*
        */
        if (Input.GetKeyDown(KeyCode.P))
        {
            source.clip = clip1;
            source.Play();
        }
    }
}
