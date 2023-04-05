using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int powerupID; // 0 = Triple Shot 1 = speed 2 = shield 3 = ammo
    [SerializeField] private AudioClip _Clip;
    private Player player;

    // Update is called once per frame
    void Update()

        //ideas for powerups: carry up to 3 of each, use numbers to activate 1-4
        //create EMP to destroy radius for every 500 points
        // create asset to destroy all enemies on screen for every 1000
        // after 300 seconds decrease spawn timer to 3 seconds


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

            AudioSource.PlayClipAtPoint(_Clip, transform.position);

            if (player != null)
            {
                
                
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        Debug.Log("tripleshot is null");
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        Debug.Log("speed is null");
                        break;
                    case 2:
                        player.ActivateShield();
                        Debug.Log("shield is null");
                        break;
                    case 3:
                        player.AmmooRefillActive();
                        Debug.Log("ammo is null");
                        break;

                    default:
                        Debug.Log("Default Value");
                        break;

                }
            }

            Destroy(this.gameObject);

        }

    }

}