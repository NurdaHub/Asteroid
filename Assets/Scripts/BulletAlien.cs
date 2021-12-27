using UnityEngine;

public class BulletAlien : BulletBase
{
    private string asteroidTag = "Asteroid";
    private string playerTag = "Player";
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var isDestroyer = collider.CompareTag(asteroidTag) || collider.CompareTag(playerTag);
        
        if (isDestroyer)
            this.gameObject.SetActive(false);
    }
}
