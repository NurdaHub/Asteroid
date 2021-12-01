using UnityEngine;

public class AsteroidBase : MonoBehaviour
{
    private float asteroidSpeed;

    public void Initialize(Vector3 _position, Quaternion _rotation)
    {
        this.transform.position = _position;
        this.transform.localRotation = _rotation;
        asteroidSpeed = Random.Range(0.5f, 1.5f);
    }

    private void Update()
    {
        MoveAsteroid();
    }

    private void MoveAsteroid()
    {
        var translate = Vector3.up * Time.deltaTime * asteroidSpeed; 
        transform.Translate(translate);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        var isDestroyer = collider.CompareTag("Alien") || collider.CompareTag("Player");

        if (isDestroyer)
        {
            gameObject.SetActive(false);
            AsteroidSpawner.OnAsteroidBroke?.Invoke();
        }
    }
}