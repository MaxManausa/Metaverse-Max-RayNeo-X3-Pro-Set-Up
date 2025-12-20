using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    /// <summary>
    /// Call this method to "destroy" the asteroid with an effect.
    /// </summary>
    public void ExplodeAsteroid()
    {
        // 1. Instantiate the particle effect at the current asteroid position
        GameObject effect = Instantiate(explosionPrefab, transform.position, transform.rotation);

        // 2. Make the asteroid inactive so it disappears immediately
        gameObject.SetActive(false);

        // 3. Start the timer to clean up both objects from memory
        StartCoroutine(CleanupRoutine(effect));
    }

    private IEnumerator CleanupRoutine(GameObject effect)
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // 4. Permanently destroy both the effect and this asteroid
        Destroy(effect);
        Destroy(gameObject);
    }
}