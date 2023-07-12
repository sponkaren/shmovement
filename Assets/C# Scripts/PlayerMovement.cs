using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown ("space")) 
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0,7,0);
        }

        if(Input.GetKey("a"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(-3,0,0);
        }
        if (Input.GetKey("d"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(3, 0, 0);
        }
    }
}
