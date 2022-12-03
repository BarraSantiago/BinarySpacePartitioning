using System;
using UnityEngine;

public class IdentifySideOnPlane : MonoBehaviour
{
    [SerializeField] private PlaneCreator[] planes;
    public bool isActive;
    private Mesh _meshFilter;
    private Vector3[] _vertices;
    private Vector3[] _objectVertices;
    private Material _renderMat;
    private int _color1;
    public Mesh GetMesh => _meshFilter;
    [SerializeField] public float minX, maxX, minY, maxY, minZ, maxZ;

    private void Awake()
    {
        _renderMat = GetComponent<MeshRenderer>().material;
        _meshFilter = GetComponent<MeshFilter>().mesh;
    }

    public void Start()
    {
        _objectVertices = new Vector3[_meshFilter.vertices.Length];
        _color1 = Shader.PropertyToID("_Color");
    }

    void Update()
    {
        CheckInsideOfBounds();
        CheckMinMax();
    }

    void CheckMinMax()
    {
        minX = Single.MaxValue;
        maxX = Single.MinValue;
        minY = Single.MaxValue;
        maxY = Single.MinValue;
        minZ = Single.MaxValue;
        maxZ = Single.MinValue;

        foreach (var t in _vertices)
        {
            if (minX > transform.TransformPoint(t).x) minX = transform.TransformPoint(t).x;
            if (maxX < transform.TransformPoint(t).x) maxX = transform.TransformPoint(t).x;
            if (minY > transform.TransformPoint(t).y) minY = transform.TransformPoint(t).y;
            if (maxY < transform.TransformPoint(t).y) maxY = transform.TransformPoint(t).y;
            if (minZ > transform.TransformPoint(t).z) minZ = transform.TransformPoint(t).z;
            if (maxZ < transform.TransformPoint(t).z) maxZ = transform.TransformPoint(t).z;
        }
    }

    void CheckInsideOfBounds()
    {
        _vertices = _meshFilter.vertices;

        for (var i = 0; i < _vertices.Length; i++)
        {
            _objectVertices[i] = transform.TransformPoint(_vertices[i]);
        }

        CheckFrustum();
    }

    public bool CheckFrustum()
    {
        foreach (PlaneCreator t in planes)
        {
            if (IsInFrustum(t))
            {
                isActive = false;
                _renderMat.SetColor(_color1, Color.red);
                return false;
            }

            isActive = true;
            _renderMat.SetColor(_color1, Color.green);
        }

        return true;
    }

    public bool IsInFrustum(PlaneCreator planeCreator)
    {
        int counter = 0;
        foreach (Vector3 t1 in _objectVertices)
        {
            if (!planeCreator.Plane.GetSide(t1))
            {
                counter++;
            }
        }

        return counter >= _meshFilter.vertices.Length;

    }
}