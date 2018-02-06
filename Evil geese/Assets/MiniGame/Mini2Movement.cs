using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/* 
  NEW SCRIPT BY UN-TWO
*/
public class Mini2Movement : MonoBehaviour {

    private int PlayerSpeed = 3;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goose")
        {
            SceneManager.LoadScene("MiniGameMenu");

        }
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("right"))
        {
            transform.position -= Vector3.left * PlayerSpeed * Time.deltaTime;
        }
        if (Input.GetKey("left"))
        {
            transform.position -= Vector3.right * PlayerSpeed * Time.deltaTime;
        }
       
    }
}
