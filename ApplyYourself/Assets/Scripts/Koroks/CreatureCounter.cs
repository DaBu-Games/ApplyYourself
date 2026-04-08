
using UnityEngine;
using TMPro;
using System.Collections;

public class CreatureCounter : MonoBehaviour
{
    public static CreatureCounter Instance { get; private set; }

    [SerializeField] private int creatureCount = 0;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    [SerializeField] private Animator animator;


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
        textMeshPro.text = "x " + creatureCount.ToString();

    }


    public void AddCreature()
    {
        creatureCount++;
        textMeshPro.text = "x" + creatureCount.ToString();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("CreatureGained") )
        {
            animator.SetTrigger("Reset");
        }
        
        animator.SetTrigger("CreatureUp");
    }


    


}
