using UnityEngine;

public class OutOfBorder : MonoBehaviour
{
    private string borderTag = "Border";
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(borderTag))
            this.transform.position = -transform.position;
    }
}
