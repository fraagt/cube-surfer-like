using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SurfCubesStack CubesStack
    {
        get => cubesStack;
        
    }

    [SerializeField] private SurfCubesStack cubesStack;

    [SerializeField] private Transform character;

    [SerializeField] private float speed;

    [SerializeField] private float sideSpeed;

    public bool IsFreezed
    {
        get => isFreezed;
    }

    private bool isFreezed;

    public Vector3 Size
    {
        get => GetComponent<BoxCollider>().bounds.size;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < cubesStack.SurfCubes.Count; i++)
        {
            if (cubesStack.SurfCubes[i].IsDead)
            {
                DeleteAftter(i);
                break;
            }
        }
    }

    private void DeleteAftter(int index)
    {
        for (int i = cubesStack.SurfCubes.Count - 1; i >= index; i--)
        {
            Pop();
        }
    }

    private void Update()
    {
        if (!isFreezed)
        {
            Movement();
        }
    }

    public void Movement()
    {
        if (isFreezed) return;

        Vector3 movement = transform.position;

        movement += ForwwardMovement();

        movement += SideKeyboard();

        transform.position = movement;
    }

    private Vector3 SideKeyboard()
    {
        Vector3 sideMovement = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.A))
            sideMovement = Vector3.left;
        if (Input.GetKeyDown(KeyCode.D))
            sideMovement = Vector3.right;

        return sideMovement * sideSpeed;
    }

    private Vector3 ForwwardMovement()
    {
        return Vector3.forward * speed * Time.deltaTime;
    }

    public void PushSurfCube()
    {
        float prevHeight = cubesStack.CalcStackHeight();

        cubesStack.AddCube();

        cubesStack.AlignCubes();

        UpdateYPosition(prevHeight);
    }

    public void PushSurfCubes(List<SurfCube> surfCubes)
    {
        float prevHeight = cubesStack.CalcStackHeight();

        cubesStack.AddCubes(surfCubes);

        cubesStack.AlignCubes();

        UpdateYPosition(prevHeight);
    }

    private void UpdateYPosition(float oldHeight)
    {
        Vector3 deltaY = Vector3.zero;
        deltaY.y = cubesStack.CalcStackHeight() - oldHeight;

        transform.position += deltaY;
    }

    public void Pop()
    {
        cubesStack.Pop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SurfCube")
        {
            SurfCubesStack cubeStack =
                other.transform.GetComponentInParent<SurfCubesStack>();

            if (cubeStack &&
                cubeStack.State == SurfCubesStack.SurfCubeStackState.New)
            {
                PushSurfCubes(cubeStack.SurfCubes);

                Destroy(cubeStack.gameObject);
            }
        }
    }
}
