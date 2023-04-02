using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private BigAsteroid bigAsteroid;
    [SerializeField] private AverageAsteroid averageAsteroid;
    [SerializeField] private SmallAsteroid smallAsteroid;
    [SerializeField] private AudioSource bigAsteroidAudio;
    [SerializeField] private AudioSource averageAsteroidAudio;
    [SerializeField] private AudioSource smallAsteroidAudio;

    private int startAsteroidsCount = 2;
    private int currentAsteroidsCount = 2;
    private int brokenPieceCount = 2;
    private int poolAsteroidCount = 6;
    private float borderWidth;
    private float borderHeight;
    
    private Camera CurrentCamera => Camera.current;
    private Pool<BigAsteroid> bigAsteroidsPool;
    private Pool<AverageAsteroid> averageAsteroidsPool;
    private Pool<SmallAsteroid> smallAsteroidsPool;

    public static float MaxDistance;
    public static Action<Transform> OnBigAsteroidBroke;
    public static Action<Transform> OnAverageAsteroidBroke;
    public static Action OnAsteroidBroke;

    private void Awake()
    {
        CalculateBorder();
        
        bigAsteroidsPool = new Pool<BigAsteroid>(bigAsteroid, poolAsteroidCount, transform);
        averageAsteroidsPool = new Pool<AverageAsteroid>(averageAsteroid, poolAsteroidCount, transform);
        smallAsteroidsPool = new Pool<SmallAsteroid>(smallAsteroid, poolAsteroidCount, transform);

        OnBigAsteroidBroke += SpawnAverageAsteroid;
        OnAverageAsteroidBroke += SpawnSmallAsteroid;
        OnAsteroidBroke += AsteroidBroke;
    }

    public void Init()
    {
        currentAsteroidsCount = startAsteroidsCount;
        DeactivateAllPools();
        SpawnBigAsteroid();
    }

    private void SpawnBigAsteroid()
    {
        for (int i = 0; i < currentAsteroidsCount; i++)
        {
            var asteroid = bigAsteroidsPool.GetFreeElement();
            var position = CalculatePosition();
            var rotation = BigAsteroidRotation(position);
            
            asteroid.Initialize(position, rotation);
        }
        
        currentAsteroidsCount++;
    }

    private void SpawnAverageAsteroid(Transform _transform)
    {
        bigAsteroidAudio.Play();
        
        for (int i = 0; i < brokenPieceCount; i++)
        {
            var asteroid = averageAsteroidsPool.GetFreeElement();
            var position = _transform.localPosition;
            var rotation = CalculateRotation(_transform, i);
            
            asteroid.Initialize(position, rotation);
        }
    }

    private void SpawnSmallAsteroid(Transform _transform)
    {
        averageAsteroidAudio.Play();
        
        for (int i = 0; i < brokenPieceCount; i++)
        {
            var asteroid = smallAsteroidsPool.GetFreeElement();
            var position = _transform.localPosition;
            var rotation = CalculateRotation(_transform, i);
            
            asteroid.Initialize(position, rotation);
        }
    }

    private void AsteroidBroke()
    {
        smallAsteroidAudio.Play();
        CheckActiveAsteroid();
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2);
        SpawnBigAsteroid();
    }

    private void CheckActiveAsteroid()
    {
        var hasActiveSmallAsteroid = smallAsteroidsPool.HasActiveElement();
        if (hasActiveSmallAsteroid)
            return;
        
        var hasActiveAverageAsteroid = averageAsteroidsPool.HasActiveElement();
        if (hasActiveAverageAsteroid)
            return;
        
        var hasActiveBigAsteroid = bigAsteroidsPool.HasActiveElement();
        if (hasActiveBigAsteroid)
            return;
        
        StartCoroutine(NextLevel());
    }

    private Vector3 CalculatePosition()
    {
        var vertical = Random.Range(0, 2) != 0;
        Vector3 currentSpawnPosition;

        if (vertical)
        {
            var yPos = Random.Range(-borderHeight, borderHeight);
            currentSpawnPosition = new Vector3(borderWidth, yPos, 0);
        }
        else
        {
            var xPos = Random.Range(-borderWidth, borderWidth);
            currentSpawnPosition = new Vector3(xPos, borderHeight, 0);
        }

        return currentSpawnPosition;
    }

    private Quaternion CalculateRotation(Transform _transform, int item)
    {
        var angle = item == 0 ? -45 : 45;
        var asteroidRotation = Quaternion.Euler(0, 0, _transform.localRotation.z + angle);

        return asteroidRotation;
    }
    
    private Quaternion BigAsteroidRotation(Vector3 _position)
    {
        var random = Random.Range(80, 100);
        var direction = Vector3.zero - _position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var currentRotation = Quaternion.AngleAxis(angle - random, Vector3.forward);

        return currentRotation;
    }

    private void DeactivateAllPools()
    {
        bigAsteroidsPool.DeactivateAllElements();
        averageAsteroidsPool.DeactivateAllElements();
        smallAsteroidsPool.DeactivateAllElements();
    }

    private void CalculateBorder()
    {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        borderWidth = width / 2f + 1f;
        borderHeight = height / 2f + 1f;
        MaxDistance = width;
    }
}
