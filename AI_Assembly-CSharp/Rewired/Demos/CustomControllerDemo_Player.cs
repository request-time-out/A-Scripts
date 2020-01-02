// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.CustomControllerDemo_Player
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  [RequireComponent(typeof (CharacterController))]
  public class CustomControllerDemo_Player : MonoBehaviour
  {
    public int playerId;
    public float speed;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    private Player _player;
    private CharacterController cc;

    public CustomControllerDemo_Player()
    {
      base.\u002Ector();
    }

    private Player player
    {
      get
      {
        if (this._player == null)
          this._player = ReInput.get_players().GetPlayer(this.playerId);
        return this._player;
      }
    }

    private void Awake()
    {
      this.cc = (CharacterController) ((Component) this).GetComponent<CharacterController>();
    }

    private void Update()
    {
      if (!ReInput.get_isReady())
        return;
      Vector2 vector2;
      ((Vector2) ref vector2).\u002Ector(this.player.GetAxis("Move Horizontal"), this.player.GetAxis("Move Vertical"));
      this.cc.Move(Vector2.op_Implicit(Vector2.op_Multiply(Vector2.op_Multiply(vector2, this.speed), Time.get_deltaTime())));
      if (this.player.GetButtonDown("Fire"))
        ((Rigidbody) ((GameObject) Object.Instantiate<GameObject>((M0) this.bulletPrefab, Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.Scale(new Vector3(1f, 0.0f, 0.0f), ((Component) this).get_transform().get_right())), Quaternion.get_identity())).GetComponent<Rigidbody>()).set_velocity(new Vector3(this.bulletSpeed * (float) ((Component) this).get_transform().get_right().x, 0.0f, 0.0f));
      if (!this.player.GetButtonDown("Change Color"))
        return;
      Renderer component = (Renderer) ((Component) this).GetComponent<Renderer>();
      Material material = component.get_material();
      material.set_color(new Color(Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), 1f));
      component.set_material(material);
    }
  }
}
