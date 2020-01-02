// Decompiled with JetBrains decompiler
// Type: MeshLut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using usmooth;

public class MeshLut
{
  private Dictionary<int, MeshData> _lut = new Dictionary<int, MeshData>();

  public bool AddMesh(GameObject go)
  {
    if (this._lut.ContainsKey(((Object) go).GetInstanceID()))
      return true;
    if (Object.op_Equality((Object) go.GetComponent<Renderer>(), (Object) null))
      return false;
    MeshFilter component = (MeshFilter) go.GetComponent(typeof (MeshFilter));
    if (Object.op_Equality((Object) component, (Object) null))
      return false;
    MeshData meshData1 = new MeshData();
    meshData1._instID = ((Object) go).GetInstanceID();
    meshData1._vertCount = component.get_mesh().get_vertexCount();
    meshData1._materialCount = ((Renderer) go.GetComponent<Renderer>()).get_sharedMaterials().Length;
    MeshData meshData2 = meshData1;
    Bounds bounds = ((Renderer) go.GetComponent<Renderer>()).get_bounds();
    Vector3 size = ((Bounds) ref bounds).get_size();
    double magnitude = (double) ((Vector3) ref size).get_magnitude();
    meshData2._boundSize = (float) magnitude;
    meshData1._camDist = !Object.op_Inequality((Object) DataCollector.MainCamera, (Object) null) ? 0.0f : Vector3.Distance(go.get_transform().get_position(), DataCollector.MainCamera.get_transform().get_position());
    this._lut.Add(meshData1._instID, meshData1);
    return true;
  }

  public void WriteMesh(int instID, UsCmd cmd)
  {
    MeshData meshData;
    if (!this._lut.TryGetValue(instID, out meshData))
      return;
    meshData.Write(cmd);
  }

  public void ClearLut()
  {
    this._lut.Clear();
  }
}
