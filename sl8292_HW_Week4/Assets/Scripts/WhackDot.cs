using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhackDot : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("click dot");
        
        transform.position = 
            new Vector3(
                Random.Range(-6f,6f), 
                Random.Range(-4f,4f), 
                0);

        GameManager.instance.Score++;
    }
}
