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
        float angle = roads.CurrentRoad.transform.rotation.eulerAngles.y;
        player.Direction = angle.Direction();

        var roadBounds = roads.CurrentRoad.RoadBounds;

        Vector3 playerPos = roadBounds.center;
        playerPos.y = 1;
        playerPos.x -= player.Direction.x * roadBounds.extents.x;
        playerPos.z -= player.Direction.z * roadBounds.extents.z;

        player.transform.position = playerPos;
        player.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }

    private void FixedUpdate()
    {
        if (player.IsFreezed) return;

        UpdateCameraTargets();

        if (roads.CurrentRoad.Turn.IsHovered)
        {
            MakePlayerTurn();
        }
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

    private void MakePlayerTurn()
    {
        if (roads.CurrentRoad.Turn.TurnDirection != TurnDirection.Forward)
        {
            Vector3 anchorPos =
                roads.CurrentRoad.TurnBounds.center;
            Vector3 turnExtentsSign =
                player.Direction;
            turnExtentsSign -= player.Direction.TurnVector(roads.CurrentRoad.Turn.TurnDirection);
            turnExtentsSign.Scale(roads.CurrentRoad.TurnBounds.extents);
            anchorPos -= turnExtentsSign;

            player.Turn(roads.CurrentRoad.Turn.TurnDirection, anchorPos);
        }

        roads.SetNextSection();
    }

    private void Update()
    {
        if (player.IsFreezed) return;

        OutOfBounds();
    }

    private bool IsCurrentRoadContains(Vector3 point)
    {
        return roads.CurrentRoad.RoadBounds.Contains(point);
    }

    private void OutOfBounds()
    {
        Bounds roadBounds =
            roads.CurrentRoad.RoadBounds;
        Vector3 playerSide =
            player.Direction.GetPerpendicular().Abs();
        Vector3 playerPos =
            player.transform.position - roadBounds.center;
        Vector3 roadCornerPoint =
            roadBounds.extents - (player.Size / 2);

        playerPos.Scale(playerSide);
        roadCornerPoint.Scale(playerSide);

        float difference =
            playerPos.magnitude - roadCornerPoint.magnitude;
        if (difference > 0)
        {
            Vector3 playerBounds =
                player.transform.position;
            playerSide = playerSide.Scale(difference);
            playerSide.Scale(playerPos.ToSign());

            playerBounds -= playerSide;

            player.transform.position = playerBounds;
        }
    }
}
