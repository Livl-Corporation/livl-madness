using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{

    public static readonly string MouseSensibilityKey = "MouseSensibility";

    private PlayerController controller;
    
    [SerializeField] private TMP_Text sensibilityText;
    [SerializeField] private Slider sensibilitySlider;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<PlayerController>();

        if (controller == null)
        {
            Debug.LogError("SettingsController : No Player Controller found");
            return;
        }
        
        
        var value = PlayerPrefs.GetFloat(MouseSensibilityKey, controller.GetMouseSensitivity());
        sensibilitySlider.value = value;
        OnSensibilityChange(value);
        
        sensibilitySlider.onValueChanged.AddListener(OnSensibilityChange);

    }

    private void OnSensibilityChange(float value)
    {
        PlayerPrefs.SetFloat(MouseSensibilityKey, value);
        controller.SetMouseSensitivity(value);
        sensibilityText.text = value.ToString("0.00");
    }

}
