// Decompiled with JetBrains decompiler
// Type: EndCapDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class EndCapDemo : MonoBehaviour
{
  public Texture2D lineTex;
  public Texture2D lineTex2;
  public Texture2D lineTex3;
  public Texture2D frontTex;
  public Texture2D backTex;
  public Texture2D capTex;

  public EndCapDemo()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    VectorLine.SetEndCap("arrow", (EndCap) 0, new Texture2D[2]
    {
      this.lineTex,
      this.frontTex
    });
    VectorLine.SetEndCap("arrow2", (EndCap) 1, new Texture2D[3]
    {
      this.lineTex2,
      this.frontTex,
      this.backTex
    });
    VectorLine.SetEndCap("rounded", (EndCap) 2, new Texture2D[2]
    {
      this.lineTex3,
      this.capTex
    });
    VectorLine vectorLine1 = new VectorLine("Arrow", new List<Vector2>(50), 30f, (LineType) 0, (Joins) 1);
    vectorLine1.set_useViewportCoords(true);
    Vector2[] vector2Array1 = new Vector2[5]
    {
      new Vector2(0.1f, 0.15f),
      new Vector2(0.3f, 0.5f),
      new Vector2(0.5f, 0.6f),
      new Vector2(0.7f, 0.5f),
      new Vector2(0.9f, 0.15f)
    };
    vectorLine1.MakeSpline(vector2Array1);
    vectorLine1.set_endCap("arrow");
    vectorLine1.Draw();
    VectorLine vectorLine2 = new VectorLine("Arrow2", new List<Vector2>(50), 40f, (LineType) 0, (Joins) 1);
    vectorLine2.set_useViewportCoords(true);
    Vector2[] vector2Array2 = new Vector2[5]
    {
      new Vector2(0.1f, 0.85f),
      new Vector2(0.3f, 0.5f),
      new Vector2(0.5f, 0.4f),
      new Vector2(0.7f, 0.5f),
      new Vector2(0.9f, 0.85f)
    };
    vectorLine2.MakeSpline(vector2Array2);
    vectorLine2.set_endCap("arrow2");
    vectorLine2.set_continuousTexture(true);
    vectorLine2.Draw();
    List<Vector2> vector2List = new List<Vector2>();
    vector2List.Add(new Vector2(0.1f, 0.5f));
    vector2List.Add(new Vector2(0.9f, 0.5f));
    VectorLine vectorLine3 = new VectorLine("Rounded", vector2List, 20f);
    vectorLine3.set_useViewportCoords(true);
    vectorLine3.set_endCap("rounded");
    vectorLine3.Draw();
  }
}
