// Decompiled with JetBrains decompiler
// Type: MorphCloneMesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class MorphCloneMesh : MonoBehaviour
{
  public MorphCloneMesh()
  {
    base.\u002Ector();
  }

  public static void Clone(out Mesh CloneData, Mesh SorceData)
  {
    CloneData = (Mesh) Object.Instantiate<Mesh>((M0) SorceData);
  }
}
