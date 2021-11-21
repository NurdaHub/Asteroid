using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private BigAsteroid bigAsteroid;
    [SerializeField] private MiddleAsteroid middleAsteroid;
    [SerializeField] private SmallAsteroid smallAsteroid;

    private int currentAsteroidsCount = 2;
    private int brokenPieceCount = 2;
    private float borderWidth;
    private float borderHeight;
    private Pool<BigAsteroid> bigAsteroidsPool;
    private Pool<MiddleAsteroid> middleAsteroidsPool;
    private Pool<SmallAsteroid> smallAsteroidsPool;
    
    public GameObject BorderLeft;
    public GameObject BorderRight;
    public GameObject BorderTop;
    public GameObject BorderBottom;

    public static float maxDistance;

    public void Init()
    {
        CalculateBoreder();
        
        bigAsteroidsPool = new Pool<BigAsteroid>(bigAsteroid, currentAsteroidsCount, this.transform);
        middleAsteroidsPool = new Pool<MiddleAsteroid>(middleAsteroid, currentAsteroidsCount, this.transform);
        smallAsteroidsPool = new Pool<SmallAsteroid>(smallAsteroid, currentAsteroidsCount, this.transform);
        SpawnBigAsteroid();
    }

    private void SpawnBigAsteroid()
    {
        for (int i = 0; i < currentAsteroidsCount; i++)
        {
            var asteroid = bigAsteroidsPool.GetFreeElement();
            var randomAngle = Random.Range(0, 360);

            var position = CalculatePosition();
            var rotation = BigAsteroidRotation(position);
            
            asteroid.Initialize(position, rotation);
            asteroid.OnBigAsteroidBroke += SpawnMiddleAsteroid;
        }
    }

    private void SpawnMiddleAsteroid(Transform _transform)
    {
        for (int i = 0; i < brokenPieceCount; i++)
        {
            var asteroid = middleAsteroidsPool.GetFreeElement();
            var position = _transform.localPosition;
            var rotation = CalculateRotation(_transform, i);
            
            asteroid.Initialize(position, rotation);
            asteroid.OnMiddleAsteroidBroke += SpawnSmallAsteroid;
        }
    }

    private void SpawnSmallAsteroid(Transform _transform)
    {
        for (int i = 0; i < brokenPieceCount; i++)
        {
            var asteroid = smallAsteroidsPool.GetFreeElement();
            var position = _transform.localPosition;
            var rotation = CalculateRotation(_transform, i);
            
            asteroid.Initialize(position, rotation);
            asteroid.OnSmallAsteroidBroke += CheckActiveAsteroids;
        }
    }

    private void CheckActiveAsteroids()
    {
        var hasActiveElement = smallAsteroidsPool.HasActiveElement();
        if (hasActiveElement)
            return;

        currentAsteroidsCount++;
        SpawnBigAsteroid();
    }

    private Vector3 CalculatePosition()
    {
        var vertical = Random.Range(0, 2) != 0;
        var currentSpawnPosition = new Vector3();

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
        var asteroidRotation = Quaternion.Euler(0, 0, _transform.rotation.z + angle);

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

    private void CalculateBoreder()
    {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        borderWidth = width / 2f + 1f;
        borderHeight = height / 2f + 1f;
        maxDistance = width;

        float leftBorderPosition = -1f * (width / 2) - 1f;
        float rightBorderPosition = width / 2 + 1f;
        
        // BorderLeft.transform.position = new Vector3(leftBorderPosition, 0, 0);
        // BorderRight.transform.position = new Vector3(rightBorderPosition, 0, 0);

        float spriteWidth = BorderTop.GetComponent<SpriteRenderer>().bounds.size.x;
        float ratio = width / spriteWidth;
        
        BorderTop.transform.localScale = new Vector3(ratio, 1, 1);
        BorderBottom.transform.localScale = new Vector3(ratio, 1, 1);
        
        Debug.Log($"height  {height} ||| width  {width}");
        Debug.Log($"spriteWidth  {spriteWidth} ||| ratio  {ratio}");
    }
}
