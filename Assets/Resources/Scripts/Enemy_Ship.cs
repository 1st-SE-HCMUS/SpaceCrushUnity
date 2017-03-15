using UnityEngine;
using System.Collections;


public class Enemy_Ship : MonoBehaviour {
    public int score = 50;
    public float Speed = 20;
    public float Damage = 40;
    public float Max_Health = 100;
    public float Curr_Health;
    public GameObject healthBar;
    public float healthBarYPos = -0.8f;
    public GameObject bullet;

    public Vector2[] NormalBarrelPosition;
    public float FireRate = 0.6f;

    public GameObject ShieldIcon;
    public float itemRate = 0.2f;
    protected Vector3 bottomLeft_World;
    protected Vector3 topRight_World;

    protected float lastShootTime;
    string defaultPathOfHealthBar = "Prefabs/Health Bar/HealthBar 0";
    int shootType = 0;
	// Use this for initialization
	void Start () 
    {
        initialize();
	}
	
	// Update is called once per frame
	void Update () 
    {

        transform.Translate(0, Speed * Time.deltaTime, 0);//Local
        if (OutScreen())
        {
            Destroy(this.gameObject);
        }

        if (Curr_Health <= 0)
        {
            PlayingController_Script.AddScore(score);// Add Score
            if(Random.value < itemRate)
            {
                Instantiate(ShieldIcon, this.transform.position, Quaternion.Euler(0, 0, 0));
            }
            Destroy(this.gameObject);
        }

        if(shootType == 0)
        {
            normalFire(ref lastShootTime);
        }
        else
        {
            taggetFire(ref lastShootTime);
        }
        
        healthBar.transform.GetChild(0).transform.localScale = new Vector3(Curr_Health / Max_Health, healthBar.transform.GetChild(0).transform.localScale.y, healthBar.transform.GetChild(0).transform.localScale.z);
        //Debug.Log();
	}
    
    void OnDestroy()
    {
        if(Curr_Health == 0)
        {
            GameObject ExposionSound = (GameObject)Instantiate(new GameObject(), this.transform.position, Quaternion.Euler(0, 0, 0));
            ExposionSound.AddComponent<AudioSource>();
            AudioClip SoundClip = (AudioClip)Resources.Load("Sounds/exlosion 3");
            ExposionSound.GetComponent<AudioSource>().PlayOneShot(SoundClip);
            Destroy(ExposionSound, 2f);
            GameObject Explosion = (GameObject)Instantiate(Resources.Load("Prefabs/Explosion 0"), this.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(Explosion, 0.25f);

        }
    }
    bool OutScreen(float Height_Ratio = 1.25f, float Width_Ratio = 1.25f) //checking if Enemy fly out 1.25 Screen
    {
        if (transform.position.y > topRight_World.y * Height_Ratio || transform.position.y < bottomLeft_World.y * Height_Ratio)
        {
            return true;
        }

        return false;
    }

    protected void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Player")
        {
            if(coll.GetComponent<Player_Ship>().Shield.active == false)
            {
                coll.GetComponent<Player_Ship>().ApplyDamage(Damage);
                ApplyDamage(50);
            }
        }
        if(coll.tag == "Shield")
        {
            Curr_Health = 0;
        }

    }

    public void ApplyDamage(float damage)
    {
        Curr_Health -= damage;
        Curr_Health = Mathf.Clamp(Curr_Health, 0, Max_Health);
    }

    public void initialize()
    {
        //Auto set Tag
        this.gameObject.tag = "Enemy";


        //Set bullet sprite
        if (bullet == null)
            bullet = Resources.Load<GameObject>("Prefabs/bullet/Enemy Bullet 0");

        //if not attach healthBar => load default;
        if (healthBar == null)
        {
            healthBar = Instantiate(Resources.Load<GameObject>(defaultPathOfHealthBar));
            healthBar.transform.SetParent(this.gameObject.transform);
            healthBar.transform.localPosition = new Vector3(0, healthBarYPos, 0);
            healthBar.transform.localScale = new Vector3(1, 1, 1);
        }



        bottomLeft_World = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        topRight_World = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));


        //Add rigidbody
        Rigidbody2D Rigid = GetComponent<Rigidbody2D>();
        if (Rigid == null)
        {
            Rigid = this.gameObject.AddComponent<Rigidbody2D>();
        }
        Rigid.isKinematic = true;


        //Add collider
        if (GetComponent<Collider2D>() == null)
        {
            this.gameObject.AddComponent<PolygonCollider2D>();
        }
        GetComponent<Collider2D>().isTrigger = true;

        //load shield icon
        if (ShieldIcon == null)
        {
            ShieldIcon = Resources.Load<GameObject>("Prefabs/Playing Item/Shield Icon");
        }

        Curr_Health = Max_Health;

        lastShootTime = Time.time;
        if(Random.value > 0.75)
        {
            shootType = 1;
        }
    }

    protected void normalFire(ref float lastShootTime)
    {
        //Debug.Log("Into fire function!");
        //if (Input.GetKey(KeyCode.Space))
        {
            //Debug.Log("Get fire key!");
            //print(Time.time - lastShootTime);

            if (Time.time - lastShootTime >= FireRate)
            {
                // Debug.Log("Fire!");
                for (int i = 0; i < NormalBarrelPosition.Length; i++)
                {
                    lastShootTime = Time.time;
                    Instantiate(bullet, transform.position + new Vector3(NormalBarrelPosition[i].x, NormalBarrelPosition[i].y, 0), Quaternion.Euler(0, 0, 90));

                    //GetComponent<AudioSource>().PlayOneShot(ShootSound);
                }

            }
        }
    }

    protected void taggetFire(ref float lastShootTime)
    {
        {
            //print(Time.time - lastShootTime);

            if (Time.time - lastShootTime >= FireRate)
            {
                GameObject bulletAddress;
                // Debug.Log("Fire!");
                for (int i = 0; i < NormalBarrelPosition.Length; i++)
                {
                    lastShootTime = Time.time;
                    bulletAddress = (GameObject)Instantiate(bullet, transform.position + new Vector3(NormalBarrelPosition[i].x, NormalBarrelPosition[i].y, 0), Quaternion.Euler(0, 0, 90));
                    bulletAddress.GetComponent<Bullet>().autoTaget = true;
                    //GetComponent<AudioSource>().PlayOneShot(ShootSound);
                }

            }
        }
    }
}
