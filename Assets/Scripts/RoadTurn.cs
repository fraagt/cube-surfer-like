using UnityEngine;

public class RoadTurn : MonoBehaviour
{
    public TurnDirection TurnDirection
    {
        get => turnDirection;
    }
    [SerializeField]
    private TurnDirection turnDirection;

    public bool IsHovered => isHovered;
    private bool isHovered = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "SurfCube")
        {
            isHovered = true;
        }
    }
}
