using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BowlingPinManager : MonoBehaviour
{
    [SerializeField] private float timeAfterReset = 2f;
    [SerializeField] private string collisionTag;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private KoroksManager koroksManager;
    
    private List<BowlingPin> pins = new();
    private int knockOverCount = 0;
    private bool allKnockedOver = false;

    private float lastKnockOverTime = 0f;


    
    private void Start()
    {
        GetAllPins();
    }

    private void Update()
    {
        if(allKnockedOver)
            return;

        if (Time.time - lastKnockOverTime > timeAfterReset)
        {
            ResetPins();
            knockOverCount = 0;
        }
    }
    
    private void GetAllPins()
    {
        pins = GetComponentsInChildren<BowlingPin>().ToList();
        
        foreach (BowlingPin pin in pins)
        {
            pin.OnPinKnockedOver += HandlePinKnockedOver;
            pin.Initialize(collisionTag);
        }
        
        //Debug.Log($"Found {pins.Count} bowling pins");
    }

    private void ResetPins()
    {
        foreach (BowlingPin pin in pins)
        {
            pin.ResetPin();
        }
    }

    private void HandlePinKnockedOver()
    {
        knockOverCount++;
        lastKnockOverTime = Time.time;
        
        if (knockOverCount >= pins.Count)
        {
            allKnockedOver = true;
            particle.Play();
            koroksManager.StartCheeringSequence();
        }
    }
}
