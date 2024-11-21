using UnityEngine;

public class Bone : MonoBehaviour
{
    public float health;
    private Vector3 respawnPoint;

    void Update()
    {
        HealthCheck(health);
    }
    private void HealthCheck(float health)
    {
        if (health <= 0)
        {
            OnDeath();
        }
    }
    private void OnDeath()
    {
        transform.position = respawnPoint;
    }
}
