using UnityEngine;
using System.Collections;

public class ItemWhilePlaying_Script : MonoBehaviour
{

	// Use this for initialization
	void Start () 
    {
	    if(GetComponent<Rigidbody2D>() == null)
        {
            this.gameObject.AddComponent<Rigidbody2D>();
        }
        GetComponent<Rigidbody2D>().isKinematic = false;
        if(GetComponent<Collider2D>() == null)
        {
            this.gameObject.AddComponent<PolygonCollider2D>();
        }
        GetComponent<Collider2D>().isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
        	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Player")
        {
            coll.GetComponent<Player_Ship>().activeShield();
            PlayingController_Script.AddScore(100);
            Destroy(this.gameObject);
        }
    }
}
