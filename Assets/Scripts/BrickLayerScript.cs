using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickLayerScript : MonoBehaviour
{
    public Transform brick;
    public float numRows, numColumns;
    public float xSpacing, ySpacing; // .5
    public float xOrigin, yOrigin; // -5 and 4
    public Color[] brickColors;
    public int[] scorePoints = {10, 20, 30, 40, 50};
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0 ; i<numRows; i++){
            for(int j=0; j<numColumns; j++){
                Transform t = Instantiate(brick);
                t.transform.parent = this.transform;
                Vector2 loc = new Vector2(xOrigin + (i * xSpacing), yOrigin + (j * ySpacing));

               
                SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
                sr.color = brickColors[j];

                t.transform.position = loc;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
