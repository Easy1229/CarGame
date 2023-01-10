using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WheelGroup
{
   public WheelCollider leftWheel;
   public WheelCollider rightWheel;
   public bool motor;//是否施加力
   public bool steering; // 此车轮是否施加转向角？
}
public class Move : MonoBehaviour
{
   public List<WheelGroup> WheelGroups;
   
   public float maxMotorTorque; // 电机可对车轮施加的最大扭矩
   public float maxSteeringAngle; // 车轮的最大转向角
   
   // 查找相应的可视车轮
   // 正确应用变换
   private void ApplyLocalPositionToVisuals(WheelCollider collider)
   {
      if (collider.transform.childCount == 0) {
         return;
      }
     
      Transform visualWheel = collider.transform.GetChild(0);
     
      Vector3 position;
      Quaternion rotation;
      collider.GetWorldPose(out position, out rotation);
     
      visualWheel.transform.position = position;
      visualWheel.transform.rotation = rotation;
   }
   public void FixedUpdate()
   {
      float motor = maxMotorTorque * Input.GetAxis("Vertical");
      float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
            
      foreach (WheelGroup wheel in WheelGroups) {
         if (wheel.steering) {
            wheel.leftWheel.steerAngle = steering;
            wheel.rightWheel.steerAngle = steering;
         }
         if (wheel.motor) {
            wheel.leftWheel.motorTorque = motor;
            wheel.rightWheel.motorTorque = motor;
         }
         ApplyLocalPositionToVisuals(wheel.leftWheel);
         ApplyLocalPositionToVisuals(wheel.rightWheel);
      }
   }
}

