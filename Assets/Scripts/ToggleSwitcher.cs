using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitcher : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI toggleText;
    
    private Toggle toggle;
    private bool _isOn;
    private string keyboardText = "CONTROL: KEYBOARD";
    private string mouseText = "CONTROL: MOUSE + KEYBOARD";

    private void OnEnable()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(Switch);
    }

    private void Switch(bool value)
    {
        toggleText.text = value ? keyboardText : mouseText;
    }
}
