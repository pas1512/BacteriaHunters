using UnityEngine;

public struct Bacterium
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

    public Bacterium(BacteriumData bacterium)
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
        _position = Vector3.zero;
        _rotation = Quaternion.identity;
        _velocity = Random.insideUnitSphere;
    }

    public Bacterium(BacteriumData data, Vector3 position) : this(data)
    {
        _position = position;
    }

    public Bacterium(Bacterium other)
    {
        typeId = other.typeId;
        size = other.size;
        lookArea = other.lookArea;
        speed = other.speed;
        targets = other.targets;
        enemies = other.enemies;
        boidsWheights = other.boidsWheights;
        pointsWheights = other.pointsWheights;
        navigationWheights = other.navigationWheights;
        _normal = other.normal;
        _rotation = other.rotation;
        _velocity = other.velocity;
        _position = other.position;
    }

    public Bacterium Duplicate()
    {
        Bacterium reuslt = new Bacterium(this);
        Vector3 offset = Random.insideUnitCircle * size;
        offset.z = offset.y;
        offset.y = 0;
        reuslt._position = position + rotation * offset;
        return reuslt;
    }

    public Bacterium Transform(Bacterium other)
    {
        Bacterium result = new Bacterium(other);
        result._position = _position;
        result._normal = _normal;
        result._rotation = _rotation;
        result._velocity = _velocity;
        return result;
    }

    public void UpdateNormal(Vector3 normal, Vector3 hit)
    {
        Vector3 oldNormal = _normal;
        _normal = normal.normalized;
        _position = hit + _normal * 0.01f;

        if (Vector3.Angle(oldNormal, _normal) > 0.1f)
        {
            _rotation = Quaternion.FromToRotation(Vector3.up, _normal);
            _velocity = Quaternion.FromToRotation(oldNormal, _normal) * _velocity;
            _position += _velocity * size * 0.05f;
        }
    }

    public void ApplyAcceleration(Vector3 acceleration, float deltaTime)
    {
        _velocity += acceleration * deltaTime;
        float oldMagnitude = _velocity.magnitude;
        _velocity = Vector3.ProjectOnPlane(_velocity, _normal).normalized * oldMagnitude;
        _velocity = Vector3.ClampMagnitude(velocity, speed);
        _position += _velocity * deltaTime;
    }

    public Matrix4x4 GetTRS() => Matrix4x4.TRS(_position, _rotation, Vector3.one * size);
}