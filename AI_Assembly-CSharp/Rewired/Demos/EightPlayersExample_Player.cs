// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.EightPlayersExample_Player
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  [RequireComponent(typeof (CharacterController))]
  public class EightPlayersExample_Player : MonoBehaviour
  {
    public int playerId;
    public float moveSpeed;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    private Player player;
    private CharacterController cc;
    private Vector3 moveVector;
    private bool fire;
    [NonSerialized]
    private bool initialized;

    public EightPlayersExample_Player()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      this.cc = (CharacterController) ((Component) this).GetComponent<CharacterController>();
    }

    private void Initialize()
    {
      this.player = ReInput.get_players().GetPlayer(this.playerId);
      this.initialized = true;
    }

    private void Update()
    {
      if (!ReInput.get_isReady())
        return;
      if (!this.initialized)
        this.Initialize();
      this.GetInput();
      this.ProcessInput();
    }

    private void GetInput()
    {
      this.moveVector.x = (__Null) (double) this.player.GetAxis("Move Horizontal");
      this.moveVector.y = (__Null) (double) this.player.GetAxis("Move Vertical");
      this.fire = this.player.GetButtonDown("Fire");
    }

    private void ProcessInput()
    {
      if (this.moveVector.x != 0.0 || this.moveVector.y != 0.0)
        this.cc.Move(Vector3.op_Multiply(Vector3.op_Multiply(this.moveVector, this.moveSpeed), Time.get_deltaTime()));
      if (!this.fire)
        return;
      ((Rigidbody) ((GameObject) Object.Instantiate<GameObject>((M0) this.bulletPrefab, Vector3.op_Addition(((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_right()), ((Component) this).get_transform().get_rotation())).GetComponent<Rigidbody>()).AddForce(Vector3.op_Multiply(((Component) this).get_transform().get_right(), this.bulletSpeed), (ForceMode) 2);
    }
  }
}
