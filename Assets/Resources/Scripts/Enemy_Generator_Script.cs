using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy_Generator_Script : MonoBehaviour 
{
    public float enemyMaxRate = 3f;
    public float enemyMinRate = 1f;
    public float perNSecond = 3f;
    public float bossSpawnTime = 17;


    //public int minShipsPerWave = 3;
    //public int maxShipsPerWave = 10;

    float maxSpawnCooldown = 0.7f;
    float minSpawnCooldown = 0.3f;
    float spawnCooldown;

    public List<GameObject> normalEnemyPrefabs = new List<GameObject>();
    public List<GameObject> bossPrefabs = new List<GameObject>();
    
    float destroyLine = 1.25f;
    //Private variable
    Vector3 bottomLeft_World;
    Vector3 topRight_World;
    float PreviousGenerationTime = 0;
    bool canGenerate = true;
    float difficulty = 1;
    float startTime;

    Coroutine bossExist = null;
    public bool bossCoroutineStarted = false;
	// Use this for initialization
	void Start () 
    {
        initialize();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //difficulty += (Time.time - startTime) / 100f;
        //print((Time.time - PreviousGenerationTime).ToString() + "    " + spawnCooldown.ToString());
        if (Time.time - PreviousGenerationTime >= spawnCooldown)
        {
            Vector3 ShipPosition = new Vector3(Random.Range(bottomLeft_World.x, topRight_World.x), topRight_World.y * destroyLine, 0);
            Instantiate(randomNormalEnemy(), ShipPosition, Quaternion.Euler(0, 0, 180));//Must re-rotate
            GameObject enemyToInstantiate = randomNormalEnemy();
            PreviousGenerationTime = Time.time;//carefull
            //print(canGenerate);
        }
        if (bossCoroutineStarted == false)
        {
            bossExist = StartCoroutine(BossSpawner());
        }


	}

    void initialize()
    {
        bottomLeft_World = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        topRight_World = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        loadEnemyPrefabs();

        maxSpawnCooldown = perNSecond/enemyMaxRate;
        minSpawnCooldown = perNSecond/enemyMinRate;

        spawnCooldown = randomCoolDownTime(minSpawnCooldown, maxSpawnCooldown);
        //Debug.Log(normalEnemyPrefabs.Count);

        startTime = Time.time;
    }

   

    void loadEnemyPrefabs(string PathAndNamePrefix = "Prefabs/Ship/Enemy Ship/Enemy Ship ")
    {
        for (int i = 0; i < normalEnemyPrefabs.Count; i++)//EX: i == 2 => Load "Enemy Ship 2"
        {
            normalEnemyPrefabs[i] = Resources.Load<GameObject>(PathAndNamePrefix + i.ToString());
        }
    }

    GameObject randomNormalEnemy()
    {
        if(normalEnemyPrefabs.Count == 0)
        {
            Debug.LogError("Must have at least an enemy prefab to instantiate!");//MUST ADD A PREFAB TO THE LIST
            return null;
        }
        //Debug.Log("Randoming");
        return normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Count)];
    }


    GameObject randomBoss()
    {
        if (bossPrefabs.Count == 0)
        {
            Debug.LogError("Must have at least an enemy prefab to instantiate!");//MUST ADD A PREFAB TO THE LIST
            return null;
        }
        //Debug.Log("Randoming");
        return bossPrefabs[Random.Range(0, bossPrefabs.Count)];
    }


    float randomCoolDownTime(float min, float max)
    {
        //print(Random.Range(min, max));
        return Random.Range(min, max);
    }
    
    IEnumerator BossSpawner()
    {
        bossCoroutineStarted = true;
        yield return new WaitForSeconds(bossSpawnTime);//wait
        Vector3 ShipPosition = new Vector3(0, topRight_World.y * destroyLine, 0);
        print("Gen boss");
        Instantiate(randomBoss(), ShipPosition, Quaternion.Euler(0, 0, 0));
        yield return null;
    }

    //IEnumerator callASkewWave(GameObject shipPrefab, int shipNumber, float waveTime, float x1)
    //{
    //    //Debug.Log("Into callASkewWave coroutin");
    //    canGenerate = false;

    //    float previousSpawnTime = 0f;
    //    float cooldown = waveTime / shipNumber;

    //    int spawedShip = 0;

    //    float previousSpawnPosition = x1;
    //    float difPos = shipPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
    //    float XPos = x1;


    //    while(spawedShip < shipNumber)
    //    {
    //        if (Time.time - previousSpawnTime >= cooldown)
    //        {
    //            Instantiate(shipPrefab, new Vector3(XPos, topRight_World.y, topRight_World.y * destroyLine), Quaternion.Euler(0, 0, 180));
    //            spawedShip++;

    //            previousSpawnTime = Time.time;
    //            previousSpawnPosition = XPos;
    //        //Debug.Log("instantiate");
    //        }
    //        yield return null;

    //    }
    //    //Debug.Log("Can Generate");
    //    canGenerate = true;
    //}
   
}
