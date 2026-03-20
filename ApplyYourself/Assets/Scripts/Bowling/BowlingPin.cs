using System;
using UnityEngine;

public class BowlingPin : MonoBehaviour
{
    public event Action OnPinKnockedOver;
        
    private HingeJoint joint;
    private string _collisionTag;

    public void Initialize(string collisionTag)
    {
        _collisionTag = collisionTag;
    }

    private void Awake()
    {
        joint = GetComponent<HingeJoint>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(_collisionTag) && joint.useSpring)
        {
            joint.useSpring = false;
            OnPinKnockedOver?.Invoke();
        }
    }

    public void ResetPin()
    {
        joint.useSpring = true;
    }
}
