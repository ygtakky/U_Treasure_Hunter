using System;
using UnityEngine.UI;

public class BaseButton : BaseUI
{
    private Button button;

    protected virtual void Start()
    {
        button = GetComponent<Button>();
    }
    
    protected void AddOnClickListener(Action action)
    {
        button.onClick.AddListener(() => action());
    }
}
