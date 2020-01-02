// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables.SharedGameObjectsToGameObjectList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
  [TaskCategory("Unity/SharedVariable")]
  [TaskDescription("Sets the SharedGameObjectList values from the GameObjects. Returns Success.")]
  public class SharedGameObjectsToGameObjectList : Action
  {
    [Tooltip("The GameObjects value")]
    public SharedGameObject[] gameObjects;
    [RequiredField]
    [Tooltip("The SharedTransformList to set")]
    public SharedGameObjectList storedGameObjectList;

    public SharedGameObjectsToGameObjectList()
    {
      base.\u002Ector();
    }

    public virtual void OnAwake()
    {
      this.storedGameObjectList.set_Value(new List<GameObject>());
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.gameObjects == null || this.gameObjects.Length == 0)
        return (TaskStatus) 1;
      this.storedGameObjectList.get_Value().Clear();
      for (int index = 0; index < this.gameObjects.Length; ++index)
        this.storedGameObjectList.get_Value().Add(this.gameObjects[index].get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.gameObjects = (SharedGameObject[]) null;
      this.storedGameObjectList = (SharedGameObjectList) null;
    }
  }
}
