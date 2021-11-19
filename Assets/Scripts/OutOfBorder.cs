using UnityEngine;

public class OutOfBorder : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Border"))
        {
            Debug.Log("out of border");
            this.transform.position = -transform.position;
        }
    }
}
