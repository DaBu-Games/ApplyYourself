using System;
using UnityEngine;

public class SwitchModel : MonoBehaviour
{
    [SerializeField]private BaseGrowable growable;
    [SerializeField]private GameObject unGrownObject;
    [SerializeField]private GameObject grownObject;

    private void Awake()
    {
        UpdateModel();
    }

    private void OnEnable()
    {
        growable.OnGrowthChanged += UpdateModel;
    }

    private void OnDisable()
    {
        growable.OnGrowthChanged -= UpdateModel;
    }

    public virtual void UpdateModel()
    {
        if (growable.HasGrown)
        {
            grownObject.SetActive(true);
            unGrownObject.SetActive(false);
        }
        else
        {
            grownObject.SetActive(false);
            unGrownObject.SetActive(true);
        }
    }
}
