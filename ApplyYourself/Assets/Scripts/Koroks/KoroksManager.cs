using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KoroksManager : MonoBehaviour
{
    [Header("Cheering Settings")]
    [SerializeField] private float cheerDuration = 3f;
    
    [Header("Bob Animation Settings")]
    [SerializeField] private float bobHeight = 0.5f;
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 2f;
    
    private List<KorokEntity> koroks = new();

    [SerializeField] private GameObject invisibleWall;

    private void Start()
    {
        GetKoroks();
        InitializeKoroks();
    }

    private void GetKoroks()
    {
        koroks = GetComponentsInChildren<KorokEntity>().ToList();
    }

    private void InitializeKoroks()
    {
        for (int i = 0; i < koroks.Count; i++)
        {
            float individualSpeed = Random.Range(minSpeed, maxSpeed);
            koroks[i].Initialize(individualSpeed, bobHeight);
        }
    }
    
    public void StartCheeringSequence()
    {
        StartCoroutine(CheeringSequence());
    }

    
    private IEnumerator CheeringSequence()
    {
        StartCheering();
        
        yield return new WaitForSeconds(cheerDuration);
        
        StopCheering();
    }

    private void StartCheering()
    {
        if (invisibleWall != null)
        {
            invisibleWall.SetActive(false);
        }

        foreach (var korok in koroks)
        {
            korok.SetEmotion(KorokEmotion.Excited);
            korok.StartCheering();
        }
    }

    private void StopCheering()
    {
        foreach (var korok in koroks)
        {
            korok.SetEmotion(KorokEmotion.Happy);
            korok.StopCheering();
        }
    }
}
