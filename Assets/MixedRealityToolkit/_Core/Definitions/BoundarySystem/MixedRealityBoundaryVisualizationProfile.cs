﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Core.Definitions.Utilities;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Core.Definitions.BoundarySystem
{
    /// <summary>
    /// Configuration profile settings for setting up boundary visualizations.
    /// </summary>
    [CreateAssetMenu(menuName = "Mixed Reality Toolkit/Mixed Reality Boundary Visualization Profile", fileName = "MixedRealityBoundaryVisualizationProfile", order = (int)CreateProfileMenuItemIndices.BoundaryVisualization)]
    public class MixedRealityBoundaryVisualizationProfile : BaseMixedRealityProfile
    {
        [SerializeField]
        [Tooltip("The approximate height of the play space, in meters.")]
        private float boundaryHeight = 3.0f;

        /// <summary>
        /// The developer defined height of the boundary, in meters.
        /// </summary>
        /// <remarks>
        /// The BoundaryHeight property is used to create a three dimensional volume for the play space.
        /// </remarks>
        public float BoundaryHeight => boundaryHeight;

        #region Floor settings

        [SerializeField]
        [Tooltip("Should the floor be displayed in the scene?")]
        private bool showFloor = true;

        /// <summary>
        /// Should the boundary system display the floor?
        /// </summary>
        public bool ShowFloor => showFloor;

        // todo: consider allowing optional custom prefab

        [SerializeField]
        [Tooltip("The material to use when displaying the floor.")]
        private Material floorMaterial = null;

        /// <summary>
        /// The material to use for the floor <see cref="GameObject"/> when created by the boundary system.
        /// </summary>
        public Material FloorMaterial => floorMaterial;

        [SerializeField]
        [Tooltip("The dimensions of the floor, in meters.")]
        private Vector2 floorScale = new Vector2(10f, 10f);

        /// <summary>
        /// The size at which to display the rectangular floor plane <see cref="GameObject"/>.
        /// </summary>
        public Vector3 FloorScale => floorScale;

        #endregion Floor settings

        #region Play area settings

        [SerializeField]
        [Tooltip("Should the play area be displayed in the scene?")]
        private bool showPlayArea = true;

        /// <summary>
        /// Should the boundary system display the play area?
        /// </summary>
        public bool ShowPlayArea => showPlayArea;

        [SerializeField]
        [Tooltip("The material to use when displaying the play area.")]
        private Material playAreaMaterial = null;

        /// <summary>
        /// The material to use for the rectangular play area <see cref="GameObject"/>.
        /// </summary>
        public Material PlayAreaMaterial => playAreaMaterial;

        #endregion Play area settings

        #region Tracked area settings

        [SerializeField]
        [Tooltip("Should the tracked area be displayed in the scene?")]
        private bool showTrackedArea = true;

        /// <summary>
        /// Should the boundary system display the tracked area?
        /// </summary>
        public bool ShowTrackedArea => showTrackedArea;

        [SerializeField]
        [Tooltip("The material to use when displaying the tracked area.")]
        private Material trackedAreaMaterial = null;

        /// <summary>
        /// The material to use for the boundary geometry <see cref="GameObject"/>.
        /// </summary>
        public Material TrackedAreaMaterial => trackedAreaMaterial;

        #endregion Tracked area settings

        #region Boundary wall settings

        [SerializeField]
        [Tooltip("Should the boundary walls be displayed in the scene?")]
        private bool showBoundaryWalls = false;

        /// <summary>
        /// Should the boundary system display the boundary geometry walls?
        /// </summary>
        public bool ShowBoundaryWalls => showBoundaryWalls;

        [SerializeField]
        [Tooltip("The material to use when displaying the boundary walls.")]
        private Material boundaryWallMaterial = null;

        /// <summary>
        /// The material to use for displaying the boundary geometry walls.
        /// </summary>
        public Material BoundaryWallMaterial => boundaryWallMaterial;

        #endregion Boundary wall settings

        #region Boundary ceiling settings

        [SerializeField]
        [Tooltip("Should the boundary ceiling be displayed in the scene?")]
        private bool showBoundaryCeiling = false;

        /// <summary>
        /// Should the boundary system display the boundary ceiling?
        /// </summary>
        public bool ShowBoundaryCeiling => showBoundaryCeiling;

        [SerializeField]
        [Tooltip("The material to use when displaying the boundary ceiling.")]
        private Material boundaryCeilingMaterial = null;

        /// <summary>
        /// The material to use for displaying the boundary ceiling.
        /// </summary>
        public Material BoundaryCeilingMaterial => boundaryCeilingMaterial;

        #endregion Boundary ceiling settings
    }
}
