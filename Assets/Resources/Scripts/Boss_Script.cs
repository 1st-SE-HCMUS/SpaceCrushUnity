using UnityEngine;
using System.Collections;

public class Boss_Script : Enemy_Ship {

    public Vector2[] SpecialBarrelPosition;
    public GameObject specialBullet;
    float lastSpecialShootTime = 0;
	int direction = 1;//move to right first
    bool stop = false;
    bool stopNormalFire = true;
    bool stopSpecialFire = true;
    Coroutine MoveCoroutine;
    Coroutine normalFireCoroutine;
    Coroutine specialFireCoroutine;
    void Start()
    {
        initialize();
        print(topRight_World/2f);
        lastSpecialShootTime = Time.time;
        if (Random.value > 0.5)
        {
            direction *= -1;//move to left first
        }

        if(specialBullet == null)
        {
            specialBullet = bullet;
        }

    }
	
	// Update is called once per frame
	void Update () 
    {
        Move(ref direction);

        Fire();

        healthBar.transform.GetChild(0).transform.localScale = new Vector3(Curr_Health / Max_Health, healthBar.transform.GetChild(0).transform.localScale.y, healthBar.transform.GetChild(0).transform.localScale.z);
        
        if (Curr_Health <= 0)
        {
            PlayingController_Script.AddScore(score);// Add Score
            if (Random.value < itemRate)
            {
                Instantiate(ShieldIcon, this.transform.position, Quaternion.Euler(0, 0, 0));
            }
            Destroy(this.gameObject);
        }
        
	}
    void OnDestroy()
    {
        if (Curr_Health == 0)
        {
            GameObject.Find("Main Camera").GetComponent<Enemy_Generator_Script>().bossCoroutineStarted = false;
            GameObject ExposionSound = (GameObject)Instantiate(new GameObject(), this.transform.position, Quaternion.Euler(0, 0, 0));
            ExposionSound.AddComponent<AudioSource>();
            int i = 1;
            if (Random.value > 0.5)
            {
                i = 2;
            }
            AudioClip SoundClip = (AudioClip)Resources.Load("Sounds/exlosion " + i.ToString());
            ExposionSound.GetComponent<AudioSource>().PlayOneShot(SoundClip);
            Destroy(ExposionSound, 2f);
            GameObject Explosion = (GameObject)Instantiate(Resources.Load("Prefabs/Explosion 0"), this.transform.position, Quaternion.Euler(0, 0, 0));
            Explosion.transform.localScale = new Vector3(20,20,0);
            Destroy(Explosion, 0.25f);

        }
    }
    void Fire()
    {
        if (normalFireCoroutine == null)
        {
            normalFireCoroutine = StartCoroutine(NormalFireCoroutine(Random.Range(3, 5)));
        }
        if (specialFireCoroutine == null)
        {
            specialFireCoroutine = StartCoroutine(SpecialFireCoroutine(Random.Range(2, 4)));
        }


        if(stopNormalFire == false)//can fire
        {
            normalFire(ref lastShootTime);
        }


        if (stopSpecialFire == false)
        {
            if (Time.time - lastSpecialShootTime >= FireRate)
            {
                GameObject bulletAddress;
                // Debug.Log("Fire!");
                for (int i = 0; i < SpecialBarrelPosition.Length; i++)
                {
                    bulletAddress = (GameObject)Instantiate(specialBullet, transform.position + new Vector3(SpecialBarrelPosition[i].x, SpecialBarrelPosition[i].y, 0), Quaternion.Euler(0, 0, 90));
                    bulletAddress.GetComponent<Bullet>().autoTaget = true;
                    //GetComponent<AudioSource>().PlayOneShot(ShootSound);
                }
                lastSpecialShootTime = Time.time;

            }
        }
        
    }
    
    void Move(ref int direction)//-1 left; 1 right
    {
        
        if (this.transform.position.x <= bottomLeft_World.x)
        {
            direction = 1;
        }
        if (this.transform.position.x >= topRight_World.x)
        {
            direction = -1;
        }

        if(MoveCoroutine == null)
        {
            MoveCoroutine = StartCoroutine(StopAndChangeDirection(Random.Range(3, 5)));
        }

        float y = 0;
        if(transform.position.y >= 1.9*topRight_World.y/3f)
        {
            y = -Speed * Time.deltaTime;
        }
        Vector3 moveDestination = new Vector3(direction * Speed * Time.deltaTime, y, 0);
        if(stop == false)
        {
            this.transform.Translate(moveDestination);
        }
    }

    IEnumerator StopAndChangeDirection(float stopTime)
    {
        yield return new WaitForSeconds(Random.Range(3, 7));
        stop = true;
        yield return new WaitForSeconds(stopTime);
        stop = false;
        if (Random.value > 0.5)
        {
            direction *= -1;
        }

        MoveCoroutine = null;
        yield break;
    }
    IEnumerator NormalFireCoroutine(float fireTime)
    {
        print("stop");
        yield return new WaitForSeconds(Random.Range(4, 5));
        stopNormalFire = false;
        print("fire");
        yield return new WaitForSeconds(fireTime);
        stopNormalFire = true;

        normalFireCoroutine = null;
        yield break;
    }

    IEnumerator SpecialFireCoroutine(float fireTime)
    {
        yield return new WaitForSeconds(Random.Range(3, 6));//stop time
        stopSpecialFire = false;
        yield return new WaitForSeconds(fireTime);
        stopSpecialFire = true;

        specialFireCoroutine = null;
        yield break;
    }

}
