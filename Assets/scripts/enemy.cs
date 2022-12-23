using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [SerializeField]
    private float horizontal;
    private float _speed = 4f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //move down at 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    
        //if bottom of screen -7.5
        //respawn at top with a new random x position
    
        if (transform.position.y >-7.5f);
        {
            //transform.position
        }
    }
}
