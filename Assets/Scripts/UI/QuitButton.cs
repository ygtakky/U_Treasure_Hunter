using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    private Button quitButton;
    
    private void Awake()
    {
        quitButton = GetComponent<Button>();
    }
    
    private void Start()
    {
        quitButton.onClick.AddListener(QuitGame);
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
}
