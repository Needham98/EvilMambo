using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
  NEW SCRIPT BY UN-TWO
*/


public class EnemyScript : MonoBehaviour {

    public int moveSpeed = -3;

    public Transform Right;
    public Transform Left;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (transform.position.x == Right.position.x || transform.position.x == Left.position.x)
        {
            moveSpeed=moveSpeed*-1;
        }

        transform.Translate(new Vector3(moveSpeed, 0, 0) * Time.deltaTime);	
 
	}

    
}
