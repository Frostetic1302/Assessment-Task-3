using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject dirtTile;
    public GameObject outsideCorner;
    public GameObject outsideWall;
    public GameObject insideCorner;
    public GameObject insideWall;
    public GameObject pellet;
    public GameObject powerPellet;
    public GameObject tJunction;
    public GameObject ghostExit;

    int[,] quad = {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,8},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    int[,] full;

    void Start()
    {
        GameObject lvl = GameObject.Find("Level");
        if (lvl != null) Destroy(lvl);

        MakeMap();
        MakeLevel();
        CamFix();
    }

    void MakeMap()
    {
        int a = quad.GetLength(0);
        int b = quad.GetLength(1);

        int rr = (a * 2) - 1;
        int cc = b * 2;

        full = new int[rr, cc];

        for (int r = 0; r < rr; r++)
        {
            for (int c = 0; c < cc; c++)
            {
                int r2;
                int c2;

                if (r < a) r2 = r;
                else r2 = rr - 1 - r;

                if (c < b) c2 = c;
                else c2 = cc - 1 - c;

                full[r, c] = quad[r2, c2];
            }
        }
    }

    void MakeLevel()
    {
        int rmax = full.GetLength(0);
        int cmax = full.GetLength(1);

        GameObject p = new GameObject("Generated Level");

        for (int r = 0; r < rmax; r++)
        {
            for (int c = 0; c < cmax; c++)
            {
                int t = full[r, c];
                GameObject pf = GetPF(t);

                if (pf == null)
                {
                    // nothing here
                }
                else
                {
                    Vector3 pos = new Vector3(c, -r, 0);
                    GameObject o = Instantiate(pf, pos, Quaternion.identity, p.transform);
                    o.transform.rotation = Rot(r, c, t);
                }
            }
        }
    }

    void CamFix()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        cam.orthographic = true;

        int r = full.GetLength(0);
        int c = full.GetLength(1);

        float xx = (c - 1) * 0.5f;
        float yy = -(r - 1) * 0.5f;

        cam.transform.position = new Vector3(xx, yy, -10);

        float asp = (float)Screen.width / Screen.height;
        float vs = r * 0.5f;
        float hs = (c * 0.5f) / asp;

        cam.orthographicSize = Mathf.Max(vs, hs) + 1;
    }

    GameObject GetPF(int t)
    {
        if (t == 0) return dirtTile;
        if (t == 1) return outsideCorner;
        if (t == 2) return outsideWall;
        if (t == 3) return insideCorner;
        if (t == 4) return insideWall;
        if (t == 5) return pellet;
        if (t == 6) return powerPellet;
        if (t == 7) return tJunction;
        if (t == 8) return ghostExit;
        return null;
    }

    int Tile(int r, int c)
    {
        if (r < 0 || c < 0 || r >= full.GetLength(0) || c >= full.GetLength(1))
        {
            return -1;
        }
        return full[r, c];
    }

    bool IsW(int t)
    {
        if (t == 1) return true;
        if (t == 2) return true;
        if (t == 3) return true;
        if (t == 4) return true;
        if (t == 7) return true;
        if (t == 8) return true;
        return false;
    }

    Quaternion Rot(int r, int c, int t)
    {
        if (t == 1) return RotOut(r, c);
        if (t == 3) return RotIn(r, c);
        if (t == 2 || t == 4) return RotWall(r, c);
        if (t == 7) return RotTJ(r, c);
        return Quaternion.identity;
    }

    Quaternion RotOut(int r, int c)
    {
        bool U = IsW(Tile(r - 1, c));
        bool D = IsW(Tile(r + 1, c));
        bool L = IsW(Tile(r, c - 1));
        bool R = IsW(Tile(r, c + 1));

        if (U && L) return Quaternion.Euler(0, 0, 0);
        if (U && R) return Quaternion.Euler(0, 0, 90);
        if (D && R) return Quaternion.Euler(0, 0, 180);
        if (D && L) return Quaternion.Euler(0, 0, 270);

        return Quaternion.identity;
    }

    Quaternion RotIn(int r, int c)
    {
        bool U = IsW(Tile(r - 1, c));
        bool D = IsW(Tile(r + 1, c));
        bool L = IsW(Tile(r, c - 1));
        bool R = IsW(Tile(r, c + 1));

        if (U && L) return Quaternion.Euler(0, 0, 0);
        if (U && R) return Quaternion.Euler(0, 0, 90);
        if (D && R) return Quaternion.Euler(0, 0, 180);
        if (D && L) return Quaternion.Euler(0, 0, 270);

        return Quaternion.identity;
    }

    Quaternion RotWall(int r, int c)
    {
        bool U = IsW(Tile(r - 1, c));
        bool D = IsW(Tile(r + 1, c));
        bool L = IsW(Tile(r, c - 1));
        bool R = IsW(Tile(r, c + 1));

        int v = 0;
        int h = 0;

        if (U) v++;
        if (D) v++;
        if (L) h++;
        if (R) h++;

        if (h > v) return Quaternion.Euler(0, 0, 90);
        return Quaternion.Euler(0, 0, 0);
    }

    Quaternion RotTJ(int r, int c)
    {
        bool U = IsW(Tile(r - 1, c));
        bool D = IsW(Tile(r + 1, c));
        bool L = IsW(Tile(r, c - 1));
        bool R = IsW(Tile(r, c + 1));

        if (!D) return Quaternion.Euler(0, 0, 90);
        if (!L) return Quaternion.Euler(0, 0, 0);
        if (!U) return Quaternion.Euler(0, 0, 270);
        return Quaternion.Euler(0, 0, 180);
    }
}
