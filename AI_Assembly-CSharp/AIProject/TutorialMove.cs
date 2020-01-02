// Decompiled with JetBrains decompiler
// Type: AIProject.TutorialMove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class TutorialMove : AgentTutorialMoveAction
  {
    public override void OnStart()
    {
      this.Agent.EventKey = EventType.Move;
      OffMeshLinkData currentOffMeshLinkData = this.Agent.NavMeshAgent.get_currentOffMeshLinkData();
      ActionPoint component = (ActionPoint) ((Component) ((OffMeshLinkData) ref currentOffMeshLinkData).get_offMeshLink()).GetComponent<ActionPoint>();
      this.Agent.TargetInSightActionPoint = component;
      this._actionMotion = new PoseKeyPair()
      {
        postureID = -1,
        poseID = -1
      };
      if (Singleton<Resources>.IsInstance() && Object.op_Inequality((Object) component, (Object) null))
      {
        AgentProfile.TutorialSetting tutorial = Singleton<Resources>.Instance.AgentProfile.Tutorial;
        if (0 <= component.ID)
        {
          int id = component.ID;
          if (((IEnumerable<int>) tutorial.GoGhroughActionIDList).Contains<int>(id))
            this._actionMotion = tutorial.GoGhroughAnimID;
          else if (((IEnumerable<int>) tutorial.ThreeStepJumpActionIDList).Contains<int>(id))
            this._actionMotion = tutorial.ThreeStepJumpAnimID;
        }
        else if (!component.IDList.IsNullOrEmpty<int>())
        {
          foreach (int id in component.IDList)
          {
            if (((IEnumerable<int>) tutorial.GoGhroughActionIDList).Contains<int>(id))
              this._actionMotion = tutorial.GoGhroughAnimID;
            else if (((IEnumerable<int>) tutorial.ThreeStepJumpActionIDList).Contains<int>(id))
              this._actionMotion = tutorial.ThreeStepJumpAnimID;
          }
        }
      }
      base.OnStart();
    }
  }
}
