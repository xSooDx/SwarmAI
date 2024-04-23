using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class BuildController : MonoBehaviour
{
    public enum BuildMode
    {
        Destroy = -1,
        None = 0,
        Wall = 1,
        Hazard = 2,
        Coin = 3,
        Attractor = 4,
        Food = 5,
    }

    public Camera cam;

    public FlowFieldGrid flowGrid;
    public GameObject wallPrefab;
    public GameObject hazardPrefab;
    public GameObject attractorPrefab;
    public GameObject coinPrefab;
    public GameObject foodPrefab;

    public int maxScale = 4;
    public int minScale = 0;

    private CommandConsumer commandConsumer = new();

    int currentScale = 1;

    BuildMode currentBuildMode;
    GameObject mousePreview;

    // Start is called before the first frame update
    void Start()
    {
        SelectBuildMode((int)BuildMode.None);
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mp = cam.ScreenToWorldPoint(Input.mousePosition);
        mp.y = 0;

        if (mousePreview)
        {
            mousePreview.transform.position = mp;

            if (currentBuildMode != BuildMode.Coin)
            {
                if (Input.GetKeyDown(KeyCode.W) && currentScale < maxScale)
                {
                    mousePreview.transform.localScale *= 2f;
                    currentScale++;
                }
                if (Input.GetKeyDown(KeyCode.S) && currentScale > minScale)
                {
                    mousePreview.transform.localScale *= 0.5f;
                    currentScale--;
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    mousePreview.transform.Rotate(Vector3.up, 45);
                }
            }

            if (Input.GetMouseButtonDown(0) && flowGrid.IsWorldPosInsideGrid(mp))
            {
                commandConsumer.ExecuteCommand(new BuildCommand(mousePreview, mousePreview.transform.position, mousePreview.transform.rotation));
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                commandConsumer.UndoLastCommand();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                commandConsumer.RedoLastUndo();
            }
        }
        else if (currentBuildMode == BuildMode.Destroy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Collider[] colliders = Physics.OverlapSphere(mp, .25f);
                if (colliders.Length > 0 && colliders[0].attachedRigidbody)
                {
                    commandConsumer.ExecuteCommand(new DeleteCommand(colliders[0].attachedRigidbody.gameObject));
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            SelectBuildMode(-1);
        }
    }

    public void SelectBuildMode(int mode)
    {
        currentBuildMode = (BuildMode)mode;
        switch (currentBuildMode)
        {
            case BuildMode.None:
                ChangeMousePreview(null);
                break;
            case BuildMode.Wall:
                ChangeMousePreview(wallPrefab);
                break;
            case BuildMode.Hazard:
                ChangeMousePreview(hazardPrefab);
                break;

            case BuildMode.Coin:
                ChangeMousePreview(coinPrefab);
                break;

            case BuildMode.Attractor:
                ChangeMousePreview(attractorPrefab);
                break;

            case BuildMode.Food:
                ChangeMousePreview(foodPrefab);
                break;

            case BuildMode.Destroy:
                ChangeMousePreview(null);
                break;
            default:
                break;
        }
    }

    void ChangeMousePreview(GameObject prefab)
    {
        Destroy(mousePreview);
        currentScale = 1;
        if (prefab != null)
        {
            mousePreview = Instantiate(prefab);
        }
    }
}
