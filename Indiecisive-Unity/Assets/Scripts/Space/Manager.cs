using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    Player playerClass;

    [SerializeField]
    EnemyManager enemies;

    [SerializeField]
    StageTimer timer;

    [SerializeField]
    protected float startDelay;

    // Start is called before the first frame update
    void Start()
    {
        enemies.InitialSpawnDelay = startDelay;
        timer.InitialStartDelay = startDelay;
    }

    // Update is called once per frame
    void Update()
    {
        checkForGameOver();
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
}
