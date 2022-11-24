using System;
using UnityEngine;

public class IdentifySideOnPlane : MonoBehaviour
{
    [SerializeField] private PlaneCreator[] planes;
    public bool isActive;
    private Mesh _mesh;
    private Vector3[] _vertices;
    private Vector3[] _objectVertices;
    private GameObject _myObject;
    private Material _renderMat;
    private int _color1;

    [SerializeField] public float minX, maxX, minY, maxY, minZ, maxZ;

    public void Start()
    {
        _myObject = gameObject;
        _mesh = _myObject.GetComponent<MeshFilter>().mesh;
        _objectVertices = new Vector3[_mesh.vertices.Length];
        _renderMat = GetComponent<MeshRenderer>().material;
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
        _vertices = _mesh.vertices;

        _mesh = _myObject.GetComponent<MeshFilter>().mesh;

        for (var i = 0; i < _vertices.Length; i++)
        {
            _objectVertices[i] = transform.TransformPoint(_vertices[i]);
        }

        foreach (var t in planes)
        {
            int counter = 0;
            foreach (var t1 in _objectVertices)
            {
                if (!t.Plane.GetSide(t1))
                {
                    counter++;
                }
            }

            if (counter >= _mesh.vertices.Length)
            {
                isActive = false;
                _renderMat.SetColor(_color1, Color.red);
                break;
            }

            isActive = true;
            _renderMat.SetColor(_color1, Color.green);
        }
    }
}