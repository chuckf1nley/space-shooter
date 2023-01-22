using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float vertical;
    private float _speed = 4f;
    private GameObject objectToSpawn;
    private Vector3 origin = Vector3.zero;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7.5f)
        {
            float randomx = Random.Range(-18f, 18f);
            transform.position = new Vector3(randomx, 10f, 0);
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

}
