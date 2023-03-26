using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public GameControllerComponent GameControllerComponent;

    public UIControllerComponent UIControllerComponent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIControllerComponent.ShowMainMenu();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            SelectCell();
        }
    }

    private void SelectCell()
    {
        Camera camera = Camera.main;

        var hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<CellComponent>() is CellComponent cell)
            {
                GameControllerComponent.ChangeColor(cell.CellColor);
            }
        }
    }
}
