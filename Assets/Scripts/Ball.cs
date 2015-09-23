using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    //test
    Vector3 m_dir;
    Vector3 m_posOfPlayer;
    Vector3 m_posOfCollision;


    //params
    private float m_speed;

    //components
    Rigidbody m_rigidBody;
    RaycastHit m_hit;

    //*******properties

    public float Speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }
    //*******properties


    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        StartPosition();

        Speed = 15f;

    }

    public void StartPosition()
    {
        transform.position = new Vector3(0, 0.2f, 0);
        transform.Rotate(new Vector3(0, Random.Range(30, 150) * (new int[] { -1, 1 }[Random.Range(0, 1)])));
    }
    void Update()
    {
        if (GameManager.Instance.State == StateType.mainGame)
        {
            if (m_rigidBody.SweepTest(transform.forward, out m_hit, Speed * 0.05f))
            {
                if (m_hit.collider.tag == "Player")
                {
                    Kicked();
                }

                else if (m_hit.collider.tag == "Wall")
                {
                    Bounce();
                }
                else
                    m_rigidBody.MovePosition(m_rigidBody.position + transform.forward * Time.deltaTime * Speed);
            }
            else
            {
                m_rigidBody.MovePosition(m_rigidBody.position + transform.forward * Time.deltaTime * Speed);
            }
        }
    }

    void Bounce()
    {
        float dotProduct = Mathf.Clamp(Vector3.Dot(-transform.forward.normalized, m_hit.normal.normalized), -1, 1); //dot procuct(cosine) between direction and hit normal
        float angle = Mathf.Round(Mathf.Acos(dotProduct) * Mathf.Rad2Deg); //half angle

        float rotateAngle = 180 - (2 * angle); // full angle of rotation

        Vector3 crossProduct = Vector3.Cross(transform.forward.normalized, m_hit.normal.normalized); // axis of rotation(angle can be + or -)
        crossProduct.x = 0; //correction due to calc errors
        crossProduct.z = 0;

        transform.Rotate(crossProduct, rotateAngle); //perform rotation around axis
    }

    void Kicked()
    {
        //Bounce();

        //collision point
        m_posOfCollision = m_hit.point;
        m_posOfCollision.y = 0;

        //player point
        m_posOfPlayer = m_hit.collider.transform.position;
        m_posOfPlayer.y = 0;

        //direction
        m_dir = m_posOfCollision - m_posOfPlayer;
        m_dir.y = 0;
        m_dir.Normalize();
        //transform.LookAt(m_dir);

        Vector3 axis = Vector3.Cross(transform.forward, m_dir);
        axis.x = 0;
        axis.z = 0;
        transform.Rotate(axis, Mathf.Round(Mathf.Acos(Mathf.Clamp(Vector3.Dot(transform.forward, m_dir), -1, 1)) * Mathf.Rad2Deg));
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(m_posOfPlayer, m_dir * 100);
    }

}

