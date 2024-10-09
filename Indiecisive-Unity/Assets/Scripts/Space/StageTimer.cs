using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTimer : MonoBehaviour
{
    [SerializeField]
    protected float stageTime; // how long you want the level to last

    [SerializeField]
    protected bool showTime; // whether or not to show the time left in numbers

    [SerializeField]
    protected TextMeshPro timerText; // TextMeshPro element for showing the time left in numbers

    private float stageClock = 0.0f; // increments each frame until it equals stageTime
    private Vector3 timerPosition; // the starting position of the progress bar

    // Start is called before the first frame update
    void Start()
    {
        stageTime *= 10; // account for editor input

        // moves the timer to the top right corner of the screen
        timerPosition = new Vector3(-Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize, 0.0f);
        transform.position = timerPosition;

        // if showTime is true, move the timerText object just uner the progress bar
        if(showTime == true)
        {
            timerText.transform.position = new Vector3(timerPosition.x + 2.75f, timerPosition.y - 1.0f, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if the time in the level has reached the set time, send the player back to the planet
        if(stageClock >= stageTime)
        {
            SceneManager.LoadScene("Planet");
        }

        // increment the stage clock
        stageClock += Time.fixedDeltaTime;

        // moves the progress bar across the screen
        transform.localScale = new Vector3(stageClock * ((Camera.main.orthographicSize * Camera.main.aspect * 2) / stageTime) * 2, 0.5f, 0.0f);

        // shows the time passed in the level in text
        timerText.text = (stageClock / 10.0f).ToString();
    }
}
