using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] List<GameObject> preFloors;
    [SerializeField] Transform[] backgroundLayer_01;
    [SerializeField] Transform[] backgroundLayer_02;

    [SerializeField] Vector3 deadline = new Vector3(-40f, 0f, 0f);
    [SerializeField] Vector3 reposition = new Vector3(115.2f, 0f, 0f);
    [SerializeField] float[] scrollSpeed;
    [SerializeField] FloorGenerator theFloorGenerator;

    void Update()
    {
        Scrolling();
        Reposition();
    }

    void Scrolling()
    {
        foreach (var floor in preFloors)
            floor.transform.Translate(Vector2.left * scrollSpeed[2] * Time.deltaTime);

        foreach(var layer in backgroundLayer_01)
            layer.Translate(Vector2.left * scrollSpeed[0] * Time.deltaTime);

        foreach (var layer in backgroundLayer_02)
            layer.Translate(Vector2.left * scrollSpeed[1] * Time.deltaTime);
    }

    void Reposition()
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

        foreach (var layer in backgroundLayer_01)
            if (layer.transform.position.x <= deadline.x)
                layer.transform.position += reposition;

        foreach (var layer in backgroundLayer_02)
            if (layer.transform.position.x <= deadline.x)
                layer.transform.position += reposition;
    }
}
