using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] GameObject preFloorGroup;
    [SerializeField] GameObject[] backgroundLayerGroup;
    [SerializeField] GameObject screenParticlePrefab;

    [Header("Values")]
    [SerializeField] Vector3 deadline = new Vector3(-60f, 0f, 0f);
    [SerializeField] Vector3 reposition = new Vector3(188f, 0f, 0f);
    [SerializeField] float[] scrollSpeed_background;
    [SerializeField] float[] scrollSpeed_background_maxDash;
    [SerializeField] float[] scrollSpeed_floor;

    List<GameObject> preFloors;
    List<Transform> backgroundLayer_01;
    List<Transform> backgroundLayer_02;
    ParticleSystem screenParticle;

    DashLevel appliedDashLevel;

    public bool canScroll;
    public bool createdLastFloor;
    public bool onEnd;

    void Awake()
    {
        Initialize();
        SubscribeEvent();
    }

    #region Initial Setting
    void Initialize()
    {
        GameObject particle = Instantiate(screenParticlePrefab, transform);
        particle.transform.position = new Vector3(11f, 0.9f, 0f);
        particle.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
        screenParticle = particle.GetComponent<ParticleSystem>();

        preFloors = new List<GameObject>();
        backgroundLayer_01 = new List<Transform>();
        backgroundLayer_02 = new List<Transform>();

        for (int i = 0; i < preFloorGroup.transform.childCount; i++)
        {
            preFloors.Add(preFloorGroup.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < backgroundLayerGroup[0].transform.childCount; i++)
        {
            backgroundLayer_01.Add(backgroundLayerGroup[0].transform.GetChild(i).transform);
        }

        for (int i = 0; i < backgroundLayerGroup[1].transform.childCount; i++)
        {
            backgroundLayer_02.Add(backgroundLayerGroup[1].transform.GetChild(i).transform);
        }
    }

    void SubscribeEvent()
    {
        TutorialEvent.tutorialEvent -= SetCanScroll;
        TutorialEvent.tutorialEvent += SetCanScroll;

        PlayerController.deadEvent -= SetCanScroll;
        PlayerController.deadEvent += SetCanScroll;

        PlayerController.dashLevelEvent -= ApplyDashLevel;
        PlayerController.dashLevelEvent += ApplyDashLevel;
    }
    #endregion

    void Start()
    {
        ScrollParticle(DashLevel.None);
    }

    void SetCanScroll(bool state)
    {
        canScroll = state;
    }

    public void ScrollParticle(DashLevel dashLevel)
    {
        var temp = screenParticle.velocityOverLifetime;

        temp.xMultiplier = -1 * (1.5f * (float)dashLevel);
    }

    void Update()
    {
        CheckStop();

        if (canScroll)
        {
            ScrollBackground();
            ScrollFloor();
            Reposition();
        }
    }

    void CheckStop()
    {
        if (FloorManager.instance.gaugeIsFull && PlayerController.instance.reachLastFloor)
        {
            if (!onEnd)
            {
                onEnd = true;
                canScroll = false;
            }
        }

    }

    void ApplyDashLevel(DashLevel dashLevel)
    {
        appliedDashLevel = dashLevel;
    }

    #region Scrolling
    void ScrollBackground()
    {   
        if (appliedDashLevel != DashLevel.Max)
        {
            foreach (var layer in backgroundLayer_01)
                layer.Translate(Vector2.left * scrollSpeed_background[0] * Time.deltaTime);

            foreach (var layer in backgroundLayer_02)
                layer.Translate(Vector2.left * scrollSpeed_background[1] * Time.deltaTime);
        }
        else
        {
            foreach (var layer in backgroundLayer_01)
                layer.Translate(Vector2.left * scrollSpeed_background_maxDash[0] * Time.deltaTime);

            foreach (var layer in backgroundLayer_02)
                layer.Translate(Vector2.left * scrollSpeed_background_maxDash[1] * Time.deltaTime);
        }
    }

    void ScrollFloor()
    {
        foreach (var floor in preFloors)
            floor.transform.Translate(Vector2.left * scrollSpeed_floor[(int)appliedDashLevel] * Time.deltaTime);
    }

    void Reposition()
    {
        if (FloorManager.instance.gaugeIsFull) // Last Floor
        {
            if (createdLastFloor)
                return;

            for (int i = 0; i < preFloors.Count; i++)
            {
                if (preFloors[i].transform.position.x <= deadline.x)
                {
                    createdLastFloor = true;

                    preFloors.Add(FloorManager.instance.floorGenerator.CreateFloor(preFloors[i].transform.localPosition, true));
                    FloorManager.instance.floorGenerator.DestroyFloor(preFloors[i]);
                    preFloors.RemoveAt(i);
                }
            }
        }
        else // Default Floor
        {
            for (int i = 0; i < preFloors.Count; i++)
            {
                if (preFloors[i].transform.position.x <= deadline.x)
                {
                    preFloors.Add(FloorManager.instance.floorGenerator.CreateFloor(preFloors[i].transform.localPosition));
                    FloorManager.instance.floorGenerator.DestroyFloor(preFloors[i]);
                    preFloors.RemoveAt(i);
                }
            }
        }

        // Background
        {
            foreach (var layer in backgroundLayer_01)
                if (layer.transform.position.x <= deadline.x)
                    layer.transform.position += reposition;

            foreach (var layer in backgroundLayer_02)
                if (layer.transform.position.x <= deadline.x)
                    layer.transform.position += reposition;
        }
    }
    #endregion
}
