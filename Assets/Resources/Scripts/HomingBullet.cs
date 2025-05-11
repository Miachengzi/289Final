using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Uncompleted
[RequireComponent(typeof(Rigidbody2D))]
public class HomingBullet : MonoBehaviour
{
    private Transform target;

    public Rigidbody2D rigidBody;
    public float angleChangingSpeed;
    public float movementSpeed;

    private Rigidbody2D rb;

    private void Start()
    {
        target = FindObjectOfType<FireElement>().transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HomingMissile();
    }

    private void HomingMissile()
    {
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        Debug.Log(direction);
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -angleChangingSpeed * rotateAmount;
        rb.velocity = transform.up * movementSpeed;
        //rb.velocity = direction * movementSpeed;
    }
}
