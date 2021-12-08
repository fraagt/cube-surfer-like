using UnityEngine;

public class SurfCube : MonoBehaviour
{
    public const string Name = "SurfCube";

    public Bounds Bounds
    {
        get => bounds;
    }
    private Bounds bounds;

    private void Awake()
    {
        bounds =
            GetComponent<MeshRenderer>().bounds;
        IsDead = false;
    }

    public bool IsDead
    {
        get => isDead;
        set
        {
            if (value)
            {
                Destroy(this, 10f);
            }

            isDead = value;
        }
    }
    private bool isDead;

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal != Vector3.up &&
                contact.normal != Vector3.down)
            {
                if(contact.point.y < transform.position.y)
                {
                    continue;
                }

                if (collision.gameObject.tag == "Barrier")
                {
                    IsDead = true;
                }
            }
        }
    }
}
