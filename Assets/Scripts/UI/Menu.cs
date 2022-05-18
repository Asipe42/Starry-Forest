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

public enum SceneType
{
    Title = 0,
    InGame
}

public class Menu : MonoBehaviour
{
    enum DirectionType
    {
        Up,
        Down
    }

    [SerializeField] TitleGuide guide;
    [SerializeField] Setting setting;

    [Space]
    [SerializeField] Bar theBar;
    [SerializeField] Text[] menus;
    [SerializeField] int normalFontSize = 45;
    [SerializeField] int bigFontSize = 50;

    [SerializeField] AudioClip menuClip;

    Dictionary<MenuType, Vector3> menuPosition;
    Dictionary<MenuType, Text> menuText;
    List<Vector3> destination;

    MenuType menuType = MenuType.NewGame;

    public bool onEnable;
    public bool onLock;

    bool onUp;
    bool onDown;
    bool onSelect;

    void Awake()
    {
        menuPosition = new Dictionary<MenuType, Vector3>();
        menuText = new Dictionary<MenuType, Text>();
        destination = new List<Vector3>();

        FadeScreen.FadeEvent -= SetEnable;
        FadeScreen.FadeEvent += SetEnable;

        setting.onSettingEvent -= SetLock;
        setting.onSettingEvent += SetLock;

        menuClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Menu");
    }

    void Start()
    {
        SetDestination();
    }

    void SetLock(bool state)
    {
        onLock = state;
    }

    void SetEnable(bool state)
    {
        onEnable = state;
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

        if (onLock)
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
        SFXController.instance.PlaySFX(menuClip);

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

    public void ChangeMenu(string menuName)
    {
        if (!onEnable)
            return;

        if (onLock)
            return;

        MenuType temp = menuType;


        switch (menuName)
        {
            case "newGame":
                menuType = MenuType.NewGame;
                break;
            case "continue":
                menuType = MenuType.Continue;
                break;
            case "option":
                menuType = MenuType.Option;
                break;
            case "exit":
                menuType = MenuType.Exit;
                break;
        }

        if (temp == menuType)
            return;

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
                Setting();
                break;
            case MenuType.Exit:
                Exit();
                break;
            default:
                Debug.LogWarning("Selected menu is incorrect");
                break;
        }
    }

    public void Select()
    {
        switch (menuType)
        {
            case MenuType.NewGame:
                NewGame();
                break;
            case MenuType.Continue:
                Continue();
                break;
            case MenuType.Option:
                Setting();
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
        onLock = true;
        guide.PopupGuide(true, 1, MenuType.NewGame);
    }

    void Continue()
    {
        Debug.Log("selected \"Continue\"");

        // TODO: Load saved Scene
    }

    void Setting()
    {
        onLock = true;
        setting.SetActivation(true);
    }

    void Exit()
    {
        onLock = true;
        guide.PopupGuide(true, 1, MenuType.Exit);
    }
}
