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

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        var isDestroyer = collider.CompareTag("Alien") || collider.CompareTag("Player");

        if (isDestroyer)
            gameObject.SetActive(false);
    }
}