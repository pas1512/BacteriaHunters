using System.Security.Cryptography;
using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected InteractionData _data;
    public InteractionData data => _data;
    public virtual Sprite sprite => _data.sprite;
    public virtual string actionName => _data.actionName;

    private bool _interacted;
    public bool interacted => _interacted;
    public bool notInteracted => !_interacted;

    public int actionId => GetInstanceID();

    public virtual bool CanShow() => true;
    public virtual bool CanBreak() => _data.cancleable;
    public abstract bool CanInteract();
    protected abstract void InteractAction(Interactor interactor);
    protected abstract void BreakAction(Interactor interactor);

    public void Interact(Interactor interactor)
    {
        _interacted = true;
        InteractAction(interactor);
    }

    public void Break(Interactor interactor)
    {
        _interacted = false;
        BreakAction(interactor);
    }
}