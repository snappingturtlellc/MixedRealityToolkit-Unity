﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Internal.Definitions.Devices;
using Microsoft.MixedReality.Toolkit.Internal.Definitions.Utilities;
using Microsoft.MixedReality.Toolkit.Internal.Interfaces.InputSystem;
using Microsoft.MixedReality.Toolkit.Internal.Utilities;
using System;
using UnityEngine;
using UnityEngine.XR;

namespace Microsoft.MixedReality.Toolkit.Internal.Devices.OpenVR
{
    // TODO - Implement
    public class GenericOpenVRController : BaseController
    {
        public GenericOpenVRController(TrackingState trackingState, Handedness controllerHandedness, IMixedRealityInputSource inputSource = null, MixedRealityInteractionMapping[] interactions = null)
                : base(trackingState, controllerHandedness, inputSource, interactions) { }

        /// <summary>
        /// The current source state reading for this OpenVR Controller.
        /// </summary>
        public XRNodeState LastStateReading;

        private Vector3 currentControllerPosition = Vector3.zero;
        private Quaternion currentControllerRotation = Quaternion.identity;

        private Vector3 currentPointerPosition = Vector3.zero;
        private Quaternion currentPointerRotation = Quaternion.identity;
        private MixedRealityPose currentPointerData = new MixedRealityPose(Vector3.zero, Quaternion.identity);

        private Vector3 currentGripPosition = Vector3.zero;
        private Quaternion currentGripRotation = Quaternion.identity;
        private MixedRealityPose currentGripData = new MixedRealityPose(Vector3.zero, Quaternion.identity);


        #region Update data functions

        /// <summary>
        /// Update the controller data from the provided platform state
        /// </summary>
        public void UpdateController(XRNodeState xrNodeState)
        {
            Debug.Assert(Interactions != null);
            for (int i = 0; i < Interactions?.Length; i++)
            {
                switch (Interactions[i].InputType)
                {
                    case DeviceInputType.None:
                        break;
                    case DeviceInputType.SpatialPointer:
                    case DeviceInputType.PointerPosition:
                    case DeviceInputType.PointerRotation:
                        UpdateControllerData(xrNodeState, Interactions[i]);
                        break;
                    case DeviceInputType.Select:
                    case DeviceInputType.Trigger:
                    case DeviceInputType.TriggerPress:
                    case DeviceInputType.PointerClick:
                        UpdateTriggerData(xrNodeState, Interactions[i]);
                        break;
                    case DeviceInputType.SpatialGrip:
                    case DeviceInputType.GripPosition:
                    case DeviceInputType.GripRotation:
                    case DeviceInputType.GripPress:
                        UpdateGripData(xrNodeState, Interactions[i]);
                        break;
                    case DeviceInputType.ThumbStick:
                    case DeviceInputType.ThumbStickPress:
                        UpdateThumbStickData(xrNodeState, Interactions[i]);
                        break;
                    case DeviceInputType.Touchpad:
                    case DeviceInputType.TouchpadTouch:
                    case DeviceInputType.TouchpadPress:
                        UpdateTouchPadData(xrNodeState, Interactions[i]);
                        break;
                    case DeviceInputType.Menu:
                        UpdateMenuData(xrNodeState, Interactions[i]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            LastStateReading = xrNodeState;
        }

        /// <summary>
        /// Update the "Controller" input from the device
        /// </summary>
        /// <param name="interactionSourceState">The InteractionSourceState retrieved from the platform</param>
        private void UpdateControllerData(XRNodeState State, MixedRealityInteractionMapping interactionMapping)
        {
            var lastState = TrackingState;

            XRNode nodeType = State.nodeType;
            if (nodeType == XRNode.LeftHand || nodeType == XRNode.RightHand)
            {
                // The source is either a hand or a controller that supports pointing.
                // We can now check for position and rotation.
                IsPositionAvailable = State.TryGetPosition(out currentControllerPosition);
                IsPositionApproximate = false;

                IsRotationAvailable = State.TryGetRotation(out currentControllerRotation);

                // Devices are considered tracked if we receive position OR rotation data from the sensors.
                TrackingState = (IsPositionAvailable || IsRotationAvailable) ? TrackingState.Tracked : TrackingState.NotTracked;

                if (CameraCache.Main.transform.parent != null)
                {
                    currentPointerData.Position = CameraCache.Main.transform.parent.TransformPoint(currentControllerPosition);
                    currentPointerData.Rotation = Quaternion.Euler(CameraCache.Main.transform.parent.TransformDirection(currentControllerRotation.eulerAngles));
                }
            }
            else
            {
                // The input source does not support tracking.
                TrackingState = TrackingState.NotApplicable;
            }

            //Update the interaction data source
            interactionMapping.SetPoseValue(currentPointerData);

            // If our value changed raise it.
            if (interactionMapping.Changed)
            {
                //Raise input system Event if it enabled
                InputSystem?.RaisePoseInputChanged(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction, currentPointerData);
            }

            if (lastState != TrackingState)
            {
                InputSystem?.RaiseSourceTrackingStateChanged(InputSource, this, TrackingState);
            }
        }

        /// <summary>
        /// Update the "Spatial Grip" input from the device
        /// </summary>
        /// <param name="interactionSourceState">The InteractionSourceState retrieved from the platform</param>
        /// <param name="interactionMapping"></param>
        private void UpdateGripData(XRNodeState State, MixedRealityInteractionMapping interactionMapping)
        {
            ////Update the interaction data source
            //interactionMapping.SetBoolValue(interactionSourceState.grasped);

            //// If our value changed raise it.
            //if (interactionMapping.Changed)
            //{
            //    //Raise input system Event if it enabled
            //    if (interactionSourceState.grasped)
            //    {
            //        InputSystem?.RaiseOnInputDown(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction);
            //    }
            //    else
            //    {
            //        InputSystem?.RaiseOnInputUp(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction);
            //    }
            //}
        }

        /// <summary>
        /// Update the Touchpad input from the device
        /// </summary>
        /// <param name="interactionSourceState">The InteractionSourceState retrieved from the platform</param>
        /// <param name="interactionMapping"></param>
        private void UpdateTouchPadData(XRNodeState State, MixedRealityInteractionMapping interactionMapping)
        {
            //switch (interactionMapping.InputType)
            //{
            //    case DeviceInputType.TouchpadTouch:
            //        {
            //            //Update the interaction data source
            //            interactionMapping.SetBoolValue(interactionSourceState.touchpadTouched);

            //            // If our value changed raise it.
            //            if (interactionMapping.Changed)
            //            {
            //                //Raise input system Event if it enabled
            //                if (interactionSourceState.touchpadTouched)
            //                {
            //                    InputSystem?.RaiseOnInputDown(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction);
            //                }
            //                else
            //                {
            //                    InputSystem?.RaiseOnInputUp(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction);
            //                }
            //            }
            //            break;
            //        }
            //    case DeviceInputType.TouchpadPress:
            //        {
            //            //Update the interaction data source
            //            interactionMapping.SetBoolValue(interactionSourceState.touchpadPressed);

            //            // If our value changed raise it.
            //            if (interactionMapping.Changed)
            //            {
            //                //Raise input system Event if it enabled
            //                if (interactionSourceState.touchpadPressed)
            //                {
            //                    InputSystem?.RaiseOnInputDown(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction);
            //                }
            //                else
            //                {
            //                    InputSystem?.RaiseOnInputUp(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction);
            //                }
            //            }
            //            break;
            //        }
            //    case DeviceInputType.Touchpad:
            //        {
            //            //Update the interaction data source
            //            interactionMapping.SetVector2Value(interactionSourceState.touchpadPosition);

            //            // If our value changed raise it.
            //            if (interactionMapping.Changed)
            //            {
            //                //Raise input system Event if it enabled
            //                InputSystem?.RaisePositionInputChanged(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction, interactionSourceState.touchpadPosition);
            //            }
            //            break;
            //        }
            //    default:
            //        throw new IndexOutOfRangeException();
            //}
        }

        /// <summary>
        /// Update the Thumbstick input from the device
        /// </summary>
        /// <param name="interactionSourceState">The InteractionSourceState retrieved from the platform</param>
        /// <param name="interactionMapping"></param>
        private void UpdateThumbStickData(XRNodeState State, MixedRealityInteractionMapping interactionMapping)
        {
            //switch (interactionMapping.InputType)
            //{
            //    case DeviceInputType.ThumbStickPress:
            //        {
            //            //Update the interaction data source
            //            interactionMapping.SetBoolValue(interactionSourceState.thumbstickPressed);

            //            // If our value changed raise it.
            //            if (interactionMapping.Changed)
            //            {
            //                //Raise input system Event if it enabled
            //                if (interactionSourceState.thumbstickPressed)
            //                {
            //                    InputSystem?.RaiseOnInputDown(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction);
            //                }
            //                else
            //                {
            //                    InputSystem?.RaiseOnInputUp(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction);
            //                }
            //            }
            //            break;
            //        }
            //    case DeviceInputType.ThumbStick:
            //        {
            //            //Update the interaction data source
            //            interactionMapping.SetVector2Value(interactionSourceState.thumbstickPosition);

            //            // If our value changed raise it.
            //            if (interactionMapping.Changed)
            //            {
            //                //Raise input system Event if it enabled
            //                InputSystem?.RaisePositionInputChanged(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction, interactionSourceState.thumbstickPosition);
            //            }
            //            break;
            //        }
            //    default:
            //        throw new IndexOutOfRangeException();
            //}
        }

        /// <summary>
        /// Update the Trigger input from the device
        /// </summary>
        /// <param name="interactionSourceState">The InteractionSourceState retrieved from the platform</param>
        /// <param name="interactionMapping"></param>
        private void UpdateTriggerData(XRNodeState State, MixedRealityInteractionMapping interactionMapping)
        {
            //switch (interactionMapping.InputType)
            //{
            //    case DeviceInputType.TriggerPress:
            //    case DeviceInputType.Select:
            //        {
            //            //Update the interaction data source
            //            interactionMapping.SetBoolValue(interactionSourceState.selectPressed);

            //            // If our value changed raise it.
            //            if (interactionMapping.Changed)
            //            {
            //                //Raise input system Event if it enabled
            //                if (interactionSourceState.selectPressed)
            //                {
            //                    InputSystem?.RaiseOnInputDown(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction);
            //                }
            //                else
            //                {
            //                    InputSystem?.RaiseOnInputUp(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction);
            //                }
            //            }
            //            break;
            //        }
            //    case DeviceInputType.Trigger:
            //        {
            //            //Update the interaction data source
            //            interactionMapping.SetFloatValue(interactionSourceState.selectPressedAmount);

            //            // If our value changed raise it.
            //            if (interactionMapping.Changed)
            //            {
            //                //Raise input system Event if it enabled
            //                InputSystem?.RaiseOnInputPressed(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction, interactionSourceState.selectPressedAmount);
            //            }
            //            break;
            //        }
            //    default:
            //        throw new IndexOutOfRangeException();
            //}
        }

        /// <summary>
        /// Update the Menu button state.
        /// </summary>
        /// <param name="interactionSourceState"></param>
        /// <param name="interactionMapping"></param>
        private void UpdateMenuData(XRNodeState State, MixedRealityInteractionMapping interactionMapping)
        {
            ////Update the interaction data source
            //interactionMapping.SetBoolValue(interactionSourceState.menuPressed);

            //// If our value changed raise it.
            //if (interactionMapping.Changed)
            //{
            //    //Raise input system Event if it enabled
            //    if (interactionSourceState.menuPressed)
            //    {
            //        InputSystem?.RaiseOnInputDown(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction);
            //    }
            //    else
            //    {
            //        InputSystem?.RaiseOnInputUp(InputSource, ControllerHandedness, interactionMapping.MixedRealityInputAction);
            //    }
            //}
        }

        #endregion Update data functions

    }
}
