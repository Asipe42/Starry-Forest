using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] List<GameObject> preFloors;
    [SerializeField] Transform[] backgroundLayer_01;
    [SerializeField] Transform[] backgroundLayer_02;
    [SerializeField] ParticleSystem particle;


    [SerializeField] Vector3 deadline = new Vector3(-40f, 0f, 0f);
    [SerializeField] Vector3 reposition = new Vector3(115.2f, 0f, 0f);
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

        PlayerController.DashAction -= ScrollParticle;
        PlayerController.DashAction += ScrollParticle;
    }

    void Start()
    {
        particle = GameObject.FindGameObjectWithTag("FieldParticle").GetComponent<ParticleSystem>();

        ScrollParticle(DashLevel.None);
    }

    void Update()
    {
        if (ProgressBar.onLast && PlayerController.instance.ReachLastFloor)
        {
            if (!onEnd)
            {
                onEnd = true;
                canScroll = false;

                playerController.StopAction();
                UIManager.instance.ShowResult(true);
            }
        }

        if (canScroll)
        {
            ScrollBackground();
            ScrollFloor();
            Reposition();
        }
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
        if (ProgressBar.onLast)
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
        else
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

        #region Background Reposition
        foreach (var layer in backgroundLayer_01)
            if (layer.transform.position.x <= deadline.x)
                layer.transform.position += reposition;

        foreach (var layer in backgroundLayer_02)
            if (layer.transform.position.x <= deadline.x)
                layer.transform.position += reposition;
        #endregion
    }
}
