using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject SpeedMeter;
    public GameObject ShieldMeter;
    public GameObject SpecialMeter;
    public GameObject LaserMeter;
    public Canvas Canvas;
    public GameObject player;
    public TextMeshProUGUI coinsText;

    public TextMeshProUGUI purchaseText;

    

    
    int currentLaserLevel = 1;
    int currentShieldLevel = 1;
    int currentSpeedLevel = 1;
    int currentSpecialLevel = 1;

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

    float scaleSpeed = 200.0f;  // Amount to scale per frame
    float maxScale = 675f;
    float minScale = 10f;
    private string originalSceneName = "Planet";
    private bool hasSwitchedScenes = false;

    void Start()
    {
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
        Canvas.ForceUpdateCanvases();

        PlayerPrefs.SetInt("credits", Coins);
        PlayerPrefs.SetInt("charge", Charge);

        PlayerPrefs.SetInt("laserLevel", currentLaserLevel);
        PlayerPrefs.SetInt("shieldLevel", currentShieldLevel);
        PlayerPrefs.SetInt("speedLevel", currentSpeedLevel);
        PlayerPrefs.SetInt("specialLevel", currentSpecialLevel);
        PlayerPrefs.Save();



        if (currentState == GameState.Planet)
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

    }

    // Enum to hold different game states
    public enum GameState
    {
        GameStart,
        Planet,
        Space,
        Explore
    }



    // The current game state
    public GameState currentState;

    public void ToSpaceState()
    {
        currentState = GameState.Space;
        SceneManager.LoadScene("Space"); // Load the game scene

    }

    public void ToBossState()
    {
        //TODO: couldn't find the enum to update current state >.> just marking it as space for now
        currentState = GameState.Space;
        SceneManager.LoadScene("Boss");
    }


    public void ShipMenu(string type)
    {
        InputController Player = player.GetComponent<InputController>();
        int coins = Player.coins;

        
        float ScaleX = 80f;  // Amount to scale
        switch (type)
        {
            case "Shield":
                RectTransform ShieldMete = ShieldMeter.GetComponent<RectTransform>();

                if (currentShieldLevel < 7 && coins >= shieldPrice)
                {
   
                    ShieldMete.localScale = new Vector3(ShieldMete.localScale.x + ScaleX, ShieldMete.localScale.y, ShieldMete.localScale.z);
                    ShieldMete.localPosition += new Vector3(ScaleX / 2, 0f, 0f);  // Move up by half the scale amount
                    Player.coins -= shieldPrice;
                    shieldPrice *= 2;
                    currentShieldLevel += 1;

                }
                
            break;

            case "Laser":
                RectTransform LaserMete = LaserMeter.GetComponent<RectTransform>();
                Debug.Log("good");
                if (currentLaserLevel < 7 && coins >= laserPrice)
                {
                    LaserMete.localScale = new Vector3(LaserMete.localScale.x + ScaleX, LaserMete.localScale.y, LaserMete.localScale.z);
                    LaserMete.localPosition += new Vector3(ScaleX / 2, 0f, 0f);  // Move up by half the scale amount
                    Player.coins -= laserPrice;
                    laserPrice *= 2;
                    currentLaserLevel += 1;
                    
                }
            break;

            case "Special":
                RectTransform SpecialMete = SpecialMeter.GetComponent<RectTransform>();
                if (currentSpecialLevel < 7 && coins >= specialPrice)
                {
                    SpecialMete.localScale = new Vector3(SpecialMete.localScale.x + ScaleX, SpecialMete.localScale.y, SpecialMete.localScale.z);
                    SpecialMete.localPosition += new Vector3(ScaleX / 2, 0f, 0f);  // Move up by half the scale amount
                    Player.coins -= specialPrice;
                    specialPrice *= 2;
                    currentSpecialLevel += 1;
                    
                }
                
            break;

            case "Speed":
                RectTransform HealthMete = SpeedMeter.GetComponent<RectTransform>();

                if(currentSpeedLevel < 7 && coins >= speedPrice)
                {
                    HealthMete.localScale = new Vector3(HealthMete.localScale.x + ScaleX, HealthMete.localScale.y, HealthMete.localScale.z);
                    HealthMete.localPosition += new Vector3(ScaleX / 2, 0f, 0f);  // Move up by half the scale amount
                    Player.coins -= speedPrice;
                    speedPrice *= 2;
                    currentSpeedLevel += 1;
                }
                
            break;
        }


    }




}
