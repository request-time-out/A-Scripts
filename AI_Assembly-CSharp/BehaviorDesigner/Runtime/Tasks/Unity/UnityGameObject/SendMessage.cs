// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject.SendMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
  [TaskCategory("Unity/GameObject")]
  [TaskDescription("Sends a message to the target GameObject. Returns Success.")]
  public class SendMessage : Action
  {
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The message to send")]
    public SharedString message;
    [Tooltip("The value to send")]
    public SharedGenericVariable value;

    public SendMessage()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (((SharedVariable<GenericVariable>) this.value).get_Value() != null)
        ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).SendMessage(this.message.get_Value(), ((SharedVariable) ((SharedVariable<GenericVariable>) this.value).get_Value().value).GetValue());
      else
        ((Task) this).GetDefaultGameObject(this.targetGameObject.get_Value()).SendMessage(this.message.get_Value());
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.targetGameObject = (SharedGameObject) null;
      this.message = (SharedString) string.Empty;
    }
  }
}
