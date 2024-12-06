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

    public TextMeshProUGUI bulletText;
    public TextMeshProUGUI bulletTextShop;

    public TextMeshProUGUI creditText;
    public TextMeshProUGUI creditTextShop;

    public TextMeshProUGUI upgradeTypeText; // Upgrade texts for shop menu
    public TextMeshProUGUI upgradeLevelText;
    public TextMeshProUGUI upgradeInfoText;
    public TextMeshProUGUI upgradePriceText;

    public TextMeshProUGUI creditsVend;

    int currentLaserLevel;
    int currentShieldLevel;
    int currentSpeedLevel;
    int currentSpecialLevel;

    private int shieldPrice = 20;
    private int laserPrice = 20;
    private int speedPrice = 20;
    private int specialPrice = 20;

    public int Charge;

    public int Coins;



    public GameObject Select;

    public int selection = 1;

    public GameObject upgrade;
    public GameObject upgradeVend;

    float scaleSpeed = 250.0f;  // Amount to scale per frame
    float maxScale = 675f;
    float minScale = 10f;
    private string originalSceneName = "Planet";
    private bool hasSwitchedScenes = false;

    public int bulletNum;
    public int bulletPrice;
    public TextMeshProUGUI bulletPriceText;
    public TextMeshProUGUI bulletCountText;
     
    public int selection2 = 1;
    public GameObject goToSpaceButton;
    public GameObject challengeBoss;

    public GameObject spaceMeter;
    public GameObject bossMeter;


    void Start()
    {

        //Retrieves playerprefs stats
        Coins = PlayerPrefs.GetInt("credits", 0);
        Charge = PlayerPrefs.GetInt("charge", 0);
        currentLaserLevel = PlayerPrefs.GetInt("laserLevel", 1);
        currentShieldLevel = PlayerPrefs.GetInt("shieldLevel", 1);
        currentSpeedLevel = PlayerPrefs.GetInt("speedLevel", 1);
        currentSpecialLevel = PlayerPrefs.GetInt("specialLevel", 1);

        float ScaleX = 80f;  // Amount to scale
        bulletNum = 0;
        bulletPrice = 5;
        for (int i = 0; i < currentLaserLevel - 1; i++)
        {
            RectTransform LaserMete = LaserMeter.GetComponent<RectTransform>();
            
            LaserMete.localScale = new Vector3(LaserMete.localScale.x + ScaleX, LaserMete.localScale.y, LaserMete.localScale.z);
            LaserMete.localPosition += new Vector3(ScaleX / 2, 0f, 0f);  // Move up by half the scale amount

            laserPrice *= 2; 
        }
        for (int i = 0; i < currentShieldLevel - 1; i++)
        {
            RectTransform ShieldMete = ShieldMeter.GetComponent<RectTransform>();
            ShieldMete.localScale = new Vector3(ShieldMete.localScale.x + ScaleX, ShieldMete.localScale.y, ShieldMete.localScale.z);
            ShieldMete.localPosition += new Vector3(ScaleX / 2, 0f, 0f);  // Move up by half the scale amount

            shieldPrice *= 2;
        }
        for (int i = 0; i < currentSpeedLevel - 1; i++)
        {
            RectTransform HealthMete = SpeedMeter.GetComponent<RectTransform>();

            HealthMete.localScale = new Vector3(HealthMete.localScale.x + ScaleX, HealthMete.localScale.y, HealthMete.localScale.z);
            HealthMete.localPosition += new Vector3(ScaleX / 2, 0f, 0f);  // Move up by half the scale amount

                speedPrice *= 2;
        }
        for (int i = 0; i < currentSpecialLevel - 1; i++)
        {
            RectTransform HealthMete = SpeedMeter.GetComponent<RectTransform>();

            HealthMete.localScale = new Vector3(HealthMete.localScale.x + ScaleX, HealthMete.localScale.y, HealthMete.localScale.z);
            HealthMete.localPosition += new Vector3(ScaleX / 2, 0f, 0f);  // Move up by half the scale amount

            specialPrice *= 2;
        }
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
            creditText.text = (" : " + Coins);
            creditTextShop.text = (" : " + Coins);
            bulletText.text = (" : " + Charge);
            bulletTextShop.text = (" : " + Charge);

            bulletCountText.text = ("x" + bulletNum);
            bulletPriceText.text = ("" + (bulletPrice * bulletNum));
            creditsVend.text = (" : " + Coins);

            //SHIP MENU
            if (Player.shipActive)
            {
                Transform spaceButton = goToSpaceButton.GetComponent<Transform>();
                Transform bossButton = challengeBoss.GetComponent<Transform>();

                if (Input.GetKeyDown(KeyCode.DownArrow) && selection2 == 1)
                {
                    selection2++;

                    spaceButton.localScale = new Vector3(spaceButton.localScale.x - 15f, spaceButton.localScale.y - 15f, 1);
                }

                else if (Input.GetKeyDown(KeyCode.UpArrow) && selection2 == 2)
                {
                    selection2--;

                    bossButton.localScale = new Vector3(bossButton.localScale.x - 15f, bossButton.localScale.y - 15f, 1);
                }

                RectTransform upgradeMeterSpace = spaceMeter.GetComponent<RectTransform>();
                RectTransform upgradeMeterBoss = bossMeter.GetComponent<RectTransform>();


                //shrink meters if no key is held
                if ((upgradeMeterSpace.localScale.x > .02f) && ((!Input.GetKey(KeyCode.C)) || (selection2 == 2)))
                {
                    upgradeMeterSpace.localScale = new Vector3(upgradeMeterSpace.localScale.x - 1f * Time.deltaTime, upgradeMeterSpace.localScale.y, 1);
                    upgradeMeterSpace.localPosition -= new Vector3((1f * Time.deltaTime) / 2, 0f, 0f);  // Move to the left as it grows
                }
                if ((upgradeMeterBoss.localScale.x > .02f) && ((!Input.GetKey(KeyCode.C)) || (selection2 == 1)))
                {
                    upgradeMeterBoss.localScale = new Vector3(upgradeMeterBoss.localScale.x - 1f * Time.deltaTime, upgradeMeterBoss.localScale.y, 1);
                    upgradeMeterBoss.localPosition -= new Vector3((1f * Time.deltaTime) / 2, 0f, 0f);  // Move to the left as it grows
                }


                if (selection2 == 1)
                {
                    spaceButton.localScale = new Vector3(bossButton.localScale.x + 15f, bossButton.localScale.y + 15f, 1);

                    // Grow the upgrade meter while C is held down and the meter hasn't reached max scale
                    if (Input.GetKey(KeyCode.C) && upgradeMeterSpace.localScale.x <= 3.2f)
                    {
                        upgradeMeterSpace.localScale = new Vector3(upgradeMeterSpace.localScale.x + 1f * Time.deltaTime, upgradeMeterSpace.localScale.y, 1);
                        upgradeMeterSpace.localPosition += new Vector3((1f * Time.deltaTime) / 2, 0f, 0f);  // Move to the left as it grows
                    }
                    if (upgradeMeterSpace.localScale.x > 3.15f)
                    {
                        SceneManager.LoadScene("Space");
                    }
                }
                else if (selection2 == 2)
                {
                    bossButton.localScale = new Vector3(spaceButton.localScale.x + 15f, spaceButton.localScale.y + 15f, 1);

                    // Grow the upgrade meter while C is held down and the meter hasn't reached max scale
                    if (Input.GetKey(KeyCode.C) && upgradeMeterBoss.localScale.x <= 4.2f)
                    {
                        upgradeMeterBoss.localScale = new Vector3(upgradeMeterBoss.localScale.x + 1f * Time.deltaTime, upgradeMeterBoss.localScale.y, 1);
                        upgradeMeterBoss.localPosition += new Vector3((1f * Time.deltaTime) / 2, 0f, 0f);  // Move to the left as it grows
                    }

                    if (upgradeMeterBoss.localScale.x > 4.15f)
                    {
                        SceneManager.LoadScene("Boss");
                    }
                }


            }



                //VENDING MACHINE
                if (Player.vendActive)
                {
                    RectTransform upgradeMeter2 = upgradeVend.GetComponent<RectTransform>();

                    //increase purchase number of bullets
                    if (Input.GetKeyDown(KeyCode.LeftArrow) && bulletNum > 1)
                    {
                        bulletNum--;
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow) && bulletNum < 20)
                    {
                        bulletNum++;
                    }

                    //increase meter size
                    if (Input.GetKey(KeyCode.C) && upgradeMeter2.localScale.x <= 5.2f)
                    {
                        upgradeMeter2.localScale = new Vector3(upgradeMeter2.localScale.x + 2f * Time.deltaTime, upgradeMeter2.localScale.y, 1);
                        upgradeMeter2.localPosition += new Vector3((2f * Time.deltaTime) / 2, 0f, 0f);  // Move to the left as it grows
                    }
                    else if (!Input.GetKey(KeyCode.C) && upgradeMeter2.localScale.x > .2f)
                    {
                        upgradeMeter2.localScale = new Vector3(upgradeMeter2.localScale.x - 2f * Time.deltaTime, upgradeMeter2.localScale.y, 1);
                        upgradeMeter2.localPosition -= new Vector3((2f * Time.deltaTime) / 2, 0f, 0f);  // Move to the right as it shrinks
                    }

                    //handle vending machine purchase
                    if (upgradeMeter2.localScale.x > 5.15f)
                    {
                        if (Coins >= bulletPrice * bulletNum)
                        {
                            Player.coins -= bulletPrice * bulletNum;
                            bulletNum = 1;
                        }

                        upgradeMeter2.localScale = new Vector3(.2f, upgradeMeter2.localScale.y, 1);
                        upgradeMeter2.localPosition = new Vector3(-5f, upgradeMeter2.localPosition.y, -1.5f); // Adjust position as needed
                    }
                }

                // SHOP MENU
                if (Player.shopActive)
                {
                    Debug.Log("ShopActive");
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
                        upgradeInfoText.text = ("Increase Primary Fire Damage");
                        upgradeTypeText.text = ("LASER");
                        upgradeLevelText.text = ("LVL " + currentLaserLevel);
                        upgradePriceText.text = ("" + laserPrice);
                    }
                    else if (selection == 2)
                    {
                        Selector.localPosition = new Vector3(-700, 115, -2);
                        upgradeInfoText.text = ("Increase Shield Duration");
                        upgradeTypeText.text = ("SHIELD");
                        upgradeLevelText.text = ("LVL " + currentShieldLevel);
                        upgradePriceText.text = ("" + shieldPrice);
                    }
                    else if (selection == 3)
                    {
                        Selector.localPosition = new Vector3(-700, -50, -2);
                        upgradeInfoText.text = ("Increase Ship Speed");
                        upgradeTypeText.text = ("SPEED");
                        upgradeLevelText.text = ("LVL " + currentSpeedLevel);
                        upgradePriceText.text = ("" + speedPrice);
                    }
                    else if (selection == 4)
                    {
                        Selector.localPosition = new Vector3(-700, -220, -2);
                        upgradeInfoText.text = ("Increase Special Ability Level");
                        upgradeTypeText.text = ("SPECIAL");
                        upgradeLevelText.text = ("LVL " + currentSpecialLevel);
                        upgradePriceText.text = ("" + specialPrice);
                    }
                    RectTransform upgradeMeter = upgrade.GetComponent<RectTransform>();
                    // Grow the upgrade meter while X is held down and the meter hasn't reached max scale
                    if (Input.GetKey(KeyCode.C) && upgradeMeter.localScale.x <= maxScale)
                    {
                        upgradeMeter.localScale = new Vector3(upgradeMeter.localScale.x + scaleSpeed * Time.deltaTime, upgradeMeter.localScale.y, 1);
                        upgradeMeter.localPosition += new Vector3((scaleSpeed * Time.deltaTime) / 2, 0f, 0f);  // Move to the left as it grows
                    }
                    // Shrink the upgrade meter when X is released and the meter is above min scale
                    else if (!Input.GetKey(KeyCode.C) && upgradeMeter.localScale.x > minScale)
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

                if (currentShieldLevel < 11 && coins >= shieldPrice)
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
                if (currentLaserLevel < 11 && coins >= laserPrice)
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
                if (currentSpecialLevel < 11 && coins >= specialPrice)
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

                if(currentSpeedLevel < 11 && coins >= speedPrice)
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
