using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateJointLimitColor : MonoBehaviour
{
    // Start is called before the first frame update
    public Color newBackgroundColor, normalBackgroundColor;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void changeBackgroundColor()
    {
        image.color = newBackgroundColor; // Change background color
    }

    public void resetBackgroundColor()
    {
        image.color = normalBackgroundColor; // Change background color to the normal one (white)
    }
}
