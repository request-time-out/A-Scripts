// Decompiled with JetBrains decompiler
// Type: SuperScrollView.DragChangSizeScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SuperScrollView
{
  public class DragChangSizeScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
  {
    private bool mIsDraging;
    public Camera mCamera;
    public float mBorderSize;
    public Texture2D mCursorTexture;
    public Vector2 mCursorHotSpot;
    private RectTransform mCachedRectTransform;
    public Action mOnDragEndAction;

    public DragChangSizeScript()
    {
      base.\u002Ector();
    }

    public RectTransform CachedRectTransform
    {
      get
      {
        if (Object.op_Equality((Object) this.mCachedRectTransform, (Object) null))
          this.mCachedRectTransform = (RectTransform) ((Component) this).get_gameObject().GetComponent<RectTransform>();
        return this.mCachedRectTransform;
      }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      this.SetCursor(this.mCursorTexture, this.mCursorHotSpot, (CursorMode) 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      this.SetCursor((Texture2D) null, this.mCursorHotSpot, (CursorMode) 0);
    }

    private void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
    {
      if (!Input.get_mousePresent())
        return;
      Cursor.SetCursor(texture, hotspot, cursorMode);
    }

    private void LateUpdate()
    {
      if (Object.op_Equality((Object) this.mCursorTexture, (Object) null))
        return;
      if (this.mIsDraging)
      {
        this.SetCursor(this.mCursorTexture, this.mCursorHotSpot, (CursorMode) 0);
      }
      else
      {
        Vector2 vector2;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.CachedRectTransform, Vector2.op_Implicit(Input.get_mousePosition()), this.mCamera, ref vector2))
        {
          this.SetCursor((Texture2D) null, this.mCursorHotSpot, (CursorMode) 0);
        }
        else
        {
          Rect rect = this.CachedRectTransform.get_rect();
          float num = ((Rect) ref rect).get_width() - (float) vector2.x;
          if ((double) num < 0.0)
            this.SetCursor((Texture2D) null, this.mCursorHotSpot, (CursorMode) 0);
          else if ((double) num <= (double) this.mBorderSize)
            this.SetCursor(this.mCursorTexture, this.mCursorHotSpot, (CursorMode) 0);
          else
            this.SetCursor((Texture2D) null, this.mCursorHotSpot, (CursorMode) 0);
        }
      }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      this.mIsDraging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      this.mIsDraging = false;
      if (this.mOnDragEndAction == null)
        return;
      this.mOnDragEndAction();
    }

    public void OnDrag(PointerEventData eventData)
    {
      Vector2 vector2;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this.CachedRectTransform, eventData.get_position(), this.mCamera, ref vector2);
      if (vector2.x <= 0.0)
        return;
      this.CachedRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, (float) vector2.x);
    }
  }
}
