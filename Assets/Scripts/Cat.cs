using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    #region 
    public float moveSpeed;
    private bool foward; 

    Vector3 vectMov;

    //get RigidBody
    private Rigidbody2D catRigidBody;

    //get Boxclider
    private BoxCollider2D catCollider;

    public Transform whiskers;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //set rigidbody and colldier
        catRigidBody = GetComponent<Rigidbody2D>();
        catCollider = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        //move cat in straight line until hit wall
        //movethis to a fixed update so its frame rate independent
        Move(moveSpeed, foward);

        //if wall hit reverse x location
    }

    public void Move(float speed, bool direction){
        if(direction){
            catRigidBody.velocity = new(speed, catRigidBody.velocity.y);
        }
        else{
            catRigidBody.velocity = new(-speed, catRigidBody.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log( "collided ");
        //this.foward = !foward;
    }
  
}
