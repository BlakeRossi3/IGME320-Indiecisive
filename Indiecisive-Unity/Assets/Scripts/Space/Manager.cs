using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField]
    Player playerClass;

    [SerializeField]
    GameObject enemies;

    [SerializeField]
    StageTimer timer;

    [SerializeField]
    protected float startDelay;

    [SerializeField]
    OnboardingText playerText;

    [SerializeField]
    GameObject enemyType1;

    [SerializeField]
    GameObject enemyType2;

    [SerializeField]
    GameObject playerHealth;
    [SerializeField]
    GameObject shieldStatus;
    [SerializeField]
    GameObject specialStatus;

    [SerializeField]
    TimeManager timeManager;

    private float flashDuration;
    SpriteRenderer heart1Color;
    SpriteRenderer heart2Color;
    SpriteRenderer heart3Color;
    SpriteRenderer heart4Color;
    SpriteRenderer heart5Color;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemies.GetComponents<EnemyManager>().Length; i++)
        {
            // meant to set spawn delay for each enemy type's manager in the Empty
            // NOT WORKING
            enemies.GetComponent<EnemyManager>().InitialSpawnDelay = startDelay;
        }
        timer.InitialStartDelay = startDelay;

        flashDuration = 3f;

        heart1Color = GameObject.Find("health1").GetComponent<SpriteRenderer>();
        heart2Color = GameObject.Find("health2").GetComponent<SpriteRenderer>();
        heart3Color = GameObject.Find("health3").GetComponent<SpriteRenderer>();
        heart4Color = GameObject.Find("health4").GetComponent<SpriteRenderer>();
        heart5Color = GameObject.Find("health5").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        checkForGameOver();

        DoOnboarding();
    }
    
    //checks if the player health has reached zero, places the game in a game over state
    //TODO: restart level menu
    private void checkForGameOver()
    {
        if (playerClass.currentHealth <= 0)
        {
            playerClass.isGameOver = true;
        }
    }

    // goes through the sequence of the onboarding level
    private void DoOnboarding()
    {
        if (SceneManager.GetActiveScene().name == "Onboarding")
        {
            if (enemyType1 != null)
            {
                if (playerText.i == 6)
                {
                    enemyType1.SetActive(true);
                    playerText.invokeBool = false;
                }

                if (enemyType1.GetComponent<EnemyType1>().onScreen)
                {
                    // TODO:: fix this maybe
                    //timeManager.DoSlowmotion();

                    playerHealth.transform.position = new Vector3(-0.6637469f, -2.980433f, 0.0f);

                    if (flashDuration >= 0)
                    {
                        heart1Color.color = Color.Lerp(Color.white, Color.clear, Mathf.PingPong(Time.time, 0.5f));
                        heart2Color.color = Color.Lerp(Color.white, Color.clear, Mathf.PingPong(Time.time, 0.5f));
                        heart3Color.color = Color.Lerp(Color.white, Color.clear, Mathf.PingPong(Time.time, 0.5f));
                        heart4Color.color = Color.Lerp(Color.white, Color.clear, Mathf.PingPong(Time.time, 0.5f));
                        heart5Color.color = Color.Lerp(Color.white, Color.clear, Mathf.PingPong(Time.time, 0.5f));
                        flashDuration -= 1.0f * Time.deltaTime;
                    }
                    else
                    {
                        heart1Color.color = Color.white;
                        heart2Color.color = Color.white;
                        heart3Color.color = Color.white;
                        heart4Color.color = Color.white;
                        heart5Color.color = Color.white;
                    }
                }
            }
            if (enemyType1 == null && playerText.i <= 9)
            {
                playerText.invokeBool = true;
            }

            if (playerText.i == 10)
            {
                shieldStatus.transform.position = new Vector3(-4.66f, -3.51f, 0.0f);
            }
            if (playerText.i == 11)
            {
                specialStatus.transform.position = new Vector3(-5.52f, -3.38f, 0.0f);
            }

            if (enemyType2 != null)
            {
                if (playerText.i == 13)
                {
                    enemyType2.SetActive(true);
                    playerText.invokeBool = false;
                }
            }
            if (enemyType2 == null)
            {
                playerText.invokeBool = true;
            }

            if (playerText.i == playerText.stringArray.Length)
            {
                SceneManager.LoadScene("Planet");
            }
        }
    }
}
