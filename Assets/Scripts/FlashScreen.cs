using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : MonoBehaviour
{

    Image flashScreen;

    void Start()
    {
        flashScreen = GetComponent<Image>();
    }

    void Update()
{
    if (flashScreen.color.a > 0)
    {
        Color targetColor = new Color(1.0f, 0.0f, 1.0f, 0); //purple color 
        flashScreen.color = Color.Lerp(flashScreen.color, targetColor, 5 * Time.deltaTime);
    }
}

    public void TookDamage()
    {
        flashScreen.color = new Color(1, 0, 0, 0.8f);
    }

    public void PickedUpBonus()
    {
        flashScreen.color = new Color(0, 0, 1, 0.8f);
    }
}