using UnityEngine;
using System.Collections;

public class AsteroidManager : MonoBehaviour
{
    public enum EnemyType { Asteroid, Alien, Warrior, Boss }

    [Header("Enemy Identity")]
    public EnemyType type = EnemyType.Asteroid;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionPrefab;

    [Header("Movement Settings")]
    public float speed = 5f;
    public Transform earthTarget;
    public Transform warriorStopPoint;

    [Header("Combat Visuals")]
    [SerializeField] private LineRenderer warriorLaserLine;
    [SerializeField] private GameObject bossBlasterPrefab;
    [SerializeField] private Transform muzzlePoint;

    [Header("Rotation Settings")]
    public float rotationSpeedMin = 20f;
    public float rotationSpeedMax = 100f;

    [Header("Combat Settings")]
    public float damageValue = 5f; // Used for Asteroid/Alien collisions and Boss shots

    private Vector3 _randomRotationAxis;
    private float _actualRotationSpeed;
    private bool _isDead = false;
    private bool _hasReachedStop = false;
    private Transform _myTransform;

    void Awake() => _myTransform = transform;

    void Start()
    {
        _randomRotationAxis = Random.onUnitSphere;
        _actualRotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);

        if (LevelManager.Instance != null && earthTarget == null)
            earthTarget = LevelManager.Instance.currentTargetTransform;

        if (type == EnemyType.Boss) InvokeRepeating(nameof(ShootBossBlaster), 2f, 2f);
        if (warriorLaserLine != null) warriorLaserLine.enabled = false;
    }

    void Update()
    {
        if (_isDead || earthTarget == null) return;

        if (type == EnemyType.Warrior && warriorStopPoint != null)
        {
            if (!_hasReachedStop)
            {
                MoveTo(warriorStopPoint.position);
                if (Vector3.Distance(_myTransform.position, warriorStopPoint.position) < 0.2f)
                {
                    _hasReachedStop = true;
                    StartCoroutine(WarriorLaserRoutine());
                }
            }
            else { UpdateLaserVisuals(); }
        }
        else
        {
            MoveTo(earthTarget.position);
        }

        // Only asteroids rotate
        if (type == EnemyType.Asteroid && !_isDead)
        {
            _myTransform.Rotate(_randomRotationAxis * (_actualRotationSpeed * Time.deltaTime));
        }
    }

    private void MoveTo(Vector3 target)
    {
        _myTransform.position = Vector3.MoveTowards(_myTransform.position, target, speed * Time.deltaTime);
        if (type != EnemyType.Asteroid) _myTransform.LookAt(target);
    }

    private IEnumerator WarriorLaserRoutine()
    {
        if (warriorLaserLine != null) warriorLaserLine.enabled = true;

        while (!_isDead)
        {
            // FIXED: Warrior laser now specifically deals exactly 1 damage per second
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddEarthHit(1f);

            yield return new WaitForSeconds(1.0f);
        }
    }

    private void UpdateLaserVisuals()
    {
        if (warriorLaserLine == null || earthTarget == null) return;
        warriorLaserLine.SetPosition(0, muzzlePoint != null ? muzzlePoint.position : _myTransform.position);
        warriorLaserLine.SetPosition(1, earthTarget.position);
    }

    private void ShootBossBlaster()
    {
        if (_isDead || bossBlasterPrefab == null || earthTarget == null) return;
        Vector3 spawnPos = muzzlePoint != null ? muzzlePoint.position : _myTransform.position;
        GameObject bolt = Instantiate(bossBlasterPrefab, spawnPos, Quaternion.identity);
        bolt.transform.LookAt(earthTarget.position);
    }

    public void ExplodeAsteroid() => HitByLaser();

    public void HitByLaser()
    {
        if (_isDead) return;
        if (ScoreManager.Instance != null) ScoreManager.Instance.AddDestroyedPoint();
        ExecuteExplosion(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isDead) return;

        if (other.CompareTag("Earth") || other.CompareTag("Moon") || other.CompareTag("TravelTarget") || other.transform == earthTarget)
        {
            // FIXED: Warriors do ZERO collision damage once they have reached their stop point
            if (type == EnemyType.Warrior && _hasReachedStop) return;

            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddEarthHit(damageValue);

            ExecuteExplosion(false);
        }
    }

    private void ExecuteExplosion(bool wasPlayerKill)
    {
        _isDead = true;
        CancelInvoke();
        StopAllCoroutines();

        if (wasPlayerKill) TriggerLaserBeamEffect();

        if (explosionPrefab != null)
        {
            GameObject effect = Instantiate(explosionPrefab, _myTransform.position, _myTransform.rotation);
            Destroy(effect, 3f);
        }

        if (warriorLaserLine != null) warriorLaserLine.enabled = false;
        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);
    }

    private void TriggerLaserBeamEffect()
    {
        GameObject playerGun = GameObject.FindWithTag("LaserBlaster");
        if (playerGun != null)
        {
            LaserEffect laserScript = playerGun.GetComponent<LaserEffect>();
            if (laserScript != null)
            {
                laserScript.FireLaser(playerGun.transform.position, _myTransform.position, 0.2f);
            }
        }
    }
}