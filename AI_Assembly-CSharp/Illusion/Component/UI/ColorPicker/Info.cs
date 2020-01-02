// Decompiled with JetBrains decompiler
// Type: Illusion.Component.UI.ColorPicker.Info
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Illusion.Component.UI.ColorPicker
{
  public class Info : MouseButtonCheck
  {
    [SerializeField]
    private Canvas canvas;
    private RectTransform myRt;

    public bool isOn { get; private set; }

    public Vector2 imagePos { get; private set; }

    public Vector2 imageRate { get; private set; }

    private void Start()
    {
      if (Object.op_Equality((Object) this.canvas, (Object) null))
        this.canvas = Info.SearchCanvas(((UnityEngine.Component) this).get_transform());
      if (Object.op_Equality((Object) this.canvas, (Object) null))
        return;
      this.myRt = (RectTransform) ((UnityEngine.Component) this).GetComponent<RectTransform>();
      // ISSUE: method pointer
      this.onPointerDown.AddListener(new UnityAction<PointerEventData>((object) this, __methodptr(\u003CStart\u003Em__0)));
      // ISSUE: method pointer
      this.onPointerUp.AddListener(new UnityAction<PointerEventData>((object) this, __methodptr(\u003CStart\u003Em__1)));
      // ISSUE: method pointer
      this.onBeginDrag.AddListener(new UnityAction<PointerEventData>((object) this, __methodptr(\u003CStart\u003Em__2)));
      // ISSUE: method pointer
      this.onDrag.AddListener(new UnityAction<PointerEventData>((object) this, __methodptr(\u003CStart\u003Em__3)));
      // ISSUE: method pointer
      this.onEndDrag.AddListener(new UnityAction<PointerEventData>((object) this, __methodptr(\u003CStart\u003Em__4)));
    }

    private static Canvas SearchCanvas(Transform transform)
    {
      Transform transform1 = transform;
      do
      {
        Canvas component = (Canvas) ((UnityEngine.Component) transform1).GetComponent<Canvas>();
        if (Object.op_Inequality((Object) component, (Object) null))
          return component;
        transform1 = transform1.get_parent();
      }
      while (Object.op_Inequality((Object) transform1, (Object) null));
      return (Canvas) null;
    }

    private void SetImagePosition(Vector2 cursorPos)
    {
      Vector2 zero = Vector2.get_zero();
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this.myRt, cursorPos, this.canvas.get_worldCamera(), ref zero);
      RectTransform rt = this.myRt;
      Vector2 vector2;
      ref Vector2 local = ref vector2;
      Rect rect1 = rt.get_rect();
      double width = (double) ((Rect) ref rect1).get_width();
      Rect rect2 = rt.get_rect();
      double height = (double) ((Rect) ref rect2).get_height();
      ((Vector2) ref local).\u002Ector((float) width, (float) height);
      float num1 = (float) (zero.x / ((Transform) rt).get_localScale().x);
      float num2 = (float) (zero.y / ((Transform) rt).get_localScale().y);
      this.imagePos = new Vector2(Mathf.Clamp(num1, 0.0f, (float) vector2.x), Mathf.Clamp(num2, 0.0f, (float) vector2.y));
      this.imageRate = new Vector2(Mathf.InverseLerp(0.0f, (float) vector2.x, num1), Mathf.InverseLerp(0.0f, (float) vector2.y, num2));
    }
  }
}
