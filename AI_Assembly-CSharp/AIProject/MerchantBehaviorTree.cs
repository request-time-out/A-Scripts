// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehaviorTree
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AIProject
{
  public class MerchantBehaviorTree : Behavior
  {
    [SerializeField]
    [Tooltip("行動管理する商人")]
    private MerchantActor _sourceMerchant;
    private ExternalBehavior _externalBehavior;
    private bool _initialized;
    private bool _isPaused;

    public MerchantBehaviorTree()
    {
      base.\u002Ector();
    }

    public MerchantActor SourceMerchant
    {
      get
      {
        return this._sourceMerchant;
      }
      set
      {
        this._sourceMerchant = value;
      }
    }

    public ExternalBehavior ExternalBehavior
    {
      get
      {
        return this._externalBehavior;
      }
      set
      {
        if (Object.op_Equality((Object) this._externalBehavior, (Object) value))
          return;
        if (Object.op_Inequality((Object) BehaviorManager.instance, (Object) null))
          ((BehaviorManager) BehaviorManager.instance).DisableBehavior((Behavior) this);
        BehaviorSource behaviorSource1 = this.GetBehaviorSource();
        if (Object.op_Inequality((Object) value, (Object) null) && value.get_Initialized())
        {
          List<SharedVariable> allVariables = behaviorSource1.GetAllVariables();
          BehaviorSource behaviorSource2 = value.get_BehaviorSource();
          behaviorSource2.set_HasSerialized(true);
          if (allVariables != null)
          {
            for (int index = 0; index < allVariables.Count; ++index)
            {
              if (allVariables[index] != null)
                behaviorSource2.SetVariable(allVariables[index].get_Name(), allVariables[index]);
            }
          }
        }
        else
        {
          behaviorSource1.set_HasSerialized(false);
          this.set_HasInheritedVariables(false);
        }
        this._initialized = false;
        this._externalBehavior = value;
        if (!this.get_StartWhenEnabled())
          return;
        this.EnableBehavior();
      }
    }

    private void Start()
    {
      if (this.get_StartWhenEnabled())
        this.set_StartWhenEnabled(false);
      base.Start();
    }

    public void OnEnable()
    {
      if (Object.op_Inequality((Object) BehaviorManager.instance, (Object) null) && this._isPaused)
      {
        ((BehaviorManager) BehaviorManager.instance).EnableBehavior((Behavior) this);
        this._isPaused = false;
      }
      else
      {
        if (!this.get_StartWhenEnabled() || !this._initialized)
          return;
        this.EnableBehavior();
      }
    }

    public static AIBehaviorManager CreateAIBehaviorManager()
    {
      if (!Object.op_Equality((Object) BehaviorManager.instance, (Object) null) || !Application.get_isPlaying())
        return (AIBehaviorManager) null;
      GameObject gameObject = new GameObject();
      ((Object) gameObject).set_name("Behavior Manager");
      return (AIBehaviorManager) gameObject.AddComponent<AIBehaviorManager>();
    }

    public void EnableBehavior()
    {
      MerchantBehaviorTree.CreateAIBehaviorManager();
      if (Object.op_Inequality((Object) BehaviorManager.instance, (Object) null))
        ((BehaviorManager) BehaviorManager.instance).EnableBehavior((Behavior) this);
      if (this._initialized)
        return;
      BehaviorSource behaviorSource = this.GetBehaviorSource();
      for (int index = 0; index < 12; ++index)
        this.get_HasEvent()[index] = this.TaskContainsMethod(((Behavior.EventTypes) index).ToString(), behaviorSource.get_RootTask());
      this._initialized = true;
    }

    private bool TaskContainsMethod(string methodName, Task task)
    {
      if (task == null)
        return false;
      MethodInfo method = ((object) task).GetType().GetMethod(methodName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (method != (MethodInfo) null && method.DeclaringType.IsAssignableFrom(((object) task).GetType()))
        return true;
      if (task is ParentTask)
      {
        ParentTask parentTask = task as ParentTask;
        if (parentTask.get_Children() != null)
        {
          for (int index = 0; index < parentTask.get_Children().Count; ++index)
          {
            if (this.TaskContainsMethod(methodName, parentTask.get_Children()[index]))
              return true;
          }
        }
      }
      return false;
    }
  }
}
