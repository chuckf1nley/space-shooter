using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartWeapon : MonoBehaviour
{
    [SerializeField]private float _speed = 4f;
    private Player _player;
    private float _playerDistance;


   
    // Update is called once per frame
    void Update()
    {
        _playerDistance = Vector3.Distance(transform.position, _player.transform.position);
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (_playerDistance <=0 )
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.y >9f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject); 
        }
    }
}
