// Decompiled with JetBrains decompiler
// Type: CurvePointControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class CurvePointControl : MonoBehaviour
{
  public int objectNumber;
  public GameObject controlObject;
  public GameObject controlObject2;

  public CurvePointControl()
  {
    base.\u002Ector();
  }

  private void OnMouseDrag()
  {
    ((Component) this).get_transform().set_position(DrawCurve.cam.ScreenToViewportPoint(Input.get_mousePosition()));
    DrawCurve.use.UpdateLine(this.objectNumber, Vector2.op_Implicit(Input.get_mousePosition()), ((Component) this).get_gameObject());
  }
}
