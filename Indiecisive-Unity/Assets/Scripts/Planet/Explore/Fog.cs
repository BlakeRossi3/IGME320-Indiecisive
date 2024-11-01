using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Fog : MonoBehaviour
{

    public GameObject astro;
    public GameObject EventSystem;
    public TextMeshProUGUI fogText;

    public GameObject panel;

    public string type;

    private bool northText;
    private bool westText;
    private bool panelOn;

    // Start is called before the first frame update
    void Start()
    {
        northText = false;
        westText = false;
        if (fogText == null)
        {

            fogText = GameObject.Find("Pirate/Canvas/FogText").GetComponent<TextMeshProUGUI>();
        }
        if (panel == null)
        {

            panel = GameObject.Find("Pirate/Canvas/Panel").GetComponent<GameObject>();
        }
        fogText.gameObject.SetActive(false);
        panel.SetActive(false );

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToObject = Vector2.Distance(transform.position, astro.transform.position);
        RoomManager roomManager = EventSystem.GetComponent<RoomManager>();

        if (Input.GetKeyDown(KeyCode.C) && distanceToObject < 8)
        {
            northText = false;
            westText = false;
            fogText.gameObject.SetActive(false);

            panel.SetActive(true);
            panelOn = true;
            
            
        }
        if (panelOn)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                panel.SetActive(false);
                roomManager.EnterNextRoom(type);
                panelOn = false;
                
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                panel.SetActive(false);
                panelOn = false;

            }
        }

        if (distanceToObject < 8 && !northText && !westText )
        {
            if(type == "North")
            {
                northText = true;
            }
            else if (type == "West")
            {
                westText = true;
            }


            fogText.gameObject.SetActive(true);
            fogText.text = "C to move fog " + type;

            
            
        }
        else if (distanceToObject > 10  && northText)
        {
            northText = false;
            fogText.gameObject.SetActive(false);
        }
        else if (distanceToObject > 10 && westText)
        {
            westText = false;
            fogText.gameObject.SetActive(false);
        }


    }
}
