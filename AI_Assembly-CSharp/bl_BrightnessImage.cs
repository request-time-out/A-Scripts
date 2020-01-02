// Decompiled with JetBrains decompiler
// Type: bl_BrightnessImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (CanvasGroup))]
public class bl_BrightnessImage : MonoBehaviour
{
  private float Value;
  private CanvasGroup _Alpha;

  public bl_BrightnessImage()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    ((Component) this).get_transform().SetAsLastSibling();
  }

  public void SetValue(float val)
  {
    this.Value = val;
    this.Alpha.set_alpha(1f - this.Value);
  }

  private CanvasGroup Alpha
  {
    get
    {
      if (Object.op_Equality((Object) this._Alpha, (Object) null))
        this._Alpha = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
      return this._Alpha;
    }
  }
}
