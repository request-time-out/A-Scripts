// Decompiled with JetBrains decompiler
// Type: NavMeshPrefabInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
[DefaultExecutionOrder(-102)]
public class NavMeshPrefabInstance : MonoBehaviour
{
  private static readonly List<NavMeshPrefabInstance> s_TrackedInstances = new List<NavMeshPrefabInstance>();
  [SerializeField]
  private NavMeshData m_NavMesh;
  [SerializeField]
  private bool m_FollowTransform;
  private NavMeshDataInstance m_Instance;
  private Vector3 m_Position;
  private Quaternion m_Rotation;

  public NavMeshPrefabInstance()
  {
    base.\u002Ector();
  }

  public NavMeshData navMeshData
  {
    get
    {
      return this.m_NavMesh;
    }
    set
    {
      this.m_NavMesh = value;
    }
  }

  public bool followTransform
  {
    get
    {
      return this.m_FollowTransform;
    }
    set
    {
      this.SetFollowTransform(value);
    }
  }

  public static List<NavMeshPrefabInstance> trackedInstances
  {
    get
    {
      return NavMeshPrefabInstance.s_TrackedInstances;
    }
  }

  private void OnEnable()
  {
    this.AddInstance();
    if (!((NavMeshDataInstance) ref this.m_Instance).get_valid() || !this.m_FollowTransform)
      return;
    this.AddTracking();
  }

  private void OnDisable()
  {
    ((NavMeshDataInstance) ref this.m_Instance).Remove();
    this.RemoveTracking();
  }

  public void UpdateInstance()
  {
    ((NavMeshDataInstance) ref this.m_Instance).Remove();
    this.AddInstance();
  }

  private void AddInstance()
  {
    if (Object.op_Implicit((Object) this.m_NavMesh))
      this.m_Instance = NavMesh.AddNavMeshData(this.m_NavMesh, ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation());
    this.m_Rotation = ((Component) this).get_transform().get_rotation();
    this.m_Position = ((Component) this).get_transform().get_position();
  }

  private void AddTracking()
  {
    if (NavMeshPrefabInstance.s_TrackedInstances.Count == 0)
    {
      // ISSUE: variable of the null type
      __Null onPreUpdate = NavMesh.onPreUpdate;
      // ISSUE: reference to a compiler-generated field
      if (NavMeshPrefabInstance.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        NavMeshPrefabInstance.\u003C\u003Ef__mg\u0024cache0 = new NavMesh.OnNavMeshPreUpdate((object) null, __methodptr(UpdateTrackedInstances));
      }
      // ISSUE: reference to a compiler-generated field
      NavMesh.OnNavMeshPreUpdate fMgCache0 = NavMeshPrefabInstance.\u003C\u003Ef__mg\u0024cache0;
      NavMesh.onPreUpdate = (__Null) Delegate.Combine((Delegate) onPreUpdate, (Delegate) fMgCache0);
    }
    NavMeshPrefabInstance.s_TrackedInstances.Add(this);
  }

  private void RemoveTracking()
  {
    NavMeshPrefabInstance.s_TrackedInstances.Remove(this);
    if (NavMeshPrefabInstance.s_TrackedInstances.Count != 0)
      return;
    // ISSUE: variable of the null type
    __Null onPreUpdate = NavMesh.onPreUpdate;
    // ISSUE: reference to a compiler-generated field
    if (NavMeshPrefabInstance.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      NavMeshPrefabInstance.\u003C\u003Ef__mg\u0024cache1 = new NavMesh.OnNavMeshPreUpdate((object) null, __methodptr(UpdateTrackedInstances));
    }
    // ISSUE: reference to a compiler-generated field
    NavMesh.OnNavMeshPreUpdate fMgCache1 = NavMeshPrefabInstance.\u003C\u003Ef__mg\u0024cache1;
    NavMesh.onPreUpdate = (__Null) Delegate.Remove((Delegate) onPreUpdate, (Delegate) fMgCache1);
  }

  private void SetFollowTransform(bool value)
  {
    if (this.m_FollowTransform == value)
      return;
    this.m_FollowTransform = value;
    if (value)
      this.AddTracking();
    else
      this.RemoveTracking();
  }

  private bool HasMoved()
  {
    return Vector3.op_Inequality(this.m_Position, ((Component) this).get_transform().get_position()) || Quaternion.op_Inequality(this.m_Rotation, ((Component) this).get_transform().get_rotation());
  }

  private static void UpdateTrackedInstances()
  {
    foreach (NavMeshPrefabInstance trackedInstance in NavMeshPrefabInstance.s_TrackedInstances)
    {
      if (trackedInstance.HasMoved())
        trackedInstance.UpdateInstance();
    }
  }
}
