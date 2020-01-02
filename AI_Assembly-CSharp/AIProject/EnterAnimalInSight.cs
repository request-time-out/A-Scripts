// Decompiled with JetBrains decompiler
// Type: AIProject.EnterAnimalInSight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class EnterAnimalInSight : AgentConditional
  {
    [SerializeField]
    private AnimalTypes targetAnimalType;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Agent.TargetInSightAnimal = (AnimalBase) null;
    }

    public virtual TaskStatus OnUpdate()
    {
      this.CheckAnimal();
      return Object.op_Inequality((Object) this.Agent.TargetInSightAnimal, (Object) null) ? (TaskStatus) 2 : (TaskStatus) 1;
    }

    private void CheckAnimal()
    {
      List<AnimalBase> animalBaseList = ListPool<AnimalBase>.Get();
      foreach (AnimalBase targetAnimal in this.Agent.TargetAnimals)
      {
        CollisionState collisionState;
        if ((targetAnimal.AnimalType & this.targetAnimalType) != (AnimalTypes) 0 && this.Agent.AnimalCollisionStateTable.TryGetValue(targetAnimal.InstanceID, out collisionState) && (targetAnimal.IsWithAgentFree(this.Agent) && collisionState == CollisionState.Enter))
          animalBaseList.Add(targetAnimal);
      }
      if (0 < animalBaseList.Count)
      {
        AnimalBase capturedInSight = this.GetCapturedInSight(this.Agent, animalBaseList);
        if (Object.op_Equality((Object) capturedInSight, (Object) null))
        {
          ListPool<AnimalBase>.Release(animalBaseList);
          return;
        }
        this.Agent.TargetInSightAnimal = capturedInSight;
      }
      ListPool<AnimalBase>.Release(animalBaseList);
    }

    private AnimalBase GetCapturedInSight(AgentActor _agent, List<AnimalBase> _animals)
    {
      AnimalBase element = _animals.GetElement<AnimalBase>(Random.Range(0, _animals.Count));
      if (Object.op_Equality((Object) element, (Object) null))
        return (AnimalBase) null;
      return this.IsCaptureInSight(_agent, element) ? element : (AnimalBase) null;
    }

    private bool IsCaptureInSight(AgentActor _agent, AnimalBase _target)
    {
      if (Object.op_Equality((Object) _target, (Object) null))
        return false;
      Vector3 position1 = _agent.FovTargetPointTable.get_Item(Actor.FovBodyPart.Head).get_position();
      int layer = LayerMask.NameToLayer("Map");
      Vector3 position2 = _target.Position;
      Vector3 vector3 = Vector3.op_Subtraction(position1, position2);
      Ray ray;
      ((Ray) ref ray).\u002Ector(position1, vector3);
      return !Physics.Raycast(ray, ((Vector3) ref vector3).get_magnitude(), 1 << layer);
    }
  }
}
