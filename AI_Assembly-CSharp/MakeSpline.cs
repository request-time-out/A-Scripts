// Decompiled with JetBrains decompiler
// Type: MakeSpline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class MakeSpline : MonoBehaviour
{
  public int segments;
  public bool loop;
  public bool usePoints;

  public MakeSpline()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    List<Vector3> vector3List = new List<Vector3>();
    int num1 = 1;
    int num2 = num1 + 1;
    // ISSUE: variable of a boxed type
    __Boxed<int> local = (ValueType) num1;
    for (GameObject gameObject = GameObject.Find("Sphere" + (object) local); Object.op_Inequality((Object) gameObject, (Object) null); gameObject = GameObject.Find("Sphere" + (object) num2++))
      vector3List.Add(gameObject.get_transform().get_position());
    if (this.usePoints)
    {
      VectorLine vectorLine = new VectorLine("Spline", new List<Vector3>(this.segments + 1), 2f, (LineType) 2);
      vectorLine.MakeSpline(vector3List.ToArray(), this.segments, this.loop);
      vectorLine.Draw();
    }
    else
    {
      VectorLine vectorLine = new VectorLine("Spline", new List<Vector3>(this.segments + 1), 2f, (LineType) 0);
      vectorLine.MakeSpline(vector3List.ToArray(), this.segments, this.loop);
      vectorLine.Draw3D();
    }
  }
}
