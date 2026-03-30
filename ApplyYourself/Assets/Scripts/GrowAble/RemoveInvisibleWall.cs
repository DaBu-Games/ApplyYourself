using UnityEngine;

public class RemoveInvisibleWall : SwitchModel
{
    [SerializeField] private GameObject invisibleWall;

    private bool avoidOnStart;

    public override void UpdateModel()
    {
        base.UpdateModel();

        if (invisibleWall != null)
        {
            avoidOnStart = !avoidOnStart;
            InvisibleWallActive();
        }
    }

    void InvisibleWallActive()
    {
        invisibleWall.SetActive(avoidOnStart);
    }
}
