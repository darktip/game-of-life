using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDebug : MonoBehaviour, IPointerClickHandler
{
    private RectTransform _rect;
    public CustomRenderTexture texture;

    private Texture _oldTexture;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();

        CopyTexture();
    }

    private void OnDisable()
    {
        texture.initializationTexture = _oldTexture;
    }

    private void CopyTexture()
    {
        var initTexture = texture.initializationTexture as Texture2D;
        _oldTexture = texture.initializationTexture;

        if (initTexture != null)
        {
            Texture2D newTexture = new Texture2D(initTexture.width, initTexture.height, TextureFormat.RGBA32, false);
            newTexture.SetPixels(initTexture.GetPixels());
            newTexture.Apply();

            texture.initializationTexture = newTexture;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, eventData.position,
            eventData.pressEventCamera, out Vector2 localPoint))
        {
            Debug.Log("Point: " + TransformToUVPoint(localPoint));

            Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
            RenderTexture.active = texture;
            newTexture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            RenderTexture.active = null;

            Vector2 pixel = TransformToUVPoint(localPoint);
            pixel.x *= texture.width;
            pixel.y *= texture.height;

            newTexture.SetPixel(Mathf.FloorToInt(pixel.x), Mathf.FloorToInt(pixel.y), Color.white);
            newTexture.Apply();

            texture.initializationTexture = newTexture;
            texture.Initialize();
        }
    }

    public Vector2 TransformToUVPoint(Vector2 localPoint)
    {
        float width = _rect.sizeDelta.x;
        float height = _rect.sizeDelta.y;

        return Vector2.Scale(localPoint, new Vector2(1 / width, 1 / height));
    }
}