using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class BacterialFauna : MonoBehaviour
{
    [SerializeField] private BacteriumOptions[] _scriptables;
    [SerializeField] private int _number;

    private bool _inited;
    private int _prevCount = 0;

    [SerializeField] private PaintableSurface[] _surfaceTransforms;
    private NativeArray<CastableSurface> _surfaces;

    private List<Bacterium> _bacteriaList;
    private TransformAccessArray _transforms;
    private NativeArray<BacteriumData> _bacteria;
    private NativeArray<Vector3> _accelerations;
    private NativeArray<FeedingData> _feedings;
    private NativeArray<PaintsData> _paints;

    public int bacteriaCount => _bacteriaList.Count;

    private void OnEnable() => StartCoroutine(LiveCycle());

    private void Start()
    {
        _surfaceTransforms = FindSurfaces(transform);
        _bacteriaList = new List<Bacterium>(_number);
        _surfaces = new NativeArray<CastableSurface>(_surfaceTransforms.Length, Allocator.Persistent);

        for (int i = 0; i < _surfaceTransforms.Length; i++)
            _surfaces[i] = new CastableSurface(_surfaceTransforms[i].transform);

        for (int i = 0; i < _number; i++)
        {
            int prefabId = Random.Range(0, _scriptables.Length);
            Vector3 poistion = GetRandomPositions(_surfaceTransforms);
            var bacterium = Instantiate(_scriptables[prefabId].prefab, poistion, Quaternion.identity);
            bacterium.SetData(_scriptables[prefabId]);
            _bacteriaList.Add(bacterium);
        }

        InitNattiveArrays();
        _inited = true;
    }

    private IEnumerator LiveCycle()
    {
        yield return new WaitUntil(() => _inited);

        while(gameObject.activeSelf && enabled)
        {
            int currentCount = _bacteriaList.Count;

            if(currentCount != _prevCount)
            {
                _prevCount = currentCount;
                UpdateNattiveArrays();
                yield return null;
            }

            if(_paints.IsCreated) _paints.Dispose();
            _paints = new NativeArray<PaintsData>(GetPaints(_surfaceTransforms), Allocator.Persistent);

            var boidsJob = new BoidsJob(_bacteria, _paints, _accelerations, _feedings);
            var boidsHandle = boidsJob.Schedule(currentCount, 0);

            var moveJob = new MoveJob(_accelerations, _surfaces, _bacteria, Time.deltaTime);
            var moveHandle = moveJob.Schedule(_transforms, boidsHandle);

            while (!moveHandle.IsCompleted)
                yield return null;

            moveHandle.Complete();

            for (int i = 0; i < currentCount; i++)
                _bacteriaList[i].ReturnData(_bacteria[i]);

            ApplyFeeds(_feedings, _paints);

            yield return null;
        }
    }

    private PaintableSurface[] FindSurfaces(Transform transform)
    {
        int size = transform.childCount;
        List<PaintableSurface> surfaces = new List<PaintableSurface>(size);

        for (int i = 0; i < size; i++)
        {
            Transform child = transform.GetChild(i);
            surfaces.AddRange(FindSurfaces(child));

            if (child.TryGetComponent(out PaintableSurface paintable))
                surfaces.Add(paintable);
        }

        return surfaces.ToArray();
    }

    private PaintsData[] GetPaints(PaintableSurface[] surfaces)
    {
        List<PaintsData> paints = new List<PaintsData>();

        for (int s = 0; s < surfaces.Length; s++)
        {
            var surfacePaints = surfaces[s].paints;

            for (int p = 0; p < surfacePaints.Length; p++)
                paints.Add(new PaintsData(s, p, surfaces[s].transform.position, surfacePaints[p]));
        }
        
        return paints.ToArray();
    }

    private Vector3 GetRandomPositions(PaintableSurface[] surfaces)
    {
        int randomId = Random.Range(0, surfaces.Length);
        Transform transform = surfaces[randomId].transform;
        float u = Random.Range(0.0f, 1.0f);
        float v = Random.Range(0.0f, 1.0f);

        Vector3 origin = transform.position - transform.rotation * (transform.localScale / 2);
        Vector3 offset = transform.localScale;
        offset.x *= u;
        offset.y *= v;
        
        return origin + transform.rotation * offset;
    }

    private void InitNattiveArrays()
    {
        int bacteriaCount = _bacteriaList.Count;
        Transform[] transformsArray = new Transform[bacteriaCount];
        _bacteria = new NativeArray<BacteriumData>(bacteriaCount, Allocator.Persistent);

        for (int i = 0; i < bacteriaCount; i++)
        {
            _bacteria[i] = new BacteriumData(_bacteriaList[i]);
            transformsArray[i] = _bacteriaList[i].transform;
        }

        _transforms = new TransformAccessArray(transformsArray);
        _accelerations = new NativeArray<Vector3>(bacteriaCount, Allocator.Persistent);
        _feedings = new NativeArray<FeedingData>(bacteriaCount, Allocator.Persistent);
    }

    private void UpdateNattiveArrays()
    {
        int bacteriaCount = _bacteriaList.Count;
        Transform[] transformsArray = new Transform[bacteriaCount];

        _bacteria.Dispose();
        _bacteria = new NativeArray<BacteriumData>(bacteriaCount, Allocator.Persistent);

        for (int i = 0; i < bacteriaCount; i++)
        {
            _bacteria[i] = new BacteriumData(_bacteriaList[i]);
            transformsArray[i] = _bacteriaList[i].transform;
        }

        _transforms.Dispose();
        _transforms = new TransformAccessArray(transformsArray);

        _accelerations.Dispose();
        _accelerations = new NativeArray<Vector3>(bacteriaCount, Allocator.Persistent);

        _feedings.Dispose();
        _feedings = new NativeArray<FeedingData>(bacteriaCount, Allocator.Persistent);
    }

    private void ApplyFeeds(NativeArray<FeedingData> feedings, NativeArray<PaintsData> paints)
    {
        List<Bacterium> copied = new List<Bacterium>(_bacteriaList);

        for (int i = 0; i < feedings.Length; i++)
        {
            if (!feedings[i].inited) continue;

            if (feedings[i].active)
            {
                if (feedings[i].isBacterium)
                {
                    int changedId = feedings[i].targetId;
                    _bacteriaList[changedId].SetData(_bacteriaList[i].data);
                    _bacteria[changedId] = new BacteriumData(_bacteriaList[changedId]);
                }
                else if(Random.Range(0, 4) == 0)
                {
                    Vector3 offset = Random.insideUnitCircle * _bacteriaList[i].size;
                    Vector3 position = _bacteriaList[i].transform.position + _bacteriaList[i].transform.rotation * offset;
                    var bacterium = Instantiate(_bacteriaList[i].data.prefab, position, Quaternion.identity);
                    bacterium.SetData(_bacteriaList[i].data);
                    copied.Add(bacterium);
                }
            }
            else
            {
                var removed = _bacteriaList[i];
                copied.Remove(removed);
                Destroy(removed.gameObject);
            }

            if (!feedings[i].isBacterium)
            {
                PaintsData paint = paints[feedings[i].targetId];
                var surface = _surfaceTransforms[paint.surfaceId];
                surface.TakePaint(paint.paintId, _bacteria[i].size);
            }
        }

        _bacteriaList = copied;
    }

    private void OnDestroy()
    {
        _surfaces.Dispose();
        _bacteria.Dispose();
        _transforms.Dispose();
        _accelerations.Dispose();
        _feedings.Dispose();

        if(_paints.IsCreated) _paints.Dispose();
    }
}