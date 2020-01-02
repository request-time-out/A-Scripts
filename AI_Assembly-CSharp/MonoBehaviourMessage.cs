// Decompiled with JetBrains decompiler
// Type: MonoBehaviourMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class MonoBehaviourMessage : MonoBehaviour
{
  public MonoBehaviourMessage()
  {
    base.\u002Ector();
  }

  public void Awake()
  {
    MonoBehaviour.print((object) ("Awake : " + ((Object) this).get_name()));
  }

  public void Start()
  {
    MonoBehaviour.print((object) ("Start : " + ((Object) this).get_name()));
  }

  public void OnEnable()
  {
    MonoBehaviour.print((object) ("OnEnable : " + ((Object) this).get_name()));
  }

  public void OnDisable()
  {
    MonoBehaviour.print((object) ("OnDisable : " + ((Object) this).get_name()));
  }

  public void OnDestroy()
  {
    MonoBehaviour.print((object) ("OnDestroy : " + ((Object) this).get_name()));
  }

  public void OnApplicationFocus(bool isFocus)
  {
    MonoBehaviour.print((object) ("OnApplicationFocus : " + ((Object) this).get_name()));
    MonoBehaviour.print((object) ("isFocus : " + (object) isFocus));
  }

  public void OnApplicationPause(bool isPause)
  {
    MonoBehaviour.print((object) ("OnApplicationPause : " + ((Object) this).get_name()));
    MonoBehaviour.print((object) ("isPause : " + (object) isPause));
  }

  public void OnApplicationQuit()
  {
    MonoBehaviour.print((object) ("OnApplicationQuit : " + ((Object) this).get_name()));
  }

  public void OnTransformChildrenChanged()
  {
    MonoBehaviour.print((object) ("OnTransformChildrenChanged : " + ((Object) this).get_name()));
  }

  public void OnTransformParentChanged()
  {
    MonoBehaviour.print((object) ("OnTransformParentChanged : " + ((Object) this).get_name()));
  }

  public void OnValidate()
  {
    MonoBehaviour.print((object) ("OnValidate : " + ((Object) this).get_name()));
  }

  public void Reset()
  {
    MonoBehaviour.print((object) ("Reset : " + ((Object) this).get_name()));
  }

  public void OnAnimatorIK()
  {
    MonoBehaviour.print((object) ("OnAnimatorIK : " + ((Object) this).get_name()));
  }

  public void OnAnimatorMove()
  {
    MonoBehaviour.print((object) ("OnAnimatorMove : " + ((Object) this).get_name()));
  }

  public void OnAudioFilterRead(float[] data, int channels)
  {
    MonoBehaviour.print((object) ("OnAudioFilterRead : " + ((Object) this).get_name()));
  }

  public void OnJointBreak()
  {
    MonoBehaviour.print((object) ("OnJointBreak : " + ((Object) this).get_name()));
  }

  public void OnParticleCollision()
  {
    MonoBehaviour.print((object) ("OnParticleCollision : " + ((Object) this).get_name()));
  }

  public void FixedUpdate()
  {
    MonoBehaviour.print((object) ("FixedUpdate : " + ((Object) this).get_name()));
  }

  public void Update()
  {
    MonoBehaviour.print((object) ("Update : " + ((Object) this).get_name()));
  }

  public void LateUpdate()
  {
    MonoBehaviour.print((object) ("LateUpdate : " + ((Object) this).get_name()));
  }

  public void OnConnectedToServer()
  {
    MonoBehaviour.print((object) ("OnConnectedToServer : " + ((Object) this).get_name()));
  }

  public void OnDisconnectedFromServer()
  {
    MonoBehaviour.print((object) ("OnDisconnectedFromServer : " + ((Object) this).get_name()));
  }

  public void OnFailedToConnect()
  {
    MonoBehaviour.print((object) ("OnFailedToConnect : " + ((Object) this).get_name()));
  }

  public void OnFailedToConnectToMasterServer()
  {
    MonoBehaviour.print((object) ("OnFailedToConnectToMasterServer : " + ((Object) this).get_name()));
  }

  public void OnMasterServerEvent()
  {
    MonoBehaviour.print((object) ("OnMasterServerEvent : " + ((Object) this).get_name()));
  }

  public void OnPlayerConnected()
  {
    MonoBehaviour.print((object) ("OnPlayerConnected : " + ((Object) this).get_name()));
  }

  public void OnPlayerDisconnected()
  {
    MonoBehaviour.print((object) ("OnPlayerDisconnected : " + ((Object) this).get_name()));
  }

  public void OnServerInitialized()
  {
    MonoBehaviour.print((object) ("OnServerInitialized : " + ((Object) this).get_name()));
  }

  public void OnMouseDown()
  {
    MonoBehaviour.print((object) ("OnMouseDown : " + ((Object) this).get_name()));
  }

  public void OnMouseUp()
  {
    MonoBehaviour.print((object) ("OnMouseUp : " + ((Object) this).get_name()));
  }

  public void OnMouseUpAsButton()
  {
    MonoBehaviour.print((object) ("OnMouseUpAsButton : " + ((Object) this).get_name()));
  }

  public void OnMouseDrag()
  {
    MonoBehaviour.print((object) ("OnMouseDrag : " + ((Object) this).get_name()));
  }

  public void OnMouseEnter()
  {
    MonoBehaviour.print((object) ("OnMouseEnter : " + ((Object) this).get_name()));
  }

  public void OnMouseExit()
  {
    MonoBehaviour.print((object) ("OnMouseExit : " + ((Object) this).get_name()));
  }

  public void OnMouseOver()
  {
    MonoBehaviour.print((object) ("OnMouseOver : " + ((Object) this).get_name()));
  }

  public void OnControllerColliderHit(ControllerColliderHit hit)
  {
    MonoBehaviour.print((object) ("OnControllerColliderHit : " + (object) hit));
  }

  public void OnTriggerEnter(Collider col)
  {
    MonoBehaviour.print((object) ("OnTriggerEnter : " + (object) col));
  }

  public void OnTriggerExit(Collider col)
  {
    MonoBehaviour.print((object) ("OnTriggerExit : " + (object) col));
  }

  public void OnTriggerStay(Collider col)
  {
    MonoBehaviour.print((object) ("OnTriggerStay : " + (object) col));
  }

  public void OnCollisionEnter(Collision col)
  {
    MonoBehaviour.print((object) ("OnCollisionEnter : " + (object) col.get_gameObject()));
  }

  public void OnCollisionExit(Collision col)
  {
    MonoBehaviour.print((object) ("OnCollisionExit : " + (object) col.get_gameObject()));
  }

  public void OnCollisionStay(Collision col)
  {
    MonoBehaviour.print((object) ("OnCollisionStay : " + (object) col.get_gameObject()));
  }

  public void OnTriggerEnter2D(Collider2D col)
  {
    MonoBehaviour.print((object) ("OnTriggerEnter2D : " + (object) col));
  }

  public void OnTriggerExit2D(Collider2D col)
  {
    MonoBehaviour.print((object) ("OnTriggerExit2D : " + (object) col));
  }

  public void OnTriggerStay2D(Collider2D col)
  {
    MonoBehaviour.print((object) ("OnTriggerStay2D : " + (object) col));
  }

  public void OnCollisionEnter2D(Collision2D col)
  {
    MonoBehaviour.print((object) ("OnCollisionEnter2D : " + (object) col.get_gameObject()));
  }

  public void OnCollisionExit2D(Collision2D col)
  {
    MonoBehaviour.print((object) ("OnCollisionExit2D : " + (object) col.get_gameObject()));
  }

  public void OnCollisionStay2D(Collision2D col)
  {
    MonoBehaviour.print((object) ("OnCollisionStay2D : " + (object) col.get_gameObject()));
  }

  public void OnPreCull()
  {
    MonoBehaviour.print((object) ("OnPreCull : " + ((Object) this).get_name()));
  }

  public void OnBecameVisible()
  {
    MonoBehaviour.print((object) ("OnBecameVisible : " + ((Object) this).get_name()));
  }

  public void OnBecameInvisible()
  {
    MonoBehaviour.print((object) ("OnBecameInvisible : " + ((Object) this).get_name()));
  }

  public void OnWillRenderObject()
  {
    MonoBehaviour.print((object) ("OnWillRenderObject : " + ((Object) this).get_name()));
  }

  public void OnPreRender()
  {
    MonoBehaviour.print((object) ("OnPreRender : " + ((Object) this).get_name()));
  }

  public void OnRenderObject()
  {
    MonoBehaviour.print((object) ("OnRenderObject : " + ((Object) this).get_name()));
  }

  public void OnPostRender()
  {
    MonoBehaviour.print((object) ("OnPostRender : " + ((Object) this).get_name()));
  }

  public void OnRenderImage(RenderTexture src, RenderTexture dest)
  {
    MonoBehaviour.print((object) ("OnRenderImage : " + ((Object) this).get_name()));
    MonoBehaviour.print((object) ("src : " + (object) src));
    MonoBehaviour.print((object) ("dest : " + (object) dest));
  }

  public void OnGUI()
  {
    MonoBehaviour.print((object) ("OnGUI : " + ((Object) this).get_name()));
  }

  public void OnDrawGizmos()
  {
    MonoBehaviour.print((object) ("OnDrawGizmos : " + ((Object) this).get_name()));
  }

  public void OnDrawGizmosSelected()
  {
    MonoBehaviour.print((object) ("OnDrawGizmosSelected : " + ((Object) this).get_name()));
  }
}
