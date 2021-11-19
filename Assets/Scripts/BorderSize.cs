using UnityEngine;

public class BorderSize : MonoBehaviour
{
    private void OnEnable()
    {
        var height = (2f * Camera.main.orthographicSize) + 3f;
        var width = height * Camera.main.aspect;
        var collider = GetComponent<BoxCollider2D>();
        
        collider.size = new Vector2(width, height);
    }
}
