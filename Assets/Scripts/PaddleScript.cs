using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour {
    private float xPos;
    public float paddleSpeed = .05f;
    public float leftWall, rightWall;

    public float rotateVelocity = 1.5f;
    public KeyCode upArrow, downArrow, rightArrow, leftArrow;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(leftArrow)) {
            if (xPos > leftWall) {
                xPos -= paddleSpeed;
            }
        }

        if (Input.GetKey(rightArrow)) {
            if (xPos < rightWall) {
                xPos += paddleSpeed;
            }
        }

        if(Input.GetKey(upArrow)){
            this.transform.Rotate(new Vector3 (0 ,0, rotateVelocity ));
            //Debug.Log("rotating");
            if(this.transform.rotation == Quaternion.Euler(0,0,180)){
                this.transform.Rotate(Vector3.zero);
            }
        }

        if(Input.GetKey(downArrow)){
            this.transform.Rotate(new Vector3 (0, 0, -1 * rotateVelocity));
            if(this.transform.rotation == Quaternion.Euler(0,0,-180)){
                this.transform.Rotate(Vector3.zero);
            }
        }

        transform.localPosition = new Vector3(xPos, transform.position.y, 0);
    }
}

