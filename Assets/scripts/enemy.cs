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
    
    }    
   
    private void Instantiate(object objectToSpawn, object position, object rotation)
    {
        throw new System.NotImplementedException();
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
            float randomx = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomx, 10,  0);
        }
      
    }
    private void OnTriggerEnter(Collider other)
    {
        //if other is player
        //damage player
        //destroy us

        if (other.tag == "Player")
        {
            Destroy(this.gameObject);
        }

        //if other is laser
        //laser
        //destroy us

        if (other.tag == "laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
    void DestroyGameObject()
    {
        Destroy (gameObject);
        }

}
