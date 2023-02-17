using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float vertical;
    [SerializeField]
    private float _speed = 3f;
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

                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
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