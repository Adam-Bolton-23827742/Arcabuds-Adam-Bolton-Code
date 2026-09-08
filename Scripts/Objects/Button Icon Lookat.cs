using UnityEngine;

public class ButtonIconLookat : MonoBehaviour
{
    [SerializeField] private Vector3 ConsantScale;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.localScale = IconScale();
    }

    private Vector3 IconScale()
    {
        return Vector3.Distance(transform.position, Camera.main.transform.position) * ConsantScale;
    }
}
