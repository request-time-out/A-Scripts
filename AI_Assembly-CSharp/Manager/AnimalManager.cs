// Decompiled with JetBrains decompiler
// Type: Manager.AnimalManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.Animal;
using AIProject.Animal.Resources;
using AIProject.SaveData;
using AIProject.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEx;

namespace Manager
{
  public sealed class AnimalManager : Singleton<AnimalManager>
  {
    private Dictionary<int, AnimalBase> animalTable = new Dictionary<int, AnimalBase>();
    private List<int> animalKeyList = new List<int>();
    private Transform _animalRoot;
    private static Subject<Unit> _commandRefreshEvent;
    private CompositeDisposable enviromentSubscribeDisposable;
    private IDisposable mapAreaCheckDisposable;

    public void StartAllAnimalCreate()
    {
      this.HabitatPointsFirstSetting();
      this.SettingAnimalPointBehavior();
    }

    public void SettingAnimalPointBehavior()
    {
      this.ClearAnimalPointBehavior();
      this.ActivateHabitatPoints();
    }

    private void HabitatPointsFirstSetting()
    {
      if (!Singleton<Manager.Resources>.IsInstance())
        return;
      this.WildCatHabitatPointSetting();
      this.WildChickenHabitatPointSetting();
      this.WildCatAndChickenHabitatPointSetting();
      this.WildMechaHabitatPointSetting();
      this.WildFrogHabitatPointSetting();
      this.WildBirdFlockHabitatPointSetting();
      this.WildButterflyHabitatPointSetting();
    }

    private void WildCatHabitatPointSetting()
    {
      if (this.WildCatPopPoints.IsNullOrEmpty<GroundAnimalHabitatPoint>())
        return;
      WorldData worldData = !Singleton<Game>.IsInstance() ? (WorldData) null : Singleton<Game>.Instance.WorldData;
      Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>> dictionary1 = worldData == null ? (Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>>) null : worldData.WildAnimalTable;
      Dictionary<int, WildAnimalData> dictionary2 = (Dictionary<int, WildAnimalData>) null;
      dictionary1?.TryGetValue(AnimalTypes.Cat, out dictionary2);
      AnimalDefinePack.SystemInfoGroup _systemInfo = Singleton<Manager.Resources>.Instance.AnimalDefinePack.SystemInfo;
      Vector2 catCreateCoolTime = _systemInfo.WildCatCreateCoolTime;
      Vector2 pointCoolTimeRange = _systemInfo.PopPointCoolTimeRange;
      foreach (GroundAnimalHabitatPoint wildCatPopPoint in this.WildCatPopPoints)
      {
        if (!Object.op_Equality((Object) wildCatPopPoint, (Object) null))
        {
          WildAnimalData wildAnimalData = (WildAnimalData) null;
          dictionary2?.TryGetValue(wildCatPopPoint.ID, out wildAnimalData);
          wildCatPopPoint.SetCoolTime(wildAnimalData == null ? 0.0f : wildAnimalData.CoolTime);
          wildCatPopPoint.CoolTime = pointCoolTimeRange;
          wildCatPopPoint.IsActive = !this.IsRain();
          wildCatPopPoint.IsCountStop = true;
          wildCatPopPoint.AddCheck = (Func<Vector3, bool>) (pos => !this.IsRain() && this.WildCats.Count < _systemInfo.WildCatMaxNum);
          wildCatPopPoint.AddAnimalAction = (Func<GroundAnimalHabitatPoint, WildGround>) (_basePoint =>
          {
            if (Object.op_Equality((Object) _basePoint, (Object) null))
              return (WildGround) null;
            WildGround _cat = this.Create<WildGround>(0, 0);
            if (Object.op_Equality((Object) _cat, (Object) null))
              return (WildGround) null;
            ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) _cat), (Action<M0>) (_ => this.RemoveAnimal((AnimalBase) _cat, false)));
            _cat.Initialize(_basePoint);
            _cat.Refresh();
            return _cat;
          });
        }
      }
    }

    private void WildChickenHabitatPointSetting()
    {
      if (this.WildChickenPopPoints.IsNullOrEmpty<GroundAnimalHabitatPoint>())
        return;
      WorldData worldData = !Singleton<Game>.IsInstance() ? (WorldData) null : Singleton<Game>.Instance.WorldData;
      Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>> dictionary1 = worldData == null ? (Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>>) null : worldData.WildAnimalTable;
      Dictionary<int, WildAnimalData> dictionary2 = (Dictionary<int, WildAnimalData>) null;
      dictionary1?.TryGetValue(AnimalTypes.Chicken, out dictionary2);
      AnimalDefinePack.SystemInfoGroup _systemInfo = Singleton<Manager.Resources>.Instance.AnimalDefinePack.SystemInfo;
      Vector2 chickenCreateCoolTime = _systemInfo.WildChickenCreateCoolTime;
      Vector2 pointCoolTimeRange = _systemInfo.PopPointCoolTimeRange;
      foreach (GroundAnimalHabitatPoint wildChickenPopPoint in this.WildChickenPopPoints)
      {
        if (!Object.op_Equality((Object) wildChickenPopPoint, (Object) null))
        {
          WildAnimalData wildAnimalData = (WildAnimalData) null;
          dictionary2?.TryGetValue(wildChickenPopPoint.ID, out wildAnimalData);
          wildChickenPopPoint.SetCoolTime(wildAnimalData == null ? 0.0f : wildAnimalData.CoolTime);
          wildChickenPopPoint.CoolTime = pointCoolTimeRange;
          wildChickenPopPoint.IsActive = !this.IsRain();
          wildChickenPopPoint.IsCountStop = true;
          wildChickenPopPoint.AddCheck = (Func<Vector3, bool>) (pos => !this.IsRain() && this.WildChickens.Count < _systemInfo.WildChickenMaxNum);
          wildChickenPopPoint.AddAnimalAction = (Func<GroundAnimalHabitatPoint, WildGround>) (_basePoint =>
          {
            if (Object.op_Equality((Object) _basePoint, (Object) null))
              return (WildGround) null;
            WildGround _chicken = this.Create<WildGround>(1, 0);
            if (Object.op_Equality((Object) _chicken, (Object) null))
              return (WildGround) null;
            ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) _chicken), (Action<M0>) (_ => this.RemoveAnimal((AnimalBase) _chicken, false)));
            _chicken.Initialize(_basePoint);
            _chicken.Refresh();
            return _chicken;
          });
        }
      }
    }

    private void WildCatAndChickenHabitatPointSetting()
    {
      if (this.WildCatAndChickenPopPoints.IsNullOrEmpty<GroundAnimalHabitatPoint>())
        return;
      WorldData worldData = !Singleton<Game>.IsInstance() ? (WorldData) null : Singleton<Game>.Instance.WorldData;
      Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>> dictionary1 = worldData == null ? (Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>>) null : worldData.WildAnimalTable;
      Dictionary<int, WildAnimalData> dictionary2 = (Dictionary<int, WildAnimalData>) null;
      dictionary1?.TryGetValue(AnimalTypes.Cat | AnimalTypes.Chicken, out dictionary2);
      AnimalDefinePack.SystemInfoGroup _systemInfo = Singleton<Manager.Resources>.Instance.AnimalDefinePack.SystemInfo;
      Vector2 catCreateCoolTime = _systemInfo.WildCatCreateCoolTime;
      Vector2 chickenCreateCoolTime = _systemInfo.WildChickenCreateCoolTime;
      Vector2 pointCoolTimeRange = _systemInfo.PopPointCoolTimeRange;
      foreach (GroundAnimalHabitatPoint andChickenPopPoint in this.WildCatAndChickenPopPoints)
      {
        if (!Object.op_Equality((Object) andChickenPopPoint, (Object) null))
        {
          WildAnimalData wildAnimalData = (WildAnimalData) null;
          dictionary2?.TryGetValue(andChickenPopPoint.ID, out wildAnimalData);
          andChickenPopPoint.SetCoolTime(wildAnimalData == null ? 0.0f : wildAnimalData.CoolTime);
          andChickenPopPoint.CoolTime = pointCoolTimeRange;
          andChickenPopPoint.IsActive = !this.IsRain();
          andChickenPopPoint.IsCountStop = true;
          andChickenPopPoint.AddCheck = (Func<Vector3, bool>) (pos => !this.IsRain() && this.WildChickens.Count < _systemInfo.WildChickenMaxNum && this.WildCats.Count < _systemInfo.WildCatMaxNum);
          andChickenPopPoint.AddAnimalAction = (Func<GroundAnimalHabitatPoint, WildGround>) (_basePoint =>
          {
            if (Object.op_Equality((Object) _basePoint, (Object) null))
              return (WildGround) null;
            WildGround _animal = this.Create<WildGround>(Random.Range(0, 100) >= 50 ? 1 : 0, 0);
            if (Object.op_Equality((Object) _animal, (Object) null))
              return (WildGround) null;
            ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) _animal), (Action<M0>) (_ => this.RemoveAnimal((AnimalBase) _animal, false)));
            _animal.Initialize(_basePoint);
            _animal.Refresh();
            return _animal;
          });
        }
      }
    }

    private void WildMechaHabitatPointSetting()
    {
      if (this.MechaHabitatPoints.IsNullOrEmpty<MechaHabitatPoint>())
        return;
      WorldData worldData = !Singleton<Game>.IsInstance() ? (WorldData) null : Singleton<Game>.Instance.WorldData;
      Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>> dictionary1 = worldData == null ? (Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>>) null : worldData.WildAnimalTable;
      Dictionary<int, WildAnimalData> dictionary2 = (Dictionary<int, WildAnimalData>) null;
      dictionary1?.TryGetValue(AnimalTypes.Mecha, out dictionary2);
      AnimalDefinePack.SystemInfoGroup _systemInfo = Singleton<Manager.Resources>.Instance.AnimalDefinePack.SystemInfo;
      foreach (MechaHabitatPoint mechaHabitatPoint in this.MechaHabitatPoints)
      {
        if (!Object.op_Equality((Object) mechaHabitatPoint, (Object) null))
        {
          WildAnimalData wildAnimalData = (WildAnimalData) null;
          dictionary2?.TryGetValue(mechaHabitatPoint.ID, out wildAnimalData);
          mechaHabitatPoint.SetCoolTime(wildAnimalData == null ? 0.0f : wildAnimalData.CoolTime);
          mechaHabitatPoint.ForcedAdd = wildAnimalData == null || wildAnimalData.IsAdded;
          mechaHabitatPoint.IsActive = true;
          mechaHabitatPoint.IsCountStop = true;
          mechaHabitatPoint.AddCheck = (Func<MechaHabitatPoint, bool>) (_basePoint =>
          {
            if (this.WildMechas.Count >= _systemInfo.WildMechaMaxNum)
              return false;
            return this.CheckAvailablePoint(_basePoint.Position, true, true) || _basePoint.ForcedAdd;
          });
          mechaHabitatPoint.AddAnimalAction = (Func<MechaHabitatPoint, WildMecha>) (_basePoint =>
          {
            if (Object.op_Equality((Object) _basePoint, (Object) null))
              return (WildMecha) null;
            _basePoint.ForcedAdd = false;
            WildMecha _mecha = this.Create<WildMecha>(4, 0);
            if (Object.op_Equality((Object) _mecha, (Object) null))
              return (WildMecha) null;
            ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) _mecha), (Action<M0>) (_ => this.RemoveAnimal((AnimalBase) _mecha, false)));
            _mecha.Initialize(_basePoint);
            _mecha.Refresh();
            return _mecha;
          });
        }
      }
    }

    private void WildFrogHabitatPointSetting()
    {
      if (this.FrogHabitatPoints.IsNullOrEmpty<FrogHabitatPoint>())
        return;
      WorldData worldData = !Singleton<Game>.IsInstance() ? (WorldData) null : Singleton<Game>.Instance.WorldData;
      Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>> dictionary1 = worldData == null ? (Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>>) null : worldData.WildAnimalTable;
      Dictionary<int, WildAnimalData> dictionary2 = (Dictionary<int, WildAnimalData>) null;
      dictionary1?.TryGetValue(AnimalTypes.Frog, out dictionary2);
      AnimalDefinePack.SystemInfoGroup _systemInfo = Singleton<Manager.Resources>.Instance.AnimalDefinePack.SystemInfo;
      foreach (FrogHabitatPoint frogHabitatPoint in this.FrogHabitatPoints)
      {
        if (!Object.op_Equality((Object) frogHabitatPoint, (Object) null))
        {
          frogHabitatPoint.CoolTimeRange = _systemInfo.FrogCoolTimeRange;
          WildAnimalData wildAnimalData = (WildAnimalData) null;
          dictionary2?.TryGetValue(frogHabitatPoint.ID, out wildAnimalData);
          frogHabitatPoint.SetCoolTime(wildAnimalData == null ? 0.0f : wildAnimalData.CoolTime);
          frogHabitatPoint.ForcedAdd = wildAnimalData == null || wildAnimalData.IsAdded;
          frogHabitatPoint.IsActive = true;
          frogHabitatPoint.IsCountStop = true;
          frogHabitatPoint.AddCheck = (Func<FrogHabitatPoint, bool>) (_basePoint =>
          {
            if (this.WildFrogs.Count >= _systemInfo.WildFrogMaxNum)
              return false;
            return this.CheckAvailablePoint(_basePoint.Position, true, true) || _basePoint.ForcedAdd;
          });
          frogHabitatPoint.AddAnimalAction = (Func<FrogHabitatPoint, WildFrog>) (_basePoint =>
          {
            if (Object.op_Equality((Object) _basePoint, (Object) null))
              return (WildFrog) null;
            _basePoint.ForcedAdd = false;
            WildFrog _frog = this.Create<WildFrog>(5, 0);
            if (Object.op_Equality((Object) _frog, (Object) null))
              return (WildFrog) null;
            ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) _frog), (Action<M0>) (_ => this.RemoveAnimal((AnimalBase) _frog, false)));
            _frog.Initialize(_basePoint);
            _frog.Refresh();
            return _frog;
          });
        }
      }
    }

    private void WildBirdFlockHabitatPointSetting()
    {
      if (this.BirdFlockHabitatPoints.IsNullOrEmpty<BirdFlockHabitatPoint>())
        return;
      AnimalDefinePack.SystemInfoGroup _systemInfo = Singleton<Manager.Resources>.Instance.AnimalDefinePack.SystemInfo;
      foreach (BirdFlockHabitatPoint flockHabitatPoint in this.BirdFlockHabitatPoints)
      {
        if (!Object.op_Equality((Object) flockHabitatPoint, (Object) null))
        {
          flockHabitatPoint.SetCoolTime();
          flockHabitatPoint.IsActive = !this.IsRain() && !this.IsNight();
          flockHabitatPoint.IsCountStop = true;
          flockHabitatPoint.AddCheck = (Func<Vector3, bool>) (pos => this.WildBirdFlocks.Count < _systemInfo.WildBirdFlockMaxNum && !this.IsRain() && !this.IsNight());
          flockHabitatPoint.AddAnimalAction = (Func<BirdFlockHabitatPoint, WildBirdFlock>) (_basePoint =>
          {
            if (Object.op_Equality((Object) _basePoint, (Object) null))
              return (WildBirdFlock) null;
            WildBirdFlock _bird = this.Create<WildBirdFlock>(6, 0);
            if (Object.op_Equality((Object) _bird, (Object) null))
              return (WildBirdFlock) null;
            ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) _bird), (Action<M0>) (_ => this.RemoveAnimal((AnimalBase) _bird, false)));
            _bird.Initialize(_basePoint);
            return _bird;
          });
        }
      }
    }

    private void WildButterflyHabitatPointSetting()
    {
      if (this.ButterflyHabitatPoints.IsNullOrEmpty<ButterflyHabitatPoint>())
        return;
      AnimalDefinePack.SystemInfoGroup systemInfo = Singleton<Manager.Resources>.Instance.AnimalDefinePack.SystemInfo;
      foreach (ButterflyHabitatPoint butterflyHabitatPoint in this.ButterflyHabitatPoints)
      {
        if (!Object.op_Equality((Object) butterflyHabitatPoint, (Object) null))
        {
          butterflyHabitatPoint.IsStop = true;
          butterflyHabitatPoint.IsActive = !this.IsRain() && !this.IsNight();
          butterflyHabitatPoint.IsCreate = false;
          butterflyHabitatPoint.AddCheck = (Func<Vector3, bool>) (pos => !this.IsRain() && !this.IsNight());
          butterflyHabitatPoint.AddAnimalAction = (Func<ButterflyHabitatPoint, WildButterfly>) (_basePoint =>
          {
            if (Object.op_Equality((Object) _basePoint, (Object) null))
              return (WildButterfly) null;
            WildButterfly _butterfly = this.Create<WildButterfly>(3, 0);
            if (Object.op_Equality((Object) _butterfly, (Object) null))
              return (WildButterfly) null;
            ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) _butterfly), (Action<M0>) (_ => this.RemoveAnimal((AnimalBase) _butterfly, false)));
            _butterfly.Initialize(_basePoint);
            return _butterfly;
          });
        }
      }
    }

    private void ActivateHabitatPoints()
    {
      if (!this.WildGroundHabitatPoints.IsNullOrEmpty<GroundAnimalHabitatPoint>())
      {
        foreach (GroundAnimalHabitatPoint groundHabitatPoint in this.WildGroundHabitatPoints)
        {
          if (Object.op_Inequality((Object) groundHabitatPoint, (Object) null) && groundHabitatPoint.IsPopPoint)
            groundHabitatPoint.IsCountStop = false;
        }
      }
      if (!this.MechaHabitatPoints.IsNullOrEmpty<MechaHabitatPoint>())
      {
        foreach (MechaHabitatPoint mechaHabitatPoint in this.MechaHabitatPoints)
        {
          if (Object.op_Inequality((Object) mechaHabitatPoint, (Object) null))
            mechaHabitatPoint.IsCountStop = false;
        }
      }
      if (!this.FrogHabitatPoints.IsNullOrEmpty<FrogHabitatPoint>())
      {
        foreach (FrogHabitatPoint frogHabitatPoint in this.FrogHabitatPoints)
        {
          if (Object.op_Inequality((Object) frogHabitatPoint, (Object) null))
            frogHabitatPoint.IsCountStop = false;
        }
      }
      if (!this.BirdFlockHabitatPoints.IsNullOrEmpty<BirdFlockHabitatPoint>())
      {
        foreach (BirdFlockHabitatPoint flockHabitatPoint in this.BirdFlockHabitatPoints)
        {
          if (Object.op_Inequality((Object) flockHabitatPoint, (Object) null))
            flockHabitatPoint.IsCountStop = false;
        }
      }
      if (this.ButterflyHabitatPoints.IsNullOrEmpty<ButterflyHabitatPoint>())
        return;
      foreach (ButterflyHabitatPoint butterflyHabitatPoint in this.ButterflyHabitatPoints)
      {
        if (Object.op_Inequality((Object) butterflyHabitatPoint, (Object) null))
          butterflyHabitatPoint.IsStop = false;
      }
    }

    private void DeactivateHabitatPoints()
    {
      if (!this.WildGroundHabitatPoints.IsNullOrEmpty<GroundAnimalHabitatPoint>())
      {
        foreach (GroundAnimalHabitatPoint groundHabitatPoint in this.WildGroundHabitatPoints)
        {
          if (Object.op_Inequality((Object) groundHabitatPoint, (Object) null) && groundHabitatPoint.IsPopPoint)
            groundHabitatPoint.IsCountStop = true;
        }
      }
      if (!this.MechaHabitatPoints.IsNullOrEmpty<MechaHabitatPoint>())
      {
        foreach (MechaHabitatPoint mechaHabitatPoint in this.MechaHabitatPoints)
        {
          if (Object.op_Inequality((Object) mechaHabitatPoint, (Object) null))
            mechaHabitatPoint.IsCountStop = true;
        }
      }
      if (!this.FrogHabitatPoints.IsNullOrEmpty<FrogHabitatPoint>())
      {
        foreach (FrogHabitatPoint frogHabitatPoint in this.FrogHabitatPoints)
        {
          if (Object.op_Inequality((Object) frogHabitatPoint, (Object) null))
            frogHabitatPoint.IsCountStop = true;
        }
      }
      if (!this.BirdFlockHabitatPoints.IsNullOrEmpty<BirdFlockHabitatPoint>())
      {
        foreach (BirdFlockHabitatPoint flockHabitatPoint in this.BirdFlockHabitatPoints)
        {
          if (Object.op_Inequality((Object) flockHabitatPoint, (Object) null))
            flockHabitatPoint.IsCountStop = true;
        }
      }
      if (this.ButterflyHabitatPoints.IsNullOrEmpty<ButterflyHabitatPoint>())
        return;
      foreach (ButterflyHabitatPoint butterflyHabitatPoint in this.ButterflyHabitatPoints)
      {
        if (Object.op_Inequality((Object) butterflyHabitatPoint, (Object) null))
          butterflyHabitatPoint.IsStop = true;
      }
    }

    public void ClearAnimalPointBehavior()
    {
      this.DeactivateHabitatPoints();
    }

    public bool IsRain(Weather _weather)
    {
      return _weather == Weather.Rain || _weather == Weather.Storm;
    }

    public bool IsRain(EnvironmentSimulator _simulator)
    {
      return !Object.op_Equality((Object) _simulator, (Object) null) && this.IsRain(_simulator.Weather);
    }

    public bool IsRain()
    {
      if (!Singleton<Map>.IsInstance())
        return false;
      EnvironmentSimulator simulator = Singleton<Map>.Instance.Simulator;
      return !Object.op_Equality((Object) simulator, (Object) null) && this.IsRain(simulator.Weather);
    }

    public bool IsNight(AIProject.TimeZone _timeZone)
    {
      return _timeZone == AIProject.TimeZone.Night;
    }

    public bool IsNight(EnvironmentSimulator _simulator)
    {
      return !Object.op_Equality((Object) _simulator, (Object) null) && this.IsNight(_simulator.TimeZone);
    }

    public bool IsNight()
    {
      if (!Singleton<Map>.IsInstance())
        return false;
      EnvironmentSimulator simulator = Singleton<Map>.Instance.Simulator;
      return !Object.op_Equality((Object) simulator, (Object) null) && this.IsNight(simulator.TimeZone);
    }

    public int AnimalCount { get; private set; }

    private Dictionary<AnimalTypes, Dictionary<BreedingTypes, GameObject>> AnimalBaseTable { get; set; } = new Dictionary<AnimalTypes, Dictionary<BreedingTypes, GameObject>>();

    private Dictionary<int, Dictionary<int, GameObject>> AnimalBaseObjectTable { get; set; } = new Dictionary<int, Dictionary<int, GameObject>>();

    public Transform AnimalRoot
    {
      get
      {
        if (Object.op_Inequality((Object) this._animalRoot, (Object) null))
          return this._animalRoot;
        if (!Singleton<Map>.IsInstance())
          return (Transform) null;
        Transform actorRoot = Singleton<Map>.Instance.ActorRoot;
        this._animalRoot = new GameObject(nameof (AnimalRoot)).get_transform();
        this._animalRoot.SetParent(actorRoot, false);
        return this._animalRoot;
      }
    }

    public bool ActiveMapScene { get; set; }

    public List<AnimalBase> Animals { get; private set; } = new List<AnimalBase>();

    public List<AnimalBase> WildAnimals { get; private set; } = new List<AnimalBase>();

    public List<AnimalBase> PetAnimals { get; private set; } = new List<AnimalBase>();

    public List<WildGround> WildCats { get; private set; } = new List<WildGround>();

    public List<WildGround> WildChickens { get; private set; } = new List<WildGround>();

    public List<WildMecha> WildMechas { get; private set; } = new List<WildMecha>();

    public List<WildFrog> WildFrogs { get; private set; } = new List<WildFrog>();

    public List<WildButterfly> WildButterflies { get; private set; } = new List<WildButterfly>();

    public List<WildBirdFlock> WildBirdFlocks { get; private set; } = new List<WildBirdFlock>();

    public List<WalkingPetAnimal> PetCats { get; private set; } = new List<WalkingPetAnimal>();

    public List<PetChicken> PetChickens { get; private set; } = new List<PetChicken>();

    public List<PetFish> PetFishes { get; private set; } = new List<PetFish>();

    public List<FlyingPetAnimal> PetButterflies { get; private set; } = new List<FlyingPetAnimal>();

    public List<WalkingPetAnimal> PetMechas { get; private set; } = new List<WalkingPetAnimal>();

    public List<MovingPetAnimal> MovingPets { get; private set; } = new List<MovingPetAnimal>();

    public List<WalkingPetAnimal> WalkingPets { get; private set; } = new List<WalkingPetAnimal>();

    public List<FlyingPetAnimal> FlyingPets { get; private set; } = new List<FlyingPetAnimal>();

    public ReadOnlyDictionary<int, AnimalBase> AnimalTable { get; private set; }

    private Dictionary<AnimalTypes, Dictionary<BreedingTypes, int>> AnimalCountTable { get; } = new Dictionary<AnimalTypes, Dictionary<BreedingTypes, int>>();

    public List<AnimalActionPoint> ActionPoints { get; private set; } = new List<AnimalActionPoint>();

    public List<GroundAnimalHabitatPoint> WildGroundHabitatPoints { get; private set; } = new List<GroundAnimalHabitatPoint>();

    public List<GroundAnimalHabitatPoint> WildCatPopPoints { get; private set; } = new List<GroundAnimalHabitatPoint>();

    public List<GroundAnimalHabitatPoint> WildChickenPopPoints { get; private set; } = new List<GroundAnimalHabitatPoint>();

    public List<GroundAnimalHabitatPoint> WildCatAndChickenPopPoints { get; private set; } = new List<GroundAnimalHabitatPoint>();

    public List<GroundAnimalHabitatPoint> WildCatDepopPoints { get; private set; } = new List<GroundAnimalHabitatPoint>();

    public List<GroundAnimalHabitatPoint> WildChickenDepopPoints { get; private set; } = new List<GroundAnimalHabitatPoint>();

    public List<GroundAnimalHabitatPoint> WildCatAndChickenDepopPoints { get; private set; } = new List<GroundAnimalHabitatPoint>();

    public List<MechaHabitatPoint> MechaHabitatPoints { get; private set; } = new List<MechaHabitatPoint>();

    public List<FrogHabitatPoint> FrogHabitatPoints { get; private set; } = new List<FrogHabitatPoint>();

    public List<ButterflyHabitatPoint> ButterflyHabitatPoints { get; private set; } = new List<ButterflyHabitatPoint>();

    public List<BirdFlockHabitatPoint> BirdFlockHabitatPoints { get; private set; } = new List<BirdFlockHabitatPoint>();

    public void ClearAnimalPoints()
    {
      this.ActionPoints.Clear();
      this.WildGroundHabitatPoints.Clear();
      this.WildCatPopPoints.Clear();
      this.WildChickenPopPoints.Clear();
      this.WildCatDepopPoints.Clear();
      this.WildCatAndChickenPopPoints.Clear();
      this.WildChickenDepopPoints.Clear();
      this.WildCatAndChickenDepopPoints.Clear();
      this.MechaHabitatPoints.Clear();
      this.FrogHabitatPoints.Clear();
      this.ButterflyHabitatPoints.Clear();
      this.BirdFlockHabitatPoints.Clear();
    }

    public void ClearAllAnimals()
    {
      if (!Object.op_Inequality((Object) this._animalRoot, (Object) null))
        return;
      if (Object.op_Inequality((Object) ((Component) this._animalRoot).get_gameObject(), (Object) null))
        Object.Destroy((Object) ((Component) this._animalRoot).get_gameObject());
      this._animalRoot = (Transform) null;
    }

    public void ReleaseAnimal()
    {
      this.ClearAnimalPointBehavior();
      this.ClearAnimalPoints();
      this.ClearAllAnimals();
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      this.AnimalTable = new ReadOnlyDictionary<int, AnimalBase>((IDictionary<int, AnimalBase>) this.animalTable);
      if (AnimalManager._commandRefreshEvent != null)
        return;
      AnimalManager._commandRefreshEvent = new Subject<Unit>();
      ObservableExtensions.Subscribe<IList<Unit>>(Observable.Where<IList<Unit>>(Observable.TakeUntilDestroy<IList<Unit>>((IObservable<M0>) Observable.Buffer<Unit, Unit>((IObservable<M0>) AnimalManager._commandRefreshEvent, (IObservable<M1>) Observable.ThrottleFrame<Unit>((IObservable<M0>) AnimalManager._commandRefreshEvent, 1, (FrameCountType) 0)), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => !_.IsNullOrEmpty<Unit>())), (Action<M0>) (_ =>
      {
        CommandArea commandArea = Map.GetCommandArea();
        if (Object.op_Equality((Object) commandArea, (Object) null))
          return;
        commandArea.RefreshCommands();
      }));
    }

    protected override void OnDestroy()
    {
      if (!Singleton<AnimalManager>.IsInstance() || Object.op_Inequality((Object) Singleton<AnimalManager>.Instance, (Object) this))
        return;
      base.OnDestroy();
    }

    private void AddCommandableObject(AnimalBase animal)
    {
      if (Object.op_Equality((Object) animal, (Object) null) || !animal.IsCommandable)
        return;
      CommandArea commandArea = Map.GetCommandArea();
      if (Object.op_Equality((Object) commandArea, (Object) null))
        return;
      commandArea.AddCommandableObject((ICommandable) animal);
    }

    private void RemoveCommandableObject(AnimalBase animal)
    {
      if (Object.op_Equality((Object) animal, (Object) null) || !animal.IsCommandable)
        return;
      CommandArea commandArea = Map.GetCommandArea();
      if (Object.op_Equality((Object) commandArea, (Object) null))
        return;
      commandArea.RemoveCommandableObject((ICommandable) animal);
    }

    public void StartSubscribe()
    {
      if (!Singleton<Map>.IsInstance())
        return;
      if (this.enviromentSubscribeDisposable != null)
        this.enviromentSubscribeDisposable.Clear();
      this.enviromentSubscribeDisposable = new CompositeDisposable();
      EnvironmentSimulator _simulator = Singleton<Map>.Instance.Simulator;
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Weather>(Observable.OnErrorRetry<Weather, Exception>(Observable.Where<Weather>(Observable.TakeUntilDestroy<Weather>((IObservable<M0>) _simulator.OnWeatherChangedAsObservable(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M1>) (_ex =>
      {
        Debug.LogException(_ex);
        Debug.Log((object) string.Format("再購読します: {0}", (object) "OnWeatherChangedAsObservable"));
      })), (Action<M0>) (_weather => this.RefreshWeather(_simulator))), (ICollection<IDisposable>) this.enviromentSubscribeDisposable);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<AIProject.TimeZone>(Observable.OnErrorRetry<AIProject.TimeZone, Exception>(Observable.Where<AIProject.TimeZone>(Observable.TakeUntilDestroy<AIProject.TimeZone>((IObservable<M0>) _simulator.OnTimeZoneChangedAsObservable(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M1>) (_ex =>
      {
        Debug.LogException(_ex);
        Debug.Log((object) string.Format("再購読します: {0}", (object) "OnTimeZoneChangedAsObservable"));
      })), (Action<M0>) (_ => this.RefreshTimeZone(_simulator))), (ICollection<IDisposable>) this.enviromentSubscribeDisposable);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.OnErrorRetry<Unit, Exception>(Observable.Where<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) _simulator.OnEnvironmentChangedAsObservable(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M1>) (_ex =>
      {
        Debug.LogException(_ex);
        Debug.Log((object) string.Format("再購読します: {0}", (object) "OnEnvironmentChangedAsObservable"));
      })), (Action<M0>) (_ => this.RefreshEnvironment())), (ICollection<IDisposable>) this.enviromentSubscribeDisposable);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<TimeSpan>(Observable.OnErrorRetry<TimeSpan, Exception>(Observable.Where<TimeSpan>(Observable.TakeUntilDestroy<TimeSpan>((IObservable<M0>) _simulator.OnMinuteAsObservable(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M1>) (_ex =>
      {
        Debug.LogException(_ex);
        Debug.Log((object) string.Format("再購読します: {0}", (object) "OnMinuteAsObservable"));
      })), (Action<M0>) (timeSpan => this.OnElapsedMinute(timeSpan)), (Action<Exception>) (_ex => Debug.LogException(_ex)), (Action) (() => {})), (ICollection<IDisposable>) this.enviromentSubscribeDisposable);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<TimeSpan>(Observable.OnErrorRetry<TimeSpan, Exception>(Observable.Where<TimeSpan>(Observable.TakeUntilDestroy<TimeSpan>((IObservable<M0>) _simulator.OnSecondAsObservable(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M1>) (_ex =>
      {
        Debug.LogException(_ex);
        Debug.Log((object) string.Format("再購読します: {0}", (object) "OnSecondAsObservable"));
      })), (Action<M0>) (timeSpan => this.OnElapsedSecond(timeSpan)), (Action<Exception>) (_ex => Debug.LogException(_ex)), (Action) (() => {})), (ICollection<IDisposable>) this.enviromentSubscribeDisposable);
      LayerMask groundCheckLayer = Singleton<Manager.Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      if (this.mapAreaCheckDisposable != null)
        this.mapAreaCheckDisposable.Dispose();
      this.mapAreaCheckDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.FixedUpdateAsObservable((Component) Singleton<Map>.Instance), (Func<M0, bool>) (_ => !this.Animals.IsNullOrEmpty<AnimalBase>())), (Action<M0>) (_ =>
      {
        foreach (AnimalBase animal in this.Animals)
        {
          if (!Object.op_Equality((Object) animal, (Object) null) && animal.OnGroundCheck && animal.Active)
            animal.UpdateCurrentMapArea(groundCheckLayer);
        }
      }));
    }

    public void StopSubscribe()
    {
      if (this.enviromentSubscribeDisposable == null)
        return;
      this.enviromentSubscribeDisposable.Clear();
      this.enviromentSubscribeDisposable = (CompositeDisposable) null;
    }

    private void RefreshWeather(EnvironmentSimulator _simulator)
    {
      foreach (AnimalBase animal in this.Animals)
      {
        if (Object.op_Inequality((Object) animal, (Object) null))
          animal.OnWeatherChanged(_simulator);
      }
      if (!this.WildGroundHabitatPoints.IsNullOrEmpty<GroundAnimalHabitatPoint>())
      {
        foreach (GroundAnimalHabitatPoint groundHabitatPoint in this.WildGroundHabitatPoints)
        {
          if (!Object.op_Equality((Object) groundHabitatPoint, (Object) null))
          {
            bool isActive = groundHabitatPoint.IsActive;
            groundHabitatPoint.IsActive = !this.IsRain(_simulator.Weather);
            if (groundHabitatPoint.IsActive && !isActive)
              groundHabitatPoint.SetCoolTime();
          }
        }
      }
      if (!this.BirdFlockHabitatPoints.IsNullOrEmpty<BirdFlockHabitatPoint>())
      {
        foreach (BirdFlockHabitatPoint flockHabitatPoint in this.BirdFlockHabitatPoints)
        {
          if (!Object.op_Equality((Object) flockHabitatPoint, (Object) null))
            flockHabitatPoint.IsActive = !this.IsRain(_simulator.Weather) && !this.IsNight(_simulator.TimeZone);
        }
      }
      if (this.ButterflyHabitatPoints.IsNullOrEmpty<ButterflyHabitatPoint>())
        return;
      foreach (ButterflyHabitatPoint butterflyHabitatPoint in this.ButterflyHabitatPoints)
      {
        if (!Object.op_Equality((Object) butterflyHabitatPoint, (Object) null))
        {
          bool isActive = butterflyHabitatPoint.IsActive;
          butterflyHabitatPoint.IsActive = !this.IsRain(_simulator.Weather) && !this.IsNight(_simulator.TimeZone);
          if (butterflyHabitatPoint.IsActive && !isActive)
            butterflyHabitatPoint.IsCreate = false;
        }
      }
    }

    private void RefreshTimeZone(EnvironmentSimulator _simulator)
    {
      foreach (AnimalBase animal in this.Animals)
      {
        if (Object.op_Inequality((Object) animal, (Object) null))
          animal.OnTimeZoneChanged(_simulator);
      }
      if (!this.BirdFlockHabitatPoints.IsNullOrEmpty<BirdFlockHabitatPoint>())
      {
        foreach (BirdFlockHabitatPoint flockHabitatPoint in this.BirdFlockHabitatPoints)
        {
          if (!Object.op_Equality((Object) flockHabitatPoint, (Object) null))
            flockHabitatPoint.IsActive = !this.IsRain(_simulator.Weather) && !this.IsNight(_simulator.TimeZone);
        }
      }
      if (this.ButterflyHabitatPoints.IsNullOrEmpty<ButterflyHabitatPoint>())
        return;
      foreach (ButterflyHabitatPoint butterflyHabitatPoint in this.ButterflyHabitatPoints)
      {
        if (!Object.op_Equality((Object) butterflyHabitatPoint, (Object) null))
        {
          bool isActive = butterflyHabitatPoint.IsActive;
          butterflyHabitatPoint.IsActive = !this.IsRain(_simulator.Weather) && !this.IsNight(_simulator.TimeZone);
          if (butterflyHabitatPoint.IsActive && !isActive)
            butterflyHabitatPoint.IsCreate = false;
        }
      }
    }

    private void RefreshEnvironment()
    {
      if (!Singleton<Map>.IsInstance())
        return;
      EnvironmentSimulator simulator = Singleton<Map>.Instance.Simulator;
      foreach (AnimalBase animal in this.Animals)
      {
        if (Object.op_Inequality((Object) animal, (Object) null))
          animal.OnEnvironmentChanged(simulator);
      }
    }

    private void OnElapsedMinute(TimeSpan _deltaTime)
    {
      foreach (AnimalBase animal in this.Animals)
      {
        if (Object.op_Inequality((Object) animal, (Object) null))
          animal.OnMinuteUpdate(_deltaTime);
      }
    }

    private void OnElapsedSecond(TimeSpan _deltaTime)
    {
      foreach (AnimalBase animal in this.Animals)
      {
        if (Object.op_Inequality((Object) animal, (Object) null))
          animal.OnSecondUpdate(_deltaTime);
      }
    }

    public void RefreshStates(Map _map)
    {
      foreach (AnimalBase animal in this.Animals)
      {
        if (!Object.op_Equality((Object) animal, (Object) null))
        {
          animal.RefreshSearchTarget();
          animal.OnEnvironmentChanged(_map.Simulator);
        }
      }
    }

    private void AddPoint(AnimalPoint _point)
    {
      if (Object.op_Equality((Object) _point, (Object) null))
        return;
      switch (_point)
      {
        case AnimalActionPoint _:
          this.ActionPoints.Add(_point as AnimalActionPoint);
          break;
        case GroundAnimalHabitatPoint _:
          GroundAnimalHabitatPoint animalHabitatPoint = _point as GroundAnimalHabitatPoint;
          this.WildGroundHabitatPoints.Add(animalHabitatPoint);
          if (animalHabitatPoint.IsCatOnly)
          {
            if (animalHabitatPoint.IsPopPoint)
              this.WildCatPopPoints.Add(animalHabitatPoint);
            if (animalHabitatPoint.IsDepopPoint)
              this.WildCatDepopPoints.Add(animalHabitatPoint);
          }
          if (animalHabitatPoint.IsChickenOnly)
          {
            if (animalHabitatPoint.IsPopPoint)
              this.WildChickenPopPoints.Add(animalHabitatPoint);
            if (animalHabitatPoint.IsDepopPoint)
              this.WildChickenDepopPoints.Add(animalHabitatPoint);
          }
          if (!animalHabitatPoint.IsBoth)
            break;
          if (animalHabitatPoint.IsPopPoint)
            this.WildCatAndChickenPopPoints.Add(animalHabitatPoint);
          if (!animalHabitatPoint.IsDepopPoint)
            break;
          this.WildCatAndChickenPopPoints.Add(animalHabitatPoint);
          break;
        case MechaHabitatPoint _:
          this.MechaHabitatPoints.Add(_point as MechaHabitatPoint);
          break;
        case FrogHabitatPoint _:
          this.FrogHabitatPoints.Add(_point as FrogHabitatPoint);
          break;
        case ButterflyHabitatPoint _:
          this.ButterflyHabitatPoints.Add(_point as ButterflyHabitatPoint);
          break;
        case BirdFlockHabitatPoint _:
          this.BirdFlockHabitatPoints.Add(_point as BirdFlockHabitatPoint);
          break;
      }
    }

    private void LogAnimalPoint()
    {
    }

    [DebuggerHidden]
    private IEnumerator SetupGroundAnimalHabitatPointsAsync(
      Waypoint[] _waypoints,
      List<GroundAnimalHabitatPoint> _popPoints,
      List<GroundAnimalHabitatPoint> _depopPoints,
      int _wait)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AnimalManager.\u003CSetupGroundAnimalHabitatPointsAsync\u003Ec__Iterator0()
      {
        _popPoints = _popPoints,
        _depopPoints = _depopPoints,
        _waypoints = _waypoints,
        _wait = _wait
      };
    }

    [DebuggerHidden]
    public IEnumerator SetupPointsAsync(PointManager _pointManager)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AnimalManager.\u003CSetupPointsAsync\u003Ec__Iterator1()
      {
        _pointManager = _pointManager,
        \u0024this = this
      };
    }

    public static int GetAnimalCount(AnimalTypes _animalType, BreedingTypes _breedingType)
    {
      if (!Singleton<AnimalManager>.IsInstance())
        return -1;
      Dictionary<AnimalTypes, Dictionary<BreedingTypes, int>> animalCountTable = Singleton<AnimalManager>.Instance.AnimalCountTable;
      if (!animalCountTable.ContainsKey(_animalType))
        animalCountTable[_animalType] = new Dictionary<BreedingTypes, int>();
      int num1 = 0;
      if (animalCountTable[_animalType].TryGetValue(_breedingType, out num1))
        return num1;
      int num2 = 0;
      animalCountTable[_animalType][_breedingType] = num2;
      return num2;
    }

    public static bool AnimalCountUp(AnimalTypes _animalType, BreedingTypes _breedingType)
    {
      if (!Singleton<AnimalManager>.IsInstance())
        return false;
      Dictionary<AnimalTypes, Dictionary<BreedingTypes, int>> animalCountTable = Singleton<AnimalManager>.Instance.AnimalCountTable;
      if (!animalCountTable.ContainsKey(_animalType))
        animalCountTable[_animalType] = new Dictionary<BreedingTypes, int>();
      int num = 0;
      animalCountTable[_animalType].TryGetValue(_breedingType, out num);
      animalCountTable[_animalType][_breedingType] = num + 1;
      return true;
    }

    public static int GetAnimalCount(AnimalBase _animal)
    {
      return Object.op_Equality((Object) _animal, (Object) null) ? -1 : AnimalManager.GetAnimalCount(_animal.AnimalType, _animal.BreedingType);
    }

    public static bool AnimalCountUp(AnimalBase _animal)
    {
      return !Object.op_Equality((Object) _animal, (Object) null) && AnimalManager.AnimalCountUp(_animal.AnimalType, _animal.BreedingType);
    }

    public bool ContainsID(int _id)
    {
      return this.animalKeyList.Contains(_id) || this.AnimalTable.ContainsKey(_id);
    }

    public bool CheckAnimalType(
      AnimalBase _animal,
      AnimalTypes _animalType,
      BreedingTypes _breedingType)
    {
      return !Object.op_Equality((Object) _animal, (Object) null) && (_animal.AnimalType & _animalType) != (AnimalTypes) 0 && _animal.BreedingType == _breedingType;
    }

    public bool AddAnimal(AnimalBase _animal)
    {
      if (Object.op_Equality((Object) _animal, (Object) null))
        return false;
      while (this.ContainsID(this.AnimalCount))
        ++this.AnimalCount;
      _animal.SetID(this.AnimalCount, 0);
      this.AddAnimalTable(_animal);
      ++this.AnimalCount;
      return true;
    }

    public bool RemoveAnimal(int _id, bool _destroy = true)
    {
      AnimalBase _animal;
      if (!this.animalTable.TryGetValue(_id, out _animal))
        return false;
      this.RemoveAnimalTable(_animal);
      if (_destroy)
        Object.Destroy((Object) ((Component) _animal).get_gameObject());
      return true;
    }

    public bool RemoveAnimal(AnimalBase _animal, bool _destroy = true)
    {
      return !Object.op_Equality((Object) _animal, (Object) null) && this.RemoveAnimal(_animal.AnimalID, _destroy);
    }

    private void AddList<T1, T2>(T1 _obj, List<T2> _list)
      where T1 : Object
      where T2 : Object
    {
      if (Object.op_Equality((Object) (object) _obj, (Object) null) || !(_obj is T2 obj) || _list.Contains(obj))
        return;
      _list.Add(obj);
    }

    private void AddList<T>(AnimalBase _animal, AnimalTypes _animalType, List<T> _list) where T : AnimalBase
    {
      if (Object.op_Equality((Object) _animal, (Object) null) || !(_animal is T) || ((_animal.AnimalType & _animalType) == (AnimalTypes) 0 || ((IEnumerable<AnimalBase>) _list).Contains<AnimalBase>(_animal)))
        return;
      _list.Add(_animal as T);
    }

    private void AddList<T>(AnimalBase _animal, BreedingTypes _breedingType, List<T> _list) where T : AnimalBase
    {
      if (Object.op_Equality((Object) _animal, (Object) null) || !(_animal is T) || (_animal.BreedingType != _breedingType || ((IEnumerable<AnimalBase>) _list).Contains<AnimalBase>(_animal)))
        return;
      _list.Add(_animal as T);
    }

    private void AddList<T>(
      AnimalBase _animal,
      AnimalTypes _animalType,
      BreedingTypes _breedingType,
      List<T> _list)
      where T : AnimalBase
    {
      if (Object.op_Equality((Object) _animal, (Object) null) || !(_animal is T) || (!this.CheckAnimalType(_animal, _animalType, _breedingType) || ((IEnumerable<AnimalBase>) _list).Contains<AnimalBase>(_animal)))
        return;
      _list.Add(_animal as T);
    }

    private void RemoveList<T>(AnimalBase _animal, List<T> _list) where T : AnimalBase
    {
      if (Object.op_Equality((Object) _animal, (Object) null) || !(_animal is T) || !((IEnumerable<AnimalBase>) _list).Contains<AnimalBase>(_animal))
        return;
      _list.Remove(_animal as T);
    }

    private void AddTable<T1, T2>(T1 _obj, int _id, Dictionary<int, T2> _table, List<int> _keys)
      where T1 : Object
      where T2 : Object
    {
      if (!(_obj is T2 obj) || _table.ContainsKey(_id))
        return;
      _table[_id] = obj;
      if (_keys.Contains(_id))
        return;
      _keys.Add(_id);
    }

    private void RemoveTable<T1, T2>(
      T1 _obj,
      int _id,
      Dictionary<int, T2> _table,
      List<int> _keys)
      where T1 : Object
      where T2 : Object
    {
      T2 obj1;
      if (!(_obj is T2 obj2) || !_table.TryGetValue(_id, out obj1) || !Object.op_Equality((Object) (object) obj1, (Object) (object) obj2))
        return;
      _table.Remove(_id);
      if (!_keys.Contains(_id))
        return;
      _keys.Remove(_id);
    }

    public void AddTargetAnimals(AgentActor _agent)
    {
      if (Object.op_Equality((Object) _agent, (Object) null))
        return;
      _agent.TargetAnimals.Clear();
      if (this.Animals.IsNullOrEmpty<AnimalBase>())
        return;
      foreach (AnimalBase animal in this.Animals)
      {
        if (!Object.op_Equality((Object) animal, (Object) null) && animal.AgentInsight)
          _agent.AddAnimal(animal);
      }
      this.Animals.RemoveAll((Predicate<AnimalBase>) (x => Object.op_Equality((Object) x, (Object) null) || Object.op_Equality((Object) ((Component) x).get_gameObject(), (Object) null)));
    }

    private bool AddAnimalTable(AnimalBase _animal)
    {
      if (Object.op_Equality((Object) _animal, (Object) null) || this.ContainsID(_animal.GetAnimalInfo().AnimalID))
        return false;
      BreedingTypes _breedingType1 = BreedingTypes.Pet;
      BreedingTypes _breedingType2 = BreedingTypes.Wild;
      this.AddTable<AnimalBase, AnimalBase>(_animal, _animal.AnimalID, this.animalTable, this.animalKeyList);
      this.AddList<AnimalBase, AnimalBase>(_animal, this.Animals);
      this.AddList<AnimalBase>(_animal, _breedingType2, this.WildAnimals);
      this.AddList<AnimalBase>(_animal, _breedingType1, this.PetAnimals);
      this.AddList<WildGround>(_animal, AnimalTypes.Cat, _breedingType2, this.WildCats);
      this.AddList<WildGround>(_animal, AnimalTypes.Chicken, _breedingType2, this.WildChickens);
      this.AddList<WildMecha>(_animal, AnimalTypes.Mecha, _breedingType2, this.WildMechas);
      this.AddList<WildFrog>(_animal, AnimalTypes.Frog, _breedingType2, this.WildFrogs);
      this.AddList<WildButterfly>(_animal, AnimalTypes.Butterfly, _breedingType2, this.WildButterflies);
      this.AddList<WildBirdFlock>(_animal, AnimalTypes.BirdFlock, _breedingType2, this.WildBirdFlocks);
      this.AddList<WalkingPetAnimal>(_animal, AnimalTypes.Cat, _breedingType1, this.PetCats);
      this.AddList<PetChicken>(_animal, AnimalTypes.Chicken, _breedingType1, this.PetChickens);
      this.AddList<PetFish>(_animal, AnimalTypes.Fish, _breedingType1, this.PetFishes);
      this.AddList<FlyingPetAnimal>(_animal, AnimalTypes.Butterfly, _breedingType1, this.PetButterflies);
      this.AddList<WalkingPetAnimal>(_animal, AnimalTypes.Mecha, _breedingType1, this.PetMechas);
      this.AddList<MovingPetAnimal>(_animal, AnimalTypes.Ground | AnimalTypes.Flying, _breedingType1, this.MovingPets);
      this.AddList<WalkingPetAnimal>(_animal, AnimalTypes.Ground, _breedingType1, this.WalkingPets);
      this.AddList<FlyingPetAnimal>(_animal, AnimalTypes.Flying, _breedingType1, this.FlyingPets);
      int animalTypeId = _animal.AnimalTypeID;
      BreedingTypes breedingType = _animal.BreedingType;
      bool flag = animalTypeId == 0 && breedingType == BreedingTypes.Pet;
      if (this.ActiveMapScene && flag)
        ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>(Observable.SkipWhile<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) _animal), (Func<M0, bool>) (_ => !_animal.Active)), 1), (Action<M0>) (_ =>
        {
          if (!Singleton<MapUIContainer>.IsInstance() || !Singleton<AnimalManager>.IsInstance())
            return;
          MiniMapControler minimapUi = Singleton<MapUIContainer>.Instance.MinimapUI;
          if (!Object.op_Inequality((Object) minimapUi, (Object) null))
            return;
          minimapUi.PetIconSet();
        }));
      if (Singleton<Map>.IsInstance())
      {
        Map instance = Singleton<Map>.Instance;
        if (Object.op_Inequality((Object) instance, (Object) null) && instance.AgentTable != null && _animal.AgentInsight)
        {
          using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = instance.AgentTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              AgentActor agentActor = enumerator.Current.Value;
              if (!Object.op_Equality((Object) agentActor, (Object) null))
                agentActor.AddAnimal(_animal);
            }
          }
        }
      }
      this.AddCommandableObject(_animal);
      return true;
    }

    private bool RemoveAnimalTable(AnimalBase _animal)
    {
      if (Object.op_Equality((Object) _animal, (Object) null) || !this.ContainsID(_animal.GetAnimalInfo().AnimalID))
        return false;
      this.RemoveTable<AnimalBase, AnimalBase>(_animal, _animal.AnimalID, this.animalTable, this.animalKeyList);
      this.RemoveList<AnimalBase>(_animal, this.Animals);
      this.RemoveList<AnimalBase>(_animal, this.WildAnimals);
      this.RemoveList<AnimalBase>(_animal, this.PetAnimals);
      this.RemoveList<WildGround>(_animal, this.WildCats);
      this.RemoveList<WildGround>(_animal, this.WildChickens);
      this.RemoveList<WildMecha>(_animal, this.WildMechas);
      this.RemoveList<WildFrog>(_animal, this.WildFrogs);
      this.RemoveList<WildButterfly>(_animal, this.WildButterflies);
      this.RemoveList<WildBirdFlock>(_animal, this.WildBirdFlocks);
      this.RemoveList<WalkingPetAnimal>(_animal, this.PetCats);
      this.RemoveList<PetChicken>(_animal, this.PetChickens);
      this.RemoveList<PetFish>(_animal, this.PetFishes);
      this.RemoveList<FlyingPetAnimal>(_animal, this.PetButterflies);
      this.RemoveList<WalkingPetAnimal>(_animal, this.PetMechas);
      this.RemoveList<MovingPetAnimal>(_animal, this.MovingPets);
      this.RemoveList<WalkingPetAnimal>(_animal, this.WalkingPets);
      this.RemoveList<FlyingPetAnimal>(_animal, this.FlyingPets);
      if (Singleton<MapUIContainer>.IsInstance() && Singleton<AnimalManager>.IsInstance())
      {
        int animalTypeId = _animal.AnimalTypeID;
        BreedingTypes breedingType = _animal.BreedingType;
        if (animalTypeId == 0 && breedingType == BreedingTypes.Pet)
        {
          MiniMapControler minimapUi = Singleton<MapUIContainer>.Instance.MinimapUI;
          if (Object.op_Inequality((Object) minimapUi, (Object) null))
            minimapUi.PetIconSet();
        }
      }
      if (Singleton<Map>.IsInstance())
      {
        Map instance = Singleton<Map>.Instance;
        if (Object.op_Inequality((Object) instance, (Object) null) && instance.AgentTable != null && _animal.AgentInsight)
        {
          using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = instance.AgentTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              AgentActor agentActor = enumerator.Current.Value;
              if (!Object.op_Equality((Object) agentActor, (Object) null))
                agentActor.RemoveAnimal(_animal);
            }
          }
        }
      }
      this.RemoveCommandableObject(_animal);
      return true;
    }

    public GameObject GetAnimalPrefab(int _animalTypeID, int _breedingTypeID)
    {
      GameObject gameObject1 = (GameObject) null;
      Dictionary<int, GameObject> dictionary1;
      if (this.AnimalBaseObjectTable.TryGetValue(_animalTypeID, out dictionary1) && dictionary1 != null)
      {
        if (dictionary1.TryGetValue(_breedingTypeID, out gameObject1))
          return gameObject1;
      }
      else
      {
        Dictionary<int, GameObject> dictionary2 = new Dictionary<int, GameObject>();
        this.AnimalBaseObjectTable[_animalTypeID] = dictionary2;
        dictionary1 = dictionary2;
      }
      Dictionary<int, AssetBundleInfo> source;
      if (!Singleton<Manager.Resources>.Instance.AnimalTable.AnimalBaseObjInfoTable.TryGetValue(_animalTypeID, out source) || source.IsNullOrEmpty<int, AssetBundleInfo>())
      {
        GameObject gameObject2 = (GameObject) null;
        dictionary1[_breedingTypeID] = gameObject2;
        return gameObject2;
      }
      AssetBundleInfo assetBundleInfo;
      if (!source.TryGetValue(_breedingTypeID, out assetBundleInfo))
      {
        GameObject gameObject2 = (GameObject) null;
        dictionary1[_breedingTypeID] = gameObject2;
        return gameObject2;
      }
      gameObject1 = CommonLib.LoadAsset<GameObject>((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset, false, (string) assetBundleInfo.manifest);
      if (Object.op_Inequality((Object) gameObject1, (Object) null))
        MapScene.AddAssetBundlePath((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.manifest);
      return gameObject1;
    }

    public T Create<T>(int _animalTypeID, int _breedingTypeID) where T : AnimalBase
    {
      if (!Singleton<Manager.Resources>.IsInstance() || !Singleton<Map>.IsInstance())
        return (T) null;
      Manager.Resources instance = Singleton<Manager.Resources>.Instance;
      GameObject animalPrefab = this.GetAnimalPrefab(_animalTypeID, _breedingTypeID);
      if (Object.op_Equality((Object) animalPrefab, (Object) null))
        return (T) null;
      Dictionary<int, AnimalModelInfo> source;
      if (!instance.AnimalTable.ModelInfoTable.TryGetValue(_animalTypeID, out source) || source.IsNullOrEmpty<int, AnimalModelInfo>())
        return (T) null;
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) animalPrefab, Vector3.get_zero(), Quaternion.get_identity(), this.AnimalRoot);
      T _animal = gameObject.GetComponent<T>();
      if (Object.op_Equality((Object) (object) (T) _animal, (Object) null))
      {
        Object.Destroy((Object) gameObject);
        return (T) null;
      }
      this.AddAnimal((AnimalBase) _animal);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) (object) (T) _animal), (Action<M0>) (_ => this.RemoveAnimal((AnimalBase) _animal, false)));
      int animalCount = AnimalManager.GetAnimalCount((AnimalBase) _animal);
      AnimalManager.AnimalCountUp((AnimalBase) _animal);
      _animal.ObjName = string.Format("{0}_{1}", (object) ((Object) animalPrefab).get_name(), (object) animalCount.ToString("00"));
      KeyValuePair<int, AnimalModelInfo> keyValuePair = source.Rand<int, AnimalModelInfo>();
      _animal.SetModelInfo(keyValuePair.Value);
      return _animal;
    }

    public AnimalBase CreateBase(int _animalTypeID, int _breedingTypeID)
    {
      if (!Singleton<Manager.Resources>.IsInstance() || !Singleton<Map>.IsInstance())
        return (AnimalBase) null;
      Manager.Resources instance = Singleton<Manager.Resources>.Instance;
      GameObject animalPrefab = this.GetAnimalPrefab(_animalTypeID, _breedingTypeID);
      if (Object.op_Equality((Object) animalPrefab, (Object) null))
        return (AnimalBase) null;
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) animalPrefab, Vector3.get_zero(), Quaternion.get_identity(), this.AnimalRoot);
      AnimalBase _animal = (AnimalBase) gameObject.GetComponent<AnimalBase>();
      if (Object.op_Equality((Object) _animal, (Object) null))
      {
        Object.Destroy((Object) gameObject);
        return (AnimalBase) null;
      }
      this.AddAnimal(_animal);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) _animal), (Action<M0>) (_ => this.RemoveAnimal(_animal, false)));
      int animalCount = AnimalManager.GetAnimalCount(_animal);
      AnimalManager.AnimalCountUp(_animal);
      _animal.ObjName = string.Format("{0}_{1}", (object) ((Object) animalPrefab).get_name(), (object) animalCount.ToString("00"));
      return _animal;
    }

    public bool CheckAvailablePoint(Vector3 _position, bool _checkDistance, bool _checkVisible)
    {
      PlayerActor player = Map.GetPlayer();
      Camera cameraComponent = Map.GetCameraComponent(player);
      bool flag1 = false;
      bool flag2 = false;
      Manager.Resources resources = !Singleton<Manager.Resources>.IsInstance() ? (Manager.Resources) null : Singleton<Manager.Resources>.Instance;
      if (_checkDistance && Object.op_Inequality((Object) player, (Object) null))
        flag1 = (double) Vector3.Distance(player.Position, _position) <= (double) resources.AnimalDefinePack.SystemInfo.PopDistance;
      if (_checkVisible && Object.op_Inequality((Object) cameraComponent, (Object) null))
      {
        Vector3 viewportPoint = cameraComponent.WorldToViewportPoint(_position);
        if (0.0 <= viewportPoint.z)
        {
          Rect rect;
          ((Rect) ref rect).\u002Ector(0.0f, 0.0f, 1f, 1f);
          float num = !Object.op_Inequality((Object) resources, (Object) null) ? 1f : resources.AnimalDefinePack.SystemInfo.ViewportPointScale;
          Vector3 vector3_1;
          ((Vector3) ref vector3_1).\u002Ector(0.5f, 0.5f, 0.0f);
          Vector3 vector3_2;
          ((Vector3) ref vector3_2).\u002Ector(num, num, 1f);
          Vector3 vector3_3 = Vector3.op_Subtraction(viewportPoint, vector3_1);
          ((Vector3) ref vector3_3).Scale(vector3_2);
          Vector3 vector3_4 = Vector3.op_Addition(vector3_3, vector3_1);
          flag2 = ((Rect) ref rect).Contains(vector3_4);
        }
        else
          flag2 = false;
      }
      return !flag1 && !flag2;
    }

    public bool CheckAvailablePointPreferDistance(Vector3 _position)
    {
      AnimalDefinePack animalDefinePack = !Singleton<Manager.Resources>.IsInstance() ? (AnimalDefinePack) null : Singleton<Manager.Resources>.Instance.AnimalDefinePack;
      if (Object.op_Equality((Object) animalDefinePack, (Object) null))
        return false;
      PlayerActor player = Map.GetPlayer();
      Camera cameraComponent = Map.GetCameraComponent(player);
      bool flag1 = false;
      bool flag2 = false;
      if (Object.op_Inequality((Object) player, (Object) null))
        flag1 = (double) Vector3.Distance(player.Position, _position) <= (double) animalDefinePack.SystemInfo.PopDistance;
      if (!flag1)
        return true;
      if (Object.op_Inequality((Object) cameraComponent, (Object) null))
      {
        Vector3 viewportPoint = cameraComponent.WorldToViewportPoint(_position);
        if (0.0 <= viewportPoint.z)
        {
          Rect rect;
          ((Rect) ref rect).\u002Ector(0.0f, 0.0f, 1f, 1f);
          float viewportPointScale = animalDefinePack.SystemInfo.ViewportPointScale;
          Vector3 vector3_1;
          ((Vector3) ref vector3_1).\u002Ector(0.5f, 0.5f, 0.0f);
          Vector3 vector3_2;
          ((Vector3) ref vector3_2).\u002Ector(viewportPointScale, viewportPointScale, 1f);
          Vector3 vector3_3 = Vector3.op_Subtraction(viewportPoint, vector3_1);
          ((Vector3) ref vector3_3).Scale(vector3_2);
          Vector3 vector3_4 = Vector3.op_Addition(vector3_3, vector3_1);
          flag2 = ((Rect) ref rect).Contains(vector3_4);
        }
        else
          flag2 = false;
      }
      return !flag1 && !flag2;
    }

    public void SetupSaveDataWildAnimals()
    {
      WorldData worldData = !Singleton<Game>.IsInstance() ? (WorldData) null : Singleton<Game>.Instance.WorldData;
      if (worldData == null)
        return;
      Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>> dictionary1 = worldData.WildAnimalTable;
      if (dictionary1 == null)
      {
        Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>> dictionary2 = new Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>>();
        worldData.WildAnimalTable = dictionary2;
        dictionary1 = dictionary2;
      }
      if (!this.WildChickenPopPoints.IsNullOrEmpty<GroundAnimalHabitatPoint>())
      {
        Dictionary<int, WildAnimalData> source;
        if (!dictionary1.TryGetValue(AnimalTypes.Chicken, out source) || source == null)
        {
          Dictionary<int, WildAnimalData> dictionary2 = new Dictionary<int, WildAnimalData>();
          dictionary1[AnimalTypes.Chicken] = dictionary2;
          source = dictionary2;
        }
        List<int> toRelease1 = ListPool<int>.Get();
        foreach (GroundAnimalHabitatPoint wildChickenPopPoint in this.WildChickenPopPoints)
        {
          if (!Object.op_Equality((Object) wildChickenPopPoint, (Object) null))
          {
            int id = wildChickenPopPoint.ID;
            WildGround user = wildChickenPopPoint.User;
            WildAnimalData wildAnimalData1;
            if (!source.TryGetValue(id, out wildAnimalData1) || wildAnimalData1 == null)
            {
              WildAnimalData wildAnimalData2 = new WildAnimalData();
              source[id] = wildAnimalData2;
              wildAnimalData1 = wildAnimalData2;
            }
            wildAnimalData1.CoolTime = !Object.op_Inequality((Object) user, (Object) null) ? wildChickenPopPoint.CoolTimeCounter : 0.0f;
            wildAnimalData1.IsAdded = Object.op_Inequality((Object) user, (Object) null);
            if (!toRelease1.Contains(id))
              toRelease1.Add(id);
          }
        }
        if (!source.IsNullOrEmpty<int, WildAnimalData>())
        {
          List<int> toRelease2 = ListPool<int>.Get();
          foreach (KeyValuePair<int, WildAnimalData> keyValuePair in source)
          {
            if (!toRelease1.Contains(keyValuePair.Key))
              toRelease2.Add(keyValuePair.Key);
          }
          foreach (int key in toRelease2)
            source.Remove(key);
          ListPool<int>.Release(toRelease2);
        }
        ListPool<int>.Release(toRelease1);
      }
      if (!this.WildCatPopPoints.IsNullOrEmpty<GroundAnimalHabitatPoint>())
      {
        Dictionary<int, WildAnimalData> source;
        if (!dictionary1.TryGetValue(AnimalTypes.Cat, out source) || source == null)
        {
          Dictionary<int, WildAnimalData> dictionary2 = new Dictionary<int, WildAnimalData>();
          dictionary1[AnimalTypes.Cat] = dictionary2;
          source = dictionary2;
        }
        List<int> toRelease1 = ListPool<int>.Get();
        foreach (GroundAnimalHabitatPoint wildCatPopPoint in this.WildCatPopPoints)
        {
          if (!Object.op_Equality((Object) wildCatPopPoint, (Object) null))
          {
            int id = wildCatPopPoint.ID;
            WildGround user = wildCatPopPoint.User;
            WildAnimalData wildAnimalData1;
            if (!source.TryGetValue(id, out wildAnimalData1) || wildAnimalData1 == null)
            {
              WildAnimalData wildAnimalData2 = new WildAnimalData();
              source[id] = wildAnimalData2;
              wildAnimalData1 = wildAnimalData2;
            }
            wildAnimalData1.CoolTime = !Object.op_Inequality((Object) user, (Object) null) ? wildCatPopPoint.CoolTimeCounter : 0.0f;
            wildAnimalData1.IsAdded = Object.op_Inequality((Object) user, (Object) null);
            if (!toRelease1.Contains(id))
              toRelease1.Add(id);
          }
        }
        if (!source.IsNullOrEmpty<int, WildAnimalData>())
        {
          List<int> toRelease2 = ListPool<int>.Get();
          foreach (KeyValuePair<int, WildAnimalData> keyValuePair in source)
          {
            if (!toRelease1.Contains(keyValuePair.Key))
              toRelease2.Add(keyValuePair.Key);
          }
          foreach (int key in toRelease2)
            source.Remove(key);
          ListPool<int>.Release(toRelease2);
        }
        ListPool<int>.Release(toRelease1);
      }
      if (!this.WildCatAndChickenPopPoints.IsNullOrEmpty<GroundAnimalHabitatPoint>())
      {
        AnimalTypes key1 = AnimalTypes.Cat | AnimalTypes.Chicken;
        Dictionary<int, WildAnimalData> source;
        if (!dictionary1.TryGetValue(key1, out source) || source == null)
        {
          Dictionary<int, WildAnimalData> dictionary2 = new Dictionary<int, WildAnimalData>();
          dictionary1[key1] = dictionary2;
          source = dictionary2;
        }
        List<int> toRelease1 = ListPool<int>.Get();
        foreach (GroundAnimalHabitatPoint wildCatPopPoint in this.WildCatPopPoints)
        {
          if (!Object.op_Equality((Object) wildCatPopPoint, (Object) null))
          {
            int id = wildCatPopPoint.ID;
            WildGround user = wildCatPopPoint.User;
            WildAnimalData wildAnimalData1;
            if (!source.TryGetValue(id, out wildAnimalData1) || wildAnimalData1 == null)
            {
              WildAnimalData wildAnimalData2 = new WildAnimalData();
              source[id] = wildAnimalData2;
              wildAnimalData1 = wildAnimalData2;
            }
            wildAnimalData1.CoolTime = !Object.op_Inequality((Object) user, (Object) null) ? wildCatPopPoint.CoolTimeCounter : 0.0f;
            wildAnimalData1.IsAdded = Object.op_Inequality((Object) user, (Object) null);
            if (!toRelease1.Contains(id))
              toRelease1.Add(id);
          }
        }
        if (!source.IsNullOrEmpty<int, WildAnimalData>())
        {
          List<int> toRelease2 = ListPool<int>.Get();
          foreach (KeyValuePair<int, WildAnimalData> keyValuePair in source)
          {
            if (!toRelease1.Contains(keyValuePair.Key))
              toRelease2.Add(keyValuePair.Key);
          }
          foreach (int key2 in toRelease2)
            source.Remove(key2);
          ListPool<int>.Release(toRelease2);
        }
        ListPool<int>.Release(toRelease1);
      }
      if (!this.FrogHabitatPoints.IsNullOrEmpty<FrogHabitatPoint>())
      {
        Dictionary<int, WildAnimalData> source;
        if (!dictionary1.TryGetValue(AnimalTypes.Frog, out source) || source == null)
        {
          Dictionary<int, WildAnimalData> dictionary2 = new Dictionary<int, WildAnimalData>();
          dictionary1[AnimalTypes.Frog] = dictionary2;
          source = dictionary2;
        }
        List<int> toRelease1 = ListPool<int>.Get();
        foreach (FrogHabitatPoint frogHabitatPoint in this.FrogHabitatPoints)
        {
          if (!Object.op_Equality((Object) frogHabitatPoint, (Object) null))
          {
            int id = frogHabitatPoint.ID;
            WildFrog user = frogHabitatPoint.User;
            WildAnimalData wildAnimalData1;
            if (!source.TryGetValue(id, out wildAnimalData1) || wildAnimalData1 == null)
            {
              WildAnimalData wildAnimalData2 = new WildAnimalData();
              source[id] = wildAnimalData2;
              wildAnimalData1 = wildAnimalData2;
            }
            wildAnimalData1.CoolTime = !Object.op_Inequality((Object) user, (Object) null) ? frogHabitatPoint.CoolTimeCounter : 0.0f;
            wildAnimalData1.IsAdded = Object.op_Inequality((Object) user, (Object) null);
            if (!toRelease1.Contains(id))
              toRelease1.Add(id);
          }
        }
        if (!source.IsNullOrEmpty<int, WildAnimalData>())
        {
          List<int> toRelease2 = ListPool<int>.Get();
          foreach (KeyValuePair<int, WildAnimalData> keyValuePair in source)
          {
            if (!toRelease1.Contains(keyValuePair.Key))
              toRelease2.Add(keyValuePair.Key);
          }
          foreach (int key in toRelease2)
            source.Remove(key);
          ListPool<int>.Release(toRelease2);
        }
        ListPool<int>.Release(toRelease1);
      }
      if (this.MechaHabitatPoints.IsNullOrEmpty<MechaHabitatPoint>())
        return;
      Dictionary<int, WildAnimalData> source1;
      if (!dictionary1.TryGetValue(AnimalTypes.Mecha, out source1) || source1 == null)
      {
        Dictionary<int, WildAnimalData> dictionary2 = new Dictionary<int, WildAnimalData>();
        dictionary1[AnimalTypes.Mecha] = dictionary2;
        source1 = dictionary2;
      }
      List<int> toRelease3 = ListPool<int>.Get();
      foreach (MechaHabitatPoint mechaHabitatPoint in this.MechaHabitatPoints)
      {
        if (!Object.op_Equality((Object) mechaHabitatPoint, (Object) null))
        {
          int id = mechaHabitatPoint.ID;
          WildMecha user = mechaHabitatPoint.User;
          WildAnimalData wildAnimalData1;
          if (!source1.TryGetValue(id, out wildAnimalData1) || wildAnimalData1 == null)
          {
            WildAnimalData wildAnimalData2 = new WildAnimalData();
            source1[id] = wildAnimalData2;
            wildAnimalData1 = wildAnimalData2;
          }
          wildAnimalData1.CoolTime = !Object.op_Inequality((Object) user, (Object) null) ? mechaHabitatPoint.CoolTimeCounter : 0.0f;
          wildAnimalData1.IsAdded = Object.op_Inequality((Object) user, (Object) null);
          if (!toRelease3.Contains(id))
            toRelease3.Add(id);
        }
      }
      if (!source1.IsNullOrEmpty<int, WildAnimalData>())
      {
        List<int> toRelease1 = ListPool<int>.Get();
        foreach (KeyValuePair<int, WildAnimalData> keyValuePair in source1)
        {
          if (!toRelease3.Contains(keyValuePair.Key))
            toRelease1.Add(keyValuePair.Key);
        }
        foreach (int key in toRelease1)
          source1.Remove(key);
        ListPool<int>.Release(toRelease1);
      }
      ListPool<int>.Release(toRelease3);
    }

    public enum AnimalType
    {
      Cat,
      Chicken,
      Fish,
      Butterfly,
      Mecha,
      Frog,
      BirdFlock,
      CatWithFish,
      CatTank,
      Chick,
      Fairy,
      DarkSpirit,
    }
  }
}
