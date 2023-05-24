using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayJointPosition : MonoBehaviour
{
    // Start is called before the first frame update
    private Image image;
    private TextMeshProUGUI imageText;
    private string initialText;
    public ArticulationBody articulation;
    public float currentPosition;

    void Start()
    {
        image = GetComponent<Image>();
        imageText = image.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (image != null && imageText != null)
        {
            var drive = articulation.GetComponent<ArticulationBody>().xDrive;

            currentPosition = drive.target;
            imageText.text = GetNewJointName(articulation.GetComponent<ArticulationBody>().name) + ": " + currentPosition.ToString("F2");

            imageText.alignment = TextAlignmentOptions.Left;
            imageText.fontSize = 18; // Set the font size to 18
            imageText.color = Color.black; // Set the font color to black
        }
        else
        {
            Debug.Log("Not text component found.");
        }
    }
    private string GetNewJointName(string jointName)
    {
        string newName = "J";
        for(int i=0; i < jointName.Length; i++)
        {
            if (jointName[i] == '1' || jointName[i] == '2')
            {
                newName = "J" + jointName[jointName.Length - 1];
            }
            else
            {
                newName = "EEF";
            }
        }
        return newName;
    }
}