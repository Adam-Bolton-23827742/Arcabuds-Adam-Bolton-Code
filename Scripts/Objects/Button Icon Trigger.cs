using UnityEngine;

public class ButtonIconTrigger : MonoBehaviour
{
    public GameObject ButtonIconParent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == ButtonIconParent.transform.parent)
        {
            ButtonIconParent.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == ButtonIconParent.transform.parent)
        {
            ButtonIconParent.SetActive(false);
        }
    }
}
