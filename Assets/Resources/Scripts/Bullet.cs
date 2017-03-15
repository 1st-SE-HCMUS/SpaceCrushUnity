using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    //Public
    public enum BULLET_TYPE
    {
        Player,
        Enemy,
    }

    public BULLET_TYPE Type = BULLET_TYPE.Player;
    public bool autoTaget = false;
    public float Speed = 35;
    public float Damage = 10;
    //Private
    Vector3 bottomLeft_World;
    Vector3 topRight_World;
    Vector3 direction;
    Vector3 taggetPoint;

    void Start()
    {
        initialize();
        
        if(Type == BULLET_TYPE.Player)
        {
            Speed += 10;
        }
        else
        {
            if (PlayingController_Script.playerShip != null)
            {
                direction = PlayingController_Script.playerShip.transform.position - this.transform.position;
                float x = this.transform.position.x + direction.x * 2;
                float y = this.transform.position.y + direction.y * 2;
                taggetPoint = new Vector3(x, y, 0);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

	// Update is called once per frame
	void Update () 
    {
        
        if(Type == BULLET_TYPE.Player)
        {
            if (!autoTaget)
            {
                transform.Translate(new Vector3(0, Speed * Time.deltaTime, 0), Space.World);
            }
        }
        else
        {
            if(!autoTaget)
            {
                transform.Translate(new Vector3(0, -Speed * Time.deltaTime, 0), Space.World);
            }
            else
            {
                
                float x = this.transform.position.x + direction.x * 2;
                float y = this.transform.position.y + direction.y * 2;
                taggetPoint = new Vector3(x, y, 0);
                taggetFire(taggetPoint);
            }

        }

        if(OutScreen())
        {
            Destroy(this.gameObject);
        }
	}

    public void taggetFire(Vector3 direction)
    {
        transform.position = Vector3.MoveTowards(this.transform.position, direction, Speed * Time.deltaTime);
    }

    bool OutScreen(float Height_Ratio = 1.25f, float Width_Ratio = 1.25f) //checking if bullet fly out 1.25 Screen
    {
        if (transform.position.y > topRight_World.y * Height_Ratio || transform.position.y < bottomLeft_World.y * Height_Ratio)
        {
            return true;
        }

        return false;
    }

    void OnTriggerEnter2D(Collider2D coll)//Apply damage
    {
        if (coll.tag == "Enemy" || coll.tag == "Player" || coll.tag == "Shield")
        {
            switch (coll.tag)
            {

                case "Enemy":
                    if(Type == BULLET_TYPE.Player)
                    {
                        coll.GetComponent<Enemy_Ship>().ApplyDamage(Damage);
                        Destroy(this.gameObject);
                    }                 
                    break;
                case "Player":
                    if (Type == BULLET_TYPE.Enemy)
                    {
                        coll.GetComponent<Player_Ship>().ApplyDamage(Damage);
                        Destroy(this.gameObject);
                    }
                    break;
                case "Shield":
                    if(Type == BULLET_TYPE.Enemy)
                    {
                        Destroy(this.gameObject);
                    }                 
                    break;
            }
        }
        
    }

    public void initialize()
    {
        //set bullet tag
        if (Type == BULLET_TYPE.Player)
        {
            gameObject.tag = "Player Bullet";
        }
        else
        {
            gameObject.tag = "Enemy Bullet";
        }


        //initialize necessary components
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

        bottomLeft_World = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        topRight_World = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
    }
}
