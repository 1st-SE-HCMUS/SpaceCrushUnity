using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
public class MainController_Script : MonoBehaviour {
    public class UserData
    {
        public UserData()
        {
            id = 0;
            name = "";
            highscore = new List<int>();
            inventory = new Inventory();
            equiped = new Equiped();
        }
        //"id": 0,
        //"name": "Ice Tea",
        //"highscore": [
        //    140000,
        //    5000
        //],
        //"inventory": {
        //    "gold": 0,
        //    "ship": ["Player_Ship 0"],
        //    "bullet": "Bullet_0",
        //    "item": []
        //},
        //"equiped": {
        //    "ship": "Player_Ship 0",
        //    "bullet": "Bullet_0",
        //    "item": [
        //        "gold boost",
        //        "atk speed boost"
        //    ]
        //}
        public int id;
        public string name;
        public List<int> highscore;
        public Inventory inventory;
        public Equiped equiped;

        public class Inventory
        {
            public Inventory()
            {
                gold = 0;
                ship = new List<string>();
                ship.Add("Player Ship 0");
                bullet = new List<Bullet>();
                bullet.Add(new Bullet());
                item = new List<string>();
            }
            public int gold = 0;
            public List<string> ship;
            public List<Bullet> bullet ;
            public List<string> item;

            
        }
        public class Equiped
        {
            public Equiped()
            {
                ship = "Player Ship 0";
                bullet = new Bullet();
                item = new List<string>();
            }
            public string ship;
            public Bullet bullet;
            public List<string> item;

        }
        public class Bullet
        {
            public Bullet()
            {
                name = "Bullet 0";
                sound = null;
            }
            public string name;
            public string sound;
        }

        public void addGold(int value)
        {
            if(value > 0)
            {
                inventory.gold += value;
            }
        }
        public int addScore(int value)//Return rank in high score
        {
            if (value == 0)
                return -1;
            highscore.Add(value);
            highscore.Sort((a,b) => -a.CompareTo(b));
            if(highscore.Count > 10)
            {
                highscore.RemoveAt(highscore.Count - 1);
            }
            return highscore.IndexOf(value);//return -1 if value not found
        }
        
    }

    public static UserData userData = new UserData(); 
    static string dataPath;


    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);

        //Load user data
        dataPath = Application.dataPath + "/Resources/data/userdata.json";

        if (!loadUserData(ref userData, dataPath))
        {
            Debug.Log("User data not found!");//popup
            Debug.Log("Creating defaul data!");
            saveData();
            Debug.Log("Created!");
        }
        else
        {
            Debug.Log("User data loaded!");
        }
    }

	// Use this for initialization
	void Start () 
    {

        Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    bool loadUserData(ref UserData userData, string dataPath)//false if file not found
    {
        if (!File.Exists(dataPath))
        {
            //Debug.Log("Data missing!");
            return false;
        }

        string dataString = File.ReadAllText(dataPath);

        userData = JsonConvert.DeserializeObject<UserData>(dataString);

        
        return true;
    }

    public static bool saveData()
    {
        File.WriteAllText(dataPath, JsonConvert.SerializeObject(userData,Formatting.Indented));

        return true;
    }

}
