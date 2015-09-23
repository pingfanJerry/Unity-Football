using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public abstract class Player : MonoBehaviour
{
    //params
    protected float m_speed;

    private float m_maxDistance;

    //components
    protected Rigidbody m_rigidBody;
    protected RaycastHit m_hit;

    // Use this for initialization
    void Start()
    {
        m_speed = 10f;

        m_rigidBody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = HandleInput();
        m_maxDistance = m_speed;

        if (input != Vector3.zero)
        {
            if (m_rigidBody.SweepTest(input, out m_hit, m_speed * 0.05f))
            {
                if (m_hit.collider.tag == "Ball" || m_hit.collider.tag == "Wall")
                {
                    m_maxDistance = 0;
                }
            }
            m_rigidBody.MovePosition(m_rigidBody.position + input * m_maxDistance * Time.deltaTime);
        }

    }

    public abstract Vector3 HandleInput();
}
