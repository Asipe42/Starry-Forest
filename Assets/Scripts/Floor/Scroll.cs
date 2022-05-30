using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] List<GameObject> preFloors;
    [SerializeField] Transform[] backgroundLayer_01;
    [SerializeField] Transform[] backgroundLayer_02;
    [SerializeField] ParticleSystem particle;

    [Space]
    [SerializeField] Vector3 deadline = new Vector3(-60f, 0f, 0f);
    [SerializeField] Vector3 reposition = new Vector3(188f, 0f, 0f);
    [SerializeField] float[] scrollSpeed_background;
    [SerializeField] float[] scrollSpeed_floor;
    [SerializeField] FloorGenerator theFloorGenerator;

    public bool canScroll;
    public bool createdLastFloor;
    public bool onEnd;

    PlayerController playerController;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        TutorialEvent.OnTutorialEvent -= SetCanScroll;
        TutorialEvent.OnTutorialEvent += SetCanScroll;

        PlayerController.deadEvent -= SetCanScroll;
        PlayerController.deadEvent += SetCanScroll;

        PlayerController.DashEvent -= ScrollParticle;
        PlayerController.DashEvent += ScrollParticle;
    }

    void Start()
    {
        ScrollParticle(DashLevel.None);
    }

    void Update()
    {
        if (FloorManager.instance.gaugeIsFull && PlayerController.instance.reachLastFloor)
        {
            if (!onEnd)
            {
                onEnd = true;
                canScroll = false;
            }
        }

        if (canScroll)
        {
            ScrollBackground();
            ScrollFloor();
            Reposition();
        }
    }

    void SetCanScroll(bool state)
    {
        canScroll = state; 
    }

    void ScrollParticle(DashLevel dashLevel)
    {
        var temp = particle.velocityOverLifetime;

        temp.xMultiplier = -1 * (1.5f * (float)dashLevel);
    }

    void ScrollBackground()
    {       
        foreach (var layer in backgroundLayer_01)
            layer.Translate(Vector2.left * scrollSpeed_background[0] * Time.deltaTime);

        foreach (var layer in backgroundLayer_02)
            layer.Translate(Vector2.left * scrollSpeed_background[1] * Time.deltaTime);
    }

    void ScrollFloor()
    {
        foreach (var floor in preFloors)
            floor.transform.Translate(Vector2.left * scrollSpeed_floor[(int)PlayerController.instance.dashLevel] * Time.deltaTime);
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

                    preFloors.Add(theFloorGenerator.CreateFloor(preFloors[i].transform.localPosition, true));
                    theFloorGenerator.DestroyFloor(preFloors[i]);
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
                    preFloors.Add(theFloorGenerator.CreateFloor(preFloors[i].transform.localPosition));
                    theFloorGenerator.DestroyFloor(preFloors[i]);
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
}
