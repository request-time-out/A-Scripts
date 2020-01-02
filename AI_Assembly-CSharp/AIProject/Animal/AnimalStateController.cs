// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalStateController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.Animal
{
  public class AnimalStateController
  {
    protected Dictionary<AnimalState, StateCondition> StateConditionTable = new Dictionary<AnimalState, StateCondition>();
    protected Dictionary<AnimalState, StateCondition> StateNonOverlapTable = new Dictionary<AnimalState, StateCondition>();
    public Dictionary<AnimalState, List<ActionTypes>> TargetActionTable = new Dictionary<AnimalState, List<ActionTypes>>();

    public AnimalStateController()
    {
    }

    public AnimalStateController(AnimalBase _animal)
    {
      this.Initialize(_animal);
    }

    public AnimalBase Animal { get; private set; }

    public AnimalTypes AnimalType
    {
      get
      {
        return Object.op_Inequality((Object) this.Animal, (Object) null) ? this.Animal.AnimalType : (AnimalTypes) 0;
      }
    }

    public BreedingTypes BreedingType
    {
      get
      {
        return Object.op_Inequality((Object) this.Animal, (Object) null) ? this.Animal.BreedingType : BreedingTypes.Wild;
      }
    }

    protected Resources.AnimalTables AnimalTable
    {
      get
      {
        return Singleton<Resources>.IsInstance() ? Singleton<Resources>.Instance.AnimalTable : (Resources.AnimalTables) null;
      }
    }

    public void Initialize(AnimalBase _animal)
    {
      this.Clear();
      this.Animal = _animal;
      if (Object.op_Equality((Object) this.Animal, (Object) null))
        return;
      this.TableSetting();
    }

    protected void TableSetting(AnimalTypes _animalType, BreedingTypes _breedingType)
    {
      this.StateConditionTable.Clear();
      this.StateNonOverlapTable.Clear();
      Dictionary<AnimalState, StateCondition> dictionary1;
      if (this.AnimalTable.StateConditionTable.ContainsKey(_animalType) && this.AnimalTable.StateConditionTable[_animalType].TryGetValue(_breedingType, out dictionary1))
      {
        foreach (KeyValuePair<AnimalState, StateCondition> keyValuePair in dictionary1)
          this.StateConditionTable[keyValuePair.Key] = new StateCondition(keyValuePair.Value);
      }
      this.TargetActionTable.Clear();
      Dictionary<AnimalState, List<ActionTypes>> dictionary2;
      if (!this.AnimalTable.StateTargetActionTable.ContainsKey(_animalType) || !this.AnimalTable.StateTargetActionTable[_animalType].TryGetValue(_breedingType, out dictionary2))
        return;
      foreach (KeyValuePair<AnimalState, List<ActionTypes>> keyValuePair in dictionary2)
        this.TargetActionTable[keyValuePair.Key] = new List<ActionTypes>((IEnumerable<ActionTypes>) keyValuePair.Value);
    }

    protected void TableSetting()
    {
      this.TableSetting(this.AnimalType, this.BreedingType);
    }

    public List<ActionTypes> GetActionList(AnimalState _state)
    {
      List<ActionTypes> actionTypesList;
      return this.TargetActionTable.TryGetValue(_state, out actionTypesList) ? actionTypesList : (List<ActionTypes>) null;
    }

    public void Clear()
    {
      this.Animal = (AnimalBase) null;
      this.StateConditionTable.Clear();
      this.StateNonOverlapTable.Clear();
      this.TargetActionTable.Clear();
    }

    public bool ChangeState(AnimalState _nextState = AnimalState.None)
    {
      if (Object.op_Equality((Object) this.Animal, (Object) null))
        return false;
      AnimalState currentState = this.Animal.CurrentState;
      AnimalState prevState = this.Animal.PrevState;
      if (!this.StateConditionTable.ContainsKey(currentState))
      {
        if (_nextState != AnimalState.None)
          this.Animal.CurrentState = _nextState;
        return false;
      }
      StateCondition _c = this.StateConditionTable[currentState];
      ConditionType conditionType = _c.conditionType;
      switch (conditionType)
      {
        case ConditionType.None:
          if (_nextState != AnimalState.None)
            this.Animal.CurrentState = _nextState;
          return false;
        case ConditionType.NonOverlap:
          if (!this.StateNonOverlapTable.ContainsKey(currentState))
          {
            this.StateNonOverlapTable[currentState] = new StateCondition(_c);
            break;
          }
          if (_c.Count <= 0)
          {
            _c.SetNextState(this.StateNonOverlapTable[currentState].AllState(false));
            break;
          }
          break;
      }
      bool flag = false;
      switch (conditionType - 1)
      {
        case ConditionType.None:
          flag = this.SetCurrentState(_c.FirstElement);
          break;
        case ConditionType.Forced:
          flag = this.SetCurrentState(_c.GetProportionState());
          break;
        case ConditionType.Proportion:
          flag = this.SetCurrentState(_c.GetRandomState());
          break;
        case ConditionType.Random:
          flag = this.SetCurrentState(_c.GetNonOverlapState());
          break;
      }
      if (flag)
        return true;
      if (_nextState != AnimalState.None)
        this.Animal.CurrentState = _nextState;
      return false;
    }

    public void SetState(AnimalState _nextState)
    {
      if (Object.op_Equality((Object) this.Animal, (Object) null))
        return;
      this.Animal.CurrentState = _nextState;
    }

    protected bool SetCurrentState(AnimalState _state)
    {
      if (Object.op_Equality((Object) this.Animal, (Object) null) || _state == AnimalState.None)
        return false;
      this.Animal.CurrentState = _state;
      return true;
    }

    protected bool SetCurrentState(OneState _state)
    {
      return this.SetCurrentState(_state.state);
    }
  }
}
