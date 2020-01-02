// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalDesireController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject.Animal
{
  public class AnimalDesireController : MonoBehaviour
  {
    private static List<DesireType> DesireTypeElements = new List<DesireType>()
    {
      DesireType.Sleepiness,
      DesireType.Loneliness,
      DesireType.Action
    };
    [SerializeField]
    private AnimalBase animal;
    private Dictionary<DesireType, float> ParamTable;
    private List<int> PriorityList;
    private Dictionary<int, DesireType> PriorityTable;
    private Dictionary<DesireType, int> SpanTable;
    private Dictionary<DesireType, Tuple<float, float>> BorderTable;
    private Dictionary<AnimalState, Dictionary<DesireType, float>> RateTable;
    private Dictionary<DesireType, Dictionary<bool, Dictionary<DesireType, ChangeParamState>>> ResultTable;

    public AnimalDesireController()
    {
      base.\u002Ector();
    }

    public bool Active { get; private set; }

    private Resources.AnimalTables AnimalTable
    {
      get
      {
        return Singleton<Resources>.Instance.AnimalTable;
      }
    }

    private AnimalDefinePack.AllAnimalInfoGroup AllAnimalInfo
    {
      get
      {
        return Singleton<Resources>.Instance.AnimalDefinePack.AllAnimalInfo;
      }
    }

    public bool AnimalActive
    {
      get
      {
        return Object.op_Inequality((Object) this.animal, (Object) null) && this.animal.ActiveState;
      }
    }

    public AnimalTypes AnimalType
    {
      get
      {
        return Object.op_Inequality((Object) this.animal, (Object) null) ? this.animal.AnimalType : (AnimalTypes) 0;
      }
    }

    public BreedingTypes BreedingType
    {
      get
      {
        return Object.op_Inequality((Object) this.animal, (Object) null) ? this.animal.BreedingType : BreedingTypes.Wild;
      }
    }

    public Dictionary<DesireType, List<AnimalState>> TargetStateTable { get; private set; }

    public Func<DesireType, bool> DesireFilledEvent { get; set; }

    public Func<DesireType, bool> ChangedCandidateDesireEvent { get; set; }

    public float GetParam(DesireType _desireType)
    {
      float num = 0.0f;
      this.ParamTable.TryGetValue(_desireType, out num);
      return num;
    }

    public bool SetParam(DesireType _desireType, float _value)
    {
      if (!this.ParamTable.ContainsKey(_desireType))
        return false;
      Tuple<float, float> tuple;
      this.ParamTable[_desireType] = !this.BorderTable.TryGetValue(_desireType, out tuple) ? Mathf.Max(_value, 0.0f) : Mathf.Clamp(_value, 0.0f, tuple.Item2);
      this.SetDesireType();
      this.CheckCandidateDesire();
      return true;
    }

    public bool ResetCurrentDesrie()
    {
      if (this.CurrentDesire == DesireType.None)
        return false;
      if (this.ParamTable.ContainsKey(this.CurrentDesire))
        this.ParamTable[this.CurrentDesire] = 0.0f;
      this.CurrentDesire = DesireType.None;
      return true;
    }

    public DesireType CurrentDesire { get; private set; }

    public bool HasCurrentDesire
    {
      get
      {
        return this.CurrentDesire != DesireType.None;
      }
    }

    public DesireType CandidateDesire { get; private set; }

    public bool HasCandidateDesire
    {
      get
      {
        return this.CandidateDesire != DesireType.None;
      }
    }

    public bool HasOnlyCandidateDesire
    {
      get
      {
        return this.CandidateDesire != DesireType.None && this.CurrentDesire == DesireType.None;
      }
    }

    private void Awake()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>(Observable.Where<long>(Observable.TakeUntilDisable<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (_ => this.Active)), (Func<M0, bool>) (_ => this.AnimalActive)), (Action<M0>) (_ => this.OnUpdate()));
    }

    public void Initialize(bool _enabled)
    {
      this.Clear();
      this.TableSetting();
      this.SetRandomParam();
      this.Active = _enabled;
    }

    private void TableSetting()
    {
      this.DesireTableSetting();
    }

    private void DesireTableSetting()
    {
      AnimalTypes animalType = this.AnimalType;
      BreedingTypes breedingType = this.BreedingType;
      this.ClearTable();
      foreach (DesireType desireTypeElement in AnimalDesireController.DesireTypeElements)
        this.ParamTable[desireTypeElement] = 0.0f;
      Dictionary<BreedingTypes, Dictionary<int, DesireType>> dictionary1;
      Dictionary<int, DesireType> dictionary2;
      if (this.AnimalTable.DesirePriorityTable.TryGetValue(animalType, out dictionary1) && dictionary1.TryGetValue(breedingType, out dictionary2))
      {
        foreach (KeyValuePair<int, DesireType> keyValuePair in dictionary2)
        {
          this.PriorityTable[keyValuePair.Key] = keyValuePair.Value;
          this.PriorityList.Add(keyValuePair.Key);
        }
        this.PriorityList.Sort();
      }
      Dictionary<BreedingTypes, Dictionary<DesireType, int>> dictionary3;
      Dictionary<DesireType, int> dictionary4;
      if (this.AnimalTable.DesireSpanTable.TryGetValue(animalType, out dictionary3) && dictionary3.TryGetValue(breedingType, out dictionary4))
      {
        foreach (KeyValuePair<DesireType, int> keyValuePair in dictionary4)
          this.SpanTable[keyValuePair.Key] = keyValuePair.Value;
      }
      Dictionary<BreedingTypes, Dictionary<DesireType, Tuple<float, float>>> dictionary5;
      Dictionary<DesireType, Tuple<float, float>> dictionary6;
      if (this.AnimalTable.DesireBorderTable.TryGetValue(animalType, out dictionary5) && dictionary5.TryGetValue(breedingType, out dictionary6))
      {
        foreach (KeyValuePair<DesireType, Tuple<float, float>> keyValuePair in dictionary6)
          this.BorderTable[keyValuePair.Key] = keyValuePair.Value;
      }
      Dictionary<BreedingTypes, Dictionary<AnimalState, Dictionary<DesireType, float>>> dictionary7;
      Dictionary<AnimalState, Dictionary<DesireType, float>> dictionary8;
      if (this.AnimalTable.DesireRateTable.TryGetValue(animalType, out dictionary7) && dictionary7.TryGetValue(breedingType, out dictionary8))
      {
        foreach (KeyValuePair<AnimalState, Dictionary<DesireType, float>> keyValuePair1 in dictionary8)
        {
          this.RateTable[keyValuePair1.Key] = new Dictionary<DesireType, float>();
          foreach (KeyValuePair<DesireType, float> keyValuePair2 in keyValuePair1.Value)
            this.RateTable[keyValuePair1.Key][keyValuePair2.Key] = keyValuePair2.Value;
        }
      }
      Dictionary<BreedingTypes, Dictionary<DesireType, List<AnimalState>>> dictionary9;
      Dictionary<DesireType, List<AnimalState>> dictionary10;
      if (this.AnimalTable.DesireTargetStateTable.TryGetValue(animalType, out dictionary9) && dictionary9.TryGetValue(breedingType, out dictionary10))
      {
        foreach (KeyValuePair<DesireType, List<AnimalState>> keyValuePair in dictionary10)
        {
          List<AnimalState> animalStateList = new List<AnimalState>();
          foreach (AnimalState animalState in keyValuePair.Value)
            animalStateList.Add(animalState);
          this.TargetStateTable[keyValuePair.Key] = animalStateList;
        }
      }
      Dictionary<BreedingTypes, Dictionary<DesireType, Dictionary<bool, Dictionary<DesireType, ChangeParamState>>>> dictionary11;
      Dictionary<DesireType, Dictionary<bool, Dictionary<DesireType, ChangeParamState>>> dictionary12;
      if (!this.AnimalTable.DesireResultTable.TryGetValue(animalType, out dictionary11) || !dictionary11.TryGetValue(breedingType, out dictionary12))
        return;
      foreach (KeyValuePair<DesireType, Dictionary<bool, Dictionary<DesireType, ChangeParamState>>> keyValuePair1 in dictionary12)
      {
        this.ResultTable[keyValuePair1.Key] = new Dictionary<bool, Dictionary<DesireType, ChangeParamState>>();
        foreach (KeyValuePair<bool, Dictionary<DesireType, ChangeParamState>> keyValuePair2 in keyValuePair1.Value)
        {
          this.ResultTable[keyValuePair1.Key][keyValuePair2.Key] = new Dictionary<DesireType, ChangeParamState>();
          foreach (KeyValuePair<DesireType, ChangeParamState> keyValuePair3 in keyValuePair2.Value)
            this.ResultTable[keyValuePair1.Key][keyValuePair2.Key][keyValuePair3.Key] = keyValuePair3.Value;
        }
      }
    }

    public void SetRandomParam()
    {
      foreach (KeyValuePair<DesireType, Tuple<float, float>> keyValuePair in this.BorderTable)
      {
        if (this.ParamTable.ContainsKey(keyValuePair.Key))
        {
          float num = Random.Range(0.0f, keyValuePair.Value.Item1 * 0.75f);
          this.ParamTable[keyValuePair.Key] = num;
        }
      }
    }

    private void OnUpdate()
    {
      if (!this.animal.ParamRisePossible || this.HasCurrentDesire)
        return;
      this.SetDesireType();
    }

    public void OnMinuteUpdate()
    {
      if (this.Active && !this.HasCurrentDesire)
        this.DesireParamRiseMinute();
      this.CheckCandidateDesire();
    }

    private void DesireParamRiseMinute()
    {
      Dictionary<DesireType, float> dictionary;
      if (!this.RateTable.TryGetValue(this.animal.CurrentState, out dictionary))
        return;
      foreach (KeyValuePair<DesireType, float> keyValuePair in dictionary)
      {
        float num;
        if (this.ParamTable.TryGetValue(keyValuePair.Key, out num))
        {
          num += keyValuePair.Value;
          Tuple<float, float> tuple;
          this.ParamTable[keyValuePair.Key] = !this.BorderTable.TryGetValue(keyValuePair.Key, out tuple) ? Mathf.Max(num, 0.0f) : Mathf.Clamp(num, 0.0f, tuple.Item2);
        }
      }
    }

    public void CheckCandidateDesire()
    {
      DesireType candidateDesire = this.CandidateDesire;
      this.CandidateDesire = DesireType.None;
      for (int index = 0; index < this.PriorityList.Count; ++index)
      {
        DesireType key = this.PriorityTable[this.PriorityList[index]];
        float num;
        Tuple<float, float> tuple;
        if (this.ParamTable.TryGetValue(key, out num) && this.BorderTable.TryGetValue(key, out tuple) && (double) tuple.Item1 <= (double) num)
        {
          this.CandidateDesire = key;
          break;
        }
      }
      if (this.CandidateDesire == candidateDesire)
        return;
      Func<DesireType, bool> candidateDesireEvent = this.ChangedCandidateDesireEvent;
      if (candidateDesireEvent == null)
        return;
      int num1 = candidateDesireEvent(this.CandidateDesire) ? 1 : 0;
    }

    public bool CheckDesireParamFilled(DesireType _desireType)
    {
      float num = 0.0f;
      Tuple<float, float> tuple;
      return this.BorderTable.TryGetValue(_desireType, out tuple) && this.ParamTable.TryGetValue(_desireType, out num) && (double) tuple.Item2 <= (double) num;
    }

    public bool SetDesireType()
    {
      for (int index = 0; index < this.PriorityList.Count; ++index)
      {
        DesireType _desireType = this.PriorityTable[this.PriorityList[index]];
        if (this.CheckDesireParamFilled(_desireType))
        {
          if (this.CurrentDesire == _desireType)
            return false;
          this.CurrentDesire = _desireType;
          Func<DesireType, bool> desireFilledEvent = this.DesireFilledEvent;
          bool? nullable = desireFilledEvent != null ? new bool?(desireFilledEvent(_desireType)) : new bool?();
          if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) != 0)
          {
            this.CheckCandidateDesire();
            return true;
          }
          this.CurrentDesire = DesireType.None;
          this.SetParam(_desireType, 0.0f);
          this.CheckCandidateDesire();
          return false;
        }
      }
      return false;
    }

    public void ReduceParameter(bool _success, DesireType _changeDesire)
    {
      AnimalTypes animalType = this.AnimalType;
      Dictionary<bool, Dictionary<DesireType, ChangeParamState>> dictionary;
      if (this.ResultTable.TryGetValue(_changeDesire, out dictionary))
      {
        foreach (KeyValuePair<bool, Dictionary<DesireType, ChangeParamState>> keyValuePair1 in dictionary)
        {
          bool key1 = keyValuePair1.Key;
          if (_success == key1)
          {
            foreach (KeyValuePair<DesireType, ChangeParamState> keyValuePair2 in keyValuePair1.Value)
            {
              DesireType key2 = keyValuePair2.Key;
              ChangeParamState changeParamState = keyValuePair2.Value;
              if (this.ParamTable.ContainsKey(key2))
              {
                float num1 = this.ParamTable[key2];
                float randomValue = changeParamState.RandomValue;
                float num2;
                switch (changeParamState.changeType)
                {
                  case ChangeType.Add:
                    num2 = num1 + randomValue;
                    break;
                  case ChangeType.Sub:
                    num2 = num1 - randomValue;
                    break;
                  case ChangeType.Cng:
                    num2 = randomValue;
                    break;
                  default:
                    continue;
                }
                Tuple<float, float> tuple;
                this.ParamTable[key2] = !this.BorderTable.TryGetValue(key2, out tuple) ? Mathf.Max(num2, 0.0f) : Mathf.Clamp(num2, 0.0f, tuple.Item2);
              }
            }
          }
        }
      }
      else
        this.ParamTable[_changeDesire] = 0.0f;
      if (this.CurrentDesire == _changeDesire)
        this.CurrentDesire = DesireType.None;
      this.CheckCandidateDesire();
    }

    public void ReduceParameter(bool _success)
    {
      this.ReduceParameter(_success, this.CurrentDesire);
    }

    public void SuccessActionPoint()
    {
      this.ReduceParameter(true);
    }

    public void FailureActionPoint(AnimalState _nextState = AnimalState.Idle)
    {
      this.ReduceParameter(false);
      this.animal.CancelActionPoint();
      if (this.animal.CurrentState == _nextState)
        return;
      this.animal.CurrentState = _nextState;
    }

    private void ClearTable()
    {
      this.ParamTable.Clear();
      this.PriorityList.Clear();
      this.PriorityTable.Clear();
      this.SpanTable.Clear();
      this.BorderTable.Clear();
      this.RateTable.Clear();
      this.TargetStateTable.Clear();
      this.ResultTable.Clear();
    }

    public void Clear()
    {
      this.Active = false;
      DesireType desireType = DesireType.None;
      this.CandidateDesire = desireType;
      this.CurrentDesire = desireType;
      this.ClearTable();
    }

    private void OnDestroy()
    {
      this.Clear();
    }
  }
}
