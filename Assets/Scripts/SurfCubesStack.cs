using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurfCubesStack : MonoBehaviour
{
    public enum SurfCubeStackState
    {
        New,
        Picked,
        Left
    }

    public Action<int> onSurfCubeCountChanged;

    [SerializeField] private GameObject surfCubePrefab;

    public List<SurfCube> SurfCubes
    {
        get => surfCubes;
    }

    private List<SurfCube> surfCubes;

    public SurfCubeStackState State
    {
        get => state;
        set => state = value;
    }

    [SerializeField] private SurfCubeStackState state;

    private void Awake()
    {
        surfCubes =
            gameObject.GetComponentsInChildren<SurfCube>().ToList();
    }

    public void AddCube()
    {
        var cubeObj = Instantiate(surfCubePrefab, transform);
        var cube = cubeObj.GetComponent<SurfCube>();

        surfCubes.Add(cube);

        cubeObj.name = "SurfCube" + surfCubes.Count;

        ReCalcCollider();
    }

    public void AddCube(SurfCube surfCube)
    {
        surfCube.transform.SetParent(transform);
        surfCubes.Add(surfCube);
        onSurfCubeCountChanged.Invoke(surfCubes.Count);
        surfCube.name = SurfCube.Name + surfCubes.Count;

        ReCalcCollider();
    }

    public void AddCubes(List<SurfCube> surfCubes)
    {
        foreach (SurfCube surfCube in surfCubes)
        {
            AddCube(surfCube);
        }

        ReCalcCollider();
    }

    public void Pop()
    {
        var cube = surfCubes[surfCubes.Count - 1];

        surfCubes.RemoveAt(surfCubes.Count - 1);
        onSurfCubeCountChanged.Invoke(surfCubes.Count);

        cube.transform.SetParent(null);

        ReCalcCollider();
    }

    public void AlignCubes()
    {
        Vector3 position = Vector3.zero;

        foreach (SurfCube surfCube in surfCubes)
        {
            float currHalfSize = surfCube.Bounds.size.y / 2f;
            position.y -= currHalfSize;

            surfCube.transform.localPosition =
                position;

            position.y -= currHalfSize;
        }
    }

    public float CalcStackHeight()
    {
        float height = 0;

        foreach (SurfCube surfCube in surfCubes)
        {
            height += surfCube.Bounds.size.y;
        }

        return height;
    }

    public void ReCalcCollider()
    {
        Vector3 size = GetComponent<BoxCollider>().size;
        size.y = CalcStackHeight();

        Vector3 center = Vector3.zero;
        center.y -= size.y / 2f;

        GetComponent<BoxCollider>().center = center;
        GetComponent<BoxCollider>().size = size;
    }
}
