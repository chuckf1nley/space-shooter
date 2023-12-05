using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerupID; // 0 = Triple Shot 1 = speed 2 = shield 3 = ammo 4 = health, 5= altfire, 6 = negspeed
    [SerializeField] private AudioClip _Clip;   
    private Player _player;
    private Vector3 _direction;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            //MoveTowardsplayer 
            _direction = _player.transform.position - transform.position;
            _direction.Normalize();
            transform.Translate(_direction * _speed * Time.deltaTime * 2);

        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y < -7.5)
        {

            Destroy(this.gameObject);
        }
    }

   

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();


            AudioSource.PlayClipAtPoint(_Clip, transform.position);


            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        // Debug.Log("shield powerup check");
                        player.ActivateShield();
                        break;
                    case 3:
                        player.AmmoRefillActive();
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
                    case 7:
                        player.HomingMissile();
                        break;

                    default:
                        Debug.Log("Default Value");
                        break;

                }

                    Destroy(this.gameObject);

            }

        }
        if (other.CompareTag("Laser"))
        {
            Laser laser = other.transform.GetComponent<Laser>();
            SmartWeapon _smartWeapon = other.transform.GetComponent<SmartWeapon>();
            Destroy(this.gameObject);

        }
    }
}