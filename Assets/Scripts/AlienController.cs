using UnityEngine;

public class AlienController : MonoBehaviour
{
    private Rigidbody2D alienRB;

    private void OnEnable()
    {
        var alienSpeed = Random.Range(70f, 100f);
        alienRB = GetComponent<Rigidbody2D>();
        alienRB.AddForce(transform.right * alienSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
            gameObject.SetActive(false);
    }
}
