using UnityEngine;

public class EnableObject : MonoBehaviour
{
    [SerializeField]private BaseGrowable growable;
    [SerializeField]private GameObject obj;

    private void Start()
    {
        UpdateObjectVisibility();
    }
    
    private void OnEnable()
    {
        growable.OnGrowthChanged += UpdateObjectVisibility;
    }

    private void OnDisable()
    {
        growable.OnGrowthChanged -= UpdateObjectVisibility;
    }

    private void UpdateObjectVisibility()
    {
        obj.SetActive(growable.HasGrown);
    }
}
