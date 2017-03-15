using UnityEngine;
using System.Collections;


public class Player_Ship : MonoBehaviour 
{
    public float Speed = 30;
    public float FireRate = 0.3f;
    public GameObject Bullet;
    public GameObject Missile;
    public GameObject Shield;
    public Vector2[] BarrelPosition;


    public float Max_Health = 100;
    public float Curr_Health;
    public AudioClip ShootSound;

    int MissileAmount;
    float lastShootTime;
    Coroutine shieldCoroutine = null;
    Vector2 MoveDirection;
    Vector3 bottomLeft_World; // coordinate of the Bottom-Left of camera in World coordinate
    Vector3 topRight_World;   // coordinate of the Top-Right of camera in World coordinate
	// Use this for initialization
	void Start () 
    {
        initialize();
	}
	
	// Update is called once per frame
	void Update () 
    {
        
        MoveControl();
        Fire(ref lastShootTime);
        
         if(Curr_Health <= 0)
        {
            Destroy(this.gameObject);
        }
	}

    void MoveControl()
    {
        MoveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        MoveDirection *= Speed * Time.deltaTime;
        transform.Translate(MoveDirection.x, MoveDirection.y, 0); // di chuyen 1 khoang theo thoi gian
        ClampToScreen();
    }

    void Fire(ref float lastShootTime)
    {
        //Debug.Log("Into fire function!");
        if(Input.GetKey(KeyCode.Space) )
        {
            //Debug.Log("Get fire key!");
            //print(Time.time - lastShootTime);

            if (Time.time - lastShootTime >= FireRate)
            {
               // Debug.Log("Fire!");
                for (int i = 0; i < BarrelPosition.Length; i++)
                {
                    lastShootTime = Time.time;
                    Instantiate(Bullet, transform.position + new Vector3(BarrelPosition[i].x, BarrelPosition[i].y, 0), Quaternion.Euler(0, 0, -90));
                    
                    GetComponent<AudioSource>().PlayOneShot(ShootSound);
                }

            }
        }
    }

    void ClampToScreen()
    {
        Vector3 NewPosition = new Vector3(0, 0, transform.position.z);
        NewPosition.x = Mathf.Clamp(transform.position.x, bottomLeft_World.x + GetComponent<SpriteRenderer>().bounds.extents.x, topRight_World.x - GetComponent<SpriteRenderer>().bounds.extents.x);
        NewPosition.y = Mathf.Clamp(transform.position.y, bottomLeft_World.y + GetComponent<SpriteRenderer>().bounds.extents.y, topRight_World.y - GetComponent<SpriteRenderer>().bounds.extents.y);
        
        transform.position = NewPosition;

    }

    public void ApplyDamage(float damage)
    {
        Curr_Health -= damage;
        Curr_Health = Mathf.Clamp(Curr_Health, 0, Max_Health);
    }

    public void initialize()
    {
        this.gameObject.tag = "Player";
        MoveDirection = new Vector2(0, 0);
        lastShootTime = Time.time;


        //set necessary components
        if (GetComponent<AudioSource>() == null)
        {
            this.gameObject.AddComponent<AudioSource>();
        }
        GetComponent<AudioSource>().volume = 0.5f;


        if (GetComponent<Rigidbody2D>() == null)
        {
            this.gameObject.AddComponent<Rigidbody2D>();
        }
        GetComponent<Rigidbody2D>().isKinematic = true;
        if (GetComponent<Collider2D>() == null)
        {
            this.gameObject.AddComponent<PolygonCollider2D>();
        }
        GetComponent<Collider2D>().isTrigger = true;


        //intantiate shield
        if (Shield == null)
        {
            Shield = Resources.Load<GameObject>("Prefabs/Playing Item/Shield");
        }

        Shield = Instantiate(Shield);
        Shield.transform.SetParent(this.gameObject.transform, false);
        Shield.SetActive(false);




        //ShootSound = (AudioClip)Resources.Load("Audios/fire");
        Curr_Health = Max_Health;

        bottomLeft_World = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        topRight_World = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0));
                
        //Set position
        this.gameObject.transform.position = new Vector3(0, bottomLeft_World.y / 2, 0);
            
        //load items
        loadItem();
    }

    void loadItem()
    {
        MainController_Script.UserData userData = MainController_Script.userData;

        //load "atk speed boost (x1.2)"
        if (userData.equiped.item.IndexOf("atk speed boost (x1.5)") != -1) //exist
        {
            FireRate /= 1.5f;
            userData.equiped.item.Remove("atk speed boost (x1.5)");
            userData.inventory.item.Remove("atk speed boost (x1.5)");
            MainController_Script.saveData();
        }

        
    }

    public void activeShield()
    {
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(activateShield(5));
    }

    IEnumerator activateShield(float time)
    {
        Shield.SetActive(true);
        yield return new WaitForSeconds(time);
        Shield.SetActive(false);
    }
}
