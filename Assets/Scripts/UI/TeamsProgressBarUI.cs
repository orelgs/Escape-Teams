using UnityEngine;
using UnityEngine.UI;

public class TeamsProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject sliderPanel;
    [SerializeField] private Slider slider;

    public void IncrementProgress(Component sender, object data)
    {
        if (data is float progress)
        {
            if (progress == 0)
            {
                Reset();

                return;
            }

            slider.value = progress;
        }
    }

    private void Reset()
    {
        slider.value = 0;
    }
}
