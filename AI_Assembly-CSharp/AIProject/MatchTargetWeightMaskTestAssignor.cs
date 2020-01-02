// Decompiled with JetBrains decompiler
// Type: AIProject.MatchTargetWeightMaskTestAssignor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject
{
  public class MatchTargetWeightMaskTestAssignor : MonoBehaviour
  {
    [Header("UI")]
    [SerializeField]
    private InputField _stateNameInput;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Button _addButton;
    [SerializeField]
    private Button _subButton;
    [SerializeField]
    private Text[] _isMatchingTargetOutputs;
    [Header("Matching Target Objects")]
    [SerializeField]
    private List<TestChara> _charaList;
    private List<ChaControl> _chaCtrlList;
    private List<MatchTargetWeightMaskTester> _testScripts;
    [SerializeField]
    private StringReactiveProperty _stateName;
    [SerializeField]
    private MatchTargetWeightMaskTestAssignor.TargetParameter[] _targets;
    [SerializeField]
    private Vector3ReactiveProperty _positionWeight;
    [SerializeField]
    private FloatReactiveProperty _rotationWeight;
    private Transform _charaRoot;

    public MatchTargetWeightMaskTestAssignor()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this._charaRoot = new GameObject("root_Chara").get_transform();
      foreach (TestChara chara in this._charaList)
      {
        if (Object.op_Inequality((Object) chara, (Object) null))
        {
          ChaControl chaControl = chara.Sex != (byte) 0 ? Singleton<Character>.Instance.CreateChara((byte) 1, ((Component) this._charaRoot).get_gameObject(), 0, (ChaFileControl) null) : Singleton<Character>.Instance.CreateChara((byte) 0, ((Component) this._charaRoot).get_gameObject(), 0, (ChaFileControl) null);
          chaControl.Load(false);
          chaControl.ChangeLookEyesPtn(3);
          chaControl.ChangeLookNeckPtn(3, 1f);
          MatchTargetWeightMaskTester weightMaskTester = (MatchTargetWeightMaskTester) ((Component) chaControl.animBody).get_gameObject().AddComponent<MatchTargetWeightMaskTester>();
          weightMaskTester.Animator = chaControl.animBody;
          weightMaskTester.Animator.set_runtimeAnimatorController(chara.Rac);
          weightMaskTester.StateName = ((ReactiveProperty<string>) this._stateName).get_Value();
          weightMaskTester.Targets = new MatchTargetWeightMaskTester.TargetParameter[this._targets.Length];
          for (int index = 0; index < this._targets.Length; ++index)
          {
            MatchTargetWeightMaskTester.TargetParameter targetParameter = weightMaskTester.Targets[index] ?? (weightMaskTester.Targets[index] = new MatchTargetWeightMaskTester.TargetParameter());
            targetParameter.Start = this._targets[index].Start;
            targetParameter.End = this._targets[index].End;
            targetParameter.Target = this._targets[index].Target;
          }
          weightMaskTester.PositionWeight = ((ReactiveProperty<Vector3>) this._positionWeight).get_Value();
          weightMaskTester.RotationWeight = ((ReactiveProperty<float>) this._rotationWeight).get_Value();
          this._chaCtrlList.Add(chaControl);
          this._testScripts.Add(weightMaskTester);
        }
      }
      ObservableExtensions.Subscribe<string>((IObservable<M0>) this._stateName, (Action<M0>) (x =>
      {
        if (!Object.op_Inequality((Object) this._stateNameInput, (Object) null))
          return;
        this._stateNameInput.set_text(x);
      }));
      ObservableExtensions.Subscribe<Vector3>((IObservable<M0>) this._positionWeight, (Action<M0>) (x =>
      {
        foreach (MatchTargetWeightMaskTester testScript in this._testScripts)
        {
          if (Object.op_Inequality((Object) testScript, (Object) null))
            testScript.PositionWeight = x;
        }
      }));
      ObservableExtensions.Subscribe<float>((IObservable<M0>) this._rotationWeight, (Action<M0>) (x =>
      {
        foreach (MatchTargetWeightMaskTester testScript in this._testScripts)
        {
          if (Object.op_Inequality((Object) testScript, (Object) null))
            testScript.RotationWeight = x;
        }
      }));
      if (Object.op_Inequality((Object) this._stateNameInput, (Object) null))
      {
        this._stateNameInput.set_text(((ReactiveProperty<string>) this._stateName).get_Value());
        // ISSUE: method pointer
        ((UnityEvent<string>) this._stateNameInput.get_onValueChanged()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CStart\u003Em__3)));
      }
      if (Object.op_Inequality((Object) this._button, (Object) null))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._button.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
      }
      else
        Debug.LogError((object) "ボタン（UI）が未設定", (Object) this);
      if (Object.op_Inequality((Object) this._addButton, (Object) null))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._addButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__5)));
      }
      if (!Object.op_Inequality((Object) this._subButton, (Object) null))
        return;
      // ISSUE: method pointer
      ((UnityEvent) this._subButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__6)));
    }

    private void Update()
    {
      if (this._isMatchingTargetOutputs == null)
        return;
      for (int index1 = 0; index1 < this._testScripts.Count; ++index1)
      {
        MatchTargetWeightMaskTester testScript = this._testScripts[index1];
        if (Object.op_Inequality((Object) testScript, (Object) null))
        {
          this._isMatchingTargetOutputs[index1].set_text(string.Format("{0:00}: isMatchingTarget = {1}", (object) index1, (object) testScript.Animator.get_isMatchingTarget()));
          if (testScript.Targets.Length != this._targets.Length)
            testScript.Targets = new MatchTargetWeightMaskTester.TargetParameter[this._targets.Length];
          for (int index2 = 0; index2 < this._targets.Length; ++index2)
          {
            MatchTargetWeightMaskTestAssignor.TargetParameter target1 = this._targets[index2];
            MatchTargetWeightMaskTester.TargetParameter target2 = testScript.Targets[index2];
            if (target1 != null && target2 != null)
            {
              target2.Start = target1.Start;
              target2.End = target1.End;
              target2.Target = target1.Target;
            }
          }
        }
      }
    }

    [Serializable]
    private class TargetParameter
    {
      [SerializeField]
      private float _end = 1f;
      [SerializeField]
      private float _start;
      [SerializeField]
      private Transform _target;

      public float Start
      {
        get
        {
          return this._start;
        }
      }

      public float End
      {
        get
        {
          return this._end;
        }
      }

      public Transform Target
      {
        get
        {
          return this._target;
        }
        set
        {
          this._target = value;
        }
      }
    }
  }
}
