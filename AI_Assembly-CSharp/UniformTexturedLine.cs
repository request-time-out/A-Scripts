// Decompiled with JetBrains decompiler
// Type: UniformTexturedLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class UniformTexturedLine : MonoBehaviour
{
  public Texture lineTexture;
  public float lineWidth;
  public float textureScale;

  public UniformTexturedLine()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    List<Vector2> vector2List = new List<Vector2>();
    vector2List.Add(new Vector2(0.0f, (float) Random.Range(0, Screen.get_height() / 2)));
    vector2List.Add(new Vector2((float) (Screen.get_width() - 1), (float) Random.Range(0, Screen.get_height())));
    VectorLine vectorLine = new VectorLine("Line", vector2List, this.lineTexture, this.lineWidth);
    vectorLine.set_textureScale(this.textureScale);
    vectorLine.Draw();
  }
}
