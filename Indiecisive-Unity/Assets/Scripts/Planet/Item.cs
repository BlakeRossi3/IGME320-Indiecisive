using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public GameObject astro;
    private float pickUpRadius = 2.0f;
    public float floatAmplitude = 0.001f;  // Amplitude of the floating animation (how high it floats)
    public float floatSpeed = 0.5f;  // Speed of the floating animation
    private Vector2 originalPosition;  // To store the original position of the item
    public GameObject uiPanel;  // The panel to display the item in
    public GameObject itemIconPrefab;  // Prefab of the item icon to display in the panel
    public TextMeshProUGUI coinCount;
    public bool isCoin;
    public bool isCharge;



    // Start is called before the first frame update
    void Start()
    {

        originalPosition = transform.position;
        if (isCoin)
        {
            floatAmplitude = 0.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPickUp();
    }
    void FixedUpdate()
    {
        float distanceToItem = Vector2.Distance(transform.position, astro.transform.position);

        if (distanceToItem < pickUpRadius || isCoin)
        {
            // Animate the item using a sine wave for gentle floating
            float newY = originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector2(originalPosition.x, newY);
        }
        else
        {
            // Reset the item's position when the player is not nearby
            transform.position = originalPosition;
        }
    }

    void CheckPickUp()
    {

        // Get distance of astro to item. then check if mouse is on item and left mouse button is down.
        float distanceToItem = Vector2.Distance(transform.position, astro.transform.position);

            if (Input.GetKeyDown(KeyCode.C) && distanceToItem < pickUpRadius)  // Left mouse button clicked and on shovel
            {
                gameObject.SetActive(false);

                InputController bag = astro.GetComponent<InputController>();

                if (!isCoin && !isCharge)
                {
                    // Add the item icon to the UI panel
                    GameObject itemIcon = Instantiate(itemIconPrefab);  // Create the icon from the prefab
                    itemIcon.transform.SetParent(uiPanel.transform, true);  // Set the parent to the panel



                    RectTransform iconRectTransform = itemIcon.GetComponent<RectTransform>();

                    // Reset the local position and scale
                    iconRectTransform.localPosition = Vector3.zero;  // This places it in the middle of the panel
                    iconRectTransform.localScale = Vector3.one;      // Make sure the scale is correct
                    iconRectTransform.anchoredPosition = new Vector2(-30f, -120f);  // Adjust as needed based on your UI layout
                    iconRectTransform.sizeDelta = new Vector2(200, 200);
                    itemIcon.SetActive(true);


                    
                    bag.shovel = true;
                }
                else if (isCoin)
                {
                    bag.coins += 12;
                    coinCount.text = (" : " + bag.coins);
                }
                else 
                {

                bag.charge += 500;

                }
                 

            }
        
    }

    
}
