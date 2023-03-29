using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    Map map;

    [SerializeField]
    GameObject grid;

    [SerializeField]
    GameObject agent;

    [SerializeField]
    GameObject destination;

    [SerializeField]
    GameObject obstaclePrefab;

    [SerializeField]
    GameObject obstaclesParent;

    [SerializeField]
    Button descriptionCloseBtn;

    [SerializeField]
    GameObject description;
    private GameObject[,] obstacleObjectsArray = new GameObject[32, 18];
    private bool valuesCalculated = false;
    private bool valuesPrinted = false;
    private bool followingPath = false;
    private bool agentSelected = false;
    private bool destinationSelected = false;
    private Location[] path;

    void Start()
    {
        map = new Map(
            32,
            18,
            (int)agent.transform.position.x,
            (int)agent.transform.position.z,
            (int)destination.transform.position.x,
            (int)destination.transform.position.z
        );
        CreateTexts();
    }

    void Update()
    {
        MouseInteractions();
        CalculateMapValues();
        PrintCalculatedValues();
        FollowPath();
        Reset();
        DescriptionPanel();
        CloseGame();
    }

    private void CloseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void DescriptionPanel()
    {
        if (description.activeSelf)
        {
            descriptionCloseBtn.onClick.AddListener(() => description.SetActive(false));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            description.SetActive(true);
        }
    }

    private void Reset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void FollowPath()
    {
        if (Input.GetKeyDown(KeyCode.Space) && valuesPrinted && !followingPath)
        {
            path = PathFinding.GetPath(
                map.digitMap,
                (int)agent.transform.position.x,
                (int)agent.transform.position.z,
                (int)destination.transform.position.x,
                (int)destination.transform.position.z
            );
            HighlightPath();

            StartCoroutine(Follow());
            followingPath = true;
        }
    }

    IEnumerator Follow()
    {
        int pathIndex = 0;
        while (path.Length > pathIndex)
        {
            agent.transform.position = new Vector3(
                path[pathIndex].x + .5f,
                .5f,
                path[pathIndex].z + .5f
            );
            pathIndex++;
            yield return new WaitForSeconds(.5f);
        }
    }

    private void HighlightPath()
    {
        for (int i = 0; i < path.Length; i++)
        {
            TextMesh textMesh = TextCreator.textMeshObjects[
                path[i].x,
                path[i].z
            ].GetComponent<TextMesh>();
            textMesh.color = Color.blue;
        }
    }

    private void CalculateMapValues()
    {
        if (Input.GetKeyDown(KeyCode.C) && !valuesCalculated)
        {
            PathFinding.CalculateMapDigits(
                map.digitMap,
                (int)agent.transform.position.x,
                (int)agent.transform.position.z,
                (int)destination.transform.position.x,
                (int)destination.transform.position.z
            );
            valuesCalculated = true;
        }
    }

    private void PrintCalculatedValues()
    {
        if (Input.GetKeyDown(KeyCode.C) && valuesCalculated && !valuesPrinted)
        {
            for (int x = 0; x < map.digitMap.GetLength(0); x++)
            {
                for (int z = 0; z < map.digitMap.GetLength(1); z++)
                {
                    TextMesh textMesh = TextCreator.textMeshObjects[x, z].GetComponent<TextMesh>();
                    textMesh.text = map.digitMap[x, z].ToString();
                }
            }
            valuesPrinted = true;
        }
    }

    private void MouseInteractions()
    {
        if (Input.GetMouseButtonDown(0) && !valuesCalculated)
        {
            Vector3 worldPosition = GetMouseWorldPosition();
            switch (map.digitMap[(int)worldPosition.x, (int)worldPosition.z])
            {
                case -1:
                    if (!agentSelected && !destinationSelected)
                    {
                        agent.SetActive(false);
                        map.digitMap[
                            (int)agent.transform.position.x,
                            (int)agent.transform.position.z
                        ] = 0;
                        TextCreator.textMeshObjects[
                            (int)agent.transform.position.x,
                            (int)agent.transform.position.z
                        ]
                            .GetComponent<TextMesh>()
                            .text = "0";
                        agentSelected = true;
                    }
                    return;
                case 1:
                    Destroy(obstacleObjectsArray[(int)worldPosition.x, (int)worldPosition.z]);
                    map.digitMap[(int)worldPosition.x, (int)worldPosition.z] = 0;
                    return;
                case 2:
                    if (!agentSelected && !destinationSelected)
                    {
                        destination.SetActive(false);
                        map.digitMap[
                            (int)destination.transform.position.x,
                            (int)destination.transform.position.z
                        ] = 0;
                        TextCreator.textMeshObjects[
                            (int)destination.transform.position.x,
                            (int)destination.transform.position.z
                        ]
                            .GetComponent<TextMesh>()
                            .text = "0";
                        destinationSelected = true;
                    }
                    return;
                case 0:
                    if (agentSelected)
                    {
                        agent.transform.position = worldPosition;
                        map.digitMap[
                            (int)agent.transform.position.x,
                            (int)agent.transform.position.z
                        ] = -1;
                        agent.SetActive(true);
                        TextCreator.textMeshObjects[
                            (int)agent.transform.position.x,
                            (int)agent.transform.position.z
                        ]
                            .GetComponent<TextMesh>()
                            .text = "-1";
                        agentSelected = false;
                    }
                    else if (destinationSelected)
                    {
                        destination.transform.position = worldPosition;
                        map.digitMap[
                            (int)destination.transform.position.x,
                            (int)destination.transform.position.z
                        ] = 2;
                        destination.SetActive(true);
                        TextCreator.textMeshObjects[
                            (int)destination.transform.position.x,
                            (int)destination.transform.position.z
                        ]
                            .GetComponent<TextMesh>()
                            .text = "2";
                        destinationSelected = false;
                    }
                    else
                    {
                        map.digitMap[(int)worldPosition.x, (int)worldPosition.z] = 1;
                        obstacleObjectsArray[(int)worldPosition.x, (int)worldPosition.z] =
                            Instantiate(
                                obstaclePrefab,
                                worldPosition,
                                Quaternion.identity,
                                obstaclesParent.transform
                            );
                    }
                    break;
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.x = (int)worldPosition.x;
        worldPosition.x += .5f;
        worldPosition.z = (int)worldPosition.z;
        worldPosition.z += .5f;
        worldPosition.y = 0;
        return worldPosition;
    }

    private void CreateTexts()
    {
        TextCreator.SetTextArraySize(map.digitMap.GetLength(0), map.digitMap.GetLength(1));
        for (int x = 0; x < map.digitMap.GetLength(0); x++)
        {
            for (int z = 0; z < map.digitMap.GetLength(1); z++)
            {
                TextCreator.CreateWorldText(
                    x,
                    z,
                    grid.transform,
                    map.digitMap[x, z].ToString(),
                    GetWorldCanvasPosition(x, z),
                    Color.white,
                    TextAnchor.MiddleCenter,
                    TextAlignment.Left,
                    5000
                );
            }
        }
    }

    private Vector3 GetWorldCanvasPosition(int x, int z)
    {
        return new Vector3(x + .5f, 0, z + .5f);
    }
}
