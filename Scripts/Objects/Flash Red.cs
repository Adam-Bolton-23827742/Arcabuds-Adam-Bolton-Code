using System.Collections;
using UnityEngine;

public class FlashRed : MonoBehaviour
{
    [SerializeField] private Renderer Renderer;
    [SerializeField] private float FlashRedDuration;

    public IEnumerator Flash()
    {
        Renderer.material.color = Color.red;
        yield return new WaitForSeconds(FlashRedDuration);
        Renderer.material.color = Color.white;
    }
}
