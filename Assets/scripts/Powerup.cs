using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int powerupID; // 0 = Triple Shot 1 = speed 2 = shield 
    [SerializeField] private AudioClip _Clip;

    // Update is called once per frame
    void Update()

        //ideas for powerups: carry up to 3 of each, use numbers to activate 1-3
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
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ActivateShield();
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