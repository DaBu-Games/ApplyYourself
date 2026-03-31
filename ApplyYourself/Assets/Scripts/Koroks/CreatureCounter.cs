using UnityEditorInternal;
using UnityEngine;
using TMPro;
using System.Collections;

public class CreatureCounter : MonoBehaviour
{
    public static CreatureCounter Instance { get; private set; }

    [SerializeField] private int creatureCount = 0;
    [SerializeField] private TextMeshProUGUI textMeshPro;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else 
        {
            Instance = this;
        }
               
    }

    private void Start()
    {
        textMeshPro.text = "Happy Creatures " + creatureCount.ToString();

    }


    public void AddCreature()
    {
        creatureCount++;
        textMeshPro.text = "Happy Creatures " + creatureCount.ToString();
    }


    


}
