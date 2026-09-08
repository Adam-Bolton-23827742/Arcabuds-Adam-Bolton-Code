using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    private ParticleSystem ParticleSystem;

    private void Start()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ParticleSystem.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
