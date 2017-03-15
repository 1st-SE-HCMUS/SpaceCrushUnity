using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.UI;


public class PlayingController_Script : MonoBehaviour {

    static int playerScore = 0;
    public static GameObject playerShip;
    public Text scoreText;
    public GameObject Health_Bar;
    public GameObject GameoverPanel;
    public GameObject PauseButton;

    bool saved = false;
    // Use this for initialization
	void Start ()
    {
        
        GameoverPanel.SetActive(false);
        loadPlayerData();
        Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update ()
    { 
        //Update score on screen
        scoreText.text = "Score: " + playerScore.ToString();//local
        //Update player Health Bar
        if (playerShip != null)//Player still alive
        {
            Health_Bar.transform.localScale = new Vector3(playerShip.GetComponent<Player_Ship>().Curr_Health / playerShip.GetComponent<Player_Ship>().Max_Health, Health_Bar.transform.localScale.y, Health_Bar.transform.localScale.z);
        }
        else //Player dead => GameOver Panel
        {
            if(!saved)//save only one time
            {
                MainController_Script.userData.addScore(playerScore);
                MainController_Script.userData.addGold(playerScore);
                MainController_Script.saveData();
                saved = true;
            }
            GameoverPanel.SetActive(true);
            PauseButton.SetActive(false);
        }
       
        

	}

    public static void AddScore(int num)
    {
        playerScore += num;
    }
    
    int loadPlayerData()
    {
        
        string shipName = MainController_Script.userData.equiped.ship;
        MainController_Script.UserData.Bullet bullet = MainController_Script.userData.equiped.bullet;
       
        playerShip = (GameObject)Instantiate(Resources.Load("Prefabs/Ship/Player Ship/" + shipName));
        playerShip.GetComponent<Player_Ship>().Bullet = Resources.Load<GameObject>("Prefabs/bullet/" + bullet.name);
        if (bullet.sound == "" || bullet.sound == null)//bullet sound was not seted => set default sound
        {
            playerShip.GetComponent<Player_Ship>().ShootSound = (AudioClip)Resources.Load("Sounds/fire 0");
        }
        else
        {
            playerShip.GetComponent<Player_Ship>().ShootSound = (AudioClip)Resources.Load("Sounds/" + bullet.sound);
        }
        Debug.Log("Player ship name: " + shipName);
        return 0;
    }
}
