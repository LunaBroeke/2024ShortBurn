using UnityEngine;

public class Bone : MonoBehaviour
{
    private bool isMoving;
    public float health;
    public float maxHealth;
    [SerializeField] private Vector3 respawnPoint;

    void Update()
    {
        HealthCheck(health);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Bone" && isMoving == true)
        {
            health--;
        }
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
        health = maxHealth;
    }
}
