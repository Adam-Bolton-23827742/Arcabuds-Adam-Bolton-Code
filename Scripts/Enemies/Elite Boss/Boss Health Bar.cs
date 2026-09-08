using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private Slider HealthSlider;
    public EnemyStats BossStats;

    private void Start()
    {
        HealthSlider = GetComponent<Slider>();
        HealthSlider.maxValue = BossStats.MaxHealth;
    }

    public void UpdateHealthSlider()
    {
        HealthSlider.value = BossStats.CurrentHealth;
    }
}
