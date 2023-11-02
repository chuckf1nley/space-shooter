using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerupID; // 0 = Triple Shot 1 = speed 2 = shield 3 = ammo 4 = health, 5= altfire, 6 = negspeed
    [SerializeField] private AudioClip _Clip;
    private float _interceptSpeed = 5f;
    private float _movementDirection;
    private float _normalDirection;
    private float _movementSpeed;
    private Player _player;
    private Laser _laser;
    private SmartWeapon _smartWeapon;

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7.5)
        {

            Destroy(this.gameObject);
        }

    }

    public void MoveTowardsPosition(Vector3 targetPosition)
    {
     //   _movementDirection = (targetPosition - transform.position).normalized;
        _interceptSpeed += .5f;
    }

    public void ResumeMovement()
    {
        _movementDirection = _normalDirection;
        _movementSpeed = _speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
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