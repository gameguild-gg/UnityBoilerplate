using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public int Score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI messageText; // Reference to the message text
    public Image popupImage; // Reference to the background image

    private void Start()
    {
        popupImage.enabled = false; // Hide the background image by default
    }
    
    // Update is called once per frame
    void Update()
    {
        scoreText.text = this.Score.ToString();
    }
    
    public void CheckForMessage()
    {
        // Check if the score has reached a certain threshold
        if (Score % 6 == 0 && Score != 0)
        {
            // Show the message
            ShowMessage();
        }
    }
    
    private void ShowMessage()
    {
        // Set the message text
        messageText.text = "Congratulations! You collected " + Score + " items!";
        // Make the message text visible
        messageText.enabled = true;
        // Show the background image
        popupImage.enabled = true;
        
        // Hide the message text after a certain delay
        Invoke("HideMessage", 30f);
    }

    private void HideMessage()
    {
        // Hide the background image
        popupImage.enabled = false;
        // Hide the message text
        messageText.enabled = false;
    }
}
