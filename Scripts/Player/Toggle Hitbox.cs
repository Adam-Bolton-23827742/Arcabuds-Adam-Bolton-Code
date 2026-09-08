using UnityEngine;

public class ToggleHitbox : MonoBehaviour
{
    public Collider BasicHitbox;

    public void ToggleOn()
    {
        BasicHitbox.enabled = true;
    }

    public void ToggleOff()
    {
        BasicHitbox.enabled = false;
    }
}
