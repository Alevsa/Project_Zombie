﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// 
/// http://www.redblobgames.com/grids/hexagons/ is the main resource I used to get hte geometry right. We're using cubic coordinated from 
/// this site, it has pretty much all the background needed.
/// 
/// Some details I think are important, X + Y + Z = 0 on all tiles. 
/// Z is the vertical plane while X&Y represent the horizontal plane, we use both X and Y
/// 
public class GridManager : Singleton<GridManager> {

    public Tile BaseTile;
    public int WorldSize = 2;

    private bool worldCreated = false;
    private Dictionary<int[], Tile> World;
    // These affect the building of a disc shaped grid. (Smoothing isn't a big deal with small grids but I'm leaving it in regardless)
    public int SmoothingFactor = 4;
    public int SmoothingModifier = 4;

    public Dictionary<int[], Tile> GetWorld()
    {
        if (worldCreated)
            return World;

        return null;
    }

    private void Init()
    {
        if (World != null)                                          
        {
            DestroyWorld();
        }
        World = new Dictionary<int[], Tile>(new EqualityComparer());
    }


    public void CreateWorld(string aType)
    {
        Init();
        StartCoroutine(Sequence
        (
             CreateGridOfType(aType)
           , InitialiseTiles()      // <- this must be called before setting neighbours cause the neighbours list needs to be newed
           , SetWorldNeighbours()
        ));
    }

    private IEnumerator InitialiseTiles()
    {
        foreach (Tile tile in World.Values)
        {
            tile.Init();
        }
        yield return null;
    }

    private void DestroyWorld()
    { 
        foreach (Tile tile in World.Values)
        {
            Destroy(tile.gameObject);
        }
        World.Clear();
    }

    private IEnumerator Sequence(params IEnumerator[] aSequence)
    {
        for (int i = 0; i < aSequence.Length; ++i)
        {
            while (aSequence[i].MoveNext())
                yield return aSequence[i].Current;
        }

        worldCreated = true;
        worldCreated = true;
    }

    private IEnumerator SetWorldNeighbours()
    {
        foreach (Tile tile in World.Values)
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
                            if (World.TryGetValue(
                            new int[]
                            {
                                 tile.CubicCoordinates[0] + x
                                ,tile.CubicCoordinates[1] + y
                                ,tile.CubicCoordinates[2] + z
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

    // PROLY NOT THE PROPER C# WAY OF DOING IT LOL BUT W/E (advice welcome)
    private IEnumerator CreateGridOfType(string aType)
    {
        switch (aType)
        {
            case "Disc":
                BuildDiscworld();
                break;
            case "Diamond":
                BuildDiamondWorld();
                break;
            case "Star":
                BuildStarworld();      // <- Only starts to look like a star at larger scales
                break;
            case "Cylinder":
                BuildCylinderWorld();  // <- rectangular shape which wraps around on the left and right edges
                break;
            case "Square":
                BuildSquareGrid();
                break;
            case "Rectangle":
                BuildRectangleGrid();
                break;
            default:
                Debug.Log("Didn't get a proper world type for creation");
                break;
        }
        yield return null;
    }

    private void BuildDiscworld()
    {
        for (int x = -WorldSize - (WorldSize/2); x < WorldSize + (WorldSize / 2); x++)
        {
            for (int y = -WorldSize - (WorldSize / 2); y < WorldSize + (WorldSize / 2); y++)
            {
                for (int z = -WorldSize - (WorldSize / 2); z < WorldSize + (WorldSize/2); z++)
                {
                    if (x + y + z == 0)
                    {
                       // Debug.Log("x " + x + " y " + y + " z " + z);
                        if (
                                (   
                                    (Mathf.Abs(z) > WorldSize) 
                                    && 
                                    (
                                        ( Mathf.Abs(x) < ( -WorldSize/ SmoothingFactor + (SmoothingModifier * (Mathf.Abs(z) - WorldSize))) )
                                        || ( Mathf.Abs(y) < (-WorldSize / SmoothingFactor + (SmoothingModifier * (Mathf.Abs(z) - WorldSize))) )
                                    )
                                )
                                || 
                                (
                                    (Mathf.Abs(y) > WorldSize) 
                                    && 
                                    (
                                        ( Mathf.Abs(x) < (-WorldSize / SmoothingFactor + (SmoothingModifier * (Mathf.Abs(y) - WorldSize))) )
                                        || ( Mathf.Abs(z) < (-WorldSize / SmoothingFactor + (SmoothingModifier * (Mathf.Abs(y) - WorldSize))) )
                                    )
                                )
                                || 
                                (
                                    (Mathf.Abs(x) > WorldSize) 
                                    &&
                                    (
                                        ( Mathf.Abs(y) < (-WorldSize / SmoothingFactor + (SmoothingModifier * (Mathf.Abs(x) - WorldSize))) )
                                        || ( Mathf.Abs(z) < (-WorldSize / SmoothingFactor + (SmoothingModifier * (Mathf.Abs(x) - WorldSize))) )
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
                            World.Add(new int[] { x, y, z }, tile);
                            tile.CubicCoordinates = new int[] { x, y, z };
                        }
                    }
                }
            }
        }
    }

    private void BuildDiamondWorld()
    {
        for (int x = -WorldSize; x < WorldSize; x++)
        {
            for (int y = -WorldSize; y < WorldSize; y++)
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
                World.Add(new int[] { x, y, z }, tile);
                tile.CubicCoordinates = new int[] { x, y, z };
            }
        }
    }

    private void BuildStarworld()
    {
        for (int x = -WorldSize - WorldSize / 2; x < WorldSize + WorldSize / 2; x++)
        {
            for (int y = -WorldSize - WorldSize / 2; y < WorldSize + WorldSize / 2; y++)
            {
                for (int z = -WorldSize - WorldSize / 2; z < WorldSize + WorldSize / 2; z++)
                {
                    if (x + y + z == 0)
                    {
                        if (
                                (
                                       (Mathf.Abs(z) > WorldSize)
                                    && (Mathf.Abs(x) > (WorldSize / (Mathf.Abs(z) - WorldSize)))
                                    && (Mathf.Abs(y) > (WorldSize / (Mathf.Abs(z) - WorldSize)))
                                )
                                ||
                                (
                                        (Mathf.Abs(y) > WorldSize)
                                    && (Mathf.Abs(x) > (WorldSize / (Mathf.Abs(y) - WorldSize)))
                                    && (Mathf.Abs(z) > (WorldSize / (Mathf.Abs(y) - WorldSize)))
                                )
                                ||
                                (
                                        (Mathf.Abs(x) > WorldSize)
                                    && (Mathf.Abs(y) > (WorldSize / (Mathf.Abs(x) - WorldSize)))
                                    && (Mathf.Abs(z) > (WorldSize / (Mathf.Abs(x) - WorldSize)))
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
                            World.Add(new int[] { x, y, z }, tile);
                            tile.CubicCoordinates = new int[] { x, y, z };
                        }
                    }
                }
            }
        }
    }
    // at z > 0 the tiles are shifted to the left by one
    private void BuildCylinderWorld()
    {
        for (int z = 0; z < WorldSize; z++)
        {
            // no idea why you have to do a float division here and round, probably some interesting reason but happy it's working for now.
            // Possible performance issue too (though not with small scale maps)
            for (int y = -Mathf.FloorToInt(z/2f); y < WorldSize + WorldSize/2 - Mathf.FloorToInt(z/2f); y++)
            {
                int x = -(z + y);
                Vector3 loc = new Vector3(x, y, z);
                Vector3 worldLoc = HexSpaceToWorldSpace(loc);
                Tile tile = (Tile)Instantiate(BaseTile
                                            , worldLoc
                                            , Quaternion.identity
                                            , transform);
                World.Add(new int[] { x, y, z }, tile);
                tile.CubicCoordinates = new int[] { x, y, z };
            }
        }
        WrapIntoCylinder();
    }

    private void WrapIntoCylinder()
    {
        Dictionary<int, Tile> leftEdge = new Dictionary<int, Tile>();
        Dictionary<int, Tile> rightEdge = new Dictionary<int, Tile>();
        for (int z = 0; z < WorldSize; z++)
        {
            int x = (-z / 2) - (z & 1);
            int y = -(z + x);
           // Debug.Log("LEFT EDGE: X " + x + " Y " + y + " Z " + z);
            rightEdge.Add(z,World[new int[]
            {
                x
            ,   y
            ,   z
            }]);
            y = (WorldSize + WorldSize / 2 - Mathf.FloorToInt(z / 2f)) - 1;
            x = -(z + y);
            leftEdge.Add(z, World[new int[]
            {
                x
            ,   y
            ,   z
            }]);
          //  Debug.Log("RIGHT EDGE: X " + x + " Y " + y + " Z " + z);
        }

        for (int z = 0; z < WorldSize; z++)
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
        for (int z = 0; z < WorldSize; z++)
        {
            for (int y = -z / 2; y < WorldSize - (z / 2); y++)
            {
                int x = -(z + y);
                Vector3 loc = new Vector3(x, y, z);
                Vector3 worldLoc = HexSpaceToWorldSpace(loc);
                Tile tile = (Tile)Instantiate(BaseTile
                                            , worldLoc
                                            , Quaternion.identity
                                            , transform);
                World.Add(new int[] { x, y, z }, tile);
                tile.CubicCoordinates = new int[] { x, y, z };
            }
        }
    }

    private void BuildRectangleGrid()
    {
        for (int z = 0; z < WorldSize; z++)
        {
            for (int y = -z / 2; y < WorldSize + WorldSize / 2 - (z/2); y++)
            {
                int x = -(z + y);
                Vector3 loc = new Vector3(x, y, z);
                Vector3 worldLoc = HexSpaceToWorldSpace(loc);
                Tile tile = (Tile)Instantiate(BaseTile
                                            , worldLoc
                                            , Quaternion.identity
                                            , transform);
                World.Add(new int[] { x, y, z }, tile);
                tile.CubicCoordinates = new int[] { x, y, z };
            }
        }
    }

    private Vector3 HexSpaceToWorldSpace(Vector3 aCoord)
    {
        float y = aCoord.z * Mathf.Sqrt(3f);
        float x = aCoord.x - aCoord.y;
        return new Vector3(x, y, 0f);
    }
}
