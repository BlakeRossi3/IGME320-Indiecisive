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
    public GameObject Ship;
    private RectTransform ship;
    public GameObject player;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI chargeText;

    public TextMeshProUGUI shieldText;
    public TextMeshProUGUI specialText;
    public TextMeshProUGUI laserText;
    public TextMeshProUGUI healthText;

    private int shieldLevel = 0;
    private int specialLevel = 0;
    private int laserLevel = 0;
    private int healthLevel = 0;

    private int shieldPrice = 20;
    private int laserPrice = 20;
    private int healthPrice = 20;
    private int specialPrice = 20;

    public int Charge;

    public int Coins;

    public TextMeshProUGUI coinCount;



    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (Ship != null)
        {
            ship = Ship.GetComponent<RectTransform>();
            
        }
        
        if (shieldText!= null)
        {
            shieldText.text = (" Shield: " + shieldPrice);
            specialText.text = ("Special: " + specialPrice);
            laserText.text = ("Laser: " + laserPrice);
            healthText.text = ("Health: " + healthPrice);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        Canvas.ForceUpdateCanvases();

        if(currentState == GameState.Planet)
        {
            ship.localEulerAngles = new Vector3(0, 0, ship.localEulerAngles.z + 30 * Time.deltaTime);

            InputController Player = player.GetComponent<InputController>();
            Coins = Player.coins;
            Charge = Player.charge;
            coinsText.text = (" : " + Coins);
            coinCount.text = (" : " + Coins);
            chargeText.text = (" : " + Charge);

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
        Space
    }

    IEnumerator  UpdateUILate()
    {
        yield return new WaitForSeconds(0.1f);

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        coinCount = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
        coinCount.text = (" : " + Coins);

        chargeText = GameObject.Find("ChargeText").GetComponent<TextMeshProUGUI>();
        chargeText.text = (" : " + Charge);
    }

    // The current game state
    public GameState currentState;

    public void ToSpaceState()
    {
        currentState = GameState.Space;
        SceneManager.LoadScene("Space"); // Load the game scene

    }


    public void OnClick(string type)
    {
        InputController Player = player.GetComponent<InputController>();
        int coins = Player.coins;

        coinsText.text = coins.ToString();

        switch (type)
        {
            case "Shield":
                shieldLevel++;
                RectTransform ShieldMete = ShieldMeter.GetComponent<RectTransform>();

                if (shieldLevel < 7 && coins >= shieldPrice)
                {
                    float shieldScaleY = 50f;  // Amount to scale
                    ShieldMete.localScale = new Vector3(ShieldMete.localScale.x, ShieldMete.localScale.y + shieldScaleY, ShieldMete.localScale.z);
                    ShieldMete.localPosition += new Vector3(0f, shieldScaleY / 2, 0f);  // Move up by half the scale amount
                    Player.coins -= shieldPrice;
                    shieldPrice *= 2;
                    
                }
                
            break;

            case "Laser":
                laserLevel++;
                RectTransform LaserMete = LaserMeter.GetComponent<RectTransform>();

                if (laserLevel < 7 && coins >= laserPrice)
                {
                    float laserScaleY = 50f;  // Amount to scale
                    LaserMete.localScale = new Vector3(LaserMete.localScale.x, LaserMete.localScale.y + laserScaleY, LaserMete.localScale.z);
                    LaserMete.localPosition += new Vector3(0f, laserScaleY / 2, 0f);  // Move up by half the scale amount
                    Player.coins -= laserPrice;
                    laserPrice *= 2;
                    
                }
            break;

            case "Special":
                specialLevel++;
                RectTransform SpecialMete = SpecialMeter.GetComponent<RectTransform>();
                if (specialLevel < 7 && coins >= specialPrice)
                {
                    float specialScaleY = 50f;  // Amount to scale
                    SpecialMete.localScale = new Vector3(SpecialMete.localScale.x, SpecialMete.localScale.y + specialScaleY, SpecialMete.localScale.z);
                    SpecialMete.localPosition += new Vector3(0f, specialScaleY / 2, 0f);  // Move up by half the scale amount
                    Player.coins -= specialPrice;
                    specialPrice *= 2;
                    
                }
                
            break;

            case "Health":
                healthLevel++;
                RectTransform HealthMete = HealthMeter.GetComponent<RectTransform>();

                if(healthLevel < 7 && coins >= healthPrice)
                {
                    float healthScaleY = 50f;  // Amount to scale
                    HealthMete.localScale = new Vector3(HealthMete.localScale.x, HealthMete.localScale.y + healthScaleY, HealthMete.localScale.z);
                    HealthMete.localPosition += new Vector3(0f, healthScaleY / 2, 0f);  // Move up by half the scale amount
                    Player.coins -= healthPrice;
                    healthPrice *= 2;
                    
                }
                
            break;
        }

        shieldText.text = (" Shield: " + shieldPrice);
        specialText.text = ("Special: " + specialPrice);
        laserText.text = ("Laser: " + laserPrice);
        healthText.text = ("Health: " + healthPrice);
    }




}
