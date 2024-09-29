using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderZone : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    [SerializeField]
    protected Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(gameObject, startPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.fixedDeltaTime);
    }
}
