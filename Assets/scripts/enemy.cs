using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [SerializeField]
    private float vertical;
    private float _speed = 4f;
    public GameObject objectToSpawn;
    public Vector3 origin = Vector3.zero;
    public float radius = 10;


    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(GameObject objectToSpawn, Vector3 position, Quaternion rotation);
    }

    // Update is called once per frame
    void Update()
    {
        //move down at 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //if bottom of screen (-7.5)
        //respawn at top (9) with a new random x position

        if (transform.position.y < -7.5f)
        {
            transform.position = new Vector3(transform.position.x,  9f,  0);
        }

        //else if (transform.position.y > 9f)
        //{
            //const float V = 9f;
            //transform.position = new Vector3(transform.position.x,  V, 0);
        //}

        void SpawnObject()
        {
            GameObject newObject = Instantiate(objectToSpawn);
           
        }

    }
}
