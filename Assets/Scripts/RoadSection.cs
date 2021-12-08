using UnityEngine;

public class RoadSection : MonoBehaviour
{
    public GameObject Road
    {
        get => road;
    }
    [SerializeField]
    private GameObject road;

    public RoadTurn Turn
    {
        get => turn;
    }
    [SerializeField]
    private RoadTurn turn;

    public Bounds RoadBounds
    {
        get => road.GetComponent<MeshRenderer>().bounds;
    }
    public Bounds TurnBounds
    {
        get => turn.GetComponent<MeshRenderer>().bounds;
    }
}
