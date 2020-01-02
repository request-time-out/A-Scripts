// Decompiled with JetBrains decompiler
// Type: PlaceholderSoftware.WetStuff.Demos.Demo_Assets.PauseTimeline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace PlaceholderSoftware.WetStuff.Demos.Demo_Assets
{
  public class PauseTimeline : MonoBehaviour
  {
    private const float MovementSpeed = 3f;
    private const float FreeLookSensitivity = 3f;
    private bool _isCameraFree;
    private bool _isLooking;
    public bool ShowPauseTimeline;

    public PauseTimeline()
    {
      base.\u002Ector();
    }

    private void Update()
    {
      if (!this._isCameraFree)
        return;
      this.FreeLook();
    }

    public void OnGUI()
    {
      if (this.ShowPauseTimeline)
      {
        PlayableDirector component = (PlayableDirector) ((Component) this).GetComponent<PlayableDirector>();
        Rect rect;
        ((Rect) ref rect).\u002Ector(20f, 50f, 130f, 30f);
        if (!this._isCameraFree)
        {
          if (GUI.Button(rect, "Free Camera"))
          {
            this._isCameraFree = true;
            component.Pause();
          }
        }
        else if (GUI.Button(rect, !((Behaviour) component).get_isActiveAndEnabled() ? "Lock Camera" : "Resume Timeline"))
        {
          this._isCameraFree = false;
          component.Resume();
        }
      }
      Rect rect1;
      ((Rect) ref rect1).\u002Ector(20f, 95f, 130f, 30f);
      if (!GUI.Button(rect1, "Back To Menu"))
        return;
      SceneManager.LoadScene("0. Demo Menu");
    }

    private void FreeLook()
    {
      if (Input.GetKey((KeyCode) 119) || Input.GetKey((KeyCode) 273))
        ((Component) this).get_transform().set_position(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_forward(), 3f), Time.get_deltaTime())));
      if (Input.GetKey((KeyCode) 97) || Input.GetKey((KeyCode) 276))
        ((Component) this).get_transform().set_position(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(((Component) this).get_transform().get_right()), 3f), Time.get_deltaTime())));
      if (Input.GetKey((KeyCode) 115) || Input.GetKey((KeyCode) 274))
        ((Component) this).get_transform().set_position(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(((Component) this).get_transform().get_forward()), 3f), Time.get_deltaTime())));
      if (Input.GetKey((KeyCode) 100) || Input.GetKey((KeyCode) 275))
        ((Component) this).get_transform().set_position(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_right(), 3f), Time.get_deltaTime())));
      if (Input.GetKey((KeyCode) 101))
        ((Component) this).get_transform().set_position(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_up(), 3f), Time.get_deltaTime())));
      if (Input.GetKey((KeyCode) 113))
        ((Component) this).get_transform().set_position(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(((Component) this).get_transform().get_up()), 3f), Time.get_deltaTime())));
      if (this._isLooking)
        ((Component) this).get_transform().set_localEulerAngles(new Vector3((float) (((Component) this).get_transform().get_localEulerAngles().x - (double) Input.GetAxis("Mouse Y") * 3.0), (float) (((Component) this).get_transform().get_localEulerAngles().y + (double) Input.GetAxis("Mouse X") * 3.0), 0.0f));
      if (Input.GetKeyDown((KeyCode) 324))
      {
        this._isLooking = true;
      }
      else
      {
        if (!Input.GetKeyUp((KeyCode) 324))
          return;
        this._isLooking = false;
      }
    }
  }
}
