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

        RenderFractal.SetFloat("_TimePassed" , time / 10f);

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
    Vector2 rotate(Vector2 pt , Vector2 pv , float ang)
    {
        Vector2 p = pt - pv;
        float s = Mathf.Sin(ang);
        float c = Mathf.Cos(ang);
        p = new Vector2(p.x * c - p.y * s , p.x * s + p.y * c);
        p += pv;
        return p;
    }
    Vector4 disttofractal(float px , float py , float pz)
    {

        
        int n = 0;
        Vector3 z = new Vector3 (px ,  py , pz);
        
        float s = 3;
        Vector2 a;

        while (n < 12) 
        {
    
            //if (z.x < 0) z.x = -z.x;
            //if (z.y < 0) z.y = -z.y;
            //if (z.z < 0) z.z = -z.z;
            /*

            if(z.x + z.y < 0) z.xy = -z.yx;
            if(z.x + z.z < 0) z.xz = -z.zx;
            if(z.y +  z.z < 0) z.zy = -z.yz;
            z = z * s - 0.5 * (s - 1.0);

            */

            z = new Vector3(Mathf.Abs(z.x) , Mathf.Abs(z.y) , Mathf.Abs(z.z));
            
            if (z.x < z.y) z = new Vector3(z.y , z.x , z.z);
            if (z.x < z.z) z = new Vector3(z.z , z.y , z.x);
            if (z.y < z.z) z = new Vector3(z.x , z.z , z.y);

            z = z * 3 + new Vector3(-2 , -2 , 0);
            if (z.z > 1) z.z = 1 + (1 - z.z);

            a = rotate(new Vector2(z.x , z.y) , new Vector2(0 , 0) , time);
            z = new Vector3(a.x , a.y , z.z);

            a = rotate(new Vector2(z.x , z.z) , new Vector2(0 , 0) , time);
            z = new Vector3(a.x , z.y , a.y);

            a = rotate(new Vector2(z.y , z.z) , new Vector2(0 , 0) , time);
            z = new Vector3(z.x , a.x , a.y);
            

            //z = z * s - 0.5 * (s - 1);
            n++;
        }
        return (new Vector4(z.x , z.y , z.z , (Mathf.Sqrt((z.x * z.x) + (z.y * z.y) + (z.z * z.z)) - 1f) * Mathf.Pow(s, -(n))));
    }
}
