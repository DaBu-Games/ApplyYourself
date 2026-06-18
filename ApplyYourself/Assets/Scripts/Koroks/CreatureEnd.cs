using System;
using UnityEngine;

public class CreatureEnd : MonoBehaviour
{
    [SerializeField] private CreatureCounter creatureCounter;
    [SerializeField] private KoroksManager koroksManager;

    private bool activated = false;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
           
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated)
        {            
            activated = true;
            SpawnCreatures();
        }
    }




    void SpawnCreatures()
    {
        for (int i = 0; i < creatureCounter.creatureCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        koroksManager.StartCheeringSequence();
    }
}
