using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitcher : MonoBehaviour
{
    [SerializeField] private Toggle firstToggle;
    [SerializeField] private Toggle secondToggle;
    
    
    private bool _isOn;
    
    public bool IsOn
    {
        get => _isOn;
        set
        {
            _isOn = value;
            Switch(_isOn);
        }
    }

    private void Switch(bool value)
    {
        if (value)
        {
            //firstToggle.isOn = 
        }
        else
        {
            
        }
    }
}
