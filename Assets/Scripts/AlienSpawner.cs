using System.Collections;
using UnityEngine;

public class AlienSpawner : MonoBehaviour
{
    [SerializeField] private GameObject alienPrefab;

    private void OnEnable()
    {
        StartCoroutine(WaitTime());
    }

    private void AlienInit()
    {
        Instantiate(alienPrefab);
    }

    private IEnumerator WaitTime()
    {
        var randomDelay = Random.Range(20, 40);
        yield return new WaitForSeconds(randomDelay);
        
        AlienInit();
    }
}
