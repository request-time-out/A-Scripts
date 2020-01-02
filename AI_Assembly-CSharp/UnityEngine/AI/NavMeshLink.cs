// Decompiled with JetBrains decompiler
// Type: UnityEngine.AI.NavMeshLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.AI
{
  [ExecuteInEditMode]
  [DefaultExecutionOrder(-101)]
  [AddComponentMenu("Navigation/NavMeshLink", 33)]
  [HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
  public class NavMeshLink : MonoBehaviour
  {
    private static readonly List<NavMeshLink> s_Tracked = new List<NavMeshLink>();
    [SerializeField]
    private int m_AgentTypeID;
    [SerializeField]
    private Vector3 m_StartPoint;
    [SerializeField]
    private Vector3 m_EndPoint;
    [SerializeField]
    private float m_Width;
    [SerializeField]
    private int m_CostModifier;
    [SerializeField]
    private bool m_Bidirectional;
    [SerializeField]
    private bool m_AutoUpdatePosition;
    [SerializeField]
    private int m_Area;
    private NavMeshLinkInstance m_LinkInstance;
    private Vector3 m_LastPosition;
    private Quaternion m_LastRotation;

    public NavMeshLink()
    {
      base.\u002Ector();
    }

    public int agentTypeID
    {
      get
      {
        return this.m_AgentTypeID;
      }
      set
      {
        this.m_AgentTypeID = value;
        this.UpdateLink();
      }
    }

    public Vector3 startPoint
    {
      get
      {
        return this.m_StartPoint;
      }
      set
      {
        this.m_StartPoint = value;
        this.UpdateLink();
      }
    }

    public Vector3 endPoint
    {
      get
      {
        return this.m_EndPoint;
      }
      set
      {
        this.m_EndPoint = value;
        this.UpdateLink();
      }
    }

    public float width
    {
      get
      {
        return this.m_Width;
      }
      set
      {
        this.m_Width = value;
        this.UpdateLink();
      }
    }

    public int costModifier
    {
      get
      {
        return this.m_CostModifier;
      }
      set
      {
        this.m_CostModifier = value;
        this.UpdateLink();
      }
    }

    public bool bidirectional
    {
      get
      {
        return this.m_Bidirectional;
      }
      set
      {
        this.m_Bidirectional = value;
        this.UpdateLink();
      }
    }

    public bool autoUpdate
    {
      get
      {
        return this.m_AutoUpdatePosition;
      }
      set
      {
        this.SetAutoUpdate(value);
      }
    }

    public int area
    {
      get
      {
        return this.m_Area;
      }
      set
      {
        this.m_Area = value;
        this.UpdateLink();
      }
    }

    private void OnEnable()
    {
      this.AddLink();
      if (!this.m_AutoUpdatePosition || !((NavMeshLinkInstance) ref this.m_LinkInstance).get_valid())
        return;
      NavMeshLink.AddTracking(this);
    }

    private void OnDisable()
    {
      NavMeshLink.RemoveTracking(this);
      ((NavMeshLinkInstance) ref this.m_LinkInstance).Remove();
    }

    public void UpdateLink()
    {
      ((NavMeshLinkInstance) ref this.m_LinkInstance).Remove();
      this.AddLink();
    }

    private static void AddTracking(NavMeshLink link)
    {
      if (NavMeshLink.s_Tracked.Count == 0)
      {
        // ISSUE: variable of the null type
        __Null onPreUpdate = NavMesh.onPreUpdate;
        // ISSUE: reference to a compiler-generated field
        if (NavMeshLink.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          NavMeshLink.\u003C\u003Ef__mg\u0024cache0 = new NavMesh.OnNavMeshPreUpdate((object) null, __methodptr(UpdateTrackedInstances));
        }
        // ISSUE: reference to a compiler-generated field
        NavMesh.OnNavMeshPreUpdate fMgCache0 = NavMeshLink.\u003C\u003Ef__mg\u0024cache0;
        NavMesh.onPreUpdate = (__Null) Delegate.Combine((Delegate) onPreUpdate, (Delegate) fMgCache0);
      }
      NavMeshLink.s_Tracked.Add(link);
    }

    private static void RemoveTracking(NavMeshLink link)
    {
      NavMeshLink.s_Tracked.Remove(link);
      if (NavMeshLink.s_Tracked.Count != 0)
        return;
      // ISSUE: variable of the null type
      __Null onPreUpdate = NavMesh.onPreUpdate;
      // ISSUE: reference to a compiler-generated field
      if (NavMeshLink.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        NavMeshLink.\u003C\u003Ef__mg\u0024cache1 = new NavMesh.OnNavMeshPreUpdate((object) null, __methodptr(UpdateTrackedInstances));
      }
      // ISSUE: reference to a compiler-generated field
      NavMesh.OnNavMeshPreUpdate fMgCache1 = NavMeshLink.\u003C\u003Ef__mg\u0024cache1;
      NavMesh.onPreUpdate = (__Null) Delegate.Remove((Delegate) onPreUpdate, (Delegate) fMgCache1);
    }

    private void SetAutoUpdate(bool value)
    {
      if (this.m_AutoUpdatePosition == value)
        return;
      this.m_AutoUpdatePosition = value;
      if (value)
        NavMeshLink.AddTracking(this);
      else
        NavMeshLink.RemoveTracking(this);
    }

    private void AddLink()
    {
      NavMeshLinkData navMeshLinkData = (NavMeshLinkData) null;
      ((NavMeshLinkData) ref navMeshLinkData).set_startPosition(this.m_StartPoint);
      ((NavMeshLinkData) ref navMeshLinkData).set_endPosition(this.m_EndPoint);
      ((NavMeshLinkData) ref navMeshLinkData).set_width(this.m_Width);
      ((NavMeshLinkData) ref navMeshLinkData).set_costModifier((float) this.m_CostModifier);
      ((NavMeshLinkData) ref navMeshLinkData).set_bidirectional(this.m_Bidirectional);
      ((NavMeshLinkData) ref navMeshLinkData).set_area(this.m_Area);
      ((NavMeshLinkData) ref navMeshLinkData).set_agentTypeID(this.m_AgentTypeID);
      this.m_LinkInstance = NavMesh.AddLink(navMeshLinkData, ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation());
      if (((NavMeshLinkInstance) ref this.m_LinkInstance).get_valid())
        ((NavMeshLinkInstance) ref this.m_LinkInstance).set_owner((Object) this);
      this.m_LastPosition = ((Component) this).get_transform().get_position();
      this.m_LastRotation = ((Component) this).get_transform().get_rotation();
    }

    private bool HasTransformChanged()
    {
      return Vector3.op_Inequality(this.m_LastPosition, ((Component) this).get_transform().get_position()) || Quaternion.op_Inequality(this.m_LastRotation, ((Component) this).get_transform().get_rotation());
    }

    private void OnDidApplyAnimationProperties()
    {
      this.UpdateLink();
    }

    private static void UpdateTrackedInstances()
    {
      foreach (NavMeshLink navMeshLink in NavMeshLink.s_Tracked)
      {
        if (navMeshLink.HasTransformChanged())
          navMeshLink.UpdateLink();
      }
    }
  }
}
