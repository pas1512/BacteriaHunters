using System;
using System.Linq;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Header("View Area")] 
    [SerializeField] private float _radius = 1.5f;
    [SerializeField] private float _angle = 90f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private Transform _origin;

    [Header("Controll")]
    [SerializeField] private KeyCode _interactionKey = KeyCode.F;
    [SerializeField] private KeyCode _cencelKey = KeyCode.Escape;

    private static KeyCode _staticKey;
    public static KeyCode interactionKey => _staticKey;

    private Interactable _currentInteractable;
    private int _lastSelected;

    private void Awake()
    {
        if(_origin == null)
        {
            Camera camera = GetComponentInChildren<Camera>();

            if(camera == null)
                _origin = transform;
            else
                _origin = camera.transform;
        }
        _staticKey = _interactionKey;
    }

    private void Update()
    {
        Interactable[] interactables = null;
        bool haveInteractables = false;

        if (_currentInteractable == null)
        {
            interactables = GetActiveInteractables();
            haveInteractables = interactables != null && interactables.Length > 0;
        }

        if (haveInteractables)
        {
            InteractionsList.Redraw(interactables, this);

            int mouseDelta = Math.Sign(Input.mouseScrollDelta.y);
            _lastSelected = InteractionsList.GetSelected(_lastSelected - mouseDelta);

            if (Input.GetKeyDown(_interactionKey))
            {
                var interactable = interactables[_lastSelected];
                interactable.Interact(this);

                if (interactable.data.continued)
                {
                    _currentInteractable = interactable;
                    InteractionsList.Hide();
                }
            }
        }
        else
        {
            InteractionsList.Hide();
        }

        if(_currentInteractable != null &&
            _currentInteractable.notInteracted)
        {
            _currentInteractable = null;
        }

        if (_currentInteractable != null &&
            Input.GetKeyDown(_cencelKey) &&
            _currentInteractable.CanBreak())
        {
            _currentInteractable.Break(this);
            _currentInteractable = null;
        }
    }

    private Interactable[] GetActiveInteractables()
    {
        var objects = Physics.OverlapSphere(_origin.position, _radius, _interactableMask);
        var inSector = objects.Where(o => GetAngleToCollider(_origin, o) < _angle / 2f);

        bool IsInteravtable(Collider collider)
        {
            return collider.TryGetComponent(out Interactable component) &&
                ( component.data.allowTrigger || !collider.isTrigger);
        }

        var interactables = inSector.Where(o => IsInteravtable(o)).Select(o => o.GetComponent<Interactable>());
        var active = interactables.Where(o => o.enabled && o.CanShow() && o.notInteracted);
        return active.GroupBy(o => o.actionId).Select(g => g.First()).ToArray();
    }

    private float GetAngleToCollider(Transform origin, Collider obj)
    {
        Vector3 lookDirection = origin.forward * _radius;
        Vector3 closest = Vector3.zero;
        var hits = Physics.RaycastAll(origin.position, lookDirection, _radius);
        var hit = hits.FirstOrDefault(o => o.collider == obj);

        if (hit.collider != null)
        {
            closest = hit.point;
        }
        else
        {
            float distance = GetLookDistanceToObject(origin, obj.transform.position); 
            Vector3 point = origin.position + lookDirection.normalized * distance;
            closest = obj.ClosestPoint(point);
        }

        Vector3 direction = closest - origin.position;
        Debug.DrawLine(origin.position, closest, Color.red, 10);
        return Vector3.Angle(origin.forward, direction);
    }

    private float GetLookDistanceToObject(Transform origin, Vector3 position)
    {
        Vector2 from = new Vector2(origin.position.x, origin.position.z);
        Vector2 to = new Vector2(position.x, position.z);
        float distance2d = Vector2.Distance(from, to);
        float angleRad = origin.rotation.x * Mathf.Deg2Rad;
        return distance2d * Mathf.Cos(angleRad);
    }

    private void OnDrawGizmos()
    {
        if (_origin == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(_origin.position, _origin.forward * _radius);
        Gizmos.color = Color.green;
        float angle = _angle * Mathf.Deg2Rad / 2;
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);
        Gizmos.DrawRay(_origin.position, _origin.rotation * new Vector3(sin, 0, cos) * _radius);
        Gizmos.DrawRay(_origin.position, _origin.rotation * new Vector3(-sin, 0, cos) * _radius);
        Gizmos.DrawRay(_origin.position, _origin.rotation * new Vector3(0, sin, cos) * _radius);
        Gizmos.DrawRay(_origin.position, _origin.rotation * new Vector3(0, -sin, cos) * _radius);
    }
}