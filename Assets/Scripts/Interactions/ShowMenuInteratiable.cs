using UnityEngine;
using UnityEngine.Events;

public class ShowMenuInteratiable : Interactable
{
    [SerializeField] private InteractionMenu _gui;
    [SerializeField] private UnityEvent _onInteract;
    [SerializeField] private UnityEvent _onBreack;
    private Interactor _interactor;

    private void Update()
    {
        if (interacted && !_gui.active) Break(_interactor);
    }

    public override bool CanInteract() => true;

    protected override void BreakAction(Interactor interactor)
    {
        _gui.Hide();
        _onBreack.Invoke();
    }

    protected override void InteractAction(Interactor interactor)
    {
        _gui.Show();
        _onInteract.Invoke();
    }
}
