using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Destructible : MonoBehaviour
{
    [SerializeField] private float destructionTime = 1f;
    [Range(0, 1)] [SerializeField] private float itemSpawnChance = 0.2f;
    [SerializeField] private GameObject[] spawnableItems;

    private void Start()
    {
        Destroy(gameObject, destructionTime);
    }

    private void OnDestroy()
    {
        if (spawnableItems.Length > 0 && Random.value < itemSpawnChance)
        {
            int randomIndex = Random.Range(0, spawnableItems.Length);
            Instantiate(spawnableItems[randomIndex], transform.position, Quaternion.identity);
        }
    }
}