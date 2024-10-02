using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject astro;
    private float pickUpRadius = 2.0f;
    public float floatAmplitude = 0.001f;  // Amplitude of the floating animation (how high it floats)
    public float floatSpeed = 0.5f;  // Speed of the floating animation
    private Vector2 originalPosition;  // To store the original position of the item
    public GameObject uiPanel;  // The panel to display the item in
    public GameObject itemIconPrefab;  // Prefab of the item icon to display in the panel

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPickUp();
    }
    void FixedUpdate()
    {
        float distanceToItem = Vector2.Distance(transform.position, astro.transform.position);

        if (distanceToItem < pickUpRadius)
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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float MouseDistanceToItem = Vector2.Distance(transform.position, mousePos);

        if (distanceToItem < pickUpRadius )
        {
            if (Input.GetMouseButtonDown(0) && MouseDistanceToItem < 2)  // Left mouse button clicked and on shovel
            {
                gameObject.SetActive(false);

                // Add the item icon to the UI panel
                Debug.Log("Item in Bag");
                GameObject itemIcon = Instantiate(itemIconPrefab);  // Create the icon from the prefab
                itemIcon.transform.SetParent(uiPanel.transform, true);  // Set the parent to the panel


                
                RectTransform iconRectTransform = itemIcon.GetComponent<RectTransform>();

                // Reset the local position and scale
                iconRectTransform.localPosition = Vector3.zero;  // This places it in the middle of the panel
                iconRectTransform.localScale = Vector3.one;      // Make sure the scale is correct
                iconRectTransform.anchoredPosition = new Vector2(-30f, -120f);  // Adjust as needed based on your UI layout
                iconRectTransform.sizeDelta = new Vector2(200, 200);
                itemIcon.SetActive(true);
                

                InputController bag = astro.GetComponent<InputController>();
                bag.shovel = true;

            }
        }
    }
}
