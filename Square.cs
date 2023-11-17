using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(128, 128);
    }

    Texture2D texture;

    public Vector2Int startPoint = new Vector2Int();
    public Vector2Int endPoint = new Vector2Int();
        
    void PentagonoM2(int Radio, Vector2Int Punto0, Color Linea)
    {
        float angulo = Mathf.Deg2Rad * 360 / 5;

        Vector2Int Punto1 = new Vector2Int(Punto0.x, Punto0.y + Radio);
        Vector2Int Punto2 = new Vector2Int(Mathf.RoundToInt(Punto0.x - Radio * Mathf.Sin(angulo)), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(angulo)));
        Vector2Int Punto3 = new Vector2Int(Mathf.RoundToInt(Punto0.x + Radio * Mathf.Sin(angulo)), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(angulo)));
        Vector2Int Punto4 = new Vector2Int(Mathf.RoundToInt(Punto0.x + Radio * Mathf.Sin(2 * angulo)), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(2 * angulo)));
        Vector2Int Punto5 = new Vector2Int(Mathf.RoundToInt(Punto0.x - Radio * Mathf.Sin(2 * angulo)), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(2 * angulo)));


    }
    float Pendiente(int x0, int y0, int x1, int y1)
    {
        float m = (y1 - y0)/(float)(x1 - x0);
        return m;
    }

    int intercepto(int x, int y, float m)
    {
        int b = Mathf.RoundToInt(y - (m * x));
        return b; 
    }

    void ABasico(int x0, int y0, int x1, int y1, Color linea)
    {
        if (x0 == x1)
        {
            if (y0 > y1) (y0, y1) = (y1, y0);
            for (int y = y0; y < y1; y++)
            {
                texture.SetPixel(x0, y, linea);
            }
        }
        else
        {
            if(x0 > x1)
            {
                (y0, y1) = (y1, y0);
                (x0, x1) = (x1, x0);
            }
            float m = Pendiente(x0, y0, x1, y1);
            print(m);
            int b = intercepto(x0, y0, m);
            int yi = 0; 
            for(int x= x0; x < x1; x++)
            {
                int y = Mathf.RoundToInt(m * x + b);
                if (m >= 1) ABasico(x, yi, x, y, linea);
                texture.SetPixel(x, y, linea);
                yi = y;
            }
        }
    }

}
    
