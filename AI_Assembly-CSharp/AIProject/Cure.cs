// Decompiled with JetBrains decompiler
// Type: AIProject.Cure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;

namespace AIProject
{
  [TaskCategory("")]
  public class Cure : AgentAction
  {
    private int _layer;
    private bool _inEnableFade;
    private float _inFadeSecond;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.StateType = AIProject.Definitions.State.Type.Immobility;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      PoseKeyPair cureId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.CureID;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[cureId.postureID][cureId.poseID];
      agent.Animation.LoadEventKeyTable(cureId.postureID, cureId.poseID);
      this._layer = info.Layer;
      this._inEnableFade = info.MainStateInfo.InStateInfo.EnableFade;
      this._inFadeSecond = info.MainStateInfo.InStateInfo.FadeSecond;
      agent.Animation.InitializeStates(info);
      agent.Animation.PlayInAnimation(this._inEnableFade, this._inFadeSecond, info.MainStateInfo.FadeOutTime, this._layer);
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.Agent.Animation.PlayingInAnimation)
        return (TaskStatus) 3;
      this.OnComplete(this.Agent);
      return (TaskStatus) 2;
    }

    private void OnComplete(AgentActor agent)
    {
      ItemIDKeyPair gauzeID = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine.GauzeID;
      StuffItem stuffItem1 = agent.AgentData.ItemList.Find((Predicate<StuffItem>) (x => x.CategoryID == gauzeID.categoryID && x.ID == gauzeID.itemID));
      StuffItem stuffItem2 = new StuffItem(stuffItem1.CategoryID, stuffItem1.ID, 1);
      agent.AgentData.ItemList.RemoveItem(stuffItem2);
    }

    public virtual void OnEnd()
    {
      this.Agent.ChangeDynamicNavMeshAgentAvoidance();
    }
  }
}
