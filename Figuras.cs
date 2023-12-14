using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Figuras : MonoBehaviour
{
    Texture2D texture;

    //DDA
    Vector2Int startP = new Vector2Int();
    Vector2Int endP = new Vector2Int();

    Vector2Int Punto0 = new Vector2Int();
    Vector2Int Punto1 = new Vector2Int();
    Vector2Int Punto2 = new Vector2Int();
    Vector2Int Punto3 = new Vector2Int();

    Vector2Int p0 = new Vector2Int();
    Vector2Int p1 = new Vector2Int();
    Vector2Int p2 = new Vector2Int();
    Vector2Int p3 = new Vector2Int();
    Vector2Int p4 = new Vector2Int();
    Vector2Int p5 = new Vector2Int();

    void Start()
    {
        texture = new Texture2D(256, 256);
        GetComponent<Renderer>().material.mainTexture = texture;
        texture.filterMode = FilterMode.Point;

        
        Punto1 = new Vector2Int(100, 100);
        Punto2 = new Vector2Int(0, 75);
        Punto3 = new Vector2Int(128, 35);

        Punto0 = new Vector2Int(50, 188);
        CirculoBressenham(Punto0, 20, Color.red);
        Punto0 = new Vector2Int(90, 188);
        CirculoBressenham(Punto0, 20, Color.red);
        Punto0 = new Vector2Int(130, 188);
        CirculoBressenham(Punto0, 20, Color.red);
        Punto0 = new Vector2Int(170, 188);
        CirculoBressenham(Punto0, 20, Color.red);
        Punto0 = new Vector2Int(210, 188);
        CirculoBressenham(Punto0, 20, Color.red);

        //Bezier
        Punto0 = new Vector2Int(40, 198);
        Punto1 = new Vector2Int(40, 178);
        linearBezier(Punto0, Punto1, 200, Color.blue);
        Punto0 = new Vector2Int(40, 198);
        Punto1 = new Vector2Int(70, 188);
        Punto2 = new Vector2Int(40, 178);
        CuadraticaBezier(Punto0, Punto1, Punto2, 200, Color.blue);

        Punto0 = new Vector2Int(90, 198);
        Punto1 = new Vector2Int(90, 178);
        linearBezier(Punto0, Punto1, 200, Color.blue);

        Punto0 = new Vector2Int(120, 198);
        Punto1 = new Vector2Int(120, 178);
        linearBezier(Punto0, Punto1, 200, Color.blue);

        Punto0 = new Vector2Int(120, 198);
        Punto1 = new Vector2Int(130, 198);
        linearBezier(Punto0, Punto1, 200, Color.blue);

        Punto0 = new Vector2Int(120, 178);
        Punto1 = new Vector2Int(130, 178);
        linearBezier(Punto0, Punto1, 200, Color.blue);

        Punto0 = new Vector2Int(120, 188);
        Punto1 = new Vector2Int(130, 188);
        linearBezier(Punto0, Punto1, 200, Color.blue);





        texture.Apply();
        
    }

    //Algoritmos (metodos)

    float Pendiente(int x0, int y0, int x1, int y1)
    {
        float m = (float)(y1 - y0) / (float)(x1 - x0);

        print(m + " Hola");

        return m;
    }

    int Intercepto(float m, int x, int y)
    {
        int b = Mathf.RoundToInt(y - (m * x));
        return b;
    }
    
    void ABasico(int x0, int y0, int x1, int y1, Color Linea)
    {
        
        if(x0 == x1)
        {
            if (y0 > y1) (y0, y1) = (y1, y0);
            
            for(int y = y0; y < y1; y++)
            {
                texture.SetPixel(x0, y, Linea);
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
            print(m + " - Pendiente");
            int b = Intercepto(m, x0, y0);
            int yi = 0;
            int yr = 0;
            for(int x = x0; x <= x1; x++)
            {
                int y = Mathf.RoundToInt(m * x + b);

                if ((m <= 1 && x != x0) && (m != 0))
                {
                    if (yi < y1)
                    {
                        yi = y;
                        
                        ABasico(x, yi, x, y, Linea);
                    }
                    
                    ABasico(x, yi, x, y, Linea);
                    yi = y - 1;
                }
                if ((m > 1 && x != x0) && (m != 0))
                {
                    
                    if (yi < y0)
                    {
                        yi = y;

                        ABasico(x, yi, x, y, Linea);
                    }
                    
                    ABasico(x, yi, x, y, Linea);
                    

                }


                //if (m > 1 && x != x0) ABasico(xi, yi, x, y, Linea);

                texture.SetPixel(x, y, Linea);
                yi = y;
                yr = y;
            }
            

        }


    }

    void ABressenham(Vector2Int Punto0, Vector2Int Punto1, Color linea, int Stroke)
    {
        int deltaX = Mathf.Abs(Punto1.x - Punto0.x);
        int deltaY = Mathf.Abs(Punto1.y - Punto0.y);
        int stepX = (Punto0.x < Punto1.x) ? 1 : -1;
        int stepY = (Punto0.y < Punto1.y) ? 1 : -1;

        int pk = deltaX - deltaY;

        int x = Punto0.x;
        int y = Punto0.y;

        while (x != Punto1.x || y != Punto1.y)
        {
            texture.SetPixel(x, y, linea);

            int pk2 = 2 * pk;

            if (pk2 > -deltaY)
            {
                pk -= deltaY;
                x += stepX;
            }
            if (pk2 < deltaX)
            {
                pk += deltaX;
                y += stepY;
            }
        }
        texture.SetPixel(Punto1.x, Punto1.y, linea);

        if (Stroke > 1)
        {
            for (int i = 1; i < Stroke; i++)
            {
                if ((deltaX / deltaY) >= 1)
                {
                    Vector2Int startP = new Vector2Int((int)Punto0.x + i, (int)Punto0.y);
                    Vector2Int endP = new Vector2Int((int)Punto1.x + i, (int)Punto1.y);
                    ABressenham(startP, endP, linea, 1);
                }
                else
                {
                    Vector2Int startP = new Vector2Int((int)Punto0.x, (int)Punto0.y + i);
                    Vector2Int endP = new Vector2Int((int)Punto1.x, (int)Punto1.y + i);
                    ABressenham(startP, endP, linea, 1);
                }
            }
        }
    }
    
    void ADDA(Vector2Int startP, Vector2Int endP, Color linea, int Stroke)
    {
        float deltaX = endP.x - startP.x;
        float deltaY = endP.y - startP.y;

        int steps = Mathf.RoundToInt(Mathf.Max(Mathf.Abs(deltaX), Mathf.Abs(deltaY)));

        float incrementoX = deltaX/ steps;
        float incrementoY = deltaY/ steps;

        float x = startP.x;
        float y = startP.y;

        for(int i = 0; i <= steps; ++i)
        {
            texture.SetPixel(Mathf.RoundToInt(x), Mathf.RoundToInt(y), linea);

            x += incrementoX;
            y += incrementoY;
        }

        if (Stroke > 1)
        {
            for (int i = 1; i < Stroke; i++)
            {
                if ((deltaX / deltaY) >= 1)
                {
                    Vector2Int start = new Vector2Int((int)startP.x + i, (int)startP.y);
                    Vector2Int end = new Vector2Int((int)endP.x + i, (int)endP.y);
                    ABressenham(start, end, linea, 1);
                }
                else
                {
                    Vector2Int start = new Vector2Int((int)startP.x, (int)startP.y + i);
                    Vector2Int end = new Vector2Int((int)endP.x, (int)endP.y + i);
                    ABressenham(start, end, linea, 1);
                }
            }
        }
    }

    //Figuras primitivas

    void TrianguloM1(Vector2Int p0, Vector2Int p1, Vector2Int p2, Color Linea)
    {
        ABasico(p0.x, p0.y, p1.x, p1.y, Linea);
        ABasico(p1.x, p1.y, p2.x, p2.y, Linea);
        ABasico(p0.x, p0.y, p2.x, p2.y, Linea);
    } //LISTO

    void TrianguloM2(int Base, int Altura, Vector2Int Punto0, Color Linea)
    {
        Vector2Int Punto1 = new Vector2Int(Punto0.x - (Base/2), Punto0.y - (Altura/2));
        Vector2Int Punto2 = new Vector2Int(Punto0.x, Punto0.y + (Altura / 2));
        Vector2Int Punto3 = new Vector2Int(Punto0.x + (Base / 2), Punto0.y - (Altura / 2));

        ABressenham(Punto1, Punto2, Linea, 1);
        ABressenham(Punto2, Punto3, Linea, 1);
        ABressenham(Punto1, Punto3, Linea, 1);
    } //LISTO

    void TrianguloM3(int Base, int Altura, Vector2Int Punto0, Color Linea)
    {
        Vector2Int Punto1 = new Vector2Int(Punto0.x + (Base / 2), Punto0.y + Altura);
        Vector2Int Punto2 = new Vector2Int(Punto0.x + Base, Punto0.y);
        

        ADDA(Punto0, Punto1, Linea, 1);
        ADDA(Punto1, Punto2, Linea, 1);
        ADDA(Punto0, Punto2, Linea, 1);

    } //LISTO

    void RectanguloM1(Vector2Int p0, Vector2Int p1, Vector2Int p2, Vector2Int p3, Color Linea)
    {
        ABasico(p0.x, p0.y, p1.x, p1.y, Linea);
        ABasico(p1.x, p1.y, p2.x, p2.y, Linea);
        ABasico(p2.x, p2.y, p3.x, p3.y, Linea);
        ABasico(p3.x, p3.y, p0.x, p0.y, Linea);
    } //LISTO

    void RectanguloM2(int l1, int l2, Vector2Int Punto0, Color Linea)
    {
        Vector2Int Punto1 = new Vector2Int(Punto0.x - (l1 / 2), Punto0.y - (l2 / 2));
        Vector2Int Punto2 = new Vector2Int(Punto0.x - (l1 / 2), Punto0.y + (l2 / 2));
        Vector2Int Punto3 = new Vector2Int(Punto0.x + (l1 / 2), Punto0.y - (l2 / 2));
        Vector2Int Punto4 = new Vector2Int(Punto0.x + (l1 / 2), Punto0.y + (l2 / 2));

        ADDA(Punto1, Punto2, Linea, 1);
        ADDA(Punto2, Punto4, Linea, 1);
        ADDA(Punto4, Punto3, Linea, 1);
        ADDA(Punto3, Punto1, Linea, 1);
    } //LISTO

    void RectanguloM3(int l1, int l2, Vector2Int Punto0, Color Linea)
    {
        Vector2Int Punto1 = new Vector2Int(Punto0.x, Punto0.y + l2);
        Vector2Int Punto2 = new Vector2Int(Punto0.x + l1, Punto0.y);
        Vector2Int Punto3 = new Vector2Int(Punto0.x + l1, Punto0.y + l2);

        ABressenham(Punto0, Punto1, Linea, 1);
        ABressenham(Punto1, Punto3, Linea, 1);
        ABressenham(Punto3, Punto2, Linea, 1);
        ABressenham(Punto0, Punto2, Linea, 1);
    } //LISTO

    void PentagonoM1(Vector2Int p0, Vector2Int p1, Vector2Int p2, Vector2Int p3, Vector2Int p4, Color Linea)
    {
        ADDA(p0, p1, Linea, 1);
        ADDA(p0, p2, Linea, 1);
        ADDA(p2, p4, Linea, 1);
        ADDA(p1, p3, Linea, 1);
        ADDA(p4, p3, Linea, 1);
    }

    void PentagonoM2(int Radio, Vector2Int Punto0, Color Linea)
    {
        float angulo = Mathf.Deg2Rad * 360 / 5;
        
        Vector2Int Punto1 = new Vector2Int(Punto0.x, Punto0.y + Radio);
        Vector2Int Punto2 = new Vector2Int(Mathf.RoundToInt(Punto0.x - Radio * Mathf.Sin(angulo)), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(angulo)));
        Vector2Int Punto3 = new Vector2Int(Mathf.RoundToInt(Punto0.x + Radio * Mathf.Sin(angulo)), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(angulo)));
        Vector2Int Punto4 = new Vector2Int(Mathf.RoundToInt(Punto0.x + Radio * Mathf.Sin(2 * angulo)), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(2 * angulo)));
        Vector2Int Punto5 = new Vector2Int(Mathf.RoundToInt(Punto0.x - Radio * Mathf.Sin(2 * angulo)), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(2 * angulo)));

        ABressenham(Punto1, Punto2, Linea, 1);
        ABressenham(Punto1, Punto3, Linea, 1);
        ABressenham(Punto2, Punto5, Linea, 1);
        ABressenham(Punto3, Punto4, Linea, 1);
        ABressenham(Punto4, Punto5, Linea, 1);

    } //LISTO

    void HexagonoM1(Vector2Int p0, Vector2Int p1, Vector2Int p2, Vector2Int p3, Vector2Int p4, Vector2Int p5, Color Linea)
    {
        ADDA(p0, p1, Linea, 1);
        ADDA(p0, p2, Linea, 1);
        ADDA(p2, p4, Linea, 1);
        ADDA(p1, p3, Linea, 1);
        ADDA(p4, p5, Linea, 1);
        ADDA(p3, p5, Linea, 1);
    } //LISTO

    void HexagonoM2(int Radio, Vector2Int Punto0, Color Linea)
    {
        float angulo = Mathf.Deg2Rad * 360 / 6;

        Vector2Int Punto1 = new Vector2Int(Punto0.x, Punto0.y + Radio);
        Vector2Int Punto2 = new Vector2Int(Mathf.RoundToInt(Punto0.x - Radio * Mathf.Sin(angulo)), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(angulo)));
        Vector2Int Punto3 = new Vector2Int(Mathf.RoundToInt(Punto0.x + Radio * Mathf.Sin(angulo)), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(angulo)));
        Vector2Int Punto4 = new Vector2Int(Mathf.RoundToInt(Punto0.x + Radio * Mathf.Sin(2 * angulo)), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(2 * angulo)));
        Vector2Int Punto5 = new Vector2Int(Mathf.RoundToInt(Punto0.x - Radio * Mathf.Sin(2 * angulo)), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(2 * angulo)));
        Vector2Int Punto6 = new Vector2Int(Mathf.RoundToInt(Punto0.x), Mathf.RoundToInt(Punto0.y + Radio * Mathf.Cos(3 * angulo)));

        ABressenham(Punto1, Punto2, Linea, 1);
        ABressenham(Punto1, Punto3, Linea, 1);
        ABressenham(Punto2, Punto5, Linea, 1);
        ABressenham(Punto3, Punto4, Linea, 1);
        ABressenham(Punto4, Punto6, Linea, 1);
        ABressenham(Punto5, Punto6, Linea, 1);

    } //LISTO

    void CirculoM1(Vector2Int Punto0, int r, Color linea)
    {

        int yb = 0, yc = 0;
        for (int x =  0; x <= r; x++)
        {
            
            int y = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(r, 2) - Mathf.Pow(x, 2)));
            
            texture.SetPixel(Punto0.x + x, Punto0.y + y, linea);
            texture.SetPixel(Punto0.x - x, Punto0.y + y, linea);
            texture.SetPixel(Punto0.x - x, Punto0.y - y, linea);
            texture.SetPixel(Punto0.x + x, Punto0.y - y, linea);

            yc = y - yb;

            if (Mathf.Abs(yc) > 1 && x > 0)
            {
                Vector2Int punto1 = new Vector2Int(Punto0.x + x, Punto0.y + y);
                Vector2Int punto2 = new Vector2Int(Punto0.x + x, Punto0.y + yb);
                ABressenham(punto1, punto2, linea, 1); 

                Vector2Int punto3 = new Vector2Int(Punto0.x - x, Punto0.y + y);
                Vector2Int punto4 = new Vector2Int(Punto0.x - x, Punto0.y + yb);
                ABressenham(punto3, punto4, linea, 1);

                Vector2Int punto5 = new Vector2Int(Punto0.x + x, Punto0.y - y);
                Vector2Int punto6 = new Vector2Int(Punto0.x + x, Punto0.y - yb);
                ABressenham(punto5, punto6, linea, 1);

                Vector2Int punto7 = new Vector2Int(Punto0.x - x, Punto0.y - y);
                Vector2Int punto8 = new Vector2Int(Punto0.x - x, Punto0.y - yb);
                ABressenham(punto7, punto8, linea, 1);
            }
            yb = y;
        }
    } //LISTO

    void CirculoBressenham(Vector2Int Punto0, int r, Color linea)
    {
        int y = r;
        int d = 3 - 2 * r;

        for(int x = 0; x <= y; x++) 
        {
            DrawCirclePoints(Punto0, x, y, linea);

            if (d <= 0)
            {
                d += 4 * x + 6;
            }
            else
            {
                d += 4 * (x - y) + 10;
                y--;    
            }
        }

    } //LISTO

    void DrawCirclePoints(Vector2Int Punto0, int x, int y, Color linea)
    {
        texture.SetPixel(Punto0.x + x, Punto0.y + y, linea);
        texture.SetPixel(Punto0.x - x, Punto0.y + y, linea);
        texture.SetPixel(Punto0.x - x, Punto0.y - y, linea);
        texture.SetPixel(Punto0.x + x, Punto0.y - y, linea);

        texture.SetPixel(Punto0.x + y, Punto0.y + x, linea);
        texture.SetPixel(Punto0.x - y, Punto0.y + x, linea);
        texture.SetPixel(Punto0.x - y, Punto0.y - x, linea);
        texture.SetPixel(Punto0.x + y, Punto0.y - x, linea);
    } //LISTO

    void linearBezier(Vector2 p0, Vector2 p1, int resolution, Color linea)
    {
        for(int i = 0; i <= resolution; i++)
        {
            float t = i / (float) resolution;
            Vector2 point = CalculateLinearBezier(t, p0, p1);
            texture.SetPixel(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), Color.blue);
        }

    } //LISTO

    Vector2 CalculateLinearBezier(float t, Vector2 p0, Vector2 p1)
    {
        Vector2 point = (1 - t) * p0 + t * p1;

        return point;
    } //LISTO

    void CuadraticaBezier(Vector2 p0, Vector2 p1, Vector2 p2, int resolution, Color linea)
    {
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector2 point = CalculateCuadraticaBezier(t, p0, p1, p2);
            texture.SetPixel(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), Color.blue);
        }

    } //LISTO

    Vector2 CalculateCuadraticaBezier(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector2 point = uu * p0 + 2 * u * t * p1 + tt * p2;

        return point;
    } //LISTO

    void CubicaBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int resolution, Color linea)
    {
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector2 point = CalculateCubicaBezier(t, p0, p1, p2, p3);
            texture.SetPixel(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), Color.blue);
        }

    } //LISTO

    Vector2 CalculateCubicaBezier(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 point = uuu * p0 + 3 * uu * t * p1 + 2 * u * tt * p2 + ttt * p3;

        return point;
    } //LISTO
}
    
