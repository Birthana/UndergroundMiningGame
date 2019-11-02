using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRandTable
{
    float[,] table;
    int rows, cols;
    float alpha;
    public SmoothRandTable(int rows, int cols, float max)
    {
        alpha = 0.5f;
        if (rows < 1 || cols < 1)
        {

        }
        this.rows = rows;
        this.cols = cols;
        table = new float[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                table[i, j] = Random.Range(0, max);
            }
        }
        //int directionChoice = Random.Range(1, 5);
    }

    public SmoothRandTable(int rows, int cols, int steps)
    {
        if (rows < 1 || cols < 1 || steps < 0)
        {

        }
        this.rows = rows;
        this.cols = cols;
        table = new float[rows, cols];
        for (int i = 0; i < steps; i++)
        {
            SmoothTable();
        }
    }

    public void SetAlpha(float a)
    {
        if (alpha >= 0 && alpha <= 1)
        {
            alpha = a;
        }
    }

    void SmoothTable()
    {
        float[,] temp = new float[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                temp[i, j] = SmoothElement(i, j);
            }
        }
        this.table = temp;
    }

    float SmoothElement(int r, int c)
    {
        int localType = 0;
        if (c == 0)
        {
            localType += 1;
        }else if (c == cols - 1)
        {
            localType += 3;
        }
        else
        {
            localType += 2;
        }
        if (r == 0)
        {
            localType += 6;
        } else if (r < rows - 1)
        {
            localType += 3;
        }

        float total = 0;
        if (localType == 5)
        {
            total += table[r - 1, c + 1];
            total += table[r - 1, c];
            total += table[r - 1, c - 1];
            total += table[r, c + 1];
            total += table[r, c - 1];
            total += table[r + 1, c + 1];
            total += table[r + 1, c];
            total += table[r + 1, c - 1];
            total *= alpha;
            total += table[r, c] * 8 * (1 - alpha);
            total /= 8;
        }
        else
        {
            total = table[r, c];
        }
        return total;
    }

    public string getCharNary(char[] symbols)
    {
        float div = 1.0f / symbols.Length;
        string temp = "";
        for (int i = 0; i < rows; i ++)
        {
            for (int j = 0; j < cols; j++)
            {
                int v = (int) System.Math.Floor(table[i, j] / div);
                temp += symbols[v >= symbols.Length ? symbols.Length - 1: v] + " ";
            }
            temp += "\n";
        }
        return temp.Trim();
    }

    public void MakePath(int r1, int c1, int r2, int c2)
    {
        table[r1, c1] = 1.0f;
        table[r2, c2] = 1.0f;
        int initV = r2 - r1;
        int initH = c2 - c1;
        int dirV = initV > 0 ? 1 : -1;
        int dirH = initH > 0 ? 1 : -1;
        int currV = initV;
        int currH = initH;
        int r = r1, c = c1;
        if (initV != 0 && initH != 0)
        {
            double initPath = 1.0 * initV / initH;
            double currPath;
            while (currV != 0 && currH != 0)
            {
                currPath = 1.0 * currV / currH;
                if ((currV > currH) 
                    ? currPath > 0 ? currPath >= initPath
                        : currPath < initPath
                    : currPath > 0 ? currPath > initPath
                        : currPath <= initPath)
                {
                    currV -= dirV;
                    r += dirV;
                }else
                {
                    currH -= dirH;
                    c += dirH;
                }
                table[r, c] = 1.0f;
            }
        }
        while (currV != 0)
        {
            currV -= dirV;
            r += dirV;
            table[r, c] = 1.0f;
        }
        while (currH != 0)
        {
            currH -= dirH;
            c += dirH;
            table[r, c] = 1.0f;
        }
    }

    public static void Main(string[] args)
    {
        SmoothRandTable srt = new SmoothRandTable(15, 30, .7f);
        
    }
}
