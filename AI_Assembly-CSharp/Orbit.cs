// Decompiled with JetBrains decompiler
// Type: Orbit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Orbit : MonoBehaviour
{
  public float orbitSpeed;
  public float rotateSpeed;
  public int orbitLineResolution;
  public Material lineMaterial;

  public Orbit()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    VectorLine vectorLine = new VectorLine("OrbitLine", new List<Vector3>(this.orbitLineResolution), 2f, (LineType) 0);
    vectorLine.set_material(this.lineMaterial);
    vectorLine.MakeCircle(Vector3.get_zero(), Vector3.get_up(), Vector3.Distance(((Component) this).get_transform().get_position(), Vector3.get_zero()));
    vectorLine.Draw3DAuto();
  }

  private void Update()
  {
    ((Component) this).get_transform().RotateAround(Vector3.get_zero(), Vector3.get_up(), this.orbitSpeed * Time.get_deltaTime());
    ((Component) this).get_transform().Rotate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_up(), this.rotateSpeed), Time.get_deltaTime()));
  }
}
