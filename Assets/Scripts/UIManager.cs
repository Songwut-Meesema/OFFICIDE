using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] coffeeIcons; 
    public Sprite coffeeFull;
    public Sprite coffeeEmpty;

    public void UpdateCoffeeUI(int coffeeCount)
    {
        for (int i = 0; i < coffeeIcons.Length; i++)
        {
            if (i < coffeeCount)
            {
                coffeeIcons[i].sprite = coffeeFull;
                coffeeIcons[i].enabled = true;
            }
            else
            {
                coffeeIcons[i].sprite = coffeeEmpty;
                coffeeIcons[i].enabled = true;
            }
        }
    }
}
