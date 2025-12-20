using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidManager : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionPrefab;

    [Header("Movement Settings")]
    public float speed = 5f;
    public Transform earthTarget; // Drag Earth here or find it by code

    [Header("Rotation Settings")]
    public float rotationSpeedMin = 20f;
    public float rotationSpeedMax = 100f;

    private Vector3 randomRotationAxis;
    private float actualRotationSpeed;

    void Start()
    {
        // 1. Generate a random axis and speed for the "tumbling" effect
        randomRotationAxis = new Vector3(Random.value, Random.value, Random.value);
        actualRotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);

        // 2. Safety: If Earth isn't assigned, try to find it by name
        if (earthTarget == null)
        {
            GameObject earthGO = GameObject.Find("Earth");
            if (earthGO != null) earthTarget = earthGO.transform;
        }
    }

    void Update()
    {
        // 3. Move toward Earth
        if (earthTarget != null)
        {
            Vector3 direction = (earthTarget.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }

        // 4. Constant random rotation
        transform.Rotate(randomRotationAxis * actualRotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if the asteroid hit the Earth
        if (other.CompareTag("Earth"))
        {
            ScoreManager.Instance.AddEarthHit();

            // Use your existing explode logic so it vanishes on impact
            ExplodeAsteroid();
        }
    }

    // Update your existing Explode method to count the kill
    public void ExplodeAsteroid()
    {
        // If it's active when this is called, it means the player shot it
        if (gameObject.activeSelf)
        {
            ScoreManager.Instance.AddDestroyedPoint();
        }

        GameObject effect = Instantiate(explosionPrefab, transform.position, transform.rotation);
        gameObject.SetActive(false);
        StartCoroutine(CleanupRoutine(effect));
    }

    private IEnumerator CleanupRoutine(GameObject effect)
    {
        yield return new WaitForSeconds(3f);
        if (effect != null) Destroy(effect);
        Destroy(this.gameObject);
    }
}