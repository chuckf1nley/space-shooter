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
    private GameObject _laserPrefab;
    public float horizontal;
    public float vertical;

    // Start is called before the first frame update
    void Start()
    {
        //if player positon

     transform.position = new Vector3(0, 0, 0);



    }

    // Update is called once per frame
    void Update()

    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);


        if (transform.position.x > 14.8f)
        {
            transform.position = new Vector3(-14.8f, transform.position.y, 0);
        }
        else if (transform.position.x < -14.8f)
    {
            transform.position = new Vector3(14.8f, transform.position.y, 0);
        }
    }
    //(
    //CalculateMovement()

    //if (Input.GetKeyDown(KeyCode.Space))
    //{
    //Instantiate(_laserPrefab, transform.position, Quaternion.identify);
    //}
    //}

    //}
    //}
    //CalculateMovement()

   
    //transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);



    //new Vector3(5, 0, 0) * 5 * real time

}