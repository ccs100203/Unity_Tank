using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float DampTime = 0.2f;                 
    public float ScreenEdgeBuffer = 4f;           
    public float MinSize = 6.5f;                  
    public Transform[] Tanks; // All Tanks in Main Scene


    private Camera myCamera;                        
    private float m_ZoomSpeed; // No Use                
    private Vector3 MoveVelocity; // No Use   
    private Vector3 Destination;              
    

    private void Awake()
    {
        myCamera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, Destination, ref MoveVelocity, DampTime);
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int nums = 0;

        for (int i = 0; i < Tanks.Length; ++i)
        {
            if (!Tanks[i].gameObject.activeSelf)
                continue;

            averagePos += Tanks[i].position;
            ++nums;
        }

        if (nums > 0)
            averagePos /= nums;

        averagePos.y = transform.position.y;

        Destination = averagePos;
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        myCamera.orthographicSize = Mathf.SmoothDamp(myCamera.orthographicSize, requiredSize, ref m_ZoomSpeed, DampTime);
    }


    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(Destination);

        float size = 0f;

        for (int i = 0; i < Tanks.Length; i++)
        {
            if (!Tanks[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(Tanks[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / myCamera.aspect);
        }
        
        size += ScreenEdgeBuffer;

        size = Mathf.Max(size, MinSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = Destination;

        myCamera.orthographicSize = FindRequiredSize();
    }
}