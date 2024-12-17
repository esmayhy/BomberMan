using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    enum PowerUps
    {
        MaxBombs,
        Range,
        Speed
    };

    [SerializeField] PowerUps powerUpType;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<PlayerController>().PlayPowerUpPickupSound();
            switch (powerUpType)
            {
                case PowerUps.MaxBombs:
                {
                    FindObjectOfType<GameManager1>().IncreaseMaxBombs();
                    Destroy(gameObject);
                    break;
                }
                case PowerUps.Range:
                {
                    FindObjectOfType<GameManager1>().IncreaseExplodeRange();
                    Destroy(gameObject);
                    break;
                }
                case PowerUps.Speed:
                {
                    FindObjectOfType<GameManager1>().IncreaseSpeed();
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
