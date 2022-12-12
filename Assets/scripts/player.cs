using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //public or private
    //data tyoe (int, bool, float string)
    //everey variable has a name
    //optional value assigned
    [SerializeField]
    private float _speed = 3.5f;
    public string playerName = "samaxe";

    // Start is called before the first frame update
    void Start()
    {
        //if player positon

        transform.position = new Vector3(0, 0, 0);



    }

    // Update is called once per frame
    void Update()
    {
                    //new Vector3(5, 0, 0) * 5 * real time
        transform.Translate(Vector3.right * _speed * Time.deltaTime);


    }
}
