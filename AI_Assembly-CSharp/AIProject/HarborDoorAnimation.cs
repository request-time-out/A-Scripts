// Decompiled with JetBrains decompiler
// Type: AIProject.HarborDoorAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public class HarborDoorAnimation : MonoBehaviour
  {
    [SerializeField]
    private int _linkID;
    [SerializeField]
    private PoseKeyPair _poseInfo;
    [SerializeField]
    private Transform _basePoint;
    [SerializeField]
    private Transform _recoveryPoint;
    private Animator _doorAnimator;

    public HarborDoorAnimation()
    {
      base.\u002Ector();
    }

    public int LinkID
    {
      get
      {
        return this._linkID;
      }
    }

    public PoseKeyPair PoseInfo
    {
      get
      {
        return this._poseInfo;
      }
    }

    public Transform BasePoint
    {
      get
      {
        return this._basePoint;
      }
    }

    public Transform RecoveryPoint
    {
      get
      {
        return this._recoveryPoint;
      }
    }

    public HarborDoorAnimData AnimData { get; protected set; }

    private void Start()
    {
      HarborDoorAnimData harborDoorAnimData = (HarborDoorAnimData) null;
      bool? nullable = HarborDoorAnimData.Table != null ? new bool?(HarborDoorAnimData.Table.TryGetValue(this._linkID, ref harborDoorAnimData)) : new bool?();
      if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) == 0 || Object.op_Equality((Object) harborDoorAnimData, (Object) null))
        return;
      this.AnimData = harborDoorAnimData;
      this._doorAnimator = harborDoorAnimData.DoorAnimator;
    }
  }
}
