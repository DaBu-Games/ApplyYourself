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

    private bool hasCheered;
    
    private List<KorokEntity> koroks = new();


    public virtual void Start()
    {
        GetKoroks();
        InitializeKoroks();
    }

    public virtual void GetKoroks()
    {
        koroks = GetComponentsInChildren<KorokEntity>().ToList();
    }

    public virtual void InitializeKoroks()
    {
        for (int i = 0; i < koroks.Count; i++)
        {
            float individualSpeed = Random.Range(minSpeed, maxSpeed);
            koroks[i].Initialize(individualSpeed, bobHeight);
        }
    }

    public virtual void StartCheeringSequence()
    {
        if (!hasCheered)
        {
            CreatureCounter.Instance.AddCreature();
            hasCheered = true;
        }

        StartCoroutine(CheeringSequence());
    }

    
    public IEnumerator CheeringSequence()
    {
        StartCheering();
        
        yield return new WaitForSeconds(cheerDuration);
        
        StopCheering();
    }

    public virtual void StartCheering()
    {
        foreach (var korok in koroks)
        {
            korok.SetEmotion(KorokEmotion.Excited);
            korok.StartCheering();
        }
    }

    public virtual void StopCheering()
    {
        foreach (var korok in koroks)
        {
            korok.SetEmotion(KorokEmotion.Happy);
            korok.StopCheering();
        }
    }
}
