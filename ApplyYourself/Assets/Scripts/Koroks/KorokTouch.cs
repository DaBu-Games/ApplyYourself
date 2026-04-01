using UnityEngine;

public class KorokTouch : KoroksManager
{
    private SphereCollider coll;

    public override void Start()
    {
        base.Start();

        coll = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            base.StartCheeringSequence();
        }
    }
}
