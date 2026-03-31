using UnityEngine;

public class KorokTouch : KoroksManager
{
    private SphereCollider coll;

    public override void Start()
    {
        base.Start();

        coll = GetComponent<SphereCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            base.StartCheeringSequence();
        }
    }
}
