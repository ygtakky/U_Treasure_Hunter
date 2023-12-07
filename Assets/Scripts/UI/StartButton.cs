using UnityEngine.SceneManagement;

public class StartButton : BaseButton
{
    protected override void Start()
    {
        base.Start();
        
        AddOnClickListener(LoadGame);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
