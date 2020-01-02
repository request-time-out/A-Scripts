// Decompiled with JetBrains decompiler
// Type: LocalNavMeshBuilder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(-102)]
public class LocalNavMeshBuilder : MonoBehaviour
{
  public Transform m_Tracked;
  public Vector3 m_Size;
  private NavMeshData m_NavMesh;
  private AsyncOperation m_Operation;
  private NavMeshDataInstance m_Instance;
  private List<NavMeshBuildSource> m_Sources;

  public LocalNavMeshBuilder()
  {
    base.\u002Ector();
  }

  [DebuggerHidden]
  private IEnumerator Start()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new LocalNavMeshBuilder.\u003CStart\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void OnEnable()
  {
    this.m_NavMesh = new NavMeshData();
    this.m_Instance = NavMesh.AddNavMeshData(this.m_NavMesh);
    if (Object.op_Equality((Object) this.m_Tracked, (Object) null))
      this.m_Tracked = ((Component) this).get_transform();
    this.UpdateNavMesh(false);
  }

  private void OnDisable()
  {
    ((NavMeshDataInstance) ref this.m_Instance).Remove();
  }

  private void UpdateNavMesh(bool asyncUpdate = false)
  {
    NavMeshSourceTag.Collect(ref this.m_Sources, 0);
    NavMeshBuildSettings settingsById = NavMesh.GetSettingsByID(0);
    Bounds bounds = this.QuantizedBounds();
    if (asyncUpdate)
      this.m_Operation = NavMeshBuilder.UpdateNavMeshDataAsync(this.m_NavMesh, settingsById, this.m_Sources, bounds);
    else
      NavMeshBuilder.UpdateNavMeshData(this.m_NavMesh, settingsById, this.m_Sources, bounds);
  }

  private static Vector3 Quantize(Vector3 v, Vector3 quant)
  {
    return new Vector3((float) quant.x * Mathf.Floor((float) (v.x / quant.x)), (float) quant.y * Mathf.Floor((float) (v.y / quant.y)), (float) quant.z * Mathf.Floor((float) (v.z / quant.z)));
  }

  private Bounds QuantizedBounds()
  {
    return new Bounds(LocalNavMeshBuilder.Quantize(!Object.op_Implicit((Object) this.m_Tracked) ? ((Component) this).get_transform().get_position() : this.m_Tracked.get_position(), Vector3.op_Multiply(0.1f, this.m_Size)), this.m_Size);
  }

  private void OnDrawGizmosSelected()
  {
    if (Object.op_Implicit((Object) this.m_NavMesh))
    {
      Gizmos.set_color(Color.get_green());
      Bounds sourceBounds1 = this.m_NavMesh.get_sourceBounds();
      Vector3 center = ((Bounds) ref sourceBounds1).get_center();
      Bounds sourceBounds2 = this.m_NavMesh.get_sourceBounds();
      Vector3 size = ((Bounds) ref sourceBounds2).get_size();
      Gizmos.DrawWireCube(center, size);
    }
    Gizmos.set_color(Color.get_yellow());
    Bounds bounds = this.QuantizedBounds();
    Gizmos.DrawWireCube(((Bounds) ref bounds).get_center(), ((Bounds) ref bounds).get_size());
    Gizmos.set_color(Color.get_green());
    Gizmos.DrawWireCube(!Object.op_Implicit((Object) this.m_Tracked) ? ((Component) this).get_transform().get_position() : this.m_Tracked.get_position(), this.m_Size);
  }
}
