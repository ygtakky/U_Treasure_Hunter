using UnityEngine;

public class BaseUI : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public virtual void Toggle()
    {
        if (gameObject.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
}
