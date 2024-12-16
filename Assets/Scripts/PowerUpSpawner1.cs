using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner1 : MonoBehaviour
{
    [SerializeField] GameObject[] powerUpPrefabs;
    

    public void BlockDestroyed(Vector3 pos)
    {
        if(Random.value < 0.25f)
        {
            Instantiate(powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)], pos, Quaternion.identity);
        }
    }
    
}
