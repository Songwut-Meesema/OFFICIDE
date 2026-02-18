using UnityEngine;
using TMPro;

public class KeyInventory : MonoBehaviour
{
    public bool hasBlueKey = false;
    public bool hasRedKey = false;
    public TextMeshProUGUI blueKeyText;
    public TextMeshProUGUI redKeyText;

    public void AddKey(string keyColor)
    {
        if (keyColor == "Blue")
        {
            hasBlueKey = true;
            if (blueKeyText != null) blueKeyText.text = "Blue Key: YES";
        }
        else if (keyColor == "Red")
        {
            hasRedKey = true;
            if (redKeyText != null) redKeyText.text = "Red Key: YES";
        }
    }
}
