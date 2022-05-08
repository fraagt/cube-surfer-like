using UnityEngine;

public class RoadSection : MonoBehaviour
{
    public GameObject Road
    {
        get => road;
    }
    [SerializeField]
    private GameObject road;

    public Bounds RoadBounds
    {
        get => road.GetComponent<MeshRenderer>().bounds;
    }
}
