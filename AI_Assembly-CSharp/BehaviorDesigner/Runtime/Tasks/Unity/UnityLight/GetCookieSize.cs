// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityLight.GetCookieSize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLight
{
  [TaskCategory("Unity/Light")]
  [TaskDescription("Stores the light's cookie size.")]
  public class GetCookieSize : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [RequiredField]
    [Tooltip("The size to store")]
    public SharedFloat storeValue;
    private Light light;
    private GameObject prevGameObject;

    public GetCookieSize()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      GameObject defaultGameObject = ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value());
      if (!Object.op_Inequality((Object) defaultGameObject, (Object) this.prevGameObject))
        return;
      this.light = (Light) defaultGameObject.GetComponent<Light>();
      this.prevGameObject = defaultGameObject;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (Object.op_Equality((Object) this.light, (Object) null))
      {
        Debug.LogWarning((object) "Light is null");
        return (TaskStatus) 1;
      }
      this.storeValue = (SharedFloat) this.light.get_cookieSize();
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.storeValue = (SharedFloat) 0.0f;
    }
  }
}
