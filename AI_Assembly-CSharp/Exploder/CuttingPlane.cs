// Decompiled with JetBrains decompiler
// Type: Exploder.CuttingPlane
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Exploder
{
  internal class CuttingPlane
  {
    private static Vector3[] rectAxis = new Vector3[3]
    {
      new Vector3(1f, 0.0f, 0.0f),
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(0.0f, 0.0f, 1f)
    };
    private readonly Random random;
    private readonly Plane plane;
    private readonly Core core;

    public CuttingPlane(Core core)
    {
      this.random = new Random();
      this.plane = new Plane(Vector3.get_one(), Vector3.get_zero());
      this.core = core;
    }

    private Plane GetRandomPlane(ExploderMesh mesh)
    {
      Vector3 normal;
      ((Vector3) ref normal).\u002Ector((float) (this.random.NextDouble() * 2.0 - 1.0), (float) (this.random.NextDouble() * 2.0 - 1.0), (float) (this.random.NextDouble() * 2.0 - 1.0));
      this.plane.Set(normal, mesh.centroid);
      return this.plane;
    }

    private Plane GetRectangularRegularPlane(ExploderMesh mesh, int attempt)
    {
      float num1 = (float) (mesh.max.x - mesh.min.x);
      float num2 = (float) (mesh.max.y - mesh.min.y);
      float num3 = (float) (mesh.max.z - mesh.min.z);
      int index = ((double) num1 <= (double) num2 ? ((double) num2 <= (double) num3 ? 2 : 1) : ((double) num1 <= (double) num3 ? 2 : 0)) + attempt;
      if (index > 2)
        return this.GetRandomPlane(mesh);
      this.plane.Set(CuttingPlane.rectAxis[index], mesh.centroid);
      return this.plane;
    }

    private Plane GetRectangularRandom(ExploderMesh mesh, int attempt)
    {
      int index = this.random.Next(0, 3) + attempt;
      if (index > 2)
        return this.GetRandomPlane(mesh);
      this.plane.Set(CuttingPlane.rectAxis[index], mesh.centroid);
      return this.plane;
    }

    public Plane GetPlane(ExploderMesh mesh, int attempt)
    {
      switch (this.core.parameters.CuttingStyle)
      {
        case ExploderObject.CuttingStyleOption.Random:
          return this.GetRandomPlane(mesh);
        case ExploderObject.CuttingStyleOption.RectangularRandom:
          return this.GetRectangularRandom(mesh, attempt);
        case ExploderObject.CuttingStyleOption.RectangularRegular:
          return this.GetRectangularRegularPlane(mesh, attempt);
        default:
          return (Plane) null;
      }
    }
  }
}
