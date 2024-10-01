using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    //Variables needed for scrolling
    [SerializeField]
    private float speed;
    [SerializeField]
    private Renderer bg;

    // Update is called once per frame
    void Update()
    {
        bg.material.mainTextureOffset += new Vector2(0, speed * Time.fixedDeltaTime);
    }
}
