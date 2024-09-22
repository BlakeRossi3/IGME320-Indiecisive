using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    //Bullet stats TODO: tune these as necessary
    private float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        //moves bullet upwards each frame
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);

        //TODO: destroy when off screen--translate position to pixels, compare to screen height
    }
}
