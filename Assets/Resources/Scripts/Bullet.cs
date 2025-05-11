using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CORE MUST HAVE Collider2D Component in Bullet GO
public class Bullet : MonoBehaviour
{
    public GameObject BoomEffect;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Block")
        {
            Debug.Log("hit Wall");
            //MARKER Boom Effect
            Instantiate(BoomEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if(other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(other.collider.gameObject.name);

            Instantiate(BoomEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);

            ITakenDamage damable = other.gameObject.GetComponent<ITakenDamage>();
            damable.TakenDamage(10);
        }
    }
}
