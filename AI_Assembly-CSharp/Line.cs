// Decompiled with JetBrains decompiler
// Type: Line
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Line : MonoBehaviour
{
  public Line()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    List<Vector2> vector2List = new List<Vector2>();
    vector2List.Add(new Vector2(0.0f, (float) Random.Range(0, Screen.get_height())));
    vector2List.Add(new Vector2((float) (Screen.get_width() - 1), (float) Random.Range(0, Screen.get_height())));
    new VectorLine(nameof (Line), vector2List, 2f).Draw();
  }
}
