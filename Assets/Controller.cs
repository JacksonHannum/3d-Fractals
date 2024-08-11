using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Vector3 campos;

    public float speed;

    float camdirx;
    float camdiry;

    public Material RenderFractal;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderFractal.SetVector("_CamPos" , campos);


        RenderFractal.SetFloat("_CamDirX" , camdirx);
        RenderFractal.SetFloat("_CamDirY" , camdiry);

        RenderFractal.SetFloat("_TimePassed" , time / 20);

        if (Input.GetKey("y"))
        {
            camdirx += Time.deltaTime;
        }
        if (Input.GetKey("h"))
        {
            camdirx -= Time.deltaTime;
        }
        if (Input.GetKey("g"))
        {
            camdiry -= Time.deltaTime;
        }
        if (Input.GetKey("j"))
        {
            camdiry += Time.deltaTime;
        }

        if (Input.GetKey("e"))
        {
            campos += new Vector3(0 , speed * Time.deltaTime , 0);
        }
        if (Input.GetKey("q"))
        {
            campos -= new Vector3(0 , speed * Time.deltaTime , 0);
        }
        if (Input.GetKey("space"))
        {
            time += Time.deltaTime;
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            float sy = Mathf.Sin(camdiry);
            float cy = Mathf.Cos(camdiry);

            campos += speed * Time.deltaTime * new Vector3((sy * Input.GetAxis("Vertical") + cy * Input.GetAxis("Horizontal")) , 0 , (cy * Input.GetAxis("Vertical") - sy * Input.GetAxis("Horizontal")));
        }
    }
}
