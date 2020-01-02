// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.StateCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.Animal
{
  public class StateCondition
  {
    private List<OneState> nextState = new List<OneState>();
    private List<OneState> nextStateTemp = new List<OneState>();

    public StateCondition()
    {
      this.conditionType = ConditionType.None;
      this.animalState = AnimalState.None;
    }

    public StateCondition(ConditionType _type, AnimalState _state)
    {
      this.conditionType = _type;
      this.animalState = _state;
    }

    public StateCondition(StateCondition _c)
    {
      this.conditionType = _c.conditionType;
      this.animalState = _c.animalState;
      this.nextState.Clear();
      this.nextState.AddRange((IEnumerable<OneState>) _c.nextState);
      this.nextStateTemp.Clear();
    }

    public ConditionType conditionType { get; protected set; }

    public AnimalState animalState { get; protected set; }

    public OneState this[int index]
    {
      get
      {
        return this.nextState[index];
      }
      set
      {
        this.nextState[index] = value;
      }
    }

    public int Count
    {
      get
      {
        return this.nextState.Count;
      }
    }

    public OneState FirstElement
    {
      get
      {
        return this.Count == 0 ? new OneState() : this.nextState[0];
      }
    }

    public void AddNextState(AnimalState _state, float _proportion = 0.0f)
    {
      this.AddNextState(new OneState(_state, _proportion));
    }

    public void AddNextState(OneState _state)
    {
      if (_state.state == AnimalState.None)
        return;
      OneState oneState = this.nextState.Find((Predicate<OneState>) (x => x.state == _state.state));
      if (oneState.state == AnimalState.None)
        this.nextState.Add(_state);
      else
        this.nextState[this.nextState.IndexOf(oneState)] = _state;
    }

    public void SetNextState(List<OneState> _nextState)
    {
      this.nextState.Clear();
      this.nextState.AddRange((IEnumerable<OneState>) _nextState);
    }

    public void RemoveNextState(AnimalState _state)
    {
      this.nextState.RemoveAll((Predicate<OneState>) (x => x.state == _state));
    }

    public void RemoveNextState(OneState _state)
    {
      this.nextState.RemoveAll((Predicate<OneState>) (x => x.Equal(_state)));
    }

    public OneState GetRandomState()
    {
      return ((IReadOnlyList<OneState>) this.nextState).IsNullOrEmpty<OneState>() ? new OneState() : this.nextState[Random.Range(0, this.nextState.Count)];
    }

    public OneState GetProportionState()
    {
      if (((IReadOnlyList<OneState>) this.nextState).IsNullOrEmpty<OneState>())
        return new OneState();
      float num1 = 0.0f;
      this.nextStateTemp.Clear();
      this.nextStateTemp.AddRange((IEnumerable<OneState>) this.nextState);
      for (int index = 0; index < this.nextStateTemp.Count; ++index)
      {
        num1 += this.nextStateTemp[index].proportion;
        OneState oneState = this.nextStateTemp[index];
        oneState.proportion = num1;
        this.nextStateTemp[index] = oneState;
      }
      float num2 = Random.Range(0.0f, num1);
      for (int index = 0; index < this.nextStateTemp.Count; ++index)
      {
        if ((double) num2 <= (double) this.nextStateTemp[index].proportion)
          return this.nextStateTemp[index];
      }
      return new OneState();
    }

    public OneState GetNonOverlapState()
    {
      if (((IReadOnlyList<OneState>) this.nextState).IsNullOrEmpty<OneState>())
        return new OneState();
      int index = Random.Range(0, this.Count);
      OneState oneState = this.nextState[index];
      this.nextState.RemoveAt(index);
      return oneState;
    }

    public List<OneState> AllState(bool _copy = true)
    {
      return _copy ? new List<OneState>((IEnumerable<OneState>) this.nextState) : this.nextState;
    }
  }
}
