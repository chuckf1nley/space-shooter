using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerupID; // 0 = Triple Shot 1 = speed 2 = shield 3 = ammo 4 = health, 5= altfire, 6 = negspeed
    [SerializeField] private AudioClip _Clip;
    private Player _player;
    private Laser _laser;
    private SmartWeapon _smartWeapon;

    // Update is called once per frame
    void Update()

    //ideas for powerups: carry up to 3 of each, use numbers to activate
    //create EMP to destroy radius for every 500 points
    // create asset to destroy all enemies on screen for every 1000


    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7.5)
        {

            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();


            AudioSource.PlayClipAtPoint(_Clip, transform.position);


            if (player != null)
            {

                //if (CompareTag("Laser"))
                //{
                //    Laser laser = other.transform.GetCompopnent<Laser>();
                //    SmartWeapon smartWeapon = other.transform.GetComponent<SmartWeapon>();
                //    Destroy(this.gameObject);


                    switch (_powerupID)
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
                        case 3:
                            player.AmmooRefillActive();
                            break;
                        case 4:
                            player.Heal();
                            break;
                        case 5:
                            player.AltFire();
                            break;
                        case 6:
                            player.NegSpeed();
                            break;

                        default:
                            Debug.Log("Default Value");
                            break;

                    }

                    Destroy(this.gameObject);

                //}
            }

        }

    }
}