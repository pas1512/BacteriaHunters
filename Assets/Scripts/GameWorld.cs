using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameWorld : MonoBehaviour
{
    [SerializeField] private int _bacterialNumber;
    [SerializeField] private int _spotsNumber;
    [SerializeField] private BacteriumData[] _bacterialVariations;
    [SerializeField] private Material[] _paintsMaterials; 
    private static GameWorld _instance;
    
    public static int bacterials => _instance._bacterialNumber;

    private NativeArray<LevelSurface> _surfaces;
    private bool _surfacesSetuped;

    [SerializeField] private PaintsContainer _paintContainer;
    private NativeArray<PaintPoint> _paints;

    [SerializeField] private BacterialContainer _bacterialsContainer;
    private NativeArray<Bacterium> _bacteria;
    private NativeArray<Vector3> _accelerations;

    private NativeArray<CastingData> _castings;

    public struct DemoRay
    {
        public Vector3 p;
        public Vector3 d;

        public DemoRay(Vector3 p, Vector3 d)
        {
            this.p = p;
            this.d = d;
        }
    }

/*  NativeArray<DemoRay> rays;
    NativeArray<DemoRay> rays1;
    NativeArray<DemoRay> rays2;
    NativeArray<DemoRay> rays3;*/

    //ceches
    int _materialsCount;
    private Dictionary<int, BacteriumData> _types;
    private Dictionary<int, int> _typeIDs;
    private Material[] _materials;
    private List<Matrix4x4>[] _matrixes;
    private Mesh _quad;
    private JobHandle _currentJob;

    public void SetBacteria(int number, BacteriumData[] types)
    {
        _bacterialNumber = number;
        _bacterialVariations = types;
    }

    public IEnumerator Start()
    {
        _instance = this;
        InitCeches();
        yield return new WaitUntil(() => _surfacesSetuped); 

        _bacterialsContainer = new BacterialContainer(_bacterialNumber, _bacterialVariations, _surfaces.ToArray());
        UpdateArray(ref _bacteria, _bacterialsContainer.bacteria);

        _paintContainer = new PaintsContainer();
        UpdateArray(ref _paints, _paintContainer.paints);

        int size = _bacterialsContainer.bacteria.Length;
        UpdateArray(ref _accelerations, size);
        UpdateArray(ref _castings, size);

        StartCoroutine(LiveCycle());
    }

    private void Update()
    {
        for (int i = 0; i < _matrixes.Length; i++)
            Graphics.DrawMeshInstanced(_quad, 0, _materials[i], _matrixes[i]);
    }

    private IEnumerator LiveCycle()
    {
        while (gameObject.activeSelf && enabled)
        {
            Bacterium[] array = _bacterialsContainer.bacteria;
            int currentCount = array.Length;
            _bacterialNumber = _bacteria.Length;
            _spotsNumber = _paints.Length;
            _paintContainer.Update(Time.deltaTime);

            if (_paintContainer.IsChanged())
            {
                _paintContainer.ClearFinished();
                UpdateArray(ref _paints, _paintContainer.paints);
            }

            if (_bacterialsContainer.IsChanged())
            {
                UpdateArray(ref _bacteria, array);
                UpdateArray(ref _accelerations, currentCount);
                UpdateArray(ref _castings, currentCount);
/*                UpdateArray(ref rays, currentCount);
                UpdateArray(ref rays1, currentCount);
                UpdateArray(ref rays2, currentCount);
                UpdateArray(ref rays3, currentCount);*/
            }

            var boidsJob = new InteractionsJob(_bacteria, _paints, _accelerations, _castings);
            var boidsHandle = boidsJob.Schedule(currentCount, 0);

            var moveJob = new MoveJob(_accelerations, _surfaces, _bacteria, Time.deltaTime);
            //moveJob.SetCustst(rays, rays1, rays2, rays3);
            _currentJob = moveJob.Schedule(currentCount, 0, boidsHandle);

            while (!_currentJob.IsCompleted)
                yield return null;

            _currentJob.Complete();
            _bacterialsContainer.SyncData(_bacteria);

            ApplyCastings();
            UpdateView();

            yield return null;
        }
    }

    public static void AddPaint(Vector3 point, Vector3 normal, int color, float size)
    {
        _instance._paintContainer.AddPaint(point, normal, color, size);
    }

    public void SetSurfaces(LevelSurface[] surfaces)
    {
        UpdateArray(ref _surfaces, surfaces);
        _surfacesSetuped = true;
    }

    private void InitCeches()
    {
        _materialsCount = _bacterialVariations.Length + _paintsMaterials.Length;
        _matrixes = new List<Matrix4x4>[_materialsCount];
        _materials = new Material[_materialsCount];

        for (int i = 0; i < _paintsMaterials.Length; i++)
        {
            _materials[i] = _paintsMaterials[i];
            _matrixes[i] = new List<Matrix4x4>();
        }

        for (int i = _paintsMaterials.Length; i < _materialsCount; i++)
        {
            _materials[i] = _bacterialVariations[i - _paintsMaterials.Length].material;
            _matrixes[i] = new List<Matrix4x4>();
        }

        _types = _bacterialVariations.ToDictionary(data => data.typeId, data => data);
        _typeIDs = new Dictionary<int, int>(_types.Count);
        int id = 0;

        foreach (var key in _types.Keys)
            _typeIDs.Add(key, id++);

        _quad = CreateMeshQuad();
    }

    private Mesh CreateMeshQuad()
    {
        Mesh mesh = new Mesh();

        int[] triangles = new int[] { 0, 2, 1, 0, 3, 2 };

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-0.5f, 0, -0.5f),
            new Vector3( 0.5f, 0, -0.5f),
            new Vector3( 0.5f, 0,  0.5f),
            new Vector3(-0.5f, 0,  0.5f)
        };

        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

        Vector3[] normals = new Vector3[]
        {
            Vector3.up, 
            Vector3.up, 
            Vector3.up, 
            Vector3.up
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = normals;

        mesh.RecalculateBounds();
        return mesh;
    }

    private void UpdateArray<T>(ref NativeArray<T> array, T[] values) where T : struct
    {
        if (array.IsCreated)
            array.Dispose();

        array = new NativeArray<T>(values, Allocator.Persistent);
    }

    private void UpdateArray<T>(ref NativeArray<T> array, int number) where T : struct
    {
        if (array.IsCreated)
            array.Dispose();

        array = new NativeArray<T>(number, Allocator.Persistent);
    }

    private void ApplyCastings()
    {
        List<int> removedBacterium = new List<int>();
        List<Bacterium> added = new List<Bacterium>();

        for (int i = 0; i < _castings.Length; i++)
        {
            if (!_castings[i].isExist) continue;

            int targetId = _castings[i].targetId;
            Bacterium current = _bacteria[i];

            if (_castings[i].isBacterium)
            {
                _bacterialsContainer.Transform(targetId, current);
                _bacteria[targetId] = _bacteria[targetId].Transform(current);
                continue;
            }
            else
            {
                _paintContainer.TakePaint(targetId, current.size);

                if (_castings[i].selfDestruction)
                {
                    ItemSpawner.TrySpawnStat(_bacteria[i].position, _bacteria[i].normal);
                    removedBacterium.Add(i);
                }
                else if (Random.Range(0, 4) == 0)
                {
                    added.Add(current.Duplicate());
                }
            }
        }

        _bacterialsContainer.Remove(removedBacterium.ToArray());
        _bacterialsContainer.Add(added.ToArray());
    }

    private void UpdateView()
    {
        for (int i = 0; i < _matrixes.Length; i++)
            _matrixes[i].Clear();

        for (int i = 0; i < _paints.Length; i++)
        {
            PaintPoint current = _paints[i];
            _matrixes[current.color].Add(current.GetTRS());
        }

        for (int i = 0; i < _bacteria.Length; i++)
        {
            Bacterium current = _bacteria[i];
            int currentId = _typeIDs[current.typeId];
            _matrixes[_paintsMaterials.Length + currentId].Add(current.GetTRS());
        }
    }

    private void OnDestroy()
    {
/*        rays.Dispose();
        rays1.Dispose();
        rays2.Dispose();
        rays3.Dispose();*/
        _currentJob.Complete();
        if (_paints.IsCreated) _paints.Dispose();
        if (_bacteria.IsCreated) _bacteria.Dispose();
        if(_accelerations.IsCreated) _accelerations.Dispose();
        if(_castings.IsCreated) _castings.Dispose();
        if(_surfaces.IsCreated) _surfaces.Dispose();
    }

    private void OnGUI()
    {
/*        GUILayout.Label($"Bacterial: {_bacterialNumber}");
        GUILayout.Label($"Spots: {_spotsNumber}");*/
    }

    private void OnDrawGizmos()
    {
/*        if (rays == default || rays.Length <= 0)
            return;

        for (int i = 0; i < rays.Length; i++)
        {
            Gizmos.DrawRay(rays[i].p, rays[i].d);
            Gizmos.DrawRay(rays1[i].p, rays1[i].d);
            Gizmos.DrawRay(rays2[i].p, rays2[i].d);
            Gizmos.DrawRay(rays3[i].p, rays3[i].d);
        }*/
    }
}