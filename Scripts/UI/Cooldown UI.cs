using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private Image Image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Image = GetComponent<Image>();
    }

    public void UpdateFillAmount(float TimeStep, bool Dead)
    {
        if (Dead)
        {
            Image.fillAmount = 1.0f;
        }

        else
        {
            if (TimeStep == 0.0f)
            {
                Image.fillAmount = 0.0f;
            }

            else
            {
                Image.fillAmount = Mathf.MoveTowards(Image.fillAmount, 1.0f, TimeStep * Time.deltaTime);
            }
        }
    }
}
