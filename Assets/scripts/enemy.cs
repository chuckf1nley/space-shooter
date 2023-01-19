using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7.5f)
        {
            float randomx = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomx, 10,  0);
        }
      
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.tag == "Player")
        {
           Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }

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
