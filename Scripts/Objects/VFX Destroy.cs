using System.Collections;
using UnityEngine;

public class VFXDestroy : MonoBehaviour
{
    [SerializeField] private float DestroyDelay = 3.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(VFXDestroyDelay());
    }

    private IEnumerator VFXDestroyDelay()
    {
        yield return new WaitForSeconds(DestroyDelay);
        Destroy(gameObject);
    }
}
