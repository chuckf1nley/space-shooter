using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float vertical;
    [SerializeField]
    private float _speed = 3f;
    //private float ID = 0f, _Triple_Shot_Powerup,  1f = _Speed_Powerup, 2 = _Shield_Powerup;
     
    [SerializeField] // 0 = Triple Shot 1 = speed 2 = shield
    private int powerupID;
    // Update is called once per frame
    void Update()

    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);




        if (transform.position.y < -7.5)
        {

            Destroy(this.gameObject);
        }


    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
           
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                //if powerup ID is 0
                if (powerupID == 0)
                {
                    player.TripleShotActive();
                }
                else if (powerupID == 1)
                {
                    Debug.Log("Collected speed");
                }
                else if (powerupID == 2)
                {
                    Debug.Log("Shileds collected");
                }
                // else if powerup os 1
                //play shield powerup
                //else if powerup is 2
                //play shields powerup
            }


            Destroy(this.gameObject);


        }

    }


}