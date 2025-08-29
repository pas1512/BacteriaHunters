using UnityEngine;

[ExecuteInEditMode]
public class SurfaceTester : MonoBehaviour
{
    [SerializeField] private Transform _testPoint;
    [SerializeField] private float _rayCastLength;
    [SerializeField] private LevelSurface _surface;

    private void Update() => _surface = new LevelSurface(transform);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireMesh(_surface.GetMesh());

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_testPoint.position, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(_testPoint.position, _testPoint.forward * _rayCastLength);

        if(_surface.Raycast(_testPoint.position, _testPoint.forward, _rayCastLength, out var hit))
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(hit.point, 0.1f);
            Gizmos.DrawRay(hit.point, hit.normal);
        }


        Gizmos.color = Color.green;
        Vector3 closest = _surface.GetClosestPosition(_testPoint.position);
        Gizmos.DrawWireSphere(closest, 0.1f);
    }
}