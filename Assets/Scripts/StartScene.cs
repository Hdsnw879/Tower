using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public ParticleSystem particle;
    public int dimensions;
    public Tile [] tileObjects;
    public List<Cell> gridComponents;
    public Cell cellObj;   
    public List<Tile> backupTile;
    bool firstWaypoint;
    public List<Tile> way; 
    private int iteration;
    private bool secondWaypoint ;
    public bool end;
    public List<int> wayIn,gateOut;
    List<Tile> instantiateObject;
    public List<Cell> cellForWay;
    public Vector3 where;
    public List<Material> skybox;
    Light gamelight;
    private float rate;
    public GameObject guide, mainStart;
    void RandomSkyBox()
    {
        gamelight = GameObject.Find("Directional Light").GetComponent<Light>();
        int indexRandom = UnityEngine.Random.Range(1,3);
        Material selectSkybox = skybox[indexRandom];
        switch (indexRandom)
        {
            case 0:
                gamelight.color = new Color(1.0f,1.0f,1.0f);
                selectSkybox = skybox[indexRandom];
                RenderSettings.skybox = selectSkybox;
                rate = 30f;
                break;
            case 1:
                gamelight.color = new Color(0.9647059f,0.8156863f,0.3882353f);
                selectSkybox = skybox[indexRandom];
                RenderSettings.skybox = selectSkybox;
                rate = 60f;
                break;
            case 2:
                gamelight.color = new Color(0.4339623f,0.4339623f,0.4339623f);
                selectSkybox = skybox[indexRandom];
                RenderSettings.skybox = selectSkybox;
                rate = 120f;
                break;
        }
    }
     void StartWaveFunction()
    {
        if(!end)
        {
            RandomSkyBox();
            iteration= 0;
            cellForWay = new List<Cell>();
            wayIn =  new List<int>();
            gateOut = new List<int>();
            instantiateObject= new List<Tile>();
            gridComponents = new List<Cell>();
            secondWaypoint = false;
            int x = UnityEngine.Random.Range((int)(dimensions * 0.4),(int)(dimensions * 0.5));
            firstWaypoint = false;
            InitializeGrid();
            Waypoint(x,10); 
            StartCoroutine(ShowWayPoint(cellForWay));
            end = true; 
            Effect();
        }
    }
    void Effect()
    {
        particle.gameObject.transform.localScale = new Vector3(2 * dimensions,2 * dimensions,2 * dimensions);
        Instantiate(particle,thatMidCell.transform.position + new Vector3(0,dimensions,0),particle.transform.rotation);
        var emission = particle.emission;
        emission.rateOverTime = rate;
    }
    void Awake()
    {
        guide.SetActive(false);
        Time.fixedDeltaTime = 0.001f;
        end = false;
        StartWaveFunction();
        
    }
    void Update()
    {
    }
    void InitializeGrid()
    {
        for(int x = 0;x < dimensions; x++)
        {
            for(int y = 0 ;y <dimensions;y++)
            {
                Cell newCell = Instantiate(cellObj,new Vector3(x * 2,0,y * 2),Quaternion.identity);
                newCell.CreateCell(false,tileObjects);
                gridComponents.Add(newCell);
            }
        }
        MidCell();
    }
    public Cell thatMidCell;
    void MidCell()
    {
        int midX = dimensions / 2;
        int midY = dimensions / 2;
        int midCell = midX + midY * dimensions;
        thatMidCell = gridComponents[midCell];
    }
    
    void Waypoint(int startPosition,int huongDi)
    {
        List<Cell> positionWayPoint = new List<Cell>(gridComponents);
        for(int x = 0; x < dimensions ; x++)
        {
            for(int y = 0; y < dimensions; y++)
            { 
                int currentArray = x + y * dimensions;
                if(currentArray == startPosition)
                {   

                    if(y >= dimensions -1)
                    {
                        WayNeedToGo(positionWayPoint[startPosition],x,y,dimensions);
                        GoForward(x,y,dimensions,false);
                        break;
                    }
                    if(firstWaypoint == false && y == 0)
                    {
                        WayNeedToGo(positionWayPoint[startPosition],x,y,dimensions);
                        GoForward(x,y,dimensions,false);
                        firstWaypoint = true;
                        break;
                    }
                    if(huongDi == 0)
                    {
                        WayNeedToGo(positionWayPoint[startPosition],x,y,dimensions);
                        CheckSurrond(x,y,dimensions);
                        break;
                    }else if(huongDi == 1)
                    {
                        WayNeedToGo(positionWayPoint[startPosition],x,y,dimensions);
                        GoForward(x,y,dimensions,false);
                        break;
                    }else if(huongDi == 3)
                    {
                        WayNeedToGo(positionWayPoint[startPosition],x,y,dimensions);
                        GoRight(x,y,dimensions);
                        break;
                    }else if(huongDi == 4)
                    {
                        WayNeedToGo(positionWayPoint[startPosition],x,y,dimensions);
                        GoLeft(x,y,dimensions);
                        break;
                    }
                }
            }
        }
    }
    
    void WayNeedToGo(Cell cell,int x,int y,int dimensions)
    {
        if(cell.collapsed == false)
        {
            cell.collapsed = true;
            Tile[] wayPointTile = new Tile[1];
            wayPointTile[0] = way[0];
            cell.RecreateCell(wayPointTile);
            cellForWay.Add(cell);
        }
    }
    
    void CheckSurrond(int x,int y,int dimensions)
    {
        
        int randomDirection = UnityEngine.Random.Range(0,3);
        switch(randomDirection)
        {
            case 0:
                if(y + 1 < dimensions)
                {
                    int goForward = x + (1 + y) * dimensions;
                    Waypoint(goForward,0);
                }
                break;
            case 1:
                
                if(x + 1 < dimensions -1 )
                {
                    int goRight = x + 1 + y * dimensions;
                    Waypoint(goRight,3);
                }else
                {
                    CheckSurrond(x,y,dimensions);
                }
                break;
            case 2:
                
                if(x - 1 >= 1)
                {
                    
                    int goLeft = x - 1 + y * dimensions;
                    Waypoint(goLeft,4);
                }else
                {
                    CheckSurrond(x,y,dimensions);
                }
                break;
        }
    }
    void GoRight(int x,int y,int dimensions)
    {
        int randomDirection = UnityEngine.Random.Range(0,2);
        switch(randomDirection)
        {
            case 0:
                if(y < dimensions)
                {
                  int goForward = x + (y + 1) * dimensions;
                  Waypoint(goForward,1);
                }
                break;
            case 1:
                if(x + 1 < dimensions - 1)
                {
                    int goRight = x + 1 + y * dimensions;
                    Waypoint(goRight,3);
                }else
                {
                    GoForward(x,y,dimensions,true);
                }
                break;
        }
    }
    void GoLeft(int x,int y,int dimensions)
    {
        int randomDirection = UnityEngine.Random.Range(0,2);
        switch(randomDirection)
        {
            case 0:
                if(y + 1 < dimensions)
                {
                  int goForward = x + (y + 1) * dimensions;
                  Waypoint(goForward,1);
                }
                break;
            case 1:
                if(x - 1 >=1)
                {
                    int goLeft = x - 1 + y * dimensions;
                    Waypoint(goLeft,4);
                }else 
                {
                    GoForward(x,y,dimensions,true);
                }
                break;
        }
    }
    void GoForward(int x,int y,int dimensions,bool twoStep)
        {
            int goForward = x + (y+1) * dimensions;
            if(!twoStep)
            {
                Waypoint(goForward,0);
                int forward = x + (y + 2) * dimensions;
                if(gridComponents[dimensions].collapsed)
                {
                    CheckSurrond(x,y,dimensions);
                }
            }
            else if(twoStep)
            {
                Waypoint(goForward,1);
                int forward = x + (y + 2) * dimensions;
                if(gridComponents[dimensions].collapsed)
                {
                    CheckSurrond(x,y,dimensions);
                }
            }
        }

    
    IEnumerator ShowWayPoint(List<Cell> cell)
    {
        if (dimensions >= 15 && !secondWaypoint)
        {
            SecondWaypoint(cellForWay);
            secondWaypoint = true;
            StartCoroutine(ShowWayPoint(cellForWay));
        }
        else
        {
            foreach (Cell t in cell)
            {
                Cell selectedCell = t;
                int gridIndex = gridComponents.IndexOf(selectedCell);
                int y = gridIndex / dimensions;
                if (y >= dimensions - 1)
                {
                    Tile gameTile = Instantiate(way[2], t.transform.position + where, transform.rotation);
                    yield return StartCoroutine(Animation(gameTile.transform,t.transform));
                    instantiateObject.Add(gameTile);
                }
                else if (y == 0)
                {
                    Tile gameTile = Instantiate(way[1], t.transform.position + where, transform.rotation);
                    yield return StartCoroutine(Animation(gameTile.transform,t.transform));
                    instantiateObject.Add(gameTile);
                }
                else
                {
                    Tile gameTile = Instantiate(way[0], t.transform.position + where, transform.rotation);
                    yield return StartCoroutine(Animation(gameTile.transform,t.transform));
                    instantiateObject.Add(gameTile);
                }
                
            }
            UpdateGeneration();
        }
    }
    void SecondWaypoint(List<Cell> cells)
    {
        int selectedIndex = UnityEngine.Random.Range((int)(cellForWay.Count*0.05f),(int)(cellForWay.Count * 0.2f));
        Cell selectedCell = cells[selectedIndex];

        // Lấy vị trí của ô trong lưới
        int gridIndex = gridComponents.IndexOf(selectedCell);
        int x = gridIndex % dimensions;
        int y = gridIndex / dimensions;
        int start = x + y * dimensions;
        Waypoint(start,0);
    }
    
    void CheckEntropy()
    {
        List<Cell> tempGrid = new List<Cell>(gridComponents);
        //Xoá những Cell đã đc đánh dấu
        tempGrid.RemoveAll(a => a.collapsed);
        tempGrid.Sort((a,b) => a.tileOptions.Length - b.tileOptions.Length );
        tempGrid.RemoveAll(a => a.tileOptions.Length != tempGrid[0].tileOptions.Length);
        
        StartCoroutine(CollapseCell(tempGrid));
    }
    
    
    // Thêm ô vào danh sách visitedCells khi ghé thăm
    public List<string> listString = new List<string>() { "Way", "GateOut", "WayIn" };
    IEnumerator CollapseCell (List<Cell> tempGrid)
    {
        int randomIndex = UnityEngine.Random.Range(0, tempGrid.Count);
        if(randomIndex >= 0 && randomIndex < tempGrid.Count)
        {
        Cell collapseCell = tempGrid[randomIndex];
        collapseCell.collapsed = true;
        collapseCell.tileOptions = collapseCell.tileOptions.Where(tile => !listString.Contains(tile.name)).ToList().ToArray();
        try
        {
            
            Tile selectTile = collapseCell.tileOptions[UnityEngine.Random.Range(0,collapseCell.tileOptions.Length)];
            collapseCell.tileOptions = new Tile[]{selectTile};
        }catch
        {
            Tile selectTile = backupTile[UnityEngine.Random.Range(0,1)];
            collapseCell.tileOptions = new Tile[]{selectTile};
        }
        Tile gameTile = Instantiate(collapseCell.tileOptions[0], collapseCell.transform.position + where, collapseCell.tileOptions[0].transform.rotation);
        instantiateObject.Add(gameTile);
        yield return StartCoroutine(Animation(gameTile.transform,collapseCell.transform));
        UpdateGeneration();
        }   
    }
    void UpdateGeneration()
    {
        List<Cell> newGenerationCell = new List<Cell>(gridComponents);

        for(int y = 0; y < dimensions; y++)
        {
            for(int x = 0; x < dimensions; x++)
            {
                var index = x + y * dimensions;
                if (gridComponents[index].collapsed)
                {
                    newGenerationCell[index] = gridComponents[index];
                }
                else
                {
                    List<Tile> options = new List<Tile>(tileObjects);
                    bool up1,down1,left1,right1 = true;
                    try
                    {
                        Cell down = gridComponents[x + (y - 1) * dimensions];
                        if(down.collapsed)
                        {
                            down1 = true;
                        }else
                        {
                            down1 = false;
                        }
                        
                    }catch
                    {
                        down1 = false;
                    }

                    try
                    {
                        Cell right = gridComponents[x + 1 + y * dimensions];
                        if(right.collapsed)
                        {
                            right1=true;
                        }else
                        {
                            right1 = false;
                        }
                    }catch
                    {
                        right1 = false;
                    }

                    try
                    {
                        Cell up = gridComponents[x + (y+1) * dimensions];
                        if(up.collapsed)
                        {
                            up1 = true;
                        }else
                        {
                            up1 = false;
                        }
                    }catch
                    {
                       up1 = false;
                    }
                    try
                    {
                        Cell left = gridComponents[x - 1 + y * dimensions];
                        if(left.collapsed)
                        {
                            left1 = true;
                        }else
                        {
                            left1 = false;
                        }
                    }catch
                    {
                        left1 = false;
                    }
                    if(up1 || down1 || left1 || right1)
                    {
                    if(y > 0)
                    {
                        Cell up = gridComponents[x + (y - 1) * dimensions];
                        List<Tile> validOptions = new List<Tile>();

                        foreach(Tile possibleOptions in up.tileOptions)
                        {
                            var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                            var valid = tileObjects[validOption].downNeighbours;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    if(x < dimensions - 1)
                    {
                        Cell left = gridComponents[x + 1 + y * dimensions];
                        List<Tile> validOptions = new List<Tile>();

                        foreach(Tile possibleOptions in left.tileOptions)
                        {
                            var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                            var valid = tileObjects[validOption].rightNeighbours;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    if (y < dimensions - 1)
                    {
                        Cell down = gridComponents[x + (y+1) * dimensions];
                        List<Tile> validOptions = new List<Tile>();

                        foreach (Tile possibleOptions in down.tileOptions)
                        {
                            var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                            var valid = tileObjects[validOption].upNeighbours;
                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    if (x > 0)
                    {
                        Cell right = gridComponents[x - 1 + y * dimensions];
                        List<Tile> validOptions = new List<Tile>();

                        foreach (Tile possibleOptions in right.tileOptions)
                        {
                            int validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                            Tile[] valid = tileObjects[validOption].leftNeighbours;
                            validOptions = validOptions.Concat(valid).ToList();
                        }
                        CheckValidity(options, validOptions);
                    }
                    Tile[] newTileList = new Tile[options.Count];

                    for(int i = 0; i < options.Count; i++) {
                        newTileList[i] = options[i];
                    }
                    newGenerationCell[index].RecreateCell(newTileList);
                    }
                }
            }
        }

        gridComponents = newGenerationCell;         
        iteration++;

        if (iteration < dimensions * dimensions )
        {
            CheckEntropy();
        }
    }
    void CheckValidity(List<Tile> optionList, List<Tile> validOption)
    {
        for(int x = optionList.Count - 1; x >=0; x--)
        {
            Tile element = optionList[x];
            if (!validOption.Contains(element))
            {
                optionList.RemoveAt(x);
            }
        }
    }
    public float when; 
    IEnumerator Animation(Transform from, Transform to)
    {
        while(from.position != to.position)
        {
        from.position = Vector3.Lerp(from.position, to.position, when );
        //Debug.Log("Current Frame: " + Time.frameCount);
        yield return new WaitForFixedUpdate();
        }
        
    }
    public void MainGame()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void Guide()
    {
        mainStart.SetActive(false);
        guide.SetActive(true);
    }
    public void ExitGuide()
    {
        mainStart.SetActive(true);
        guide.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

