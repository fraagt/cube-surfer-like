using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private Roads roads;

    [SerializeField]
    private CameraFollower playerCamera;

    private void Start()
    {
        InitPlayer();

        var cubes = new List<SurfCube>();
        for (int i = 0; i < 5; i++)
        {
            var cube = Instantiate(player.CubesStack.SurfCubes[0]);
            cubes.Add(cube);
        }
        
        player.PushSurfCubes(cubes);
    }

    private void InitPlayer()
    {
        var roadBounds = roads.CurrentRoad.RoadBounds;

        Vector3 playerPos = roadBounds.center;
        playerPos.y = 1;
        playerPos.x -= roadBounds.extents.x;
        playerPos.z -= roadBounds.extents.z;

        player.transform.position = playerPos;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    private void FixedUpdate()
    {
        if (player.IsFreezed) return;

        UpdateCameraTargets();
    }

    private void UpdateCameraTargets()
    {
        playerCamera.targets = GetSurfCubeTransforms();
    }

    private List<Transform> GetSurfCubeTransforms()
    {
        List<Transform> surfCubeTransforms =
            new List<Transform>();

        foreach(SurfCube surfCube in player.CubesStack.SurfCubes)
        {
            surfCubeTransforms.Add(surfCube.transform);
        }

        return surfCubeTransforms;
    }

    private void Update()
    {
        if (player.IsFreezed) return;

        OutOfBounds();
    }

    private void OutOfBounds()
    {
        Bounds roadBounds =
            roads.CurrentRoad.RoadBounds;
        Vector3 playerPos =
            player.transform.position - roadBounds.center;
        Vector3 roadCornerPoint =
            roadBounds.extents - (player.Size / 2);

        float difference =
            playerPos.magnitude - roadCornerPoint.magnitude;
        if (difference > 0)
        {
            Vector3 playerBounds =
                player.transform.position;

            player.transform.position = playerBounds;
        }
    }
}
