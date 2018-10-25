using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// A Parent transform for our map, this will prevent our hierarchy to be flooded with tiles
    /// </summary>
    [SerializeField]
    private Transform map;

    /// <summary>
    /// Map data fall all the map layers grass trees etc.
    /// </summary>
    [SerializeField]
    private Texture2D[] mapData;

    /// <summary>
    /// A map element represents a tile that we can create in our game.
    /// </summary>
    [SerializeField]
    private MapElement[] mapElements;

    /// <summary>
    /// This tile is used for measuring the distance between tiles
    /// </summary>
    [SerializeField]
    private Sprite defaultTile;

    /// <summary>
    /// Dictionay for all water tiles
    /// </summary>
    private Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();

    /// <summary>
    /// An atlas containing all our water tiles
    /// </summary>
    [SerializeField]
    private SpriteAtlas waterAtlas;

    /// <summary>
    /// The position of the bottom left corner of the screen
    /// </summary>
    private Vector3 WorldStartPos
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        }
    }

	// Use this for initialization
	void Start ()
    { 
        GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Generates our map
    /// </summary>
    private void GenerateMap()
    {
        int height = mapData[0].height;
        int width = mapData[0].width;

        for (int i = 0; i < mapData.Length; i++)//Looks through all our map layers
        {
            for (int x = 0; x < mapData[i].width; x++) //Runs through all pixels on the layer 
            {
                for (int y = 0; y < mapData[i].height; y++)
                {
                    Color c = mapData[i].GetPixel(x, y); //Gets the color of the current pixel

                    //Checks if we have a tile that suits the color of the pixel on the map
                    MapElement newElement = Array.Find(mapElements, e => e.MyColor == c);

                    if (newElement != null) //If we found an element with the correct color
                    {
                        //Calculate x and y position of the tile
                        float xPos = WorldStartPos.x + (defaultTile.bounds.size.x * x);
                        float yPos = WorldStartPos.y + (defaultTile.bounds.size.y * y);

                        //Create the tile
                        GameObject go = Instantiate(newElement.MyElementPrefab);

                        //Set the tile's position
                        go.transform.position = new Vector2(xPos, yPos);

                        if (newElement.MyTileTag == "Water")
                        {
                            waterTiles.Add(new Point(x,y), go);
                        }
                        //Checks if we are placing a tree
                        if (newElement.MyTileTag == "Tree")
                        {
                            //If we are placing a tree then we need to manage the sort order
                            go.GetComponent<SpriteRenderer>().sortingOrder = height*2 - y*2;
                        }

                        //Make the tile a child of map
                        go.transform.parent = map;

                    }

                }
            }
        }

        CheckWater();
    }

    /// <summary>
    /// Checks all tiles around each water tile, so thar we can swap the sprite to the correct one
    /// </summary>
    public void CheckWater()
    {
        foreach (KeyValuePair<Point, GameObject> tile in waterTiles)
        {
            string composition = TileCheck(tile.Key);

            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("0");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("1");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("2");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("3");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("4");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("5");
            }
            if (composition[1] == 'W' && composition[4] == 'W' && composition[3] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("6");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("7");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("8");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("9");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("10");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("11");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("12");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("13");
            }
            if (composition[3] == 'W' && composition[5] == 'E' && composition[6] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("14");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[1] == 'W' && composition[2] == 'E' && composition[4] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("15");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("16");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("17");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                int randomTile = UnityEngine.Random.Range(0, 100);
                if (randomTile < 15)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("19");
                }
            }
            if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W')
            {
                int randomTile = UnityEngine.Random.Range(0, 100);
                if (randomTile < 10)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("20");
                }

            }

        }
    }

    /// <summary>
    /// Checks all neighbours of each tile 
    /// </summary>
    /// <param name="currentPoint">The position of the tile we are checking</param>
    /// <returns></returns>
    private string TileCheck(Point currentPoint)
    {
        string composition = string.Empty; //The composition that we are using

        for (int x = -1; x <= 1; x++)//Runs through all neighbours 
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0) //Makes sure that we aren't checking our self
                {
                    //If the value is a watertile
                    if (waterTiles.ContainsKey(new Point(currentPoint.MyX+x, currentPoint.MyY+y)))
                    {
                        composition += "W";
                    }
                    else //Else we assume it is empty 
                    {
                        composition += "E";
                    }
                }
            }
        }
        return composition;

    }
}

[Serializable]
public class MapElement
{
    /// <summary>
    /// This tile tag, this is used to check what tile we are placing
    /// </summary>
    [SerializeField]
    private string tileTag;

    /// <summary>
    /// The color of the tile, this is used to compare the tile with colors on the map layers
    /// </summary>
    [SerializeField]
    private Color color;

    /// <summary>
    /// Prefab that we use to spawn the til in our world
    /// </summary>
    [SerializeField]
    private GameObject elementPrefab;

    /// <summary>
    /// Property for accessing the prefab
    /// </summary>
    public GameObject MyElementPrefab
    {
       get
        {
            return elementPrefab;
        }
    }

    /// <summary>
    /// Property for accessing the color
    /// </summary>
    public Color MyColor
    {
        get
        {
            return color;
        }
    }

    /// <summary>
    /// Property for accessing the tag
    /// </summary>
    public string MyTileTag
    {
        get
        {
            return tileTag;
        }
    }
}
/// <summary>
/// Struct for determening grid positions in our GameWorld
/// </summary>
public struct Point
{
    public int MyX { get; set; }
    public int MyY { get; set; }

    public Point(int x, int y)
    {
        this.MyX = x;
        this.MyY = y;
    }


}
