using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundService : MonoBehaviour
{

    // # of AudioSources you put inside the Empty GameObject (in this case, 10)
    [SerializeField] AudioSource source0;
    [SerializeField] AudioSource source1;
    [SerializeField] AudioSource source2;
    [SerializeField] AudioSource source3;
    [SerializeField] AudioSource source4;
    [SerializeField] AudioSource source5;
    [SerializeField] AudioSource source6;
    [SerializeField] AudioSource source7;
    [SerializeField] AudioSource source8;
    [SerializeField] AudioSource source9;


    AudioSource[] AudioSourceArray = new AudioSource[10];

    // Add add one for each sound you will need for this script's usage (in this case, 2)
    [SerializeField] AudioClip clip1;
    [SerializeField] AudioClip clip2;

    //private bool isPlayingFootsteps = false;

    // Update is called once per frame
    void Start()
    {
        AudioSourceArray[0] = source0;
        AudioSourceArray[1] = source1;
        AudioSourceArray[2] = source2;
        AudioSourceArray[3] = source3;
        AudioSourceArray[4] = source4;
        AudioSourceArray[5] = source5;
        AudioSourceArray[6] = source6;
        AudioSourceArray[7] = source7;
        AudioSourceArray[8] = source8;
        AudioSourceArray[9] = source9;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playSound(clip1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            playSound(clip2);
        }

    }
    public void playSound(AudioClip clip)
    {
        for (int i = 0; i < AudioSourceArray.Length; ++i)
        {
            if (AudioSourceArray[i].isPlaying == false)
            {
                AudioSourceArray[i].clip = clip;
                AudioSourceArray[i].Play();
                break;
            }
        }
    }
}
