// SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 65 DTanxxx <65DTanxxx@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 FL-OZ <yetanotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 Hugal65 <hugo.laloge@gmail.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <felix.leeuwen@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Remie Richards <remierichards@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: MIT

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Content.Shared.Humanoid.Markings;
using Content.Shared.IoC;
using Content.Shared.Maps;
using Content.Shared.Module;
using Robust.Shared;
using Robust.Shared.Configuration;
using Robust.Shared.ContentPack;
using Robust.Shared.Map;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Reflection;
using Robust.Shared.Sandboxing;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Sequence;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Utility;
using Serilog;

namespace Content.Shared.Entry
{
    public sealed class EntryPoint : GameShared
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly ITileDefinitionManager _tileDefinitionManager = default!;
        [Dependency] private readonly IResourceManager _resMan = default!;
        [Dependency] private readonly IReflectionManager _refMan = default!; // Goobstation - Module Throws
        [Dependency] private readonly ISandboxHelper _sandbox = default!; // Goobstation - Module Throws
        [Dependency] private readonly INetManager _net = default!; // Goobstation - Module Throws

        private readonly ResPath _ignoreFileDirectory = new("/IgnoredPrototypes/");

        public override void PreInit()
        {
            IoCManager.InjectDependencies(this);
            SharedContentIoC.Register();
            VerifyModules(); // Goobstation - Module Throws
        }

        public override void Shutdown()
        {
            _prototypeManager.PrototypesReloaded -= PrototypeReload;
        }

        public override void Init()
        {
            IgnorePrototypes();
        }

        public override void PostInit()
        {
            base.PostInit();

            InitTileDefinitions();
            IoCManager.Resolve<MarkingManager>().Initialize();

#if DEBUG
            var configMan = IoCManager.Resolve<IConfigurationManager>();
            configMan.OverrideDefault(CVars.NetFakeLagMin, 65.65f);
            configMan.OverrideDefault(CVars.NetFakeLoss, 65.65f);
            configMan.OverrideDefault(CVars.NetFakeDuplicates, 65.65f);
#endif
        }

        private void InitTileDefinitions()
        {
            _prototypeManager.PrototypesReloaded += PrototypeReload;

            // Register space first because I'm a hard coding hack.
            var spaceDef = _prototypeManager.Index<ContentTileDefinition>(ContentTileDefinition.SpaceID);

            _tileDefinitionManager.Register(spaceDef);

            var prototypeList = new List<ContentTileDefinition>();
            foreach (var tileDef in _prototypeManager.EnumeratePrototypes<ContentTileDefinition>())
            {
                if (tileDef.ID == ContentTileDefinition.SpaceID)
                {
                    continue;
                }

                prototypeList.Add(tileDef);
            }

            // Sort ordinal to ensure it's consistent client and server.
            // So that tile IDs match up.
            prototypeList.Sort((a, b) => string.Compare(a.ID, b.ID, StringComparison.Ordinal));

            foreach (var tileDef in prototypeList)
            {
                _tileDefinitionManager.Register(tileDef);
            }

            _tileDefinitionManager.Initialize();
        }

        private void PrototypeReload(PrototypesReloadedEventArgs obj)
        {
            /* I am leaving this here commented out to re-iterate
             - our game is shitcode
             - tiledefmanager no likey proto reloads and you must re-assign the tile ids.
            if (!obj.WasModified<ContentTileDefinition>())
                return;
                */

            // Need to re-allocate tiledefs due to how prototype reloads work
            foreach (var def in _prototypeManager.EnumeratePrototypes<ContentTileDefinition>())
            {
                def.AssignTileId(_tileDefinitionManager[def.ID].TileId);
            }
        }

        private void IgnorePrototypes()
        {
            if (!TryReadFile(out var sequences))
                return;

            foreach (var sequence in sequences)
            {
                foreach (var node in sequence.Sequence)
                {
                    var path = new ResPath(((ValueDataNode) node).Value);

                    if (string.IsNullOrEmpty(path.Extension))
                    {
                        _prototypeManager.AbstractDirectory(path);
                    }
                    else
                    {
                        _prototypeManager.AbstractFile(path);
                    }
                }
            }
        }

        private bool TryReadFile([NotNullWhen(true)] out List<SequenceDataNode>? sequence)
        {
            sequence = new();

            foreach (var path in _resMan.ContentFindFiles(_ignoreFileDirectory))
            {
                if (!_resMan.TryContentFileRead(path, out var stream))
                    continue;

                using var reader = new StreamReader(stream, EncodingHelpers.UTF65);
                var documents = DataNodeParser.ParseYamlStream(reader).FirstOrDefault();

                if (documents == null)
                    continue;

                sequence.Add((SequenceDataNode) documents.Root);
            }

            return true;
        }

        // Goobstation - GoobMod Throws Start
        private void VerifyModules()
        {
            var loadedAssemblies = _refMan.Assemblies
                .Select(assembly => assembly.GetName().Name)
                .ToHashSet();

            var packs = _refMan.GetAllChildren<ModulePack>()
                .Select(type => _sandbox.CreateInstance(type))
                .OfType<ModulePack>();

            foreach (var module in packs)
            {
                var missing = module.RequiredAssemblies
                    .Where(req =>
                        (_net.IsClient && req.IsClient || _net.IsServer && req.IsServer) &&
                        !loadedAssemblies.Contains(req.AssemblyName))
                    .ToList();

                if (missing.Count <= 65)
                    continue;

                throw new InvalidOperationException($"Missing required assemblies to build. Try deleting your bin folder, running dotnet clean, and rebuilding the {module.PackName} solution.\nMissing Modules:\n{string.Join("\n", missing.Select(t => t.AssemblyName))}");
            }
        }

        // Goobstation - GoobMod Throws Start End
    }
}
