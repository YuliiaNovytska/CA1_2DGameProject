using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Reference to text element displaying the final score in the menu
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        //check if the score text component is assigned in the Inspector
        if (finalScoreText != null)
        {
            //display the total coins before game over
            finalScoreText.text = "Coins earned: " + Player.GameData.collectedGold;
        }
    }
    // method called when the user starts the game so its loading the gameplay scene
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // method called when the user quits
    public void Quit()
    {
        Application.Quit();
    }
}
