// Decompiled with JetBrains decompiler
// Type: AIProject.RandomSearchArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;

namespace AIProject
{
  public class RandomSearchArea : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      List<SearchAreaProbabilities.AddProb> addProbList = ListPool<SearchAreaProbabilities.AddProb>.Get();
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      if (agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(10))
        addProbList.Add(new SearchAreaProbabilities.AddProb(6, statusProfile.FishingSearchProbBuff));
      if (agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(18))
        addProbList.Add(new SearchAreaProbabilities.AddProb(0, statusProfile.HandSearchProbBuff));
      if (agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(42))
        addProbList.Add(new SearchAreaProbabilities.AddProb(4, statusProfile.PickelSearchProbBuff));
      if (agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(47))
        addProbList.Add(new SearchAreaProbabilities.AddProb(3, statusProfile.ShovelSearchProbBuff));
      if (agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(48))
        addProbList.Add(new SearchAreaProbabilities.AddProb(5, statusProfile.NetSearchProbBuff));
      Dictionary<int, bool> dictionary = DictionaryPool<int, bool>.Get();
      dictionary[0] = true;
      dictionary[3] = agent.AgentData.EquipedShovelItem.ID > -1;
      dictionary[4] = agent.AgentData.EquipedPickelItem.ID > -1;
      dictionary[5] = agent.AgentData.EquipedPickelItem.ID > -1;
      dictionary[6] = agent.AgentData.EquipedFishingItem.ID > -1;
      agent.SearchAreaID = addProbList.Count <= 0 ? Singleton<Resources>.Instance.CommonDefine.ProbSearchAreaProfile.Lottery(dictionary, (List<SearchAreaProbabilities.AddProb>) null) : Singleton<Resources>.Instance.CommonDefine.ProbSearchAreaProfile.Lottery(dictionary, addProbList);
      DictionaryPool<int, bool>.Release(dictionary);
      ListPool<SearchAreaProbabilities.AddProb>.Release(addProbList);
      return (TaskStatus) 2;
    }
  }
}
