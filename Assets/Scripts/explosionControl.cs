using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionControl : MonoBehaviour
{
    private Rigidbody myRigidBody;

    private Vector3 explodeDirection = Vector3.zero;

    private float explodeSpeed = 200f;

    private float explodeRange = 2f;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, startPosition) >= explodeRange)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        myRigidBody.velocity = explodeDirection * explodeSpeed * Time.deltaTime;
    }

    public void SetExplosion(Vector3 direction, float speed, float range)
    {
        explodeDirection = direction;
        explodeSpeed = speed;
        explodeRange = range;

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Block")
        {
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Bomb")
        {
            other.gameObject.GetComponent<bomb>().Explode();
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().Die();
        }
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().Die();
        }
        if (other.gameObject.tag == "Destructible")
        {
            FindObjectOfType<PowerUpSpawner1>().BlockDestroyed(other.transform.position);

            // Play destroy block animation
            other.gameObject.GetComponent<Animator>().SetTrigger("isDestroyed");

            Destroy(other.gameObject, .5f);
            Destroy(gameObject);
        }
    }

}
