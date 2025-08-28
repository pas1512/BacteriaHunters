using UnityEngine;

public class RayCastTester : MonoBehaviour
{
    private const int DIRECTION_COUNTS = 4;
    private readonly static Vector3[] DIRECTIONS =
    {
        new Vector3( 0,  0.000f,  1.0f),
        new Vector3( 0, -0.866f, -0.5f),
        new Vector3( 0,  0.866f, -0.5f),
        new Vector3( 0, -1.000f,  0.0f)
    };

    [SerializeField] private float _speed;
    [SerializeField] private Color _color;
    [SerializeField] private float _size;

    void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        transform.Rotate(0, _speed * Time.deltaTime * Mathf.PI * 2, 0);
    }

    private bool GetFar(out Vector3 far)
    {
        far = Vector3.zero;
        float min = float.MaxValue;
        bool any = false;
        Vector3 or = transform.position + (transform.up * 0.5f * _size);

        for (int i = 0; i < DIRECTION_COUNTS; i++)
        {
            if(Physics.Raycast(or, transform.rotation * DIRECTIONS[i], out var hit, _size * 2f)
                && hit.distance < min)
            {
                min = hit.distance;
                far = hit.point;
                or += transform.rotation * (DIRECTIONS[i] * _size * 2f);
                any = true;
            }
        }

        return any;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(transform.position, _size);
        
        Vector3 or = transform.position + (transform.up * 0.5f * _size);

        Gizmos.color = new Color(0, 0.2f, 0.8f);
        Gizmos.DrawRay(or, transform.rotation * DIRECTIONS[0] * _size * 2f);

        or += DIRECTIONS[0] * _size * 2f;
        Gizmos.color = new Color(0, 0.8f, 0.2f);
        Gizmos.DrawRay(or, transform.rotation * DIRECTIONS[1] * _size * 2f);

        or += DIRECTIONS[1] * _size * 2f;
        Gizmos.color = new Color(0.5f, 0.1f, 0.4f);
        Gizmos.DrawRay(or, transform.rotation * DIRECTIONS[2] * _size * 2f);

        or += DIRECTIONS[2] * _size * 2f;
        Gizmos.color = new Color(1, 0, 0);
        Gizmos.DrawRay(or, transform.rotation * DIRECTIONS[3] * _size * 2f);

        if(GetFar(out var val))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(val, 0.01f);
        }
    }
}