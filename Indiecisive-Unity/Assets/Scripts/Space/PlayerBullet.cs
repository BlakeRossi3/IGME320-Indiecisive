using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    
    //Bullet stats TODO: tune these as necessary
    private float speed = 1f;
    private Vector3 screenPosition;
    

    // Update is called once per frame
    void Update()
    {

        //moves bullet upwards each frame
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);

        //gets current position in pixels
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        //if the bullet is offscreen, destroys it
        if (screenPosition.y > Screen.height)
        {
            Destroy(gameObject);
        }

    }


}
