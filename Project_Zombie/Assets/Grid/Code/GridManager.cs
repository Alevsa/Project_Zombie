using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// 
/// http://www.redblobgames.com/grids/hexagons/ is the main resource I used to get hte geometry right. We're using cubic coordinated from 
/// this site, it has pretty much all the background needed.
/// 
/// Some details I think are important, X + Y + Z = 0 on all tiles. 
/// Z is the vertical plane while X&Y represent the horizontal plane, we use both X and Y
/// 
public class GridManager : Singleton<GridManager>
{
    public enum GridType
    {
        Square,
        Disc,
        Diamond,
        Cylinder,
        Rectangle,
        Star,
    }

    public Tile BaseTile;
    public int GridSize = 2;

    public Dictionary<int[], Tile> Grid;
    // These affect the building of a disc shaped grid. (Smoothing isn't a big deal with small grids but I'm leaving it in regardless)
    public int SmoothingFactor = 4;
    public int SmoothingModifier = 4;

    private void Init()
    {
        if (Grid != null)
        {
            DestroyGrid();
        }
        Grid = new Dictionary<int[], Tile>(new EqualityComparer());
    }

    public void CreateGrid(GridType type)
    {
        Init();
        StartCoroutine(Sequence
        (
             CreateGridOfType(type)
           , InitialiseTiles()      // <- this must be called before setting neighbours cause the neighbours list needs to be newed
           , SetGridNeighbours()
        ));
    }

    private IEnumerator InitialiseTiles()
    {
        foreach (Tile tile in Grid.Values)
        {
            tile.Init();
        }
        yield return null;
    }

    private void DestroyGrid()
    {
        foreach (Tile tile in Grid.Values)
        {
            Destroy(tile.gameObject);
        }
        Grid.Clear();
    }

    private IEnumerator Sequence(params IEnumerator[] aSequence)
    {
        for (int i = 0; i < aSequence.Length; ++i)
        {
            while (aSequence[i].MoveNext())
                yield return aSequence[i].Current;
        }

        EventManager.Send<GridCreated>(this, new GridCreated(Grid));
        Debug.Log("Number of tiles created: " + Grid.Count, gameObject);
    }

    private IEnumerator SetGridNeighbours()
    {
        foreach (Tile tile in Grid.Values)
        {
            Tile Output;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        if (x + y + z == 0)
                        {
                            if (Grid.TryGetValue(
                            new int[]
                            {
                                 tile.HexDetails.x + x
                                ,tile.HexDetails.y + y
                                ,tile.HexDetails.z + z
                            }, out Output))
                            {
                                if (Output != tile)
                                {
                                    tile.Neighbours.Add(Output);
                                }
                            }
                            else
                            {
                                tile.EdgeTile = true;                             
                            }
                        }
                    }
                }
            }
        }
        yield return null;
    }

    private IEnumerator CreateGridOfType(GridType type)
    {
        switch (type)
        {
            case GridType.Disc:
                BuildDiscGrid();
                break;
            case GridType.Diamond:
                BuildDiamondGrid();
                break;
            case GridType.Star:
                BuildStarGrid();      // <- Only starts to look like a star at larger scales
                break;
            case GridType.Cylinder:
                BuildCylinderGrid();  // <- rectangular shape which wraps around on the left and right edges
                break;
            case GridType.Square:
                BuildSquareGrid();
                break;
            case GridType.Rectangle:
                BuildRectangleGrid();
                break;
            default:
                Debug.Log("Didn't get a proper grid type for creation");
                break;
        }
        yield return null;
    }

    private void BuildDiscGrid()
    {
        for (int x = -GridSize - (GridSize/2); x < GridSize + (GridSize / 2); x++)
        {
            for (int y = -GridSize - (GridSize / 2); y < GridSize + (GridSize / 2); y++)
            {
                for (int z = -GridSize - (GridSize / 2); z < GridSize + (GridSize/2); z++)
                {
                    if (x + y + z == 0)
                    {
                       // Debug.Log("x " + x + " y " + y + " z " + z);
                        if (
                                (   
                                    (Mathf.Abs(z) > GridSize) 
                                    && 
                                    (
                                        ( Mathf.Abs(x) < ( -GridSize/ SmoothingFactor + (SmoothingModifier * (Mathf.Abs(z) - GridSize))) )
                                        || ( Mathf.Abs(y) < (-GridSize / SmoothingFactor + (SmoothingModifier * (Mathf.Abs(z) - GridSize))) )
                                    )
                                )
                                || 
                                (
                                    (Mathf.Abs(y) > GridSize) 
                                    && 
                                    (
                                        ( Mathf.Abs(x) < (-GridSize / SmoothingFactor + (SmoothingModifier * (Mathf.Abs(y) - GridSize))) )
                                        || ( Mathf.Abs(z) < (-GridSize / SmoothingFactor + (SmoothingModifier * (Mathf.Abs(y) - GridSize))) )
                                    )
                                )
                                || 
                                (
                                    (Mathf.Abs(x) > GridSize) 
                                    &&
                                    (
                                        ( Mathf.Abs(y) < (-GridSize / SmoothingFactor + (SmoothingModifier * (Mathf.Abs(x) - GridSize))) )
                                        || ( Mathf.Abs(z) < (-GridSize / SmoothingFactor + (SmoothingModifier * (Mathf.Abs(x) - GridSize))) )
                                    )
                                )
                            )
                        {
                           // Debug.Log("Not placing here");
                        }
                        else
                        {
                            Vector3 loc = new Vector3(x, y, z);
                            Vector3 worldLoc = HexSpaceToWorldSpace(loc);
                            Tile tile = (Tile)Instantiate(BaseTile
                                                        , worldLoc
                                                        , Quaternion.identity
                                                        , transform);
                            Grid.Add(new int[] { x, y, z }, tile);
                            tile.HexDetails.x = x;
                            tile.HexDetails.y = y;
                            tile.HexDetails.z = z;
                        }
                    }
                }
            }
        }
    }

    private void BuildDiamondGrid()
    {
        for (int x = -GridSize; x < GridSize; x++)
        {
            for (int y = -GridSize; y < GridSize; y++)
            {
                // there's only a single value that can make the coordinates total zero
                // so there's no reason to loop through x or to check if it's zero.
                int z = -(x + y);
                Vector3 loc = new Vector3(x, y, z);
                Vector3 worldLoc = HexSpaceToWorldSpace(loc);
                Tile tile = (Tile)Instantiate(BaseTile
                                            , worldLoc
                                            , Quaternion.identity
                                            , transform);
                Grid.Add(new int[] { x, y, z }, tile);
                tile.HexDetails.x = x;
                tile.HexDetails.y = y;
                tile.HexDetails.z = z;
            }
        }
    }

    private void BuildStarGrid()
    {
        for (int x = -GridSize - GridSize / 2; x < GridSize + GridSize / 2; x++)
        {
            for (int y = -GridSize - GridSize / 2; y < GridSize + GridSize / 2; y++)
            {
                for (int z = -GridSize - GridSize / 2; z < GridSize + GridSize / 2; z++)
                {
                    if (x + y + z == 0)
                    {
                        if (
                                (
                                       (Mathf.Abs(z) > GridSize)
                                    && (Mathf.Abs(x) > (GridSize / (Mathf.Abs(z) - GridSize)))
                                    && (Mathf.Abs(y) > (GridSize / (Mathf.Abs(z) - GridSize)))
                                )
                                ||
                                (
                                        (Mathf.Abs(y) > GridSize)
                                    && (Mathf.Abs(x) > (GridSize / (Mathf.Abs(y) - GridSize)))
                                    && (Mathf.Abs(z) > (GridSize / (Mathf.Abs(y) - GridSize)))
                                )
                                ||
                                (
                                        (Mathf.Abs(x) > GridSize)
                                    && (Mathf.Abs(y) > (GridSize / (Mathf.Abs(x) - GridSize)))
                                    && (Mathf.Abs(z) > (GridSize / (Mathf.Abs(x) - GridSize)))
                                )
                            )
                        {
                           // Debug.Log("Not placing here");
                        }
                        else
                        {
                            Vector3 loc = new Vector3(x, y, z);
                            Vector3 worldLoc = HexSpaceToWorldSpace(loc);
                            Tile tile = (Tile)Instantiate(BaseTile
                                                        , worldLoc
                                                        , Quaternion.identity
                                                        , transform);
                            Grid.Add(new int[] { x, y, z }, tile);
                            tile.HexDetails.x = x;
                            tile.HexDetails.y = y;
                            tile.HexDetails.z = z;
                        }
                    }
                }
            }
        }
    }
    // at z > 0 the tiles are shifted to the left by one
    private void BuildCylinderGrid()
    {
        for (int z = 0; z < GridSize; z++)
        {
            // no idea why you have to do a float division here and round, probably some interesting reason but happy it's working for now.
            // Possible performance issue too (though not with small scale maps)
            for (int y = -Mathf.FloorToInt(z/2f); y < GridSize + GridSize/2 - Mathf.FloorToInt(z/2f); y++)
            {
                int x = -(z + y);
                Vector3 loc = new Vector3(x, y, z);
                Vector3 worldLoc = HexSpaceToWorldSpace(loc);
                Tile tile = (Tile)Instantiate(BaseTile
                                            , worldLoc
                                            , Quaternion.identity
                                            , transform);
                Grid.Add(new int[] { x, y, z }, tile);
                tile.HexDetails.x = x;
                tile.HexDetails.y = y;
                tile.HexDetails.z = z;
            }
        }
        WrapIntoCylinder();
    }

    private void WrapIntoCylinder()
    {
        Dictionary<int, Tile> leftEdge = new Dictionary<int, Tile>();
        Dictionary<int, Tile> rightEdge = new Dictionary<int, Tile>();
        for (int z = 0; z < GridSize; z++)
        {
            int x = (-z / 2) - (z & 1);
            int y = -(z + x);
           // Debug.Log("LEFT EDGE: X " + x + " Y " + y + " Z " + z);
            rightEdge.Add(z,Grid[new int[]
            {
                x
            ,   y
            ,   z
            }]);
            y = (GridSize + GridSize / 2 - Mathf.FloorToInt(z / 2f)) - 1;
            x = -(z + y);
            leftEdge.Add(z, Grid[new int[]
            {
                x
            ,   y
            ,   z
            }]);
          //  Debug.Log("RIGHT EDGE: X " + x + " Y " + y + " Z " + z);
        }

        for (int z = 0; z < GridSize; z++)
        {
            leftEdge[z].Neighbours.Add(rightEdge[z]);
            rightEdge[z].Neighbours.Add(leftEdge[z]);
            if ((z & 1) == 0)
            {
                Tile output;
                if (leftEdge.TryGetValue((z - 1), out output))
                {
                    rightEdge[z].Neighbours.Add(output);
                    output.Neighbours.Add(rightEdge[z]);
                }
                if (leftEdge.TryGetValue((z + 1), out output))
                {
                    rightEdge[z].Neighbours.Add(output);
                    output.Neighbours.Add(rightEdge[z]);
                }
            }
        }
    }

    private void BuildSquareGrid()
    {
        for (int z = 0; z < GridSize; z++)
        {
            for (int y = -z / 2; y < GridSize - (z / 2); y++)
            {
                int x = -(z + y);
                Vector3 loc = new Vector3(x, y, z);
                Vector3 worldLoc = HexSpaceToWorldSpace(loc);
                Tile tile = (Tile)Instantiate(BaseTile
                                            , worldLoc
                                            , Quaternion.identity
                                            , transform);
                Grid.Add(new int[] { x, y, z }, tile);
                tile.HexDetails.x = x;
                tile.HexDetails.y = y;
                tile.HexDetails.z = z;
            }
        }
    }

    private void BuildRectangleGrid()
    {
        for (int z = 0; z < GridSize; z++)
        {
            for (int y = -z / 2; y < GridSize + GridSize / 2 - (z/2); y++)
            {
                int x = -(z + y);
                Vector3 loc = new Vector3(x, y, z);
                Vector3 worldLoc = HexSpaceToWorldSpace(loc);
                Tile tile = (Tile)Instantiate(BaseTile
                                            , worldLoc
                                            , Quaternion.identity
                                            , transform);
                Grid.Add(new int[] { x, y, z }, tile);
                tile.HexDetails.x = x;
                tile.HexDetails.y = y;
                tile.HexDetails.z = z;
            }
        }
    }

    public void LoadMap(HexInfo[] aHexInfo)
    {
        Init();
        foreach (HexInfo info in aHexInfo)
        {
            Vector3 loc = new Vector3(info.x, info.y, info.z);
            Vector3 worldLoc = HexSpaceToWorldSpace(loc);
            Tile tile = (Tile)Instantiate(BaseTile
                                            , worldLoc
                                            , Quaternion.identity
                                            , transform);
            Grid.Add(new int[] { info.x, info.y, info.z }, tile);
            tile.HexDetails = info;
            tile.Init();
        }
        StartCoroutine("SetGridNeighbours");
    }

    private Vector3 HexSpaceToWorldSpace(Vector3 aCoord)
    {
        float y = aCoord.z * Mathf.Sqrt(3f);
        float x = aCoord.x - aCoord.y;
        return new Vector3(x, y, 0f);
    }
}
