using UnityEngine;
using UnityEngine.UI;

public class InteractionSelectionPointer : MonoBehaviour
{
    [SerializeField] private Text _text;

    private void OnEnable()
    {
        _text.text = Interactor.interactionKey.ToString();
    }
}