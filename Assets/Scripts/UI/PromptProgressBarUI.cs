using UnityEngine;
using UnityEngine.UI;

public class PromptProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject sliderPanel;
    [SerializeField] private Slider slider;

    private void Start()
    {
        sliderPanel.SetActive(false);
    }

    public void IncrementProgress(float progress)
    {
        if (progress == 0)
        {
            Reset();

            return;
        }

        sliderPanel.SetActive(true);
        slider.value = progress;
    }

    private void Reset()
    {
        slider.value = 0;
        sliderPanel.SetActive(false);
    }
}
