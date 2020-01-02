// Decompiled with JetBrains decompiler
// Type: AIProject.PetAnimal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
using AIProject.Animal.Resources;
using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class PetAnimal : AgentAction
  {
    private AnimalBase animal;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Agent.RuntimeDesire = Desire.Type.Animal;
      this.Agent.ChangeStaticNavMeshAgentAvoidance();
      this.animal = this.Agent.TargetInSightAnimal;
      if (Object.op_Equality((Object) this.animal, (Object) null))
        return;
      this.animal.SetSearchTargetEnabled(false, false);
      this.Agent.StateType = AIProject.Definitions.State.Type.Immobility;
      this.animal.BackupState = this.animal.CurrentState;
      Dictionary<int, AnimalPlayState> source1;
      if (!Singleton<Manager.Resources>.Instance.AnimalTable.WithAgentAnimeTable.TryGetValue(this.animal.AnimalTypeID, out source1) || source1.IsNullOrEmpty<int, AnimalPlayState>())
        return;
      List<int> source2 = ListPool<int>.Get();
      source2.AddRange((IEnumerable<int>) source1.Keys);
      this.Agent.StartWithAnimalSequence(this.animal, source2.GetElement<int>(Random.Range(0, source2.Count)));
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.Agent.LivesWithAnimalSequence)
        return (TaskStatus) 3;
      this.End();
      return (TaskStatus) 2;
    }

    private void End()
    {
      int desireKey = Desire.GetDesireKey(this.Agent.RuntimeDesire);
      if (desireKey != -1)
        this.Agent.SetDesire(desireKey, 0.0f);
      this.Agent.RuntimeDesire = Desire.Type.None;
      if (this.animal is IPetAnimal animal)
      {
        AIProject.SaveData.AnimalData animalData = animal.AnimalData;
        if (animalData != null)
        {
          double num = (double) animalData.AddFavorability(this.Agent.ID, 1f);
        }
      }
      this.ReleaseAnimal();
    }

    public virtual void OnEnd()
    {
      this.ReleaseAnimal();
      this.Agent.ClearItems();
      this.Agent.StopWithAnimalSequence();
      this.Agent.ChangeDynamicNavMeshAgentAvoidance();
      this.Agent.StateType = AIProject.Definitions.State.Type.Normal;
      ((Task) this).OnEnd();
    }

    public virtual void OnBehaviorComplete()
    {
      this.ReleaseAnimal();
      ((Task) this).OnBehaviorComplete();
    }

    private void ReleaseAnimal()
    {
      if (!Object.op_Inequality((Object) this.animal, (Object) null))
        return;
      this.animal.SetImpossible(false, (Actor) this.Agent);
      if (Object.op_Equality((Object) this.Agent.TargetInSightAnimal, (Object) this.animal))
      {
        this.Agent.TargetInSightAnimal = (AnimalBase) null;
        this.animal.SetSearchTargetEnabled(true, false);
      }
      this.animal = (AnimalBase) null;
    }
  }
}
