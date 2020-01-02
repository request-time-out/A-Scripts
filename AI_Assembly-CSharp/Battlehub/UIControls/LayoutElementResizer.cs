// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.LayoutElementResizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Battlehub.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  public class LayoutElementResizer : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    public LayoutElement Target;
    public RectTransform Parent;
    public LayoutElement SecondaryTarget;
    public Texture2D CursorTexture;
    public float XSign;
    public float YSign;
    public float MaxSize;
    public bool HasMaxSize;
    private bool m_pointerInside;
    private bool m_pointerDown;
    private float m_midX;
    private float m_midY;
    private CursorHelper m_cursorHelper;

    public LayoutElementResizer()
    {
      base.\u002Ector();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
      if (!Object.op_Inequality((Object) this.Parent, (Object) null) || !Object.op_Inequality((Object) this.SecondaryTarget, (Object) null))
        return;
      if ((double) this.XSign != 0.0)
      {
        this.Target.set_flexibleWidth(Mathf.Clamp01(this.Target.get_flexibleWidth()));
        this.SecondaryTarget.set_flexibleWidth(Mathf.Clamp01(this.SecondaryTarget.get_flexibleWidth()));
      }
      if ((double) this.YSign != 0.0)
      {
        this.Target.set_flexibleHeight(Mathf.Clamp01(this.Target.get_flexibleHeight()));
        this.SecondaryTarget.set_flexibleHeight(Mathf.Clamp01(this.SecondaryTarget.get_flexibleHeight()));
      }
      this.m_midY = this.Target.get_flexibleHeight() / (this.Target.get_flexibleHeight() + this.SecondaryTarget.get_flexibleHeight());
      LayoutElementResizer layoutElementResizer1 = this;
      double midY = (double) layoutElementResizer1.m_midY;
      Rect rect1 = this.Parent.get_rect();
      double num1 = (double) Math.Max(((Rect) ref rect1).get_height() - this.Target.get_minHeight() - this.SecondaryTarget.get_minHeight(), 0.0f);
      layoutElementResizer1.m_midY = (float) (midY * num1);
      this.m_midX = this.Target.get_flexibleWidth() / (this.Target.get_flexibleWidth() + this.SecondaryTarget.get_flexibleWidth());
      LayoutElementResizer layoutElementResizer2 = this;
      double midX = (double) layoutElementResizer2.m_midX;
      Rect rect2 = this.Parent.get_rect();
      double num2 = (double) Math.Max(((Rect) ref rect2).get_width() - this.Target.get_minWidth() - this.SecondaryTarget.get_minWidth(), 0.0f);
      layoutElementResizer2.m_midX = (float) (midX * num2);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
      if (Object.op_Inequality((Object) this.Parent, (Object) null) && Object.op_Inequality((Object) this.SecondaryTarget, (Object) null))
      {
        if ((double) this.XSign != 0.0)
        {
          float num1 = this.m_midX + (float) eventData.get_delta().x * (float) Math.Sign(this.XSign);
          double num2 = (double) num1;
          Rect rect = this.Parent.get_rect();
          double num3 = (double) ((Rect) ref rect).get_width() - (double) this.Target.get_minWidth() - (double) this.SecondaryTarget.get_minWidth();
          float num4 = (float) (num2 / num3);
          this.Target.set_flexibleWidth(num4);
          this.SecondaryTarget.set_flexibleWidth(1f - num4);
          this.m_midX = num1;
        }
        if ((double) this.YSign != 0.0)
        {
          float num1 = this.m_midY + (float) eventData.get_delta().y * (float) Math.Sign(this.YSign);
          double num2 = (double) num1;
          Rect rect = this.Parent.get_rect();
          double num3 = (double) ((Rect) ref rect).get_height() - (double) this.Target.get_minHeight() - (double) this.SecondaryTarget.get_minHeight();
          float num4 = (float) (num2 / num3);
          this.Target.set_flexibleHeight(num4);
          this.SecondaryTarget.set_flexibleHeight(1f - num4);
          this.m_midY = num1;
        }
        if ((double) this.XSign != 0.0)
        {
          this.Target.set_flexibleWidth(Mathf.Clamp01(this.Target.get_flexibleWidth()));
          this.SecondaryTarget.set_flexibleWidth(Mathf.Clamp01(this.SecondaryTarget.get_flexibleWidth()));
        }
        if ((double) this.YSign == 0.0)
          return;
        this.Target.set_flexibleHeight(Mathf.Clamp01(this.Target.get_flexibleHeight()));
        this.SecondaryTarget.set_flexibleHeight(Mathf.Clamp01(this.SecondaryTarget.get_flexibleHeight()));
      }
      else
      {
        if ((double) this.XSign != 0.0)
        {
          LayoutElement target = this.Target;
          target.set_preferredWidth(target.get_preferredWidth() + (float) eventData.get_delta().x * (float) Math.Sign(this.XSign));
          if (this.HasMaxSize && (double) this.Target.get_preferredWidth() > (double) this.MaxSize)
            this.Target.set_preferredWidth(this.MaxSize);
        }
        if ((double) this.YSign == 0.0)
          return;
        LayoutElement target1 = this.Target;
        target1.set_preferredHeight(target1.get_preferredHeight() + (float) eventData.get_delta().y * (float) Math.Sign(this.YSign));
        if (!this.HasMaxSize || (double) this.Target.get_preferredHeight() <= (double) this.MaxSize)
          return;
        this.Target.set_preferredHeight(this.MaxSize);
      }
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
      this.m_pointerInside = true;
      this.m_cursorHelper.SetCursor((object) this, this.CursorTexture, new Vector2(0.5f, 0.5f), (CursorMode) 0);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
      this.m_pointerInside = false;
      if (this.m_pointerDown)
        return;
      this.m_cursorHelper.ResetCursor((object) this);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
      this.m_pointerDown = true;
      if ((double) this.Target.get_preferredWidth() < -1.0)
        this.Target.set_preferredWidth(this.Target.get_minWidth());
      if ((double) this.Target.get_preferredHeight() >= -1.0)
        return;
      this.Target.set_preferredHeight(this.Target.get_minHeight());
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
      this.m_pointerDown = false;
      if (this.m_pointerInside)
        return;
      this.m_cursorHelper.ResetCursor((object) this);
    }
  }
}
