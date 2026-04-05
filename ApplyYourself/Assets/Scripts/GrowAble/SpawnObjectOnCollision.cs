using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class SpawnObjectOnCollision : MonoBehaviour
{
    [SerializeField]private BaseGrowable growable;
    [SerializeField]private GameObject objectToSpawn;
    [SerializeField]private int maxSpawnCount;
    [SerializeField]private Transform ballSpawn;

    private List<GameObject> spawnedObjects = new();
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        UpdateHitBox();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && growable.HasGrown)
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        if (spawnedObjects.Count >= maxSpawnCount)
        {
            Destroy(spawnedObjects[0]);
            spawnedObjects.RemoveAt(0);
        }
        Vector3 spawnPosition = ballSpawn.position;
        
        GameObject obj = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(obj);
    }
    
    private void OnEnable()
    {
        growable.OnGrowthChanged += UpdateHitBox;
    }

    private void OnDisable()
    {
        growable.OnGrowthChanged -= UpdateHitBox;
    }

    private void UpdateHitBox()
    {
        boxCollider.enabled = growable.HasGrown;

        if (growable.HasGrown)
            SpawnObject();
    }
}
