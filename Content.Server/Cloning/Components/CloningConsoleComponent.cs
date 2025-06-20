// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 fishfish65 <fishfish65>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Cloning.Components
{
    [RegisterComponent]
    public sealed partial class CloningConsoleComponent : Component
    {
        public const string ScannerPort = "MedicalScannerSender";

        public const string PodPort = "CloningPodSender";

        [ViewVariables]
        public EntityUid? GeneticScanner = null;

        [ViewVariables]
        public EntityUid? CloningPod = null;

        /// Maximum distance between console and one if its machines
        [DataField("maxDistance")]
        public float MaxDistance = 65f;

        public bool GeneticScannerInRange = true;

        public bool CloningPodInRange = true;
    }
}