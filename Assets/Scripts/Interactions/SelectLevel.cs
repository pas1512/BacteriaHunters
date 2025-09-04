using UnityEngine;

public class SelectLevel : Interactable
{
    [SerializeField] private SelectLevelGUI _gui;
    private Interactor _interactor;

    private void Update()
    {
        if (interacted && !_gui.active) BreakAction(_interactor);
    }

    public override bool CanInteract() => true;

    protected override void BreakAction(Interactor interactor)
    {
        _gui.Hide();
    }

    protected override void InteractAction(Interactor interactor)
    {
        _gui.Show();
    }
}
