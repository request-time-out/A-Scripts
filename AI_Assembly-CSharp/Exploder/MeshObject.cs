// Decompiled with JetBrains decompiler
// Type: Exploder.MeshObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder
{
  internal struct MeshObject
  {
    public ExploderMesh mesh;
    public Material material;
    public ExploderTransform transform;
    public Transform parent;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;
    public GameObject original;
    public ExploderOption option;
    public GameObject skinnedOriginal;
    public GameObject bakeObject;
    public float distanceRatio;
    public int id;
  }
}
