using UnityEngine;

public class MovementPrototype : MonoBehaviour
{
    private const int DIRECTION_COUNTS = 4;
    private readonly static Vector3[] DIRECTIONS =
    {
        new Vector3( 0,  0.000f,  1.0f),
        new Vector3( 0, -0.866f, -0.5f),
        new Vector3( 0,  0.866f, -0.5f),
        new Vector3( 0, -1.000f,  0.0f)
    };


    [SerializeField] private float _maxDistance;
    [SerializeField] private float _noiseFrq;
    [SerializeField] private float _noiseAmp;
    [SerializeField] private float _radius;
    [SerializeField] private Color _color = (Color.green / 2 + Color.blue / 2);
    [SerializeField] private float _speed;

    private bool SearchNextSurface(out Vector3 pos, out Vector3 normal)
    {
        pos = Vector3.zero;
        normal = Vector3.zero;

        if (Physics.Raycast(transform.position, transform.forward, out var hit, _radius * 1.25f))
        {
            pos = hit.point;
            normal = hit.normal;
            return true;
        }

        return false;
    }

    private bool SearchNextSurface2(out Vector3 position, out Vector3 normal, float radius)
    {
        position = Vector3.zero;
        normal = Vector3.zero;
        float min = float.MaxValue;
        bool any = false;
        Vector3 origin = transform.position;

        for (int i = 0; i < DIRECTION_COUNTS; i++)
        {
            if (Physics.Raycast(origin, transform.rotation * DIRECTIONS[i], out var hit, radius)
                && hit.distance < min)
            {
                min = hit.distance;
                position = hit.point;
                normal = hit.normal;
                any = true;
            }

            origin += transform.rotation * (DIRECTIONS[i] * radius);
        }

        return any;
    }

    private void Update()
    {
        float angle = Vector3.SignedAngle(transform.forward, (-transform.position).normalized, Vector3.up);
        float distance = transform.position.magnitude / _maxDistance;
        distance = Mathf.Pow(Mathf.Clamp01(distance), 3);

        float noise = Mathf.PerlinNoise1D(Time.time * _noiseFrq);
        noise = ((noise * 2) - 1) * _noiseAmp;

        if (SearchNextSurface2(out var pos, out var normal, _radius + _speed * 0.02f))
        {
            if (Vector3.Distance(normal, transform.up) > 0.001f)
            {
                transform.position = pos + (normal * _radius);
                Quaternion rot = Quaternion.FromToRotation(transform.up, normal);
                transform.rotation = rot * transform.rotation;
            }
        }

        transform.Rotate(0, Mathf.Lerp(noise, angle, distance), 0);
        transform.Translate(0, 0, _speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(transform.position, _radius);

        Vector3 origin = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, transform.rotation * DIRECTIONS[0] * _speed);
        origin += transform.rotation * DIRECTIONS[0] * _speed;

        Gizmos.color = Color.red / 2 + Color.yellow / 2;
        Gizmos.DrawRay(origin, transform.rotation * DIRECTIONS[1] * _speed);
        origin += transform.rotation * DIRECTIONS[1] * _speed;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(origin, transform.rotation * DIRECTIONS[2] * _speed);
        origin += transform.rotation * DIRECTIONS[2] * _speed;

        Gizmos.color = Color.green * 0.71f + Color.blue * 0.29f;
        Gizmos.DrawRay(origin, transform.rotation * DIRECTIONS[3] * _speed);

        if (SearchNextSurface2(out var pos, out var normal, _speed))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pos, 0.03f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(pos, normal * 0.06f);
        }
    }
}