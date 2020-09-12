using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public int score;
    public int numBricks;
    public int streak;
    public int lives;
    public AudioSource[] backgroundMusic = new AudioSource[5];
    private bool flashMob = false;
    public Camera camera;
    private int frames = 0;
    public static GameManagerScript s;
    public GameObject flyingObjects;

    private Queue<GameObject> circles = new Queue<GameObject>();
    private int circleCount = 0;


    // Start is called before the first frame update
    void Start()
    {   
        backgroundMusic[2].Pause();
        s = this;
        numBricks = 0;
        lives = 3;
        DontDestroyOnLoad(this);
    }

    void Update(){
        frames++;
        if(frames >= 120){
            flashColor();
            frames =0;
            if(circleCount < 100){
                GameObject newCircle = Instantiate(flyingObjects, new Vector2(Random.Range(-7, 7), 5), Quaternion.identity);
                
                
                Rigidbody circle = newCircle.GetComponent<Rigidbody>();
                

                Renderer circleRenderer = newCircle.GetComponent<Renderer>();
                circleRenderer.material.color = Random.ColorHSV(1f, 0.5f, 0.5f, 0.5f, 0.5f, 1f);
                circle.AddForce(Random.Range(-250,250),0 , 0);
                circles.Enqueue(newCircle);
                circleCount ++;
            }
        }
        if(circleCount > 50){
            DeleteCircles();
        }
    }

    private void DeleteCircles(){
        GameObject circle = circles.Peek();
        if(circle.transform.position.y < -10){
            circles.Dequeue();
            Destroy(circle);
            Debug.Log("Circle destroyed");
        }
    }
    // Update is called once per frame
    public void AddBrick(){
        numBricks++;
        //Debug.Log("numBricks: " + numBricks);
    }
    public void DecreaseLives(){
        lives--;
        if(lives <1 ){
            for(int i=0; i<5; i++){
                backgroundMusic[i].Pause();
            }
            backgroundMusic[1].Play(); //hehe
            flashMob = true;
            flashColor();
        }
        if(lives<0){
            flashMob = false;
            SceneManager.LoadScene("GameOver");
        }
        //Debug.Log("lives: " + lives);
    }

    private void flashColor(){
        if(flashMob){
            camera.backgroundColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
    }

    public void IncreaseStreak(){
        streak++;
        if(backgroundMusic[1].isPlaying){

        }
        else{
            switch(streak){
                        case 1:
                            break;
                        case 5:
                            
                            backgroundMusic[0].Play();
                            break;
                        case 10:
                            backgroundMusic[0].Pause();
                            backgroundMusic[2].Play();
                            break;
                        case 15:
                            backgroundMusic[2].Pause();
                            backgroundMusic[3].Play();
                            break;
                        case 20:
                            backgroundMusic[3].Pause();
                            backgroundMusic[4].Play();
                            break;
                        default:
                            break;
            }
        }
        
        Debug.Log("Streak: "+  streak);
    }
    public void ResetStreak(){
        streak = 0;
    }
}
