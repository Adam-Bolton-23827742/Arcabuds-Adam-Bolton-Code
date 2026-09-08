using UnityEngine;

public class ExitBarrier : MonoBehaviour
{
    private EnemyStats Stats;
    [SerializeField] private GameObject Barrier;
    [SerializeField] private GameObject[] ObjectsToSetActive;

    private void Start()
    {
        Stats = GetComponentInParent<EnemyStats>();
    }

    public void OpenBarrier()
    {
        if (Stats.CurrentHealth <= 0)
        {
            if (Barrier != null)
            {
                Barrier.SetActive(false);
            }

            foreach (GameObject Object in ObjectsToSetActive)
            {
                Object.SetActive(true);
            }
        }
    }
}
