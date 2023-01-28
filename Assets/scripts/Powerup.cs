using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float vertical;
    [SerializeField]
    private float _speed = 3f;
    private GameObject _Triple_Shot_Powerup;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()

    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {


        if (transform.position.y >= -7.5)
        {

            Destroy(this.gameObject);
        }

       
    }
    private void OnTriggerCollision(Collider2D other)
    {

         if (other.tag == "Player")
        {
           // Player player = other.transform.GetComponenmt<Player>();


        }

    }




    //OnTriggerCollision
    //only be collectable by player
    //on collected destroy object
}
