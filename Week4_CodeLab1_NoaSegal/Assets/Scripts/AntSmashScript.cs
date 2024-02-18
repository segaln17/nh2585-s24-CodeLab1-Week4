using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AntSmashScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    { //when clicked on, move to a random location and increase score
        transform.position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        GameManager.instance.Score++;
    }
}
