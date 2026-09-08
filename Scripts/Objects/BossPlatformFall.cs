using System.Collections;
using UnityEngine;

public class BossPlatformFall : MonoBehaviour
{
    [SerializeField] private GameObject Platform;
    [SerializeField] private float FallSpeed = 1.0f, LifeTime;
    [SerializeField] private GameObject[] ObjectsToDestroy;
    private float Timer;
    private bool IsRunning;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !IsRunning)
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        IsRunning = true;

        foreach (GameObject obj in ObjectsToDestroy)
        {
            Destroy(obj);
        }

        while (Timer < LifeTime)
        {
            transform.Translate(Vector3.down * Time.deltaTime * FallSpeed);
            Timer += Time.deltaTime;
            yield return null;
        }

        Destroy(Platform);
    }
}
