using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionPrefab;

    [Header("Movement Settings")]
    public float speed = 5f;
    public Transform earthTarget;

    [Header("Rotation Settings")]
    public float rotationSpeedMin = 20f;
    public float rotationSpeedMax = 100f;

    private Vector3 _randomRotationAxis;
    private float _actualRotationSpeed;
    private bool _isDead = false;
    private Transform _myTransform;

    void Awake()
    {
        _myTransform = transform;
    }

    void Start()
    {
        _randomRotationAxis = Random.onUnitSphere;
        _actualRotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);

        if (earthTarget == null)
        {
            GameObject earthGO = GameObject.Find("Earth");
            if (earthGO != null) earthTarget = earthGO.transform;
        }
    }

    void Update()
    {
        if (_isDead || earthTarget == null) return;

        Vector3 direction = (earthTarget.position - _myTransform.position).normalized;
        _myTransform.position += direction * speed * Time.deltaTime;

        _myTransform.Rotate(_randomRotationAxis * (_actualRotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isDead && other.CompareTag("Earth"))
        {
            // Choice A: Calling with 0. ScoreManager handles random damage.
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddEarthHit(0);
            }
            ExecuteExplosion(false);
        }
    }

    public void ExplodeAsteroid() => HitByLaser();

    public void HitByLaser()
    {
        if (_isDead) return;

        if (ScoreManager.Instance != null) ScoreManager.Instance.AddDestroyedPoint();
        ExecuteExplosion(true);
    }

    private void ExecuteExplosion(bool wasPlayerKill)
    {
        _isDead = true;

        if (wasPlayerKill) TriggerLaserBeam();

        if (explosionPrefab != null)
        {
            GameObject effect = Instantiate(explosionPrefab, _myTransform.position, _myTransform.rotation);
            Destroy(effect, 3f);
        }

        gameObject.SetActive(false);
        Destroy(this.gameObject, 3.1f);
    }

    private void TriggerLaserBeam()
    {
        GameObject muzzle = GameObject.FindWithTag("LaserBlaster");
        LaserEffect laserScript = FindObjectOfType<LaserEffect>();

        if (muzzle != null && laserScript != null)
        {
            laserScript.FireLaser(muzzle.transform.position, _myTransform.position, 0.2f);
        }
    }
}