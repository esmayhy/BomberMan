using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float MoveSpeed = 0f;

    Rigidbody myRigidbody;
    [SerializeField] GameObject bombPrefab;

    private GameManager1 myGameManager;

    private int maxBombs = 1;
    private int currentBombsPlaced = 0;

    private bool hasControl = true;
    [SerializeField] private float destroyTime = 2f;

    private bool isPaused = false;
    private bool isDead = false;

    [SerializeField] private LayerMask whatAreBombLayers;

    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();
        myGameManager = FindObjectOfType<GameManager1>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasControl && !isPaused)
        {
            Movement();
            UpdateAnimator();
            PlaceBomb();
        }
    }

    private void Movement()
    {
        Vector3 newVelocity = new Vector3();

        if (Input.GetKey(KeyCode.UpArrow))
        {
            newVelocity += new Vector3(0f, 0f, MoveSpeed);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            newVelocity += new Vector3(0f, 0f, -MoveSpeed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            newVelocity += new Vector3(-MoveSpeed, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            newVelocity += new Vector3(MoveSpeed, 0f, 0f);
        }
        myRigidbody.velocity = newVelocity;
        //transform.position = transform.position + (newPosition * Time.deltaTime);
    }
    private void PlaceBomb()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (currentBombsPlaced < maxBombs))
        {
            Vector3 center = new Vector3((transform.position.x), 17f, (transform.position.z));
            Collider[] hitColliders = Physics.OverlapSphere(center, 0.5f, whatAreBombLayers);
            if (hitColliders.Length > 0)
            {
                return;
            }

            GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            bomb.transform.position = center;
            currentBombsPlaced++;
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;

            hasControl = false;

            myRigidbody.velocity = Vector3.zero;

            myRigidbody.isKinematic = true;

            Destroy(gameObject, destroyTime);

            myGameManager.PlayerDied();

            myAnimator.SetBool("isDead", true);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Die();
        }

    }

    public void BombExploded()
    {
        currentBombsPlaced--;
    }
    public float GetDestroyDelayTime()
    {
        return destroyTime;
    }

    public void InitializePlayer(int bombs, float speed)
    {
        maxBombs = bombs;
        MoveSpeed = speed;
    }
    public void SetPaused(bool state)
    {
        isPaused = state;
    }

    private void UpdateAnimator()
    {
        if (myRigidbody.velocity == Vector3.zero)
        {
            myAnimator.SetBool("isWalking", false);
        }
        else
        {
            myAnimator.SetBool("isWalking", true);
        }
    }

    public void PlayVictory()
    {
        hasControl = false;
        myAnimator.SetBool("isVictory", true);
    }

}
