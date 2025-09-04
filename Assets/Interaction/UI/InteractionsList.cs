using System;
using UnityEngine;

public class InteractionsList : MonoBehaviour
{
    [SerializeField] private Interaction[] _interactions;
    private int _currentNumber;
    private int _selected;
    private int interactionsNumber => _interactions.Length;

    private static InteractionsList _instance;

    private void Start()
    {
        gameObject.SetActive(false);
        _instance = this;

        for (int i = 0; i < interactionsNumber; i++)
        {
            _interactions[i].Unselect();
            _interactions[i].gameObject.SetActive(false);
        }
    }

    public static void Redraw(Interactable[] interactables, Interactor interactor)
    {
        if(_instance == null)
            return;

        _instance.RedrawInteractions(interactables, interactor);
    }

    public static void Hide()
    {
        if (_instance == null)
            return;

        _instance._currentNumber = 0;
        _instance.gameObject.SetActive(false);
    }

    public static int GetSelected(int id)
    {
        if (_instance == null)
            return 0;

        int selected = (int)Mathf.Repeat(id, _instance._currentNumber);
        _instance._interactions[_instance._selected].Unselect();
        _instance._interactions[selected].Select();
        _instance._selected = selected;
        return selected;
    }

    public void RedrawInteractions(Interactable[] interactables, Interactor interactor)
    {
        if(interactables.Length == 0)
        {
            _currentNumber = 0;
            gameObject.SetActive(false);
            return;
        }
        else
        {
            _currentNumber = Math.Min(interactables.Length, interactionsNumber);
            gameObject.SetActive(true);
        }

        for (int i = 0; i < interactionsNumber; i++)
        {
            _interactions[i].Unselect();
            _interactions[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _currentNumber; i++)
        {
            _interactions[i].gameObject.SetActive(true);
            _interactions[i].Init(interactables[i].actionName, interactables[i].sprite);
            _interactions[i].SetActive(interactables[i].CanInteract());
        }
    }
}