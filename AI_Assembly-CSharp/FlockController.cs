// Decompiled with JetBrains decompiler
// Type: FlockController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
  public FlockChild _childPrefab;
  public int _childAmount;
  public bool _slowSpawn;
  public float _spawnSphere;
  public float _spawnSphereHeight;
  public float _spawnSphereDepth;
  public float _minSpeed;
  public float _maxSpeed;
  public float _minScale;
  public float _maxScale;
  public float _soarFrequency;
  public string _soarAnimation;
  public string _flapAnimation;
  public string _idleAnimation;
  public float _diveValue;
  public float _diveFrequency;
  public float _minDamping;
  public float _maxDamping;
  public float _waypointDistance;
  public float _minAnimationSpeed;
  public float _maxAnimationSpeed;
  public float _randomPositionTimer;
  public float _positionSphere;
  public float _positionSphereHeight;
  public float _positionSphereDepth;
  public bool _childTriggerPos;
  public bool _forceChildWaypoints;
  public float _forcedRandomDelay;
  public bool _flatFly;
  public bool _flatSoar;
  public bool _birdAvoid;
  public int _birdAvoidHorizontalForce;
  public bool _birdAvoidDown;
  public bool _birdAvoidUp;
  public int _birdAvoidVerticalForce;
  public float _birdAvoidDistanceMax;
  public float _birdAvoidDistanceMin;
  public float _soarMaxTime;
  public LayerMask _avoidanceMask;
  public List<FlockChild> _roamers;
  public Vector3 _posBuffer;
  public int _updateDivisor;
  public float _newDelta;
  public int _updateCounter;
  public float _activeChildren;
  public bool _groupChildToNewTransform;
  public Transform _groupTransform;
  public string _groupName;
  public bool _groupChildToFlock;
  public Vector3 _startPosOffset;
  public Transform _thisT;

  public FlockController()
  {
    base.\u002Ector();
  }

  public void Start()
  {
    this._thisT = ((Component) this).get_transform();
    if ((double) this._positionSphereDepth == -1.0)
      this._positionSphereDepth = this._positionSphere;
    if ((double) this._spawnSphereDepth == -1.0)
      this._spawnSphereDepth = this._spawnSphere;
    this._posBuffer = Vector3.op_Addition(this._thisT.get_position(), this._startPosOffset);
    if (!this._slowSpawn)
      this.AddChild(this._childAmount);
    if ((double) this._randomPositionTimer <= 0.0)
      return;
    this.InvokeRepeating("SetFlockRandomPosition", this._randomPositionTimer, this._randomPositionTimer);
  }

  public void AddChild(int amount)
  {
    if (this._groupChildToNewTransform)
      this.InstantiateGroup();
    for (int index = 0; index < amount; ++index)
    {
      FlockChild flockChild = (FlockChild) Object.Instantiate<FlockChild>((M0) this._childPrefab);
      flockChild._spawner = this;
      this._roamers.Add(flockChild);
      this.AddChildToParent(((Component) flockChild).get_transform());
    }
  }

  public void AddChildToParent(Transform obj)
  {
    if (this._groupChildToFlock)
    {
      obj.set_parent(((Component) this).get_transform());
    }
    else
    {
      if (!this._groupChildToNewTransform)
        return;
      obj.set_parent(this._groupTransform);
    }
  }

  public void RemoveChild(int amount)
  {
    for (int index = 0; index < amount; ++index)
    {
      FlockChild roamer = this._roamers[this._roamers.Count - 1];
      this._roamers.RemoveAt(this._roamers.Count - 1);
      Object.Destroy((Object) ((Component) roamer).get_gameObject());
    }
  }

  public void Update()
  {
    if ((double) this._activeChildren > 0.0)
    {
      if (this._updateDivisor > 1)
      {
        ++this._updateCounter;
        this._updateCounter %= this._updateDivisor;
        this._newDelta = Time.get_deltaTime() * (float) this._updateDivisor;
      }
      else
        this._newDelta = Time.get_deltaTime();
    }
    this.UpdateChildAmount();
  }

  public void InstantiateGroup()
  {
    if (Object.op_Inequality((Object) this._groupTransform, (Object) null))
      return;
    GameObject gameObject = new GameObject();
    this._groupTransform = gameObject.get_transform();
    this._groupTransform.set_position(this._thisT.get_position());
    if (this._groupName != string.Empty)
      ((Object) gameObject).set_name(this._groupName);
    else
      ((Object) gameObject).set_name(((Object) this._thisT).get_name() + " Fish Container");
  }

  public void UpdateChildAmount()
  {
    if (this._childAmount >= 0 && this._childAmount < this._roamers.Count)
    {
      this.RemoveChild(1);
    }
    else
    {
      if (this._childAmount <= this._roamers.Count)
        return;
      this.AddChild(1);
    }
  }

  public void OnDrawGizmos()
  {
    if (Object.op_Equality((Object) this._thisT, (Object) null))
      this._thisT = ((Component) this).get_transform();
    if (!Application.get_isPlaying() && Vector3.op_Inequality(this._posBuffer, Vector3.op_Addition(this._thisT.get_position(), this._startPosOffset)))
      this._posBuffer = Vector3.op_Addition(this._thisT.get_position(), this._startPosOffset);
    if ((double) this._positionSphereDepth == -1.0)
      this._positionSphereDepth = this._positionSphere;
    if ((double) this._spawnSphereDepth == -1.0)
      this._spawnSphereDepth = this._spawnSphere;
    Gizmos.set_color(Color.get_blue());
    Gizmos.DrawWireCube(this._posBuffer, new Vector3(this._spawnSphere * 2f, this._spawnSphereHeight * 2f, this._spawnSphereDepth * 2f));
    Gizmos.set_color(Color.get_cyan());
    Gizmos.DrawWireCube(this._thisT.get_position(), new Vector3((float) ((double) this._positionSphere * 2.0 + (double) this._spawnSphere * 2.0), (float) ((double) this._positionSphereHeight * 2.0 + (double) this._spawnSphereHeight * 2.0), (float) ((double) this._positionSphereDepth * 2.0 + (double) this._spawnSphereDepth * 2.0)));
  }

  public void SetFlockRandomPosition()
  {
    Vector3 zero = Vector3.get_zero();
    zero.x = (__Null) ((double) Random.Range(-this._positionSphere, this._positionSphere) + this._thisT.get_position().x);
    zero.z = (__Null) ((double) Random.Range(-this._positionSphereDepth, this._positionSphereDepth) + this._thisT.get_position().z);
    zero.y = (__Null) ((double) Random.Range(-this._positionSphereHeight, this._positionSphereHeight) + this._thisT.get_position().y);
    this._posBuffer = zero;
    if (!this._forceChildWaypoints)
      return;
    for (int index = 0; index < this._roamers.Count; ++index)
      this._roamers[index].Wander(Random.get_value() * this._forcedRandomDelay);
  }

  public void destroyBirds()
  {
    for (int index = 0; index < this._roamers.Count; ++index)
      Object.Destroy((Object) ((Component) this._roamers[index]).get_gameObject());
    this._childAmount = 0;
    this._roamers.Clear();
  }
}
