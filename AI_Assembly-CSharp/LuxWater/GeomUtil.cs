// Decompiled with JetBrains decompiler
// Type: LuxWater.GeomUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Reflection;
using UnityEngine;

namespace LuxWater
{
  public static class GeomUtil
  {
    private static Action<Plane[], Matrix4x4> _calculateFrustumPlanes_Imp;

    public static void CalculateFrustumPlanes(Plane[] planes, Matrix4x4 worldToProjectMatrix)
    {
      if (GeomUtil._calculateFrustumPlanes_Imp == null)
      {
        MethodInfo method = typeof (GeometryUtility).GetMethod("Internal_ExtractPlanes", BindingFlags.Static | BindingFlags.NonPublic, (Binder) null, new Type[2]
        {
          typeof (Plane[]),
          typeof (Matrix4x4)
        }, (ParameterModifier[]) null);
        if (method == (MethodInfo) null)
          throw new Exception("Failed to reflect internal method. Your Unity version may not contain the presumed named method in GeometryUtility.");
        GeomUtil._calculateFrustumPlanes_Imp = Delegate.CreateDelegate(typeof (Action<Plane[], Matrix4x4>), method) as Action<Plane[], Matrix4x4>;
        if (GeomUtil._calculateFrustumPlanes_Imp == null)
          throw new Exception("Failed to reflect internal method. Your Unity version may not contain the presumed named method in GeometryUtility.");
      }
      GeomUtil._calculateFrustumPlanes_Imp(planes, worldToProjectMatrix);
    }
  }
}
