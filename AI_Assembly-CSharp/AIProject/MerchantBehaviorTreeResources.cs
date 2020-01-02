// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehaviorTreeResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class MerchantBehaviorTreeResources : SerializedMonoBehaviour
  {
    [SerializeField]
    [HideInInspector]
    private MerchantBehaviorTree currentTree;
    [SerializeField]
    private MerchantActor sourceMerchant;
    [SerializeField]
    private Dictionary<Merchant.ActionType, MerchantBehaviorTree> behaviorTreeTable;

    public MerchantBehaviorTreeResources()
    {
      base.\u002Ector();
    }

    public MerchantBehaviorTree CurrentTree
    {
      get
      {
        return this.currentTree;
      }
    }

    public Merchant.ActionType Mode { get; private set; }

    public MerchantActor SourceMerchant
    {
      get
      {
        return this.sourceMerchant;
      }
    }

    public MerchantBehaviorTree GetBehaviorTree(Merchant.ActionType _mode)
    {
      MerchantBehaviorTree merchantBehaviorTree = (MerchantBehaviorTree) null;
      return this.behaviorTreeTable.TryGetValue(_mode, out merchantBehaviorTree) ? merchantBehaviorTree : (MerchantBehaviorTree) null;
    }

    public void Initialize()
    {
      if (this.behaviorTreeTable == null)
        this.behaviorTreeTable = new Dictionary<Merchant.ActionType, MerchantBehaviorTree>();
      foreach (Merchant.ActionType actionType in Enum.GetValues(typeof (Merchant.ActionType)))
      {
        MerchantBehaviorTree merchantBehavior = Singleton<Resources>.Instance.BehaviorTree.GetMerchantBehavior(actionType);
        if (!Object.op_Equality((Object) merchantBehavior, (Object) null))
        {
          MerchantBehaviorTree merchantBehaviorTree = (MerchantBehaviorTree) Object.Instantiate<MerchantBehaviorTree>((M0) merchantBehavior);
          ((Component) merchantBehaviorTree).get_transform().SetParent(((Component) this).get_transform(), false);
          ((Component) merchantBehaviorTree).get_transform().SetPositionAndRotation(Vector3.get_zero(), Quaternion.get_identity());
          this.behaviorTreeTable[actionType] = merchantBehaviorTree;
          merchantBehaviorTree.SourceMerchant = this.sourceMerchant;
        }
      }
    }

    public bool IsMatchCurrentTree(Merchant.ActionType _mode)
    {
      MerchantBehaviorTree merchantBehaviorTree = (MerchantBehaviorTree) null;
      this.behaviorTreeTable.TryGetValue(_mode, out merchantBehaviorTree);
      return Object.op_Equality((Object) this.currentTree, (Object) merchantBehaviorTree);
    }

    public void ChangeMode(Merchant.ActionType _mode)
    {
      MerchantBehaviorTree merchantBehaviorTree = (MerchantBehaviorTree) null;
      if (!this.behaviorTreeTable.TryGetValue(_mode, out merchantBehaviorTree) || Object.op_Equality((Object) merchantBehaviorTree, (Object) null))
        return;
      this.Mode = _mode;
      if (this.currentTree != null)
        this.currentTree.DisableBehavior(false);
      merchantBehaviorTree.DisableBehavior(false);
      this.currentTree = merchantBehaviorTree;
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ => this.currentTree.EnableBehavior()));
    }

    private void OnEnable()
    {
      if (this.currentTree == null)
        return;
      this.currentTree.EnableBehavior();
    }

    private void OnDisable()
    {
      if (this.currentTree == null)
        return;
      this.currentTree.DisableBehavior(true);
    }

    public void StopBehaviorTree()
    {
      if (!Object.op_Inequality((Object) this.currentTree, (Object) null))
        return;
      this.currentTree.DisableBehavior(false);
    }

    [Button("TreeにSourceMerchantを反映")]
    private void AttachSourceMarchant()
    {
      if (this.behaviorTreeTable.IsNullOrEmpty<Merchant.ActionType, MerchantBehaviorTree>())
        return;
      foreach (MerchantBehaviorTree merchantBehaviorTree in this.behaviorTreeTable.Values)
      {
        if (!Object.op_Equality((Object) merchantBehaviorTree, (Object) null))
          merchantBehaviorTree.SourceMerchant = this.sourceMerchant;
      }
    }

    [Button("TreeTableをリフレッシュ")]
    [HideInPlayMode]
    private void RefreshTreeTable()
    {
      if (this.behaviorTreeTable == null)
        this.behaviorTreeTable = new Dictionary<Merchant.ActionType, MerchantBehaviorTree>();
      this.behaviorTreeTable.Clear();
      foreach (Merchant.ActionType index in Enum.GetValues(typeof (Merchant.ActionType)))
        this.behaviorTreeTable[index] = (MerchantBehaviorTree) null;
    }
  }
}
