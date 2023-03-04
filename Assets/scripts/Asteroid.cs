using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        //roatate obj on z axis 3m/s

    }

    //check for Laser collision (Trigger)
    //intantiate explosion of the atseroid (us)
    //destroy explosion after 3 seconds

    private void OnTriggerEnter2D(Collider2D other)
  {

    if (other.tag == "laser")
     {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.15f);
     }
  }




}
