// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomUIDrag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace CharaCustom
{
  public class CustomUIDrag : CustomCanvasSort
  {
    [SerializeField]
    private CustomUIDrag.CanvasType type = CustomUIDrag.CanvasType.ColorWin;
    [SerializeField]
    private RectTransform rtMove;
    [SerializeField]
    private RectTransform rtRect;
    [SerializeField]
    private RectTransform rtRange;
    private CameraControl_Ver2 camCtrl;
    private bool isDrag;

    private CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    private void Start()
    {
      if (Object.op_Equality((Object) this.camCtrl, (Object) null) && Object.op_Implicit((Object) Camera.get_main()))
        this.camCtrl = (CameraControl_Ver2) ((Component) Camera.get_main()).GetComponent<CameraControl_Ver2>();
      this.UpdatePosition();
    }

    public void UpdatePosition()
    {
      switch (this.type)
      {
        case CustomUIDrag.CanvasType.SubWin:
          this.rtMove.set_anchoredPosition(this.customBase.customSettingSave.winSubLayout);
          break;
        case CustomUIDrag.CanvasType.DrawWin:
          this.rtMove.set_anchoredPosition(this.customBase.customSettingSave.winDrawLayout);
          break;
        case CustomUIDrag.CanvasType.PatternWin:
          this.rtMove.set_anchoredPosition(this.customBase.customSettingSave.winPatternLayout);
          break;
        case CustomUIDrag.CanvasType.ColorWin:
          this.rtMove.set_anchoredPosition(this.customBase.customSettingSave.winColorLayout);
          break;
      }
    }

    private void CalcDragPos(PointerEventData ped)
    {
      Vector2 vector2;
      ((Vector2) ref vector2).\u002Ector((float) (this.rtMove.get_anchoredPosition().x + ped.get_delta().x), (float) (this.rtMove.get_anchoredPosition().y + ped.get_delta().y));
      vector2.x = (__Null) (double) Mathf.Clamp((float) vector2.x, 0.0f, (float) (this.rtRange.get_sizeDelta().x - this.rtRect.get_sizeDelta().x));
      vector2.y = (__Null) -(double) Mathf.Clamp((float) -vector2.y, 0.0f, (float) (this.rtRange.get_sizeDelta().y - this.rtRect.get_sizeDelta().y));
      this.rtMove.set_anchoredPosition(vector2);
      switch (this.type)
      {
        case CustomUIDrag.CanvasType.SubWin:
          this.customBase.customSettingSave.winSubLayout = vector2;
          break;
        case CustomUIDrag.CanvasType.DrawWin:
          this.customBase.customSettingSave.winDrawLayout = vector2;
          break;
        case CustomUIDrag.CanvasType.PatternWin:
          this.customBase.customSettingSave.winPatternLayout = vector2;
          break;
        case CustomUIDrag.CanvasType.ColorWin:
          this.customBase.customSettingSave.winColorLayout = vector2;
          break;
      }
    }

    public override void OnPointerDown(PointerEventData ped)
    {
      base.OnPointerDown(ped);
      if (Input.GetMouseButton(0))
        ;
    }

    public override void OnBeginDrag(PointerEventData ped)
    {
      base.OnBeginDrag(ped);
      if (!Input.GetMouseButton(0))
        return;
      this.isDrag = true;
      if (Object.op_Implicit((Object) this.camCtrl))
        this.camCtrl.NoCtrlCondition = (BaseCameraControl_Ver2.NoCtrlFunc) (() => true);
      this.CalcDragPos(ped);
    }

    public override void OnDrag(PointerEventData ped)
    {
      base.OnDrag(ped);
      if (!this.isDrag)
        return;
      this.CalcDragPos(ped);
    }

    public override void OnEndDrag(PointerEventData ped)
    {
      base.OnEndDrag(ped);
      if (!this.isDrag)
        return;
      this.isDrag = false;
      if (!Object.op_Implicit((Object) this.camCtrl))
        return;
      this.camCtrl.NoCtrlCondition = (BaseCameraControl_Ver2.NoCtrlFunc) (() => false);
    }

    public enum CanvasType
    {
      SubWin,
      DrawWin,
      PatternWin,
      ColorWin,
    }
  }
}
