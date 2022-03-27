using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuType
{
    NewGame = 0,
    Continue,
    Option,
    Exit
}

public class Menu : MonoBehaviour
{
    enum DirectionType
    {
        Up,
        Down
    }

    [SerializeField] Guide guide;
    [SerializeField] Bar theBar;
    [SerializeField] Text[] menus;
    [SerializeField] int normalFontSize = 45;
    [SerializeField] int bigFontSize = 50;

    Dictionary<MenuType, Vector3> menuPosition;
    Dictionary<MenuType, Text> menuText;
    List<Vector3> destination;

    MenuType menuType = MenuType.NewGame;

    public bool onEnable;

    bool onUp;
    bool onDown;
    bool onSelect;

    void Awake()
    {
        menuPosition = new Dictionary<MenuType, Vector3>();
        menuText = new Dictionary<MenuType, Text>();
        destination = new List<Vector3>();       
    }

    void Start()
    {
        SetDestination();
    }

    void SetDestination()
    {
        foreach (var menu in menus)
        {
            destination.Add(menu.transform.localPosition);
        }

        menuPosition.Add(MenuType.NewGame, destination[0]);
        menuPosition.Add(MenuType.Continue, destination[1]);
        menuPosition.Add(MenuType.Option, destination[2]);
        menuPosition.Add(MenuType.Exit, destination[3]);

        menuText.Add(MenuType.NewGame, menus[0]) ;
        menuText.Add(MenuType.Continue, menus[1]);
        menuText.Add(MenuType.Option, menus[2]);
        menuText.Add(MenuType.Exit, menus[3]);
    }

    void Update()
    {
        InputKey();
    }

    void InputKey()
    {
        if (!onEnable)
            return;

        onUp = Input.GetKeyDown(KeyCode.UpArrow);
        onDown = Input.GetKeyDown(KeyCode.DownArrow);
        onSelect = Input.GetButtonDown("Submit");

        if (onUp && !onDown)
        {
            ChangeMenu(DirectionType.Up);
        }
        else if (onDown && !onUp)
        {
            ChangeMenu(DirectionType.Down);
        }

        if (onSelect)
        {
            Select(menuType);
        }
    }

    void ChangeMenu(DirectionType directionType)
    {
        if (directionType == DirectionType.Up)
        {
            switch (menuType)
            {
                case MenuType.NewGame:
                    menuType = MenuType.Exit;
                    break;
                case MenuType.Continue:
                    menuType = MenuType.NewGame;
                    break;
                case MenuType.Option:
                    menuType = MenuType.Continue;
                    break;
                case MenuType.Exit:
                    menuType = MenuType.Option;
                    break;
                default:
                    break;
            }
        }
        else if (directionType == DirectionType.Down)
        {
            switch (menuType)
            {
                case MenuType.NewGame:
                    menuType = MenuType.Continue;
                    break;
                case MenuType.Continue:
                    menuType = MenuType.Option;
                    break;
                case MenuType.Option:
                    menuType = MenuType.Exit;
                    break;
                case MenuType.Exit:
                    menuType = MenuType.NewGame;
                    break;
                default:
                    break;
            }
        }

        StopAllCoroutines();
        StartCoroutine(theBar.MoveBar(menuPosition[menuType]));
        ChangeFontSize(menuText[menuType]);
        ChangeFontStyle(menuText[menuType]);

        Debug.Log("now selected menu type is: " + menuType);
    }

    void ChangeFontSize(Text selectedMenu)
    {
        foreach (var menu in menus)
            menu.fontSize = normalFontSize;

        selectedMenu.fontSize = bigFontSize;
    }

    void ChangeFontStyle(Text selectedMenu)
    {
        foreach (var menu in menus)
            menu.fontStyle = FontStyle.Normal;

        selectedMenu.fontStyle = FontStyle.Bold;
    }

    void Select(MenuType type)
    {
        switch (type)
        {
            case MenuType.NewGame:
                NewGame();
                break;
            case MenuType.Continue:
                Continue();
                break;
            case MenuType.Option:
                Option();
                break;
            case MenuType.Exit:
                Exit();
                break;
            default:
                Debug.LogWarning("Selected menu is incorrect");
                break;
        }
    }

    void NewGame()
    {
        guide.PopupGuide(true, 1, MenuType.NewGame);
    }

    void Continue()
    {
        Debug.Log("selected \"Continue\"");
        // TODO: Load saved Scene
    }

    void Option()
    {
        Debug.Log("selected \"Option\"");
        // TODO: Popup Option
    }

    void Exit()
    {
        guide.PopupGuide(true, 1, MenuType.Exit);
    }
}
