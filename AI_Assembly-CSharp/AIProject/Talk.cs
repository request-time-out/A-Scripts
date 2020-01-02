// Decompiled with JetBrains decompiler
// Type: AIProject.Talk
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
  public class Talk : AgentAction
  {
    private int _relationShip = 50;
    private Actor _partner;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      Actor listener = this._partner = agent.CommandPartner;
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
      agent.RuntimeDesire = Desire.Type.Lonely;
      int listenID = 0;
      Dictionary<int, int> listenerRelationTable;
      if (listener is AgentActor)
      {
        AgentActor partner = this._partner as AgentActor;
        int id = this.Agent.ID;
        if (!partner.AgentData.FriendlyRelationShipTable.TryGetValue(id, out this._relationShip))
        {
          int num = 50;
          partner.AgentData.FriendlyRelationShipTable[id] = num;
          this._relationShip = num;
        }
        listenerRelationTable = Singleton<Resources>.Instance.Animation.TalkListenerRelationTable;
      }
      else
      {
        int id = listener.ID;
        if (!agent.AgentData.FriendlyRelationShipTable.TryGetValue(id, out this._relationShip))
        {
          int num = 50;
          agent.AgentData.FriendlyRelationShipTable[id] = num;
          this._relationShip = num;
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
      if (this.Agent.LivesTalkSequence)
        return (TaskStatus) 3;
      this.OnComplete();
      return (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      this.Agent.StopTalkSequence();
      this.Agent.ChangeDynamicNavMeshAgentAvoidance();
      if (Object.op_Inequality((Object) this._partner, (Object) null))
        this._partner.ChangeDynamicNavMeshAgentAvoidance();
      this._partner = (Actor) null;
    }

    private void OnComplete()
    {
      AgentActor agent = this.Agent;
      int desireKey1 = Desire.GetDesireKey(agent.RuntimeDesire);
      if (desireKey1 != -1)
        agent.SetDesire(desireKey1, 0.0f);
      agent.RuntimeDesire = Desire.Type.None;
      if (agent.CommandPartner is AgentActor)
      {
        int num1 = this._relationShip + Random.Range(-10, 10);
        agent.AgentData.FriendlyRelationShipTable[agent.CommandPartner.ID] = Mathf.Clamp(num1, 0, 100);
        agent.ApplySituationResultParameter(23);
        AgentActor commandPartner = agent.CommandPartner as AgentActor;
        ChaFileGameInfo fileGameInfo = agent.ChaControl.fileGameInfo;
        if (agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(39))
          commandPartner.AddStatus(3, -20f);
        int num2 = fileGameInfo.flavorState[4];
        int num3 = fileGameInfo.flavorState[7];
        int desireKey2 = Desire.GetDesireKey(Desire.Type.H);
        float? desire = agent.GetDesire(desireKey2);
        StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
        bool flag = num2 >= statusProfile.LesbianBorder && num3 >= statusProfile.GirlsActionBorder && (desire.HasValue && (double) desire.GetValueOrDefault() >= (double) statusProfile.LesbianBorderDesire);
        int lesbianBorder = Singleton<Resources>.Instance.StatusProfile.LesbianBorder;
        if (flag)
        {
          agent.Partner = this._partner;
          commandPartner.Partner = (Actor) agent;
          agent.ChangeBehavior(Desire.ActionType.GotoLesbianSpot);
          commandPartner.BehaviorResources.ChangeMode(Desire.ActionType.GotoLesbianSpotFollow);
        }
        else
        {
          agent.ChangeBehavior(Desire.ActionType.Normal);
          commandPartner.ChangeBehavior(Desire.ActionType.Normal);
        }
      }
      else if (agent.CommandPartner is MerchantActor)
      {
        int num1;
        if (!Singleton<Resources>.Instance.MerchantProfile.ResultAddFriendlyRelationShipTable.TryGetValue(Merchant.ActionType.TalkWithAgent, out num1))
          num1 = 0;
        int num2 = this._relationShip + num1;
        agent.AgentData.FriendlyRelationShipTable[agent.CommandPartner.ID] = Mathf.Clamp(num2, 0, 100);
        agent.ApplySituationResultParameter(25);
        MerchantActor commandPartner = agent.CommandPartner as MerchantActor;
        StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
        int desireKey2 = Desire.GetDesireKey(Desire.Type.H);
        float? desire = agent.GetDesire(desireKey2);
        ChaFileGameInfo fileGameInfo = agent.ChaControl.fileGameInfo;
        int num3 = fileGameInfo.flavorState[4];
        int num4 = fileGameInfo.flavorState[7];
        if (statusProfile.LesbianBorder <= num3 && statusProfile.GirlsActionBorder <= num4 && (desire.HasValue && (double) statusProfile.LesbianBorderDesire <= (double) desire.GetValueOrDefault()))
        {
          agent.Partner = this._partner;
          this._partner.Partner = (Actor) agent;
          agent.ChangeBehavior(Desire.ActionType.GotoLesbianSpot);
          commandPartner.ChangeBehavior(Merchant.ActionType.GotoLesbianSpotFollow);
        }
        else
        {
          agent.ChangeBehavior(Desire.ActionType.Normal);
          commandPartner.ChangeBehavior(commandPartner.LastNormalMode);
        }
      }
      int desireKey3 = Desire.GetDesireKey(Desire.Type.Lonely);
      agent.SetDesire(desireKey3, 0.0f);
      agent.TargetInSightActor = (Actor) null;
      agent.CommandPartner = (Actor) null;
    }
  }
}
