using UnityEditor;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace SpaceNavigatorDriver
{
    struct SpaceNavigatorEnterpriseHIDState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('H', 'I', 'D');
    
        [InputControl(name = "xAxis", format = "SHRT", layout = "Axis", displayName = "xAxis", parameters = "scale=true, scaleFactor=10")]
        public short xAxis;
        [InputControl(name = "zAxis", format = "SHRT", layout = "Axis", displayName = "zAxis", parameters = "scale=true, scaleFactor=10")]
        public short zAxis;
        [InputControl(name = "yAxis", format = "SHRT", layout = "Axis", displayName = "yAxis", parameters = "scale=true, scaleFactor=10")]
        public short yAxis;
    
        [InputControl(name = "xRot", format = "SHRT", layout = "Axis", displayName = "xRot", parameters = "scale=true, scaleFactor=-10")]
        public short xRot;
        [InputControl(name = "yRot", format = "SHRT", layout = "Axis", displayName = "yRot", parameters = "scale=true, scaleFactor=-10")]
        public short yRot;
        [InputControl(name = "zRot", format = "SHRT", layout = "Axis", displayName = "zRot", parameters = "scale=true, scaleFactor=-10")]
        public short zRot;
    }

#if UNITY_EDITOR
    [InitializeOnLoad] // Make sure static constructor is called during startup.
#endif
    [InputControlLayout(stateType = typeof(SpaceNavigatorEnterpriseHIDState))]
    public class SpaceNavigatorEnterpriseHID : SpaceNavigatorHID
    {
        public AxisControl xAxis  { get; private set; }
        public AxisControl yAxis  { get; private set; }
        public AxisControl zAxis  { get; private set; }
        
        public override Vector3 Translation => new(
            xAxis.ReadValue(), 
            -yAxis.ReadValue(), 
            -zAxis.ReadValue());
        
        public AxisControl xRot  { get; private set; }
        public AxisControl yRot  { get; private set; }
        public AxisControl zRot  { get; private set; }

        public override Vector3 Rotation => new(
            xRot.ReadValue(),
            -zRot.ReadValue(),
            yRot.ReadValue());
        
        static SpaceNavigatorEnterpriseHID()
        {
            // Register a layout with product ID, so this layout will have a higher score than SpaceNavigatorHID
            InputSystem.RegisterLayout<SpaceNavigatorEnterpriseHID>(
                matches: new InputDeviceMatcher()
                    .WithInterface("HID")
                    .WithManufacturer("3Dconnexion.*")
                    .WithCapability("productId", 0xC633));
            DebugLog("SpaceNavigatorEnterpriseHID : Register layout for SpaceNavigator Enterprise productId:0x????");
        }
        
        protected override void FinishSetup()
        {
            base.FinishSetup();
            xAxis = GetChildControl<AxisControl>("xAxis");
            yAxis = GetChildControl<AxisControl>("yAxis");
            zAxis = GetChildControl<AxisControl>("zAxis");
            
            xRot = GetChildControl<AxisControl>("xRot");
            yRot = GetChildControl<AxisControl>("yRot");
            zRot = GetChildControl<AxisControl>("zRot");
        }
    }
}