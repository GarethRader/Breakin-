using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float ballSpeed;
    public float maxSpeed = 20f;
    public float minSpeed = 2f;

    public AudioSource scoreSound, blip;
    private int frames = 0;
    
    private int[] dirOptions = {-1, 1};
    private int hDir;

    GameObject ball; 
    public Renderer ballRenderer;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        ball = this.gameObject;
        ballRenderer = ball.GetComponent<Renderer>(); 
        Reset(); 
    }
    void Update() {
        if(frames != 120){
            frames ++;
        }
        
        if(GameManagerScript.s.lives < 2){
            if(frames == 120){
                Color newColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                ballRenderer.material.color = newColor;
            }
        }
        
    }

    // Start the Ball Moving
    public IEnumerator Launch() {
        yield return new WaitForSeconds(1.5f);
        
        // Figure out directions
        hDir = dirOptions[Random.Range(0, dirOptions.Length)];
        
        // Add a horizontal force
        //rb.AddForce(transform.right * ballSpeed * hDir); // Randomly go Left or Right
        // easier to have ball starting straight
        // Add a vertical force
        if(GameManagerScript.s.lives < 2){
            rb.AddForce(transform.up * ballSpeed * -1);
        }
        else{
            rb.AddForce(transform.up * ballSpeed * -5);
        }
         // Force it to start going down
    }

    private void Reset() {
        
        rb.velocity = Vector2.zero;
        ballSpeed = 2;
        transform.position = new Vector2(0, -2);
        StartCoroutine("Launch");
    }

    float hitFactor(Vector2 ballPos, Vector2 paddlePos){
        // used to find the area where the ball hit the paddle
        // to find angle to bounce off of
        // if ball hits top of paddle, it will angle up, likewise at the bottom...
        return(ballPos.x - paddlePos.x);
    }
    // if the ball goes out of bounds
    private void OnCollisionEnter2D(Collision2D other)
    {
        // did we hit a wall?
        if (other.gameObject.tag == "wall")
        {
            Debug.Log("wall hit");
            // make pitch lower
            blip.pitch = 0.75f;
            blip.Play();
            SpeedCheck();
            GameManagerScript.s.ResetStreak();
        }

        // did we hit a paddle?
        if (other.gameObject.tag == "paddle")
        {
            // make pitch higher
            blip.pitch = 1f;
            blip.Play();
            SpeedCheck();
            float x = hitFactor(this.transform.position, other.transform.position);
            // will always hit in the opposite direction of your goal
            
            Vector2 dir = new Vector2(x,rb.velocity.y).normalized;
            GetComponent<Rigidbody2D>().velocity = dir * ballSpeed;
        }
        
        // did we hit the floor?
        if (other.gameObject.name == "BottomWall") {
            Debug.Log("BottomWall hit");
            GameManagerScript.s.DecreaseLives();
            GameManagerScript.s.ResetStreak();
            Reset();

        }

        if(other.gameObject.tag == "Brick"){
            Debug.Log("Brick destroyed");
            GameManagerScript.s.IncreaseStreak();
            GameManagerScript.s.AddBrick();
            Destroy(other.gameObject);
        }
    }

    
    private void SpeedCheck() {
        
        // Prevent ball from going too fast
        //if (Mathf.Abs(rb.velocity.x) > maxSpeed) rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y);
        //if (Mathf.Abs(rb.velocity.y) > maxSpeed) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.9f);
//
        //if (Mathf.Abs(rb.velocity.x) < minSpeed) rb.velocity = new Vector2(rb.velocity.x * 1.1f, rb.velocity.y);
        //if (Mathf.Abs(rb.velocity.y) < minSpeed) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 1.1f);


        // Prevent too shallow of an angle
        if (Mathf.Abs(rb.velocity.x) < minSpeed) {
            // shorthand to check for existing direction
            rb.velocity = new Vector2((rb.velocity.x < 0) ? -minSpeed : minSpeed, rb.velocity.y);
        }

        if (Mathf.Abs(rb.velocity.y) < minSpeed) {
            // shorthand to check for existing direction
            rb.velocity = new Vector2(rb.velocity.x, (rb.velocity.y < 0) ? -minSpeed : minSpeed);
        }
        
        Debug.Log(rb.velocity);

    }


}
