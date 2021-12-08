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
    [SerializeField]
    private SurfCubesStack cubesStack;

    [SerializeField]
    private Transform character;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float sideSpeed;

    public Vector3 Direction
    {
        get => direction;
        set => direction = value;
    }
    private Vector3 direction;

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

        movement += SideMovement();

        movement += SideKeyboard();

        transform.position = movement;
    }

    private Vector3 SideKeyboard()
    {
        Vector3 sideMovement = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 moveByAxis = direction.GetPerpendicular();

            moveByAxis.x *= (moveByAxis.x < 0) ? -1 : 1;
            moveByAxis.z *= (moveByAxis.z > 0) ? -1 : 1;

            sideMovement.x += -moveByAxis.x * sideSpeed * 20;
            sideMovement.z += -moveByAxis.z * sideSpeed * 20;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 moveByAxis = direction.GetPerpendicular();

            moveByAxis.x *= (moveByAxis.x < 0) ? -1 : 1;
            moveByAxis.z *= (moveByAxis.z > 0) ? -1 : 1;

            sideMovement.x += moveByAxis.x * sideSpeed * 20;
            sideMovement.z += moveByAxis.z * sideSpeed * 20;
        }

        return sideMovement;
    }

    private Vector3 SideMovement()
    {
        Vector3 sideMovement = Vector3.zero;

        if (Input.touchCount != 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 moveByAxis = direction.GetPerpendicular().Abs();

                sideMovement.x += moveByAxis.x * touch.deltaPosition.x * sideSpeed;
                sideMovement.z += moveByAxis.z * touch.deltaPosition.x * sideSpeed;
            }
        }

        return sideMovement;
    }

    private Vector3 ForwwardMovement()
    {
        return direction * speed * Time.deltaTime;
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

    #region PlayerTurning
    public void Turn(TurnDirection turn, Vector3 anchorVector)
    {
        isFreezed = true;

        StartCoroutine(Turning2(turn.Angle(), anchorVector));

        direction = direction.TurnVector(turn);
    }

    IEnumerator Turning(float angle, Vector3 anchor)
    {
        float radius = AlongDirectionDistance(anchor);
        float quoterCircumference = 0.5f * Mathf.PI * radius;
        float duration = quoterCircumference / speed;
        float stepAngle = angle / (duration / Time.fixedDeltaTime);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.RotateAround(anchor, Vector3.up, stepAngle);

            elapsed += Time.fixedDeltaTime;

            yield return null;
        }

        isFreezed = false;
    }

    IEnumerator Turning2(float angle, Vector3 anchor)
    {
        float duration = Mathf.Abs(angle) / (speed * 10);
        float stepAngle = (speed * 10) * Time.fixedDeltaTime;
        stepAngle *= (angle < 0) ? -1 : 1;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.RotateAround(anchor, Vector3.up, stepAngle);

            elapsed += Time.fixedDeltaTime;

            yield return null;
        }

        isFreezed = false;
    }

    IEnumerator Turning3(float angle, Vector3 anchor)
    {
        float elapsed = 0.0f;
        while (elapsed < 5f)
        {
            Vector3 relativePos = (anchor - transform.position);
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            Quaternion current = transform.localRotation;

            transform.localRotation = Quaternion.Slerp(current, rotation, Time.fixedDeltaTime);
            transform.Translate(0, 0, 3 * Time.fixedDeltaTime);

            elapsed += Time.fixedDeltaTime;

            yield return null;
        }

        isFreezed = false;
    }

    private float AlongDirectionDistance(Vector3 point)
    {
        float distance = 0;

        if (direction.x != 0)
            distance = Mathf.Abs(transform.position.z - point.z);
        if (direction.z != 0)
            distance = Mathf.Abs(transform.position.x - point.x);

        distance = (distance == 0) ? 1 : distance;

        return distance;
    }
    #endregion PlayerTurning

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
