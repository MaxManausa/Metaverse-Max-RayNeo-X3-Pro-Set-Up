using UnityEngine;

public class BlasterBolt : MonoBehaviour
{
    public float speed = 40f;
    public float damage = 1f;
    public float lifetime = 4f;

    void Start()
    {
        // Cleanup if it misses everything
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move forward based on where LookAt() pointed us in AsteroidManager
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check for Planet targets
        if (other.CompareTag("Earth") || other.CompareTag("Moon") || other.CompareTag("TravelTarget"))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddEarthHit(damage);
            }

            // Optional: Spawn a tiny impact effect here if you have one
            Destroy(gameObject);
        }
    }
}