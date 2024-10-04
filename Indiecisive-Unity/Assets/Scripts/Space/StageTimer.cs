using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTimer : MonoBehaviour
{
    [SerializeField]
    protected float stageTime;

    private float stageClock = 0.0f;
    private Vector3 timerPosition;

    // Start is called before the first frame update
    void Start()
    {
        timerPosition = new Vector3(-Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize, 0.0f);
        transform.position = timerPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(stageClock >= stageTime)
        {
            SceneManager.LoadScene("Planet");
        }
        stageClock += Time.fixedDeltaTime;
        transform.localScale = new Vector3(stageClock * ((Camera.main.orthographicSize * Camera.main.aspect * 2) / stageTime) * 2, 0.5f, 0.0f);
    }
}
