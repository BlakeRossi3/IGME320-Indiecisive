using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    protected float stageTime; // how long you want the level to last

    [SerializeField]
    protected bool showTime; // whether or not to show the time left in numbers

    [SerializeField]
    protected TextMeshPro timerText; // TextMeshPro element for showing the time left in numbers

    private float stageClock = 0.0f; // increments each frame until it equals stageTime
    private Vector3 timerPosition; // the starting position of the progress bar

    private float initialStartDelay; // how long it takes for the timer to start

    private GameObject movingIcon;

    public float InitialStartDelay { get { return initialStartDelay; } set { initialStartDelay = value; } }

    // Start is called before the first frame update
    void Start()
    {
        // moves the timer to the top right corner of the screen
        timerPosition = new Vector3(-Camera.main.orthographicSize * Camera.main.aspect, -Camera.main.orthographicSize, 0.0f);
        transform.position = timerPosition;

        // find out if theres a way to set it up on the screen and then just multiply or divide by the camera size
        // to work in different aspect ratios

        // set scale of bar and ship
        transform.localScale = new Vector3(1.5f, Camera.main.orthographicSize * 7.5f, 0.0f);
        
        // assign the icon that will move across the screen
        movingIcon = GameObject.Find("ProgressShip");

        // if showTime is true, move the timerText object just uner the progress bar
        if(showTime == true)
        {
            timerText.transform.position = new Vector3(timerPosition.x + 3.25f, timerPosition.y + 1.0f, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(initialStartDelay <= 0.0f)
        {
            // if the time in the level has reached the set time, send the player back to the planet
            if (stageClock >= stageTime)
            {
                SceneManager.LoadScene("Planet");
            }

            // increment the stage clock
            stageClock += Time.deltaTime;

            // moves the progress bar across the screen
            movingIcon.transform.position += new Vector3(0.0f, stageClock * ((Camera.main.orthographicSize * 2) / stageTime), 0.0f);

            // shows the time passed in the level in text
            timerText.text = (stageClock).ToString();
        }
        initialStartDelay -= Time.deltaTime;
    }
}
