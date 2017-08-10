using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class EnhancedVerticalLayoutGroup : MonoBehaviour
{
    [Header("Padding")]
    public float left;
    public float right, top, bottom;
    [Space]
    public float spacing;
    [Space]
    //[Header("Force expand")]
    //public bool width;
    //public bool height;
    //[Space]

    public bool ignoreLockingIfContainerFull;
    public EnhancedLayoutItem[] items;

    private Vector2 size, actualSize, nextIndexPosition;
    private RectTransform recTransform;
    private int childCount, fixedSizeChildCount, flexibleSizeChildCount;
    private float fixedSpace, flexibleSpace;

    private void OnEnable()
    {
        //EditorApplication.hierarchyWindowChanged += HierarchyChanged;
        this.childCount = 0;
        Init();
        Refresh();
    }

    private void HierarchyChanged()
    {
        Init();
    }

    public void Init()
    {
        if (childCount == this.transform.childCount) return;
        this.childCount = this.transform.childCount;
        items = new EnhancedLayoutItem[this.childCount];

        for (int i = 0; i < items.Length; i++)
        {
            Transform child = this.transform.GetChild(i);
            Image image = child.GetComponent<Image>();
            RectTransform rect = child.GetComponent<RectTransform>();
            if (image != null && image.sprite != null)
            {
                rect.pivot = new Vector2(0f, 1f);
                rect.anchorMax = new Vector2(0f, 1f);
                rect.anchorMin = new Vector2(0f, 1f);
                items[i].key = image.sprite.name;
                items[i].aspectRatioFilter = new AspectRadioProperty(
                    (float)image.sprite.texture.width / (float)image.sprite.texture.height,
                    AspectRatioMode.WidthControlsHeight);
                //Debug.Log("width(" + image.sprite.texture.width + ") / height(" + image.sprite.texture.height + ") = " + ((float)image.sprite.texture.width / (float)image.sprite.texture.height));
            }
        }
    }

    private void OnValidate()
    {
        if (!GUI.changed) return;
        Refresh();
    }

    private void Refresh()
    {
        this.recTransform = this.GetComponent<RectTransform>();
        this.size = recTransform.rect.size;
        this.fixedSpace = this.flexibleSpace = this.flexibleSizeChildCount = this.fixedSizeChildCount = 0;
        this.actualSize = new Vector2(size.x - this.left - this.right, size.y - this.top - this.bottom - this.spacing * (this.childCount - 1));
        for (int i = 0; i < items.Length; i++)
        {
            bool check1 = false, check2 = false;
            if (items[i].aspectRatioFilter.aspectRatioLocking &&
                items[i].aspectRatioFilter.aspectRatioMode == AspectRatioMode.WidthControlsHeight)
            {
                this.fixedSpace += items[i].aspectRatioFilter.aspectRatioLocking ?
                    GetComplementaryDimension(this.actualSize.x,
                                            items[i].aspectRatioFilter.aspectRatio,
                                            AspectRatioMode.WidthControlsHeight) : 0f;
                check1 = true;
            }
            if (items[i].dimensions._fixedHeight)
            {
                this.fixedSpace += items[i].dimensions.fixedHeight;
                check2 = true;
            }
            if (check1 || check2)
            {
                this.fixedSizeChildCount++;
            }
        }

        bool ignoreFixedSizes = this.ignoreLockingIfContainerFull && this.fixedSpace > this.actualSize.y;
        if (ignoreFixedSizes)
        {
            this.flexibleSpace = actualSize.y;
            this.fixedSpace = this.fixedSizeChildCount = 0;
            this.flexibleSizeChildCount = this.childCount;
        }
        else
        {
            this.flexibleSpace = actualSize.y - this.fixedSpace;
            this.flexibleSizeChildCount = this.childCount - this.fixedSizeChildCount;
        }
        this.nextIndexPosition = new Vector2(this.left, -this.top);

        for (int i = 0; i < items.Length; i++)
        {
            Transform child = this.transform.GetChild(i);
            RectTransform rect = child.GetComponent<RectTransform>();
            rect.anchoredPosition = this.nextIndexPosition;

            float width = 0, height = 0;

            if (ignoreFixedSizes)
            {
                width = this.actualSize.x;
                height = this.flexibleSpace / this.flexibleSizeChildCount;
            }
            else
            {
                if (items[i].dimensions._fixedHeight)
                {
                    height = items[i].dimensions.fixedHeight;
                }
                else
                {
                    if (!(items[i].aspectRatioFilter.aspectRatioLocking &&
                        items[i].aspectRatioFilter.aspectRatioMode == AspectRatioMode.WidthControlsHeight))
                    {
                        height = this.flexibleSpace / this.flexibleSizeChildCount;
                        //Debug.Log("height=" + height + "(" + this.flexibleSpace + "/" + this.flexibleSizeChildCount + ")");
                    }
                }
                if (items[i].dimensions._fixedWidth)
                {
                    width = items[i].dimensions.fixedWidth;
                }
                else
                {
                    if (!(items[i].aspectRatioFilter.aspectRatioLocking &&
                        items[i].aspectRatioFilter.aspectRatioMode == AspectRatioMode.HeightControlsWidth))
                        width = this.actualSize.x;
                }
                if (items[i].aspectRatioFilter.aspectRatioLocking)
                {
                    if (items[i].aspectRatioFilter.aspectRatioMode == AspectRatioMode.HeightControlsWidth)
                    {
                        if (items[i].dimensions._fixedWidth) Debug.LogError("Illegal 'AspectRadioMode-FixedDimension' combination.");
                        if (items[i].dimensions._fixedHeight) width = height * items[i].aspectRatioFilter.aspectRatio;
                        else width = GetComplementaryDimension(height, items[i].aspectRatioFilter.aspectRatio, AspectRatioMode.HeightControlsWidth);
                    }
                    if (items[i].aspectRatioFilter.aspectRatioMode == AspectRatioMode.WidthControlsHeight)
                    {
                        if (items[i].dimensions._fixedHeight) Debug.LogError("Illegal 'AspectRadioMode-FixedDimension' combination.");
                        if (items[i].dimensions._fixedWidth) height = width / items[i].aspectRatioFilter.aspectRatio;
                        else height = GetComplementaryDimension(width, items[i].aspectRatioFilter.aspectRatio, AspectRatioMode.WidthControlsHeight);
                    }
                }
            }
            rect.sizeDelta = new Vector2(width, height);
            //Debug.Log(child.name + " = " + recTransform.rect);
            this.nextIndexPosition -= new Vector2(0f, height + this.spacing);
        }
        //Debug.Log("this.actualSize=" + this.actualSize + ", this.flexibleSpace=" + this.flexibleSpace + ", this.fixedSpace=" + this.fixedSpace);
    }

    private int GetComplementaryDimension(float _widthOrHeight, float _ratio, AspectRatioMode _mode)
    {
        int value;
        switch (_mode)
        {
            case AspectRatioMode.WidthControlsHeight:
                value = Mathf.RoundToInt(_widthOrHeight / _ratio);
                break;
            case AspectRatioMode.HeightControlsWidth:
                value = Mathf.RoundToInt(_widthOrHeight * _ratio);
                break;
            default:
                value = -1;
                Debug.LogError("GetComplementaryDimension: Wrong 'AspectRatioMode'.");
                break;
        }
        return value;
    }

}

[Serializable]
public struct AspectRadioProperty
{
    public AspectRadioProperty(float _aspectRatio, AspectRatioMode _mode)
    {
        this.aspectRatioLocking = true;
        this.aspectRatio = _aspectRatio;
        this.aspectRatioMode = _mode;
    }

    public bool aspectRatioLocking;
    public float aspectRatio;
    public AspectRatioMode aspectRatioMode;
}

[Serializable]
public struct FixedDimensionsProperty
{
    public bool _fixedWidth;
    public float fixedWidth;
    public bool _fixedHeight;
    public float fixedHeight;
}

[Serializable]
public struct EnhancedLayoutItem
{
    [HideInInspector] public string key;
    public FixedDimensionsProperty dimensions;
    public AspectRadioProperty aspectRatioFilter;
}

public enum AspectRatioMode { WidthControlsHeight, HeightControlsWidth }
