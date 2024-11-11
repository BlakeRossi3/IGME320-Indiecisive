using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject HealthMeter;
    public GameObject ShieldMeter;
    public GameObject SpecialMeter;
    public GameObject LaserMeter;
    public Canvas Canvas;
    public GameObject player;
    public TextMeshProUGUI coinsText;

    public TextMeshProUGUI purchaseText;

    private int shieldLevel = 0;
    private int specialLevel = 0;
    private int laserLevel = 0;
    private int speedLevel = 0;

    private int shieldPrice = 20;
    private int laserPrice = 20;
    private int speedPrice = 20;
    private int specialPrice = 20;

    public int Charge;

    public int Coins;

    public TextMeshProUGUI coinCount;

    public GameObject Select;

    public int selection = 1;

    public GameObject upgrade;

    float scaleSpeed = 150.0f;  // Amount to scale per frame
    float maxScale = 675f;
    float minScale = 10f;
    private string originalSceneName = "Planet"; // Replace with your original scene name
    private bool hasSwitchedScenes = false;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if returning to the original scene
        if (scene.name == originalSceneName && hasSwitchedScenes)
        {
            Destroy(gameObject);
        }
        else
        {
            hasSwitchedScenes = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        Canvas.ForceUpdateCanvases();
       

        if(currentState == GameState.Planet)
        {

            InputController Player = player.GetComponent<InputController>();
            Coins = Player.coins;
            Charge = Player.charge;
            coinsText.text = (" : " + Coins);
            coinCount.text = (" : " + Coins);

            //shop menu logic
            if (Select != null && Player.shopActive)
            {
                //move selection via arrow keys
                if (Input.GetKeyDown(KeyCode.DownArrow) && selection < 4)
                {
                    selection++;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) && selection > 1)
                {
                    selection--;
                }
                RectTransform Selector = Select.GetComponent<RectTransform>();
                if (selection == 1) 
                {
                    Selector.localPosition = new Vector3(-700, 270, -2);
                    coinsText.text = (Coins + " / " + laserPrice);
                }
                else if (selection == 2)
                {
                    Selector.localPosition = new Vector3(-700, 115, -2);
                    coinsText.text = (Coins + " / " + shieldPrice);
                }
                else if (selection == 3)
                {
                    Selector.localPosition = new Vector3(-700, -50, -2);
                    coinsText.text = (Coins + " / " + speedPrice);
                }
                else if (selection == 4)
                {
                    Selector.localPosition = new Vector3(-700, -220, -2);
                    coinsText.text = (Coins + " / " + specialPrice);
                }
                RectTransform upgradeMeter = upgrade.GetComponent<RectTransform>();
                // Grow the upgrade meter while X is held down and the meter hasn't reached max scale
                if (Input.GetKey(KeyCode.X) && upgradeMeter.localScale.x <= maxScale)
                {
                    upgradeMeter.localScale = new Vector3(upgradeMeter.localScale.x + scaleSpeed * Time.deltaTime, upgradeMeter.localScale.y, 1);
                    upgradeMeter.localPosition += new Vector3((scaleSpeed * Time.deltaTime) / 2, 0f, 0f);  // Move to the left as it grows
                }
                // Shrink the upgrade meter when X is released and the meter is above min scale
                else if (!Input.GetKey(KeyCode.X) && upgradeMeter.localScale.x > minScale)
                {
                    upgradeMeter.localScale = new Vector3(upgradeMeter.localScale.x - scaleSpeed * Time.deltaTime, upgradeMeter.localScale.y, 1);
                    upgradeMeter.localPosition -= new Vector3((scaleSpeed * Time.deltaTime) / 2, 0f, 0f);  // Move to the right as it shrinks
                }



                // Check if the upgrade meter is full and call the appropriate ShipMenu
                if (upgradeMeter.localScale.x > maxScale - 10)
                {
                    if (selection == 1)
                    {
                        ShipMenu("Laser");
                    }
                    else if (selection == 2)
                    {
                        ShipMenu("Shield");
                    }
                    else if (selection == 3)
                    {
                        ShipMenu("Speed");
                    }
                    else if (selection == 4)
                    {
                        ShipMenu("Special");
                    }

                    // Reset the upgrade meter to minimum scale and position
                    upgradeMeter.localScale = new Vector3(minScale, upgradeMeter.localScale.y, 1);
                    upgradeMeter.localPosition = new Vector3(-446, upgradeMeter.localPosition.y, -1.5f); // Adjust position as needed
                }


            }

        }
        else if (currentState == GameState.Space)
        {

            StartCoroutine(UpdateUILate());
            
        }  
    }

    // Enum to hold different game states
    public enum GameState
    {
        GameStart,
        Planet,
        Space,
        Explore
    }

    IEnumerator  UpdateUILate()
    {
        yield return new WaitForSeconds(0.1f);

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        coinCount = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
        coinCount.text = (" : " + Coins);
    }

    // The current game state
    public GameState currentState;

    public void ToSpaceState()
    {
        currentState = GameState.Space;
        SceneManager.LoadScene("Space"); // Load the game scene

    }


    public void ShipMenu(string type)
    {
        InputController Player = player.GetComponent<InputController>();
        int coins = Player.coins;

        
        float ScaleX = 80f;  // Amount to scale
        switch (type)
        {
            case "Shield":
                shieldLevel++;
                RectTransform ShieldMete = ShieldMeter.GetComponent<RectTransform>();

                if (shieldLevel < 7 && coins >= shieldPrice)
                {
   
                    ShieldMete.localScale = new Vector3(ShieldMete.localScale.x + ScaleX, ShieldMete.localScale.y, ShieldMete.localScale.z);
                    ShieldMete.localPosition += new Vector3(ScaleX / 2, 0f, 0f);  // Move up by half the scale amount
                    Player.coins -= shieldPrice;
                    shieldPrice *= 2;
                    
                }
                
            break;

            case "Laser":
                laserLevel++;
                RectTransform LaserMete = LaserMeter.GetComponent<RectTransform>();
                Debug.Log("good");
                if (laserLevel < 7 && coins >= laserPrice)
                {
                    LaserMete.localScale = new Vector3(LaserMete.localScale.x + ScaleX, LaserMete.localScale.y, LaserMete.localScale.z);
                    LaserMete.localPosition += new Vector3(ScaleX / 2, 0f, 0f);  // Move up by half the scale amount
                    Player.coins -= laserPrice;
                    laserPrice *= 2;
                    
                }
            break;

            case "Special":
                specialLevel++;
                RectTransform SpecialMete = SpecialMeter.GetComponent<RectTransform>();
                if (specialLevel < 7 && coins >= specialPrice)
                {
                    SpecialMete.localScale = new Vector3(SpecialMete.localScale.x + ScaleX, SpecialMete.localScale.y, SpecialMete.localScale.z);
                    SpecialMete.localPosition += new Vector3(ScaleX / 2, 0f, 0f);  // Move up by half the scale amount
                    Player.coins -= specialPrice;
                    specialPrice *= 2;
                    
                }
                
            break;

            case "Speed":
                speedLevel++;
                RectTransform HealthMete = HealthMeter.GetComponent<RectTransform>();

                if(speedLevel < 7 && coins >= speedPrice)
                {
                    HealthMete.localScale = new Vector3(HealthMete.localScale.x + ScaleX, HealthMete.localScale.y, HealthMete.localScale.z);
                    HealthMete.localPosition += new Vector3(ScaleX / 2, 0f, 0f);  // Move up by half the scale amount
                    Player.coins -= speedPrice;
                    speedPrice *= 2;
                    
                }
                
            break;
        }


    }




}
