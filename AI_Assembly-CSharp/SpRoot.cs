// Decompiled with JetBrains decompiler
// Type: SpRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class SpRoot : MonoBehaviour
{
  public Camera renderCamera;
  public float baseScreenWidth;
  public float baseScreenHeight;

  public SpRoot()
  {
    base.\u002Ector();
  }

  public float GetSpriteRate()
  {
    return (float) (1.0 / ((double) this.baseScreenHeight / 2.0 * 0.00999999977648258 * ((double) Screen.get_height() / ((double) this.baseScreenHeight * ((double) Screen.get_width() / (double) this.baseScreenWidth)))));
  }

  public float GetSpriteCorrectY()
  {
    return (float) (((double) Screen.get_height() - (double) Screen.get_width() / (double) this.baseScreenWidth * (double) this.baseScreenHeight) * (2.0 / (double) Screen.get_height()) * 0.5);
  }
}
