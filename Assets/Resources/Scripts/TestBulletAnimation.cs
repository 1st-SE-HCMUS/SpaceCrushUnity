using UnityEngine;
using System.Collections;

public class TestBulletAnimation : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GetComponent<Animator>().Play("Bullet_hit_0");
            Destroy(this.gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }


}
