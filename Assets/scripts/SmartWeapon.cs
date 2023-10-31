using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartWeapon : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _chaseSpeed = 2.5f;
    private Player _player;
    private float _playerDistance;
    private float _interceptDistance;
    private bool _isPlayerAlive;

    private Vector2 target;
    private Vector2 position;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _isPlayerAlive = true;
      

        if(_player == null)
        {
            Debug.Log("player is null on smartweapon");
        }
    //    target = new Vector2(0.0f, 0.0f);
    //    position = gameObject.transform.position;

       //Debug.Break();
    }

    // Update is called once per frame
    void Update()
    {
            if (_playerDistance >= -1 )
            {
                Weapon();
            }
        //{
        float step = _speed * Time.deltaTime;

        // move sprite towards the target location
        transform.position = Vector2.MoveTowards(transform.position, target, step);
    }

    //google search for unity movetowards 2d

    void OnGUI()
    {
        Event currentEvent = Event.current;
        Vector2 playerPos = new Vector2();
        Vector2 point = new Vector2();

        //// compute where the player is in world space
        //playerPos.x = currentEvent.playerPostion.x;
        //playerPos.y = cam.pixelHeight - currentEvent.playerPosition.y;
        //point = cam.ScreenToWorldPoint(new Vector3(playerPos.x, playerPos.y, 0.0f));

    }



    public void Weapon()
    {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            Vector3 direction = transform.position - _player.transform.position;
            direction = direction.normalized;

            transform.Translate(direction * _chaseSpeed * Time.deltaTime);


        if (transform.position.y > 9f)
        {            
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
        if(transform.position.x > 18f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
        else if (transform.position.x < -18f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);

            }
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_isPlayerAlive == true)
        {
            if (other.tag == "Player")
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                    _isPlayerAlive = false;
                }
            }
        }
    }


}
