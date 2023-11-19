using UnityEngine;

public class ToggleButton : BaseButton
{
    [SerializeField] private BaseUI screenToToggle;

    protected override void Start()
    {
        base.Start();
        
        AddOnClickListener(ToggleScreen);
    }
    
    private void ToggleScreen()
    {
        screenToToggle.Toggle();
    }
}
