using UnityEngine;

public class SetUp : MonoBehaviour
{
    private CompanionState CompanionState;
    private Scale Scale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CompanionState = GetComponent<CompanionState>();
        Scale = GetComponent<Scale>();

        if (CompanionState.enabled)
        {
            transform.localScale = Scale.CompanionScale;
        }

        else
        {
            transform.localScale = Scale.PlayerScale;
        }
    }
}
