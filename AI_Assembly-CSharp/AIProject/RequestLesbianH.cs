// Decompiled with JetBrains decompiler
// Type: AIProject.RequestLesbianH
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class RequestLesbianH : AgentAction
  {
    private int _relationShip = 50;
    private Actor _partner;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      Actor targetInSightActor = agent.TargetInSightActor;
      agent.CommandPartner = targetInSightActor;
      Actor listener = this._partner = targetInSightActor;
      if (Object.op_Equality((Object) listener, (Object) null))
        return;
      agent.SetActiveOnEquipedItem(false);
      listener.SetActiveOnEquipedItem(false);
      agent.StopNavMeshAgent();
      listener.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      listener.ChangeStaticNavMeshAgentAvoidance();
      agent.DisableActionFlag();
      if (listener is AgentActor)
        (listener as AgentActor).DisableActionFlag();
      agent.RuntimeDesire = Desire.Type.H;
      int listenID = 0;
      Dictionary<int, int> listenerRelationTable;
      if (listener is AgentActor)
      {
        AgentActor agentActor = listener as AgentActor;
        int id = agent.ID;
        if (!agentActor.AgentData.FriendlyRelationShipTable.TryGetValue(id, out this._relationShip))
        {
          int defaultRelationShip = Singleton<Resources>.Instance.AgentProfile.DefaultRelationShip;
          agentActor.AgentData.FriendlyRelationShipTable[id] = defaultRelationShip;
          this._relationShip = defaultRelationShip;
        }
        listenerRelationTable = Singleton<Resources>.Instance.Animation.TalkListenerRelationTable;
      }
      else
      {
        int id = listener.ID;
        if (!agent.AgentData.FriendlyRelationShipTable.TryGetValue(id, out this._relationShip))
        {
          int defaultRelationShip = Singleton<Resources>.Instance.AgentProfile.DefaultRelationShip;
          agent.AgentData.FriendlyRelationShipTable[id] = defaultRelationShip;
          this._relationShip = defaultRelationShip;
        }
        listenerRelationTable = Singleton<Resources>.Instance.Animation.MerchantListenerRelationTable;
      }
      List<KeyValuePair<int, int>> toRelease = ListPool<KeyValuePair<int, int>>.Get();
      foreach (KeyValuePair<int, int> keyValuePair in listenerRelationTable)
        toRelease.Add(keyValuePair);
      toRelease.Sort((Comparison<KeyValuePair<int, int>>) ((v1, v2) => v1.Value - v2.Value));
      for (int index = 0; index < toRelease.Count; ++index)
      {
        KeyValuePair<int, int> keyValuePair = toRelease[index];
        if (this._relationShip <= keyValuePair.Value)
        {
          listenID = keyValuePair.Key;
          break;
        }
      }
      ListPool<KeyValuePair<int, int>>.Release(toRelease);
      agent.StartTalkSequence(listener, agent.ChaControl.fileParam.personality, listenID);
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this._partner, (Object) null))
        return (TaskStatus) 1;
      AgentActor agent = this.Agent;
      if (agent.LivesTalkSequence)
        return (TaskStatus) 3;
      ChaFileGameInfo fileGameInfo = agent.ChaControl.fileGameInfo;
      int num1 = fileGameInfo.flavorState[4];
      int num2 = fileGameInfo.flavorState[7];
      int desireKey = Desire.GetDesireKey(Desire.Type.H);
      float? desire = agent.GetDesire(desireKey);
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      bool flag = num1 >= statusProfile.LesbianBorder && num2 >= statusProfile.GirlsActionBorder && (desire.HasValue && (double) desire.GetValueOrDefault() >= (double) statusProfile.LesbianBorderDesire);
      Actor partner = this._partner;
      if (flag)
      {
        agent.Partner = partner;
        partner.Partner = (Actor) agent;
        agent.ChangeBehavior(Desire.ActionType.GotoLesbianSpot);
        switch (partner)
        {
          case AgentActor _:
            AgentActor agentActor = partner as AgentActor;
            Desire.ActionType mode = Desire.ActionType.GotoLesbianSpotFollow;
            agentActor.Mode = mode;
            agentActor.BehaviorResources.ChangeMode(mode);
            break;
          case MerchantActor _:
            (partner as MerchantActor).ChangeBehavior(Merchant.ActionType.GotoLesbianSpotFollow);
            break;
        }
      }
      else
      {
        if ((double) Random.get_value() < 0.5)
        {
          agent.ChangeBehavior(Desire.ActionType.SearchMasturbation);
        }
        else
        {
          agent.SetDesire(desireKey, 0.0f);
          agent.ChangeBehavior(Desire.ActionType.Normal);
        }
        switch (partner)
        {
          case AgentActor _:
            (partner as AgentActor).ChangeBehavior(Desire.ActionType.Normal);
            break;
          case MerchantActor _:
            MerchantActor merchantActor = partner as MerchantActor;
            merchantActor.ChangeBehavior(merchantActor.LastNormalMode);
            break;
        }
      }
      agent.CommandPartner = (Actor) null;
      agent.TargetInSightActor = (Actor) null;
      switch (partner)
      {
        case AgentActor _:
          (partner as AgentActor).CommandPartner = (Actor) null;
          break;
        case MerchantActor _:
          (partner as MerchantActor).CommandPartner = (Actor) null;
          break;
      }
      return (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      this.Agent.ChangeDynamicNavMeshAgentAvoidance();
      if (Object.op_Inequality((Object) this._partner, (Object) null))
        this._partner.ChangeDynamicNavMeshAgentAvoidance();
      this.Agent.StopTalkSequence();
      this._partner = (Actor) null;
    }
  }
}
