using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBox : MonoBehaviour
{
    [SerializeField] private GameObject scrollBoxContent = null;
    [SerializeField] private float offset = 0f;

    public void ClearScrollBox()
    {
        if (scrollBoxContent.transform.childCount > 0)
        {
            do
            {
                Destroy(scrollBoxContent.transform.GetChild(0).gameObject);
            } while (scrollBoxContent.transform.childCount > 0);
        }
    }

    public void AddToScrollBox(GameObject objectToAdd)
    {
        Debug.Log($"parent was {objectToAdd.transform.parent.name}");

        objectToAdd.transform.parent = scrollBoxContent.transform;
        int numChildren = scrollBoxContent.transform.childCount;

        RectTransform rt = objectToAdd.GetComponent<RectTransform>();
        rt.SetPositionAndRotation(new Vector3(rt.rect.x, rt.rect.y + (offset * numChildren), 0f), Quaternion.identity);

        Debug.Log($"parent now is {objectToAdd.transform.parent.name}");
    }
}
