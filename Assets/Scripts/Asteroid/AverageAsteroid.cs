using UI;
using UnityEngine;

namespace Asteroid
{
    public class AverageAsteroid : AsteroidBase
    {
        private int points = 50;
        private string bulletTag = "Bullet";

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag(bulletTag))
            {
                PlayerScore.Instance.UpdateScore(points);
                AsteroidSpawner.OnAverageAsteroidBroke?.Invoke(this.transform);
                gameObject.SetActive(false);
            }
        
            base.OnTriggerEnter2D(collider);
        }
    }
}
