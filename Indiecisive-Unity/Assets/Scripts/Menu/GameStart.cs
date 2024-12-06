using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{

    public GameObject upgrade;
    public GameObject upgrade2;
    public GameObject StartButton;
    public GameObject QuitButton;
    private int selection = 1;
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
        Transform startButton = StartButton.GetComponent<Transform>();
        Transform quitButton = QuitButton.GetComponent<Transform>();
        if (Input.GetKeyDown(KeyCode.DownArrow) && selection == 1)
        {
            selection++;
            
            startButton.localScale = new Vector3(startButton.localScale.x - 0.2f, startButton.localScale.y - 0.2f, 1);
        }
    
        else if (Input.GetKeyDown(KeyCode.UpArrow) && selection == 2)
        {
            selection--;

            quitButton.localScale = new Vector3(quitButton.localScale.x - 0.2f, quitButton.localScale.y - 0.2f, 1);
        }

        RectTransform upgradeMeter2 = upgrade2.GetComponent<RectTransform>();
        RectTransform upgradeMeter = upgrade.GetComponent<RectTransform>();

        //shrink meters if no key is held
        if ((upgradeMeter.localScale.x > 0.1f) && ((!Input.GetKey(KeyCode.C)) || (selection == 2)))
        {
            upgradeMeter.localScale = new Vector3(upgradeMeter.localScale.x - 1.0f * Time.deltaTime, upgradeMeter.localScale.y, 1);
            upgradeMeter.localPosition -= new Vector3((1.0f * Time.deltaTime) / 2, 0f, 0f);  // Move to the left as it grows
        }
        if ((upgradeMeter2.localScale.x > 0.1f) && ((!Input.GetKey(KeyCode.C)) || (selection == 1)))
        {
            upgradeMeter2.localScale = new Vector3(upgradeMeter2.localScale.x - 1.0f * Time.deltaTime, upgradeMeter2.localScale.y, 1);
            upgradeMeter2.localPosition -= new Vector3((1.0f * Time.deltaTime) / 2, 0f, 0f);  // Move to the left as it grows
        }


        if (selection == 1 )
        {
            startButton.localScale = new Vector3(quitButton.localScale.x + 0.2f, quitButton.localScale.y + 0.2f, 1);

            // Grow the upgrade meter while C is held down and the meter hasn't reached max scale
            if (Input.GetKey(KeyCode.C) && upgradeMeter.localScale.x <= 1.8f)
            {
                upgradeMeter.localScale = new Vector3(upgradeMeter.localScale.x + 1.0f * Time.deltaTime, upgradeMeter.localScale.y, 1);
                upgradeMeter.localPosition += new Vector3((1.0f * Time.deltaTime) / 2, 0f, 0f);  // Move to the left as it grows
            }
            if (upgradeMeter.localScale.x > 1.79f)
            {
                SceneManager.LoadScene("Planet");
            }
        }
        else if(selection == 2)
        {
            quitButton.localScale = new Vector3(startButton.localScale.x + 0.2f, startButton.localScale.y + 0.2f, 1);
            
            // Grow the upgrade meter while C is held down and the meter hasn't reached max scale
            if (Input.GetKey(KeyCode.C) && upgradeMeter2.localScale.x <= 2.7f)
            {
                upgradeMeter2.localScale = new Vector3(upgradeMeter2.localScale.x + 1.0f * Time.deltaTime, upgradeMeter2.localScale.y, 1);
                upgradeMeter2.localPosition += new Vector3((1.0f * Time.deltaTime) / 2, 0f, 0f);  // Move to the left as it grows
            }

            if (upgradeMeter2.localScale.x > 2.69f)
            {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
        }
        
    }
}
