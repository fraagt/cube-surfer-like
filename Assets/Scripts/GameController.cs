using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Roads _roads;
    [SerializeField] private CameraFollower _playerCamera;
    [SerializeField] private GameScreenView _gameScreenView;

    private void Start()
    {
        ShowMainMenu();
        InitPlayer();
    }

    private void OnEnable()
    {
        _player.CubesStack.onSurfCubeCountChanged += OnSurfCubeCountChanged;
    }

    private void OnDisable()
    {
        _player.CubesStack.onSurfCubeCountChanged -= OnSurfCubeCountChanged;
    }

    private void OnSurfCubeCountChanged(int cubesCount)
    {
        _gameScreenView.SetScore(cubesCount);
    }

    private void InitPlayer()
    {
        var roadBounds = _roads.CurrentRoad.RoadBounds;

        Vector3 playerPos = roadBounds.center;
        playerPos.y = 1;
        playerPos.z -= roadBounds.extents.z;

        _player.transform.position = playerPos;
        _player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        _player.IsFreezed = true;

        _gameScreenView.SetScore(_player.CubesStack.SurfCubes.Count);
    }

    private void FixedUpdate()
    {
        if (_player.IsFreezed) return;

        UpdateCameraTargets();
    }

    private void ShowMainMenu()
    {
        
    }

    private void UpdateCameraTargets()
    {
        _playerCamera.targets = GetSurfCubeTransforms();
    }

    private List<Transform> GetSurfCubeTransforms()
    {
        List<Transform> surfCubeTransforms =
            new List<Transform>();

        foreach (SurfCube surfCube in _player.CubesStack.SurfCubes)
        {
            surfCubeTransforms.Add(surfCube.transform);
        }

        return surfCubeTransforms;
    }

    private void Update()
    {
        if (_player.IsFreezed) return;

        OutOfBounds();
    }

    private void OutOfBounds()
    {
        Bounds roadBounds =
            _roads.CurrentRoad.RoadBounds;
        Vector3 playerPos =
            _player.transform.position - roadBounds.center;
        Vector3 roadCornerPoint =
            roadBounds.extents - (_player.Size / 2);

        float difference =
            playerPos.magnitude - roadCornerPoint.magnitude;
        if (difference > 0)
        {
            Vector3 playerBounds =
                _player.transform.position;

            _player.transform.position = playerBounds;
        }
    }
}
