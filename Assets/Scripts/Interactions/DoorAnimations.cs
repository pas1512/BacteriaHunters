using UnityEngine;

public class DoorAnimations : MonoBehaviour
{
    [SerializeField] private string _openParameter = "Opened";
    [SerializeField] private Animator _animator;
    private void Start() => _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    public void SetState(bool state) => _animator.SetBool(_openParameter, state);
}