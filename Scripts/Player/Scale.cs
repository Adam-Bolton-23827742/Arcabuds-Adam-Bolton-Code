using UnityEngine;

public class Scale : MonoBehaviour
{
    [SerializeField] private float ScaleSpeed;
    public Vector3 PlayerScale, CompanionScale;
    [HideInInspector] public bool CanChangeScale;
    [HideInInspector] public Transform NextPlayer, CurrentPlayer;

    private Vector3 UpdateScale(Vector3 CurrentScale, Vector3 TargetScale)
    {
        return Vector3.MoveTowards(CurrentScale, TargetScale, ScaleSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanChangeScale)
        {
            if (NextPlayer != null)
            {
                NextPlayer.localScale = UpdateScale(NextPlayer.localScale, PlayerScale);
            }

            if (CurrentPlayer != null)
            {
                CurrentPlayer.localScale = UpdateScale(CurrentPlayer.localScale, CompanionScale);
            }

            if (NextPlayer != null && CurrentPlayer != null)
            {
                if (NextPlayer.localScale == PlayerScale && CurrentPlayer.localScale == CompanionScale)
                {
                    CanChangeScale = false;
                }
            }

            else if (NextPlayer == null && CurrentPlayer != null)
            {
                if (CurrentPlayer.localScale == CompanionScale)
                {
                    CanChangeScale = false;
                }
            }

            else if (NextPlayer != null && CurrentPlayer == null)
            {
                if (NextPlayer.localScale == PlayerScale)
                {
                    CanChangeScale = false;
                }
            }
        }

        else
        {
            CurrentPlayer = null;
            NextPlayer = null;
        }
    }
}
