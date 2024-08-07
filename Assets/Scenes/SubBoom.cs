using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine {
    float depth;
    float velocity;
    GameObject go;

    public Submarine()
    {
        depth = Random.Range(-4.5f, 2);
        
        //Nikki
        //For now, I limited the range of the velocity from -1 to 0.5f as I had some problems with the translate method
        //Where the submarine would not move at all.
        velocity = Random.Range(0.5f, 1.5f);
        go = new GameObject("Submarine");
        go.transform.localScale = new Vector3(2, 0.5f, 1);
        go.transform.position = new Vector3(0, depth, 0);
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        Texture2D tex = Resources.Load<Texture2D>("blank_square");
        Sprite sprite = Sprite.Create(tex, new UnityEngine.Rect(0.0f,0.0f,tex.width,tex.height), new Vector2(0.5f, 0.5f), (float) tex.width);
        renderer.sprite = sprite;
    }

    public void UpdatePosition(float dt) {
        Vector3 pos = go.transform.position;
        pos.x += dt * velocity;

        // Wrap the subs around if they've wandered off screen
        if (pos.x < -12) {
            pos.x = 12;
        }

        if (pos.x > 12) {
            pos.x = -12;
        }

        go.transform.position = pos;

        //Nikki
        //added this line of code to translate the movement of the submarines to the right
        go.transform.Translate(Vector2.right * Time.deltaTime * velocity);
    }
}

public class SubBoom : MonoBehaviour
{
    GameObject ocean;
    GameObject destroyer;
    GameObject depthCharge;
    List<Submarine> submarines;

    // Start is called before the first frame update
    void Start()
    {
        Texture2D tex = Resources.Load<Texture2D>("blank_square");
        Sprite sprite;
        SpriteRenderer renderer;

        // Setup the ocean background
        ocean = new GameObject("Ocean");
        ocean.transform.localScale = new Vector3(22, 8, 1);
        ocean.transform.position = new Vector3(0, -1, 0);
        renderer = ocean.AddComponent<SpriteRenderer>();
        renderer.color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        sprite = Sprite.Create(tex, new UnityEngine.Rect(0.0f,0.0f,tex.width,tex.height), new Vector2(0.5f, 0.5f), (float) tex.width);
        renderer.sprite = sprite;
    
        // Setup the destroyer
        destroyer = new GameObject("Destroyer");
        destroyer.transform.localScale = new Vector3(3, 0.5f, 1);
        destroyer.transform.position = new Vector3(0, 3.1f, 0);
        renderer = destroyer.AddComponent<SpriteRenderer>();
        renderer.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        sprite = Sprite.Create(tex, new UnityEngine.Rect(0.0f,0.0f,tex.width,tex.height), new Vector2(0.5f, 0.5f), (float) tex.width);
        renderer.sprite = sprite;

        // Setup empty submarines list
        submarines = new List<Submarine>();
        submarines.Add(new Submarine());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = destroyer.transform.position;

        // Handle user input
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("space key was pressed");
            //Nikki
            //get destroyer pos
            //create charge from destroyer pos with user input
            //have charge move in a linear direction downwards
            //user input then blows up charge to destroy submarine
            CreateCharge(pos);
        }

        if (Input.GetKey("left"))
        {
            pos.x -= Time.deltaTime * 3;
            pos.x = Mathf.Max(pos.x, -9.25f);
            destroyer.transform.position = pos;
        }

        if (Input.GetKey("right"))
        {
            pos.x += Time.deltaTime * 3;
            pos.x = Mathf.Min(pos.x, 9.25f);
            destroyer.transform.position = pos;
        }

        // Update submarine positions
        foreach (var sub in submarines) {
            sub.UpdatePosition(Time.deltaTime);
        }
    }

    void CreateCharge(Vector3 destroyerPosition)
    {
        Texture2D tex = Resources.Load<Texture2D>("blank_circle");
        Sprite sprite;
        SpriteRenderer renderer;

        depthCharge = new GameObject("Depth Charge");
        depthCharge.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        depthCharge.transform.position = destroyerPosition;

        renderer = depthCharge.AddComponent<SpriteRenderer>();
        renderer.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        sprite = Sprite.Create(tex, new UnityEngine.Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), (float)tex.width);
        renderer.sprite = sprite;

        //Nikki
        //neither of these inputs work since the if statement to make this method happen in the first place only checks if the space key was pressed
        //not if the space key was held or released
        if (Input.GetKey("space"))
            depthCharge.transform.Translate(Vector2.down * Time.deltaTime * 1.1f);

        if (Input.GetKeyUp("space"))
        {
            Debug.Log("Space Key was released!");
        }
    }
}
