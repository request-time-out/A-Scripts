// Decompiled with JetBrains decompiler
// Type: AIProject.JointPoser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ReMotion;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class JointPoser : Poser
  {
    [SerializeField]
    private JointPoser.JointData _jointData;
    private Transform _poseRoot;
    private Transform[] _children;
    private Transform[] _poseChildren;
    private Vector3[] _defaultLocalPositions;
    private Quaternion[] _defaultLocalRotations;

    public JointPoser()
    {
      base.\u002Ector();
    }

    public bool LeftRight { get; set; }

    private void OnEnable()
    {
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.5f, false), false), (Action<M0>) (x => this.weight = (__Null) (double) ((TimeInterval<float>) ref x).get_Value()));
    }

    private void OnDisable()
    {
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.5f, false), false), (Action<M0>) (x => this.weight = (__Null) (1.0 - (double) ((TimeInterval<float>) ref x).get_Value())));
    }

    public void LoadJointData(string assetbundleName, string assetName)
    {
      this._jointData = AssetUtility.LoadAsset<JointPoser.JointData>(assetbundleName, assetName, string.Empty);
    }

    protected virtual void InitiatePoser()
    {
      this._children = ((IEnumerable<Transform>) ((Component) this).GetComponentsInChildren<Transform>(true)).Where<Transform>((Func<Transform, bool>) (x => ((IEnumerable<string>) this._jointData.NullList).Contains<string>(((Object) x).get_name()))).ToArray<Transform>();
      this._defaultLocalPositions = new Vector3[this._children.Length];
      this._defaultLocalRotations = new Quaternion[this._children.Length];
      for (int index = 0; index < this._children.Length; ++index)
      {
        this._defaultLocalPositions[index] = this._children[index].get_localPosition();
        this._defaultLocalRotations[index] = this._children[index].get_localRotation();
      }
    }

    public virtual void AutoMapping()
    {
      this._poseChildren = !Object.op_Equality((Object) this.poseRoot, (Object) null) ? ((IEnumerable<Transform>) ((Component) this.poseRoot).GetComponentsInChildren<Transform>(true)).Where<Transform>((Func<Transform, bool>) (x => ((IEnumerable<string>) this._jointData.NullList).Contains<string>(((Object) x).get_name()))).ToArray<Transform>() : new Transform[0];
      this._poseRoot = (Transform) this.poseRoot;
    }

    protected virtual void FixPoserTransforms()
    {
      for (int index = 0; index < this._children.Length; ++index)
      {
        this._children[index].set_localPosition(this._defaultLocalPositions[index]);
        this._children[index].set_localRotation(this._defaultLocalRotations[index]);
      }
    }

    protected virtual void UpdatePoser()
    {
      if (this.weight <= 0.0 || this.localPositionWeight <= 0.0 && this.localRotationWeight <= 0.0)
        return;
      if (Object.op_Inequality((Object) this._poseRoot, (Object) this.poseRoot))
        base.AutoMapping();
      if (Object.op_Equality((Object) this.poseRoot, (Object) null))
        return;
      if (this._children.Length != this._poseChildren.Length)
      {
        Debug.LogWarning((object) "Number of children does not match with the pose", (Object) ((Component) this).get_transform());
      }
      else
      {
        float num1 = (float) (this.localRotationWeight * this.weight);
        float num2 = (float) (this.localPositionWeight * this.weight);
        for (int index = 0; index < this._children.Length; ++index)
        {
          if (Object.op_Inequality((Object) this._children[index], (Object) ((Component) this).get_transform()))
          {
            this._children[index].set_localRotation(Quaternion.Lerp(this._children[index].get_localRotation(), this._poseChildren[index].get_localRotation(), num1));
            this._children[index].set_localPosition(Vector3.Lerp(this._children[index].get_localPosition(), this._poseChildren[index].get_localPosition(), num2));
          }
        }
      }
    }

    public class JointData : ScriptableObject
    {
      [SerializeField]
      private string[] _nullList;

      public JointData()
      {
        base.\u002Ector();
      }

      public string[] NullList
      {
        get
        {
          return this._nullList;
        }
      }
    }
  }
}
