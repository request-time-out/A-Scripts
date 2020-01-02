// Decompiled with JetBrains decompiler
// Type: ReallyBasicLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using Vectrosity;

public class ReallyBasicLine : MonoBehaviour
{
  public ReallyBasicLine()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    VectorLine.SetLine(Color.get_white(), new Vector2[2]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2((float) (Screen.get_width() - 1), (float) (Screen.get_height() - 1))
    });
  }
}
