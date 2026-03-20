using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class SpawnObjectOnCollision : MonoBehaviour
{
    [SerializeField]private BaseGrowable growable;
    [SerializeField]private GameObject objectToSpawn;
    [SerializeField]private int maxSpawnCount;
    [SerializeField]private Vector3 spawnOffset;

    private List<GameObject> spawnedObjects = new();
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && growable.HasGrown)
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
        Vector3 spawnPosition = transform.position + spawnOffset;
        
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
