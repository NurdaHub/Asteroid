using UnityEngine;

public class AsteroidBase : MonoBehaviour
{
    private Rigidbody2D asteroidRB;
    private float asteroidSpeed;

    public void Initialize(Vector3 _position, Quaternion _rotation)
    {
        this.transform.position = _position;
        this.transform.rotation = _rotation;
        asteroidSpeed = Random.Range(30f, 50f);
        asteroidRB = GetComponent<Rigidbody2D>();
        asteroidRB.AddForce(transform.up * asteroidSpeed);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision enter  " + transform.position);
        var collisionGO = collision.gameObject;
        var isDestroyer = collisionGO.CompareTag("Alien") || collisionGO.CompareTag("Player");
        
        if (isDestroyer)
        {
            gameObject.SetActive(false);

            Debug.Log("broke   " + transform.position);
        }
    }
}