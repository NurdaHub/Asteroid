using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private BigAsteroid bigAsteroid;
    [SerializeField] private MiddleAsteroid middleAsteroid;
    [SerializeField] private SmallAsteroid smallAsteroid;

    private int currentAsteroidsCount = 5;
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

    private void OnEnable()
    {
        CalculateBoreder();
        
        bigAsteroidsPool = new Pool<BigAsteroid>(bigAsteroid, currentAsteroidsCount, this.transform);
        middleAsteroidsPool = new Pool<MiddleAsteroid>(middleAsteroid, currentAsteroidsCount, this.transform);
        smallAsteroidsPool = new Pool<SmallAsteroid>(smallAsteroid, currentAsteroidsCount, this.transform);
        SpawnAsteroid();
    }

    private void SpawnAsteroid()
    {
        for (int i = 0; i < currentAsteroidsCount; i++)
        {
            var asteroid = bigAsteroidsPool.GetFreeElement();
            var position = CalculatePosition();
            asteroid.Initialize(position, middleAsteroidsPool, smallAsteroidsPool);
        }

        currentAsteroidsCount++;
    }

    private Vector3 CalculatePosition()
    {
        var xPos = Random.Range(-borderWidth, borderWidth);
        var yPos = Random.Range(-borderHeight, borderHeight);
        var currentSpawnPosition = new Vector3(borderWidth, yPos, 0);

        return currentSpawnPosition;
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
