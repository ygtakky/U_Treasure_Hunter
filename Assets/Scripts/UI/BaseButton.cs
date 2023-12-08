using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : BaseUI
{
    [Header("Configuration")]
    [SerializeField] private ButtonDataSO buttonDataSO;
    
    private Button button;

    protected virtual void Start()
    {
        button = GetComponent<Button>();

        if (buttonDataSO.SfxEventChannel != null || buttonDataSO.ButtonClickSfx != null)
        {
            AddOnClickListener(() => buttonDataSO.SfxEventChannel.RaiseEvent(this, new AudioEventArgs(buttonDataSO.ButtonClickSfx)));
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    protected void AddOnClickListener(Action action)
    {
        button.onClick.AddListener(() => action());
    }
}
