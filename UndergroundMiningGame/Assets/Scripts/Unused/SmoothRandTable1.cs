using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class SmoothRandTable1 : MonoBehaviour
{
    private float[,] table;
    private int rows, cols, start;
    private float alpha;

    public TextMeshProUGUI area;
    public Tilemap tileMap;
    public Sprite[] placeholders;

    private void smoothTable()
    {
        float[,] temp = new float[rows, cols];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                temp[r, c] = smoothElement(r, c);
            }
        }
        table = temp;
    }

    private float smoothElement(int r, int c)
    {
        if (r == 0 || c == 0 || r == rows - 1 || c == cols - 1)
        {
            return table[r, c];
        }
        float total = 0;
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
        return total;
    }

    private int getWallVal(int r, int c)
    {
        int total = 0;
        if (c == 0 || r + 1 == rows || table[r + 1, c - 1] < 0.5)
        {
            total += 1;
        }
        if (r + 1 == rows || table[r + 1, c] < 0.5)
        {
            total += 2;
        }
        if (c + 1 == cols || r + 1 == rows || table[r + 1, c + 1] < 0.5)
        {
            total += 4;
        }
        if (c == 0 || table[r, c - 1] < 0.5)
        {
            total += 8;
        }
        if (c + 1 == cols || table[r, c + 1] < 0.5)
        {
            total += 16;
        }
        if (c == 0 || r == 0 || table[r - 1, c - 1] < 0.5)
        {
            total += 32;
        }
        if (r == 0 || table[r - 1, c] < 0.5)
        {
            total += 64;
        }
        if (c + 1 == cols || r == 0 || table[r - 1, c + 1] < 0.5)
        {
            total += 128;
        }
        if (r <= 1 || table[r - 2, c] < 0.5)
        {
            total += 256;
        }
        return total;
    }

    private string getTile(int r, int c)
    {
        if (table[r, c] < 0.5)
        {
            //wall
            switch (getWallVal(r, c))
            {
                case 251:
                case 251 + 256:
                    return "ITL";
                case 248:
                case 248 + 256:
                case 248 + 1:
                case 248 + 1 + 256:
                case 248 + 4:
                case 248 + 4 + 256:
                case 248 + 5:
                case 248 + 5 + 256:
                    return "IT";
                case 254:
                case 254 + 256:
                    return "ITR";
                case 127:
                case 127 + 256:
                    return "IBL";
                case 31:
                case 31 + 256:
                case 31 + 32:
                case 31 + 32 + 256:
                case 31 + 128:
                case 31 + 128 + 256:
                case 31 + 160:
                case 31 + 160 + 256:
                    return "IB";
                case 223:
                case 223 + 256:
                    return "IBR";
                case 363:
                case 363 + 128:
                case 363 + 4:
                case 363 + 132:
                    return "IL";
                case 470:
                case 470 + 32:
                case 470 + 1:
                case 470 + 33:
                    return "IR";
                case 107:
                    return "OR";
                case 214:
                    return "OL";
                case 22:
                case 22 + 256:
                case 22 + 64:
                case 22 + 256 + 64:
                case 22 + 8:
                case 22 + 256 + 8:
                case 22 + 32:
                case 22 + 256 + 32:
                case 150:
                case 150 + 256:
                case 23:
                case 23 + 256:
                case 151:
                case 151 + 256:
                    return "OTL";
                case 208:
                case 208 + 256:
                case 208 + 2:
                case 208 + 256 + 2:
                case 208 + 8:
                case 208 + 256 + 8:
                case 208 + 1:
                case 208 + 256 + 1:
                case 240:
                case 240 + 256:
                case 244:
                case 244 + 256:
                case 212:
                case 212 + 256:
                    return "OBL";
                case 11:
                case 11 + 256:
                case 11 + 64:
                case 11 + 256 + 64:
                case 11 + 16:
                case 11 + 256 + 16:
                case 11 + 128:
                case 11 + 256 + 128:
                case 43:
                case 43 + 256:
                case 47:
                case 47 + 256:
                case 15:
                case 15 + 256:
                    return "OTR";
                case 104:
                case 104 + 256:
                case 104 + 2:
                case 104 + 256 + 2:
                case 104 + 16:
                case 104 + 256 + 16:
                case 104 + 4:
                case 104 + 256 + 4:
                case 232:
                case 232 + 256:
                case 233:
                case 233 + 256:
                case 105:
                case 105 + 256:
                    return "OBR";
                case 255:
                case 255 + 256:
                    return "FLOOR";
                default:
                    return "SW";
            }
        }
        else
        {
            //floor
            return "FLOOR";
        }

    }

    private void makePath(int r1, int c1, int r2, int c2)
    {
        table[r1, c1] = 1.37f;
        table[r2, c2] = 1.37f;
        int initV = r2 - r1;
        int initH = c2 - c1;
        int dirV = initV > 0 ? 1 : -1;
        int dirH = initH > 0 ? 1 : -1;
        int currV = initV;
        int currH = initH;
        int r = r1, c = c1;
        if (initV != 0 && initH != 0)
        {
            //some ratio
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
                }
                else
                {
                    currH -= dirH;
                    c += dirH;
                }
                table[r, c] = 1.37f;

            }
        }
        while (currV != 0)
        {
            currV -= dirV;
            r += dirV;
            table[r, c] = 1.37f;
        }
        while (currH != 0)
        {
            currH -= dirH;
            c += dirH;
            table[r, c] = 1.37f;
        }
    }

    private void makeCrookedPath(int r1, int c1, int r2, int c2)
    {
        int dumbDist = Mathf.Abs(r2 - r1) + Mathf.Abs(c2 - c1);
        int crook;
        int r = r1, c = c1;
        int rNext, cNext;
        while (dumbDist > 9)
        {
            crook = Random.Range(0, 8);
            rNext = r + (r2 - r > 0 ? crook : -crook);
            if (rNext < 1)
            {
                rNext = 1;
            }
            else if (rNext >= rows - 1)
            {
                rNext = rows - 2;
            }
            cNext = c + (c2 - c > 0 ? 8 - crook : crook - 8);
            if (cNext < 1)
            {
                cNext = 1;
            }
            else if (cNext >= cols - 1)
            {
                cNext = cols - 2;
            }
            makePath(r, c, rNext, cNext);
            r = rNext;
            c = cNext;
            dumbDist = Mathf.Abs(r2 - r) + Mathf.Abs(c2 - c);
        }
        makePath(r, c, r2, c2);
    }

    /// <summary>This function makes a new table and generates a cave system in it</summary>
    public void GenerateLoopingCave()
    {
        alpha = 0.5f;
        rows = 40;
        cols = 40;
        table = new float[rows, cols];
        for (int r = 1; r < rows - 1; r++)
        {
            for (int c = 1; c < cols - 1; c++)
            {
                table[r, c] = Random.Range(0.2f, 0.4f);
            }
        }
        start = Random.Range(0, rows - 2) + 1;
        int end1 = Random.Range(0, rows - 2) + 1;
        int end2r = (end1 + rows / 2) % rows;
        if (end2r == 0)
        {
            end2r++;
        }
        if (end2r == rows - 1)
        {
            end2r--;
        }

        int end2c = cols / 2 + Random.Range(0, cols / 2 - 2);
        makeCrookedPath(start, 0, end1, cols - 2);
        makeCrookedPath(start, 0, end2r, end2c);
        makePath(start, 0, start, 1);
        makeCrookedPath(end1, cols - 2, end2r, end2c);
        smoothTable();
        smoothTable();
    }

    /// <summary>This function returns the table as a 2D array of strings which represent map tiles</summary>
    public string[,] GetTileStringMatrix()
    {
        string[,] temp = new string[rows, cols];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                temp[r, c] = getTile(r, c);
            }
        }
        return temp;
    }

    /// <summary>
    ///  <para>This function returns the row at which there is an opening to the cave. </para>
    ///  <para>The first two columns in this row are guaranteed to be traversable floor.</para>
    ///  <para>Result is an int where table[GetStartPoint()][0] and table[GetStartPoint()][1] are the guaranteed traversable floors.</para>
    /// </summary>
    public int GetStartPoint()
    {
        return start;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateLoopingCave();
        string[,] tileMatrix = GetTileStringMatrix();

        string[,] testMatrix = new string[4, 4];
        testMatrix[0, 0] = "ITL";
        testMatrix[0, 1] = "IT";
        testMatrix[0, 2] = "IT";
        testMatrix[0, 3] = "ITR";
        testMatrix[1, 0] = "IL";
        testMatrix[1, 1] = "FLOOR";
        testMatrix[1, 2] = "FLOOR";
        testMatrix[1, 3] = "IR";
        testMatrix[0, 3] = "ITR";
        testMatrix[2, 0] = "IL";
        testMatrix[2, 1] = "FLOOR";
        testMatrix[2, 2] = "FLOOR";
        testMatrix[2, 3] = "IR";
        testMatrix[3, 0] = "IBL";
        testMatrix[3, 1] = "IB";
        testMatrix[3, 2] = "IB";
        testMatrix[3, 3] = "IBR";

        tileMatrix = RotateMatrix(tileMatrix, 40);
        for (int i = 0; i < 40; i++)
        {
            for (int j = 0; j < 40; j++)
            {
                //area.text += tileMatrix[i, j] + " ";

                string tempString = tileMatrix[i, j];
                Tile temp = ScriptableObject.CreateInstance<Tile>();
                switch (tempString)
                {
                    case "FLOOR":
                        //temp.sprite = placeholders[0];
                        break;
                    case "ITL":
                        temp.sprite = placeholders[1];
                        break;
                    case "IT":
                        temp.sprite = placeholders[2];
                        break;
                    case "ITR":
                        temp.sprite = placeholders[3];
                        break;
                    case "IL":
                        temp.sprite = placeholders[4];
                        break;
                    case "IR":
                        temp.sprite = placeholders[5];
                        break;
                    case "IBL":
                        temp.sprite = placeholders[6];
                        break;
                    case "IB":
                        temp.sprite = placeholders[7];
                        break;
                    case "IBR":
                        temp.sprite = placeholders[8];
                        break;
                    case "OTL":
                        temp.sprite = placeholders[9];
                        break;
                    case "OTR":
                        temp.sprite = placeholders[10];
                        break;
                    case "OL":
                        temp.sprite = placeholders[11];
                        break;
                    case "OR":
                        temp.sprite = placeholders[12];
                        break;
                    case "OBL":
                        temp.sprite = placeholders[13];
                        break;
                    case "OBR":
                        temp.sprite = placeholders[14];
                        break;
                    case "SW":
                        temp.sprite = placeholders[15];
                        break;
                }
                tileMap.SetTile(new Vector3Int(i, j, 0), temp);
            }
            //area.text += "\n";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string[,] RotateMatrix(string[,] matrix, int N)
    {
        string[,] ret = matrix;

        //for (int i = 0; i < n; ++i)
        //{
        //    for (int j = 0; j < n; ++j)
        //    {
        //        ret[i, j] = matrix[n - j - 1, i];
        //    }
        //}

        for (int i = 0; i < N / 2; i++)
        {
            for (int j = i; j < N - i - 1; j++)
            {
                string temp = ret[i, j];
                ret[i, j] = ret[N - 1 - j, i];
                ret[N - 1 - j, i] = ret[N - 1 - i, N - 1 - j];
                ret[N - 1 - i, N - 1 - j] = ret[j, N - 1 - i];
                ret[j, N - 1 - i] = temp;
            }
        }

        return ret;
    }
}
