using UnityEngine;

public class GoalHitBox : MonoBehaviour
{
    [SerializeField] private KoroksManager koroksManager;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private string collisionTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(collisionTag))
        {
            koroksManager.StartCheeringSequence();
            particle.Play();
        }
    }
}
