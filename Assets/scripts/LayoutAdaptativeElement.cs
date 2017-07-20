using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutAdaptativeElement : MonoBehaviour {

    private RectTransform parent;
    private float accumulatedWidth = 0f;

    void Start() {
        parent = transform.parent.GetComponent<RectTransform>();
        foreach (Transform t in transform.parent) {
            if (!t.name.Equals(transform.name)) {
                accumulatedWidth += t.GetComponent<RectTransform>().rect.width;
            }
        }
        if (accumulatedWidth < parent.rect.width) {
            RectTransform r = transform.GetComponent<RectTransform>();
            r.sizeDelta = new Vector2(parent.rect.width - accumulatedWidth, r.sizeDelta.y);
            //Debug.Log(r.sizeDelta);
        }
    }


}
