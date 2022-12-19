using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //public or private
    //data tyoe (int, bool, float string)
    //everey variable has a name
    //optional value assigned
    [SerializeField]
    private float _speed = 3.5f;
    public string playerName = "samaxe";
    public float horizontal;
    public float vertical;
    [SerializeField]
    private GameObject _laserPrefab;
    public Vector3 laserOffset = new Vector3(0, 3, 0);

    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {

        {

        }


        {
            CalculaateMovement();

            if (Input.GetKeyDown(KeyCode.Space))
            {
               Instantiate(_laserPrefab, transform.position  + laserOffset, Quaternion.identity);
            }
        }


        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);


        }

        void CalculaateMovement()

        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            transform.Translate(direction * _speed * Time.deltaTime);

            if (transform.position.y >= 0)
            {
                transform.position = new Vector3(transform.position.x, 0, 0);

            }
            else if (transform.position.y <= -3.8f)
            {
                transform.position = new Vector3(transform.position.x, -3.8f, 0);
            }



            if (transform.position.x > 11f)
            {
                transform.position = new Vector3(-11f, transform.position.y, 0);
            }
            else if (transform.position.x < -11f)
            {
                transform.position = new Vector3(11f, transform.position.y, 0);
            }
        }
    }
}