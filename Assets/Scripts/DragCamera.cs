using UnityEngine;

public class DragCamera : MonoBehaviour
{
    public float dragSpeed = 140;
    public float zoomSpeed = 10000;

    void Start()
    {
        transform.rotation = Quaternion.Euler(30, 45, 0);
        transform.position = new Vector3(-11, 10, -11);
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            var h = -(Input.GetAxis("Mouse X"));
            var v = -(Input.GetAxis("Mouse Y"));

            Vector3 move = new Vector3(h, 0, v) * Time.deltaTime * dragSpeed;
            transform.Translate(move);

        }
        if (transform.position.x < (-10 - DataManager.MAP_SIZE))
        {
            transform.position = new Vector3(-10 - DataManager.MAP_SIZE, 10, transform.position.z);
        }
        if (transform.position.x > (-10 + DataManager.MAP_SIZE))
        {
            transform.position = new Vector3(-10 + DataManager.MAP_SIZE, 10, transform.position.z);
        }
        if (transform.position.z < (-10 - DataManager.MAP_SIZE))
        {
            transform.position = new Vector3(transform.position.x, 10, -10 - DataManager.MAP_SIZE);
        }
        if (transform.position.z > (-10 + DataManager.MAP_SIZE))
        {
            transform.position = new Vector3(transform.position.x, 10, -10 + DataManager.MAP_SIZE);
        }
        transform.position = new Vector3(transform.position.x,
                                          10,
                                          transform.position.z);


        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            this.GetComponent<Camera>().orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed;
            if (this.GetComponent<Camera>().orthographicSize < 1)
            {
                this.GetComponent<Camera>().orthographicSize = 1;
            } else if (this.GetComponent<Camera>().orthographicSize > 50)
            {
                this.GetComponent<Camera>().orthographicSize = 50;
            }
        }
    }

}