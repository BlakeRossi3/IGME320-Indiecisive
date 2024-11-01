using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



public class RoomManager : MonoBehaviour
{
    public GameObject treePrefab;
   // public GameObject enemyPrefab;
    public GameObject coinPrefab;
    // public GameObject doorPrefab;
    public GameObject fogPrefab;

    public int minTrees = 3;
    public int maxTrees = 5;

    public int minEnemies = 2;
    public int maxEnemies = 10;

    public int minCoins = 10;
    public int maxCoins = 20;

    private int currentRoomNumber = 1;

    private int coinNum;
    public GameObject Astro;

    public GameObject northFog;
    public GameObject eastFog;
    public GameObject southFog;
    public GameObject westFog;

    public GameObject northWall;
    public GameObject eastWall;
    public GameObject westWall;
    public GameObject southWall;

    public int northMoveMultiplier;
    public int westMoveMultiplier;

    public GameObject Panel;

    public GameObject ground;

    private GameObject[,] fog = new GameObject[10, 4];

    //bounds for spawning in new room
    private int minX; private int maxX; private int minY; private int maxY;

    Vector3 northOffset;
    Vector3 eastOffset;
    Vector3 westOffset;
    Vector3 southOffset;

    void Start()
    {
        // Common fog offsets (relative positioning based on direction)
        northOffset = new Vector3(0, 14, 0);
        eastOffset = new Vector3(14, 0, 0);
        westOffset = new Vector3(-14, 0, 0);
        southOffset = new Vector3(0, -14, 0);

        fog[0, 0] = northFog;
        fog[0, 1] = eastFog;
        fog[0, 2] = westFog;
        fog[0, 3] = southFog;

        minX = -8; maxX = 8;
        minY = -8; maxY = 8;

        GenerateRoom();

    }
    void update()
    {
        InputController Player = Astro.GetComponent<InputController>();
        coinNum = Player.coins;
    }

    void GenerateRoom()
    {


        // Generate trees
        int treeCount = Random.Range(minTrees, maxTrees + 1);
        for (int i = 0; i < treeCount; i++)
        {
            Vector2 randomPosition = GetRandomPosition();
            Instantiate(treePrefab, randomPosition, Quaternion.identity);
        }

        // Generate enemies with increasing difficulty
        //int enemyCount = Random.Range(minEnemies, maxEnemies + currentRoomNumber);
        //for (int i = 0; i < enemyCount; i++)
        // {
        //     Vector2 randomPosition = GetRandomPosition();
        //     Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        // }

        // Generate coins

        int coinCount = Random.Range(minCoins, maxCoins + currentRoomNumber);
        for (int i = 0; i < coinCount; i++)
        {
            Vector2 randomPosition = GetRandomPosition();
            GameObject coin = Instantiate(coinPrefab, randomPosition, Quaternion.identity);

            Item itemScript = coin.GetComponent<Item>();
            itemScript.astro = Astro;



        }

}

    Vector2 GetRandomPosition()
    {
        // Generate a random position within a specific boundary
        float x = Random.Range(minX, maxX);  // Customize boundary
        float y = Random.Range(minY, maxY);  // Customize boundary
        return new Vector2(x, y);
    }

   public void EnterNextRoom(string direction)
   {


        if (direction == "North")
        {
            MoveFogAndExpandWalls(direction, northOffset, eastOffset, westOffset, 14);

            northOffset = new Vector3(northOffset.x, northOffset.y + 14, northOffset.z);
        }
        else if (direction == "West")
        {

            MoveFogAndExpandWalls(direction, westOffset, northOffset, southOffset, 14);
            westOffset = new Vector3(westOffset.x -14, westOffset.y, westOffset.z);
        }
}

// Helper method to move fog, expand walls, and create new fog
void MoveFogAndExpandWalls(string direction, Vector3 mainDirectionOffset, Vector3 sideOffset1, Vector3 sideOffset2, float moveOffset)
{
        SpriteRenderer spriteRenderer = ground.GetComponent<SpriteRenderer>();

        // Move main direction wall (e.g., North Wall when going north)
        if (direction == "North")
        {

        
            // Extend the height by moveOffset without stretching
            spriteRenderer.size = new Vector2(spriteRenderer.size.x, spriteRenderer.size.y + moveOffset);

            // Adjust the position to keep it centered while extending
            ground.transform.position = new Vector3(
                ground.transform.position.x,
                ground.transform.position.y + moveOffset / 2,
                ground.transform.position.z);

            minY += 14;
            maxY += 14;
            // Move the north wall up by the cloud movement offset
            northWall.transform.position = new Vector3(northWall.transform.position.x,
                                                               northWall.transform.position.y + moveOffset,
                                                               northWall.transform.position.z);
        

            // Scale east and west walls in the north direction (100% scale up)
            eastWall.transform.localScale = new Vector3(eastWall.transform.localScale.x, eastWall.transform.localScale.y + moveOffset, eastWall.transform.localScale.z);
            westWall.transform.localScale = new Vector3(westWall.transform.localScale.x, westWall.transform.localScale.y + moveOffset, westWall.transform.localScale.z);

            // Move the east and west walls up by half of the wallMoveOffset to adjust their position correctly
            eastWall.transform.position = new Vector3(eastWall.transform.position.x, eastWall.transform.position.y + moveOffset / 2, eastWall.transform.position.z);
            westWall.transform.position = new Vector3(westWall.transform.position.x, westWall.transform.position.y + moveOffset / 2, westWall.transform.position.z);

            // Move fog in the main direction
            MoveFogInDirection("North", 0, mainDirectionOffset, "main");

            // Move east fog and add new fog if needed
            Debug.Log(sideOffset1);
            MoveFogInDirection("North", 1, sideOffset1, "side");

            // Move west fog and add new fog if needed
            Debug.Log(sideOffset2);
            MoveFogInDirection("North", 2, sideOffset2, "side");

        }
        else if (direction == "West")
        {
            // Extend the height by moveOffset without stretching
            spriteRenderer.size = new Vector2(spriteRenderer.size.x + moveOffset, spriteRenderer.size.y);

            // Adjust the position to keep it centered while extending
            ground.transform.position = new Vector3(
                ground.transform.position.x - moveOffset / 2,
                ground.transform.position.y,
                ground.transform.position.z);


            minX -= 14;
            maxX -= 14;
            // Move the west wall left (west) by the cloud movement offset
            westWall.transform.position = new Vector3(westWall.transform.position.x - moveOffset, westWall.transform.position.y, westWall.transform.position.z);

            // Scale north and south walls in the west direction (100% scale up)
            northWall.transform.localScale = new Vector3(northWall.transform.localScale.x, northWall.transform.localScale.y + moveOffset, northWall.transform.localScale.z);
            southWall.transform.localScale = new Vector3(southWall.transform.localScale.x, southWall.transform.localScale.y + moveOffset, southWall.transform.localScale.z);

            // Move the north and south walls left by half of the wallMoveOffset to adjust their position correctly
            northWall.transform.position = new Vector3(northWall.transform.position.x - moveOffset / 2, northWall.transform.position.y, northWall.transform.position.z);
            southWall.transform.position = new Vector3(southWall.transform.position.x - moveOffset / 2, southWall.transform.position.y, southWall.transform.position.z);

            MoveFogInDirection("West", 2, mainDirectionOffset, "main");

            // Move north fog and add new fog if needed
            MoveFogInDirection("West", 0, sideOffset1, "side");

            // Move south fog and add new fog if needed
            MoveFogInDirection("West", 3, sideOffset2, "side");

        }

        GenerateRoom();


    }

void MoveFogInDirection(string direction, int index, Vector3 offset, string type)
{
    for (int i = 0; i < 10; i++)
    {
        if (fog[i, index] != null)
        {
                if (direction == "North")
                {
                    // Only modify the y-value by adding 18 to the current y-position
                    fog[i, index].transform.position = new Vector3(fog[i, index].transform.position.x,
                                                                   fog[i, index].transform.position.y + 14,
                                                                   0);
                    
                }
                else if (direction == "West")
                {
                    if (index == 0)
                    {
                        Debug.Log(fog[i, index].transform.position.y);
                    }
                    // Only modify the y-value by adding 18 to the current y-position
                    fog[i, index].transform.position = new Vector3(fog[i, index].transform.position.x - 14,
                                                                   fog[i, index].transform.position.y,
                                                                   0);
                    
                    
                }

                
        }
        else
        {
            if (type != "main")
            {
                    GameObject newFog = null;
                    if (direction == "North")
                    {
                        Debug.Log(offset.x);
                        newFog = Instantiate(fogPrefab, offset, Quaternion.identity); // create new fog


                    }
                    else if (direction == "West")
                    {

                        newFog = Instantiate(fogPrefab, offset, Quaternion.identity); // create new fog

                    }

                   
                    fog[i, index] = newFog; // add it to the array

                    // Attach astro and event system 
                    Fog fogScript = newFog.GetComponent<Fog>();
                    fogScript.astro = Astro;
                    fogScript.EventSystem = gameObject;
                    fogScript.panel = Panel;
                    if (index == 0)
                    {
                        fogScript.type = "North";
                    }
                    else if (index == 1)
                    {
                        fogScript.type = "East";
                    }
                    else if (index == 2)
                    {
                        fogScript.type = "West";
                    }
                    else if (index == 3)
                    {
                        fogScript.type = "South";
                    }
                    // Rotate fog based on direction
                    Transform rotateFog = newFog.GetComponent<Transform>();
                    if (index == 1) // East fog
                    {
                        rotateFog.localEulerAngles = new Vector3(0, 0, rotateFog.localEulerAngles.z + 270);
                    }
                    else if (index == 2) // West fog
                    {
                        rotateFog.localEulerAngles = new Vector3(0, 0, rotateFog.localEulerAngles.z + 90);
                    }
                    else if (index == 3) // West fog
                    {
                        rotateFog.localEulerAngles = new Vector3(0, 0, rotateFog.localEulerAngles.z + 180);
                    }

                    break; // Break after creating new fog
            }
                    

            
        }
    }
}


    
}
