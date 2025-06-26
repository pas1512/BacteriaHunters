using UnityEngine;

public struct BacteriumData
{
    public readonly int typeId;
    public readonly float size;
    public readonly float lookArea;
    public readonly float speed;
    public readonly TargetsSet targets;
    public readonly TargetsSet enemies;
    public readonly Vector3 boidsWheights;
    public readonly Vector2 pointsWheights;
    public readonly Vector2 navigationWheights;

    private Vector3 _normal;
    public Vector3 normal => _normal;

    private Vector3 _position;
    public Vector3 position => _position;

    private Vector3 _velocity;
    public Vector3 velocity => _velocity;

    private Quaternion _rotation;
    public Quaternion rotation => _rotation;

    public BacteriumData(Bacterium bacterium)
    {
        typeId = bacterium.typeId;
        size = bacterium.size;
        lookArea = bacterium.lookArea;
        speed = bacterium.speed;
        targets = bacterium.targets;
        enemies = bacterium.enemies;
        boidsWheights = bacterium.boidsWheights;
        pointsWheights = bacterium.pointsWheights;
        navigationWheights = bacterium.navigationWheights;

        _normal = Vector3.up;
        _position = bacterium.transform.position;
        _rotation = bacterium.transform.rotation;
        _velocity = bacterium.velcity;
    }

    public void UpdateNormal(Vector3 normal, Vector3 hit)
    {
        Vector3 oldNormal = _normal;
        _normal = normal.normalized;
        _position = hit + _normal * 0.01f;

        if (Vector3.Angle(_normal, normal) > 0.01f)
        {
            _rotation = Quaternion.FromToRotation(Vector3.up, _normal);
            _velocity = Quaternion.FromToRotation(oldNormal, _normal) * _velocity;
            _position += _velocity * size * 0.05f;
        }
    }

    public void ApplyAcceleration(Vector3 acceleration, float deltaTime)
    {
        _velocity += acceleration * deltaTime;
        float speed = _velocity.magnitude;
        speed = Mathf.Clamp(speed, 0, this.speed);
        _velocity = Vector3.ProjectOnPlane(_velocity, _normal).normalized;
        _position += _velocity * speed * deltaTime;
    }
}