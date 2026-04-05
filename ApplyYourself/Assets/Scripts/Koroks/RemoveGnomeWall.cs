using UnityEngine;

public class RemoveGnomeWall : KoroksManager
{
    [SerializeField] private GameObject gnomeWall;

    public override void StartCheering()
    {
        base.StartCheering();

        if ( gnomeWall!= null)
        {
            gnomeWall.SetActive(false);
        }
    }
}
