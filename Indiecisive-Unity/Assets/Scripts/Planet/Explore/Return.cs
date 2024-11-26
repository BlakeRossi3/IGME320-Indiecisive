using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Return : MonoBehaviour
{
    public GameObject Player;
    public GameObject Menu;
    private bool menuSwitch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputController player = Player.GetComponent<InputController>();

        float distanceToObject = Vector2.Distance(transform.position, Player.transform.position);

        // Check if the player is within interaction radius of the object
        if (distanceToObject <= 2f)
        {
            player.interactionText.gameObject.SetActive(true);
            player.interactionText.text = "C to Interact";
            player.textSwitch = true;

            if(Input.GetKeyDown(KeyCode.C))
            {
                Menu.SetActive(true);
                menuSwitch = true;
            }
            if (menuSwitch)
            {
                if(Input.GetKeyDown(KeyCode.X))
                {
                    SceneManager.LoadScene("Planet"); 

                }
                else if(Input.GetKeyDown(KeyCode.Z))
                {
                    menuSwitch = false;
                    Menu.SetActive(false);
                }

            }

        }
        else
        {
            player.interactionText.gameObject.SetActive(false);
            player.textSwitch = false;
            Menu.SetActive(false);
        }
    }
}
