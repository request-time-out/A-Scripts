// Decompiled with JetBrains decompiler
// Type: AIProject.DynamicNavMeshBuilder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class DynamicNavMeshBuilder : MonoBehaviour
  {
    [SerializeField]
    protected Transform _tracked;
    [SerializeField]
    protected Vector3 _size;
    protected NavMeshData _navMesh;
    protected AsyncOperation _operation;
    protected NavMeshDataInstance _instance;
    private List<NavMeshBuildSource> m_Sources;
    private IEnumerator _enumerator;

    public DynamicNavMeshBuilder()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      this._navMesh = new NavMeshData();
      this._instance = NavMesh.AddNavMeshData(this._navMesh);
      if (Object.op_Equality((Object) this._tracked, (Object) null))
        this._tracked = ((Component) this).get_transform();
      if (this._enumerator == null)
        this._enumerator = this.LoadNavMesh();
      this.StartCoroutine(this._enumerator);
    }

    private void OnDisable()
    {
      this.StopCoroutine(this._enumerator);
      ((NavMeshDataInstance) ref this._instance).Remove();
    }

    [DebuggerHidden]
    private IEnumerator LoadNavMesh()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new DynamicNavMeshBuilder.\u003CLoadNavMesh\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected virtual void UpdateNavMesh(bool asyncUpdate = false)
    {
      NavMeshBuildTag.Collect(ref this.m_Sources);
      NavMeshBuildSettings settingsById = NavMesh.GetSettingsByID(0);
      Bounds bounds = this.QuantizedBounds();
      if (asyncUpdate)
        this._operation = NavMeshBuilder.UpdateNavMeshDataAsync(this._navMesh, settingsById, this.m_Sources, bounds);
      else
        NavMeshBuilder.UpdateNavMeshData(this._navMesh, settingsById, this.m_Sources, bounds);
    }

    private static Vector3 Quantize(Vector3 v, Vector3 quant)
    {
      return new Vector3((float) quant.x * Mathf.Floor((float) (v.x / quant.x)), (float) quant.y * Mathf.Floor((float) (v.y / quant.y)), (float) quant.z * Mathf.Floor((float) (v.z / quant.z)));
    }

    protected Bounds QuantizedBounds()
    {
      return new Bounds(DynamicNavMeshBuilder.Quantize(!Object.op_Implicit((Object) this._tracked) ? ((Component) this).get_transform().get_position() : this._tracked.get_position(), Vector3.op_Multiply(0.1f, this._size)), this._size);
    }

    protected virtual void OnDrawGizmosSelected()
    {
      if (Object.op_Implicit((Object) this._navMesh))
      {
        Gizmos.set_color(Color.get_green());
        Bounds sourceBounds1 = this._navMesh.get_sourceBounds();
        Vector3 center = ((Bounds) ref sourceBounds1).get_center();
        Bounds sourceBounds2 = this._navMesh.get_sourceBounds();
        Vector3 size = ((Bounds) ref sourceBounds2).get_size();
        Gizmos.DrawWireCube(center, size);
      }
      Gizmos.set_color(Color.get_yellow());
      Bounds bounds = this.QuantizedBounds();
      Gizmos.DrawWireCube(((Bounds) ref bounds).get_center(), ((Bounds) ref bounds).get_size());
      Gizmos.set_color(Color.get_green());
      Gizmos.DrawWireCube(!Object.op_Implicit((Object) this._tracked) ? ((Component) this).get_transform().get_position() : this._tracked.get_position(), this._size);
    }
  }
}
