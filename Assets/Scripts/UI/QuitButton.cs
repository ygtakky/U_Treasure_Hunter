using UnityEngine;

public class QuitButton : BaseButton
{
    protected override void Start()
    {
        base.Start();
        
        AddOnClickListener(QuitGame);
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
}
