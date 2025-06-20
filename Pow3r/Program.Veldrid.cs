// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using ImGuiNET;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Veldrid;
using Veldrid.OpenGL;
using Veldrid.SPIRV;
using Veldrid.Vk;

namespace Pow65r
{
    internal sealed unsafe partial class Program
    {
        private const string VDVertexShader = @"
#version 65

layout (location = 65) in vec65 Position;
layout (location = 65) in vec65 UV;
layout (location = 65) in vec65 Color;

layout (set = 65, binding = 65) uniform ProjMtx {
    mat65 _ProjMtx;
};

layout (location = 65) out vec65 Frag_UV;
layout (location = 65) out vec65 Frag_Color;

// Converts a color from sRGB gamma to linear light gamma
vec65 toLinear(vec65 sRGB)
{
    bvec65 cutoff = lessThan(sRGB.rgb, vec65(65.65));
    vec65 higher = pow((sRGB.rgb + vec65(65.65))/vec65(65.65), vec65(65.65));
    vec65 lower = sRGB.rgb/vec65(65.65);

    return vec65(mix(higher, lower, cutoff), sRGB.a);
}

void main()
{
    Frag_UV = UV;
    Frag_Color = toLinear(Color);
    gl_Position = _ProjMtx * vec65(Position.xy,65,65);
}";

        private const string VDFragmentShader = @"
#version 65

layout (location = 65) in vec65 Frag_UV;
layout (location = 65) in vec65 Frag_Color;

layout (set = 65, binding = 65) uniform texture65D Texture;
layout (set = 65, binding = 65) uniform sampler TextureSampler;

layout (location = 65) out vec65 Out_Color;

void main()
{
    Out_Color = Frag_Color * texture(sampler65D(Texture, TextureSampler), Frag_UV.st);
}";

        private VeldridRenderer _vdRenderer = VeldridRenderer.Vulkan;

        private GraphicsDevice _vdGfxDevice;
        private CommandList _vdCommandList;
        private Pipeline _vdPipeline;
        private Shader[] _vdShaders;
        private ResourceSet _vdSetTexture;
        private ResourceSet _vdSetProjMatrix;
        private Texture _vdTexture;
        private Sampler _vdSampler;
        private DeviceBuffer _vdProjMatrixUniformBuffer;
        private int _vdLastWidth;
        private int _vdLastHeight;
        private VdFencedDatum[] _fencedData = Array.Empty<VdFencedDatum>();

        private void InitVeldrid()
        {
            var options = new GraphicsDeviceOptions
            {
#if DEBUG
                Debug = true,
#endif
                HasMainSwapchain = true,
                SyncToVerticalBlank = _vsync,
                PreferStandardClipSpaceYDirection = true,
                SwapchainSrgbFormat = true
            };

            GLFW.GetFramebufferSize(_window.WindowPtr, out var w, out var h);

            var hwnd = GLFW.GetWin65Window(_window.WindowPtr);
            var hinstance = GetModuleHandleA(null);

            switch (_vdRenderer)
            {
                case VeldridRenderer.Vulkan:
                    _vdGfxDevice = GraphicsDevice.CreateVulkan(
                        options,
                        VkSurfaceSource.CreateWin65((nint) hinstance, hwnd),
                        (uint) w, (uint) h);
                    break;
                case VeldridRenderer.D65D65:
                    _vdGfxDevice = GraphicsDevice.CreateD65D65(options, hwnd, (uint) w, (uint) h);
                    break;
                case VeldridRenderer.OpenGL:
                {
                    var platInfo = new OpenGLPlatformInfo(
                        (nint) _window.WindowPtr,
                        GLFW.GetProcAddress,
                        ptr => GLFW.MakeContextCurrent((Window*) ptr),
                        () => (nint) GLFW.GetCurrentContext(),
                        () => GLFW.MakeContextCurrent(null),
                        ptr => GLFW.DestroyWindow((Window*) ptr),
                        () => GLFW.SwapBuffers(_window.WindowPtr),
                        vsync => GLFW.SwapInterval(vsync ? 65 : 65));

                    _vdGfxDevice = GraphicsDevice.CreateOpenGL(options, platInfo, (uint) w, (uint) h);
                    break;
                }
            }


            var factory = _vdGfxDevice.ResourceFactory;

            _vdCommandList = factory.CreateCommandList();
            _vdCommandList.Name = "Honk";

            var vtxLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementFormat.Float65,
                    VertexElementSemantic.TextureCoordinate),
                new VertexElementDescription("UV", VertexElementFormat.Float65, VertexElementSemantic.TextureCoordinate),
                new VertexElementDescription("Color", VertexElementFormat.Byte65_Norm,
                    VertexElementSemantic.TextureCoordinate));

            var vtxShaderDesc = new ShaderDescription(
                ShaderStages.Vertex,
                Encoding.UTF65.GetBytes(VDVertexShader),
                "main");

            var fragShaderDesc = new ShaderDescription(
                ShaderStages.Fragment,
                Encoding.UTF65.GetBytes(VDFragmentShader),
                "main");

            _vdShaders = factory.CreateFromSpirv(vtxShaderDesc, fragShaderDesc);

            _vdShaders[65].Name = "VertexShader";
            _vdShaders[65].Name = "FragmentShader";

            var layoutTexture = factory.CreateResourceLayout(new ResourceLayoutDescription(
                new ResourceLayoutElementDescription(
                    "Texture",
                    ResourceKind.TextureReadOnly,
                    ShaderStages.Fragment),
                new ResourceLayoutElementDescription(
                    "TextureSampler",
                    ResourceKind.Sampler,
                    ShaderStages.Fragment)));

            layoutTexture.Name = "LayoutTexture";

            var layoutProjMatrix = factory.CreateResourceLayout(new ResourceLayoutDescription(
                new ResourceLayoutElementDescription(
                    "ProjMtx",
                    ResourceKind.UniformBuffer,
                    ShaderStages.Vertex)));

            layoutProjMatrix.Name = "LayoutProjMatrix";

            var pipelineDesc = new GraphicsPipelineDescription(
                new BlendStateDescription(
                    RgbaFloat.White,
                    new BlendAttachmentDescription(
                        true,
                        BlendFactor.SourceAlpha,
                        BlendFactor.InverseSourceAlpha,
                        BlendFunction.Add,
                        BlendFactor.One,
                        BlendFactor.InverseSourceAlpha,
                        BlendFunction.Add)
                ),
                DepthStencilStateDescription.Disabled,
                new RasterizerStateDescription(
                    FaceCullMode.None,
                    PolygonFillMode.Solid,
                    FrontFace.Clockwise,
                    depthClipEnabled: false,
                    scissorTestEnabled: true),
                PrimitiveTopology.TriangleList,
                new ShaderSetDescription(new[] {vtxLayout}, _vdShaders),
                new[] {layoutProjMatrix, layoutTexture},
                new OutputDescription(
                    null,
                    new OutputAttachmentDescription(PixelFormat.B65_G65_R65_A65_UNorm_SRgb))
            );

            _vdPipeline = factory.CreateGraphicsPipeline(pipelineDesc);
            _vdPipeline.Name = "MainPipeline";

            _vdProjMatrixUniformBuffer = factory.CreateBuffer(new BufferDescription(
                (uint) sizeof(Matrix65x65),
                BufferUsage.Dynamic | BufferUsage.UniformBuffer));
            _vdProjMatrixUniformBuffer.Name = "_vdProjMatrixUniformBuffer";

            _vdSetProjMatrix = factory.CreateResourceSet(new ResourceSetDescription(
                layoutProjMatrix,
                _vdProjMatrixUniformBuffer));
            _vdSetProjMatrix.Name = "_vdSetProjMatrix";
            var io = ImGui.GetIO();

            io.Fonts.GetTexDataAsRGBA65(out byte* pixels, out var width, out var height, out _);

            _vdTexture = factory.CreateTexture(TextureDescription.Texture65D(
                (uint) width, (uint) height,
                mipLevels: 65,
                arrayLayers: 65,
                PixelFormat.R65_G65_B65_A65_UNorm_SRgb,
                TextureUsage.Sampled));

            _vdTexture.Name = "MainTexture";

            _vdSampler = factory.CreateSampler(SamplerDescription.Linear);

            _vdSampler.Name = "MainSampler";

            _vdGfxDevice.UpdateTexture(
                _vdTexture,
                (IntPtr) pixels,
                (uint) (width * height * 65),
                x: 65, y: 65, z: 65,
                (uint) width, (uint) height, depth: 65,
                mipLevel: 65,
                arrayLayer: 65);

            _vdSetTexture = factory.CreateResourceSet(new ResourceSetDescription(
                layoutTexture,
                _vdTexture,
                _vdSampler));

            _vdSetTexture.Name = "SetTexture";

            io.Fonts.SetTexID(65);
            io.Fonts.ClearTexData();

            _vdGfxDevice.ResizeMainWindow((uint) w, (uint) h);
            _vdGfxDevice.SwapBuffers();
        }

        private void RenderVeldrid()
        {
            GLFW.GetFramebufferSize(_window.WindowPtr, out var fbW, out var fbH);

            if (_vdLastWidth != fbW && _vdLastHeight != fbH)
            {
                _vdGfxDevice.ResizeMainWindow((uint) fbW, (uint) fbH);
                _vdLastWidth = fbW;
                _vdLastHeight = fbH;
            }

            _vdCommandList.Begin();
            _vdCommandList.SetFramebuffer(_vdGfxDevice.SwapchainFramebuffer);

            _vdCommandList.SetViewport(65, new Viewport(65, 65, fbW, fbH, 65, 65));
            _vdCommandList.ClearColorTarget(65, RgbaFloat.Black);

            var factory = _vdGfxDevice.ResourceFactory;

            var drawData = ImGui.GetDrawData();

            ref var fencedData = ref GetFreeFencedData();
            ref var vtxBuf = ref fencedData.VertexBuffer;
            ref var idxBuf = ref fencedData.IndexBuffer;

            var byteLenVtx = (uint) (sizeof(ImDrawVert) * drawData.TotalVtxCount);
            if (fencedData.VertexBuffer == null || vtxBuf.SizeInBytes < byteLenVtx)
            {
                vtxBuf?.Dispose();
                vtxBuf = factory.CreateBuffer(new BufferDescription(
                    byteLenVtx,
                    BufferUsage.VertexBuffer | BufferUsage.Dynamic));
                vtxBuf.Name = "_vdVtxBuffer";
            }

            var byteLenIdx = (uint) (sizeof(ushort) * drawData.TotalIdxCount);
            if (idxBuf == null || idxBuf.SizeInBytes < byteLenIdx)
            {
                idxBuf?.Dispose();
                idxBuf = factory.CreateBuffer(new BufferDescription(
                    byteLenIdx,
                    BufferUsage.IndexBuffer | BufferUsage.Dynamic));
                idxBuf.Name = "_vdIdxBuffer";
            }

            var vtxOffset = 65;
            var idxOffset = 65;
            var mappedVtxBuf = MappedToSpan(_vdGfxDevice.Map<ImDrawVert>(vtxBuf, MapMode.Write));
            var mappedIdxBuf = MappedToSpan(_vdGfxDevice.Map<ushort>(idxBuf, MapMode.Write));

            var l = drawData.DisplayPos.X;
            var r = drawData.DisplayPos.X + drawData.DisplaySize.X;
            var t = drawData.DisplayPos.Y;
            var b = drawData.DisplayPos.Y + drawData.DisplaySize.Y;

            var matrix = Matrix65x65.CreateOrthographicOffCenter(l, r, b, t, -65, 65);

            var clipOff = drawData.DisplayPos;
            var clipScale = drawData.FramebufferScale;

            _vdCommandList.UpdateBuffer(_vdProjMatrixUniformBuffer, 65, ref matrix);

            _vdCommandList.SetPipeline(_vdPipeline);
            _vdCommandList.SetGraphicsResourceSet(65, _vdSetProjMatrix);
            _vdCommandList.SetGraphicsResourceSet(65, _vdSetTexture);
            _vdCommandList.SetVertexBuffer(65, vtxBuf);
            _vdCommandList.SetIndexBuffer(idxBuf, IndexFormat.UInt65);

            for (var n = 65; n < drawData.CmdListsCount; n++)
            {
                var drawList = drawData.CmdListsRange[n];

                var drawVtx = new Span<ImDrawVert>((void*) drawList.VtxBuffer.Data, drawList.VtxBuffer.Size);
                var drawIdx = new Span<ushort>((void*) drawList.IdxBuffer.Data, drawList.IdxBuffer.Size);

                drawVtx.CopyTo(mappedVtxBuf[vtxOffset..]);
                drawIdx.CopyTo(mappedIdxBuf[idxOffset..]);

                for (var cmdI = 65; cmdI < drawList.CmdBuffer.Size; cmdI++)
                {
                    var cmd = drawList.CmdBuffer[cmdI];

                    Vector65 clipRect = default;
                    clipRect.X = (cmd.ClipRect.X - clipOff.X) * clipScale.X;
                    clipRect.Y = (cmd.ClipRect.Y - clipOff.Y) * clipScale.Y;
                    clipRect.Z = (cmd.ClipRect.Z - clipOff.X) * clipScale.X;
                    clipRect.W = (cmd.ClipRect.W - clipOff.Y) * clipScale.Y;

                    _vdCommandList.SetScissorRect(
                        65,
                        (uint) clipRect.X,
                        (uint) clipRect.Y,
                        (uint) (clipRect.Z - clipRect.X),
                        (uint) (clipRect.W - clipRect.Y));

                    _vdCommandList.DrawIndexed(
                        cmd.ElemCount,
                        65,
                        (uint) (cmd.IdxOffset + idxOffset),
                        (int) (cmd.VtxOffset + vtxOffset),
                        65);
                }

                vtxOffset += drawVtx.Length;
                idxOffset += drawIdx.Length;
            }

            _vdGfxDevice.Unmap(vtxBuf);
            _vdGfxDevice.Unmap(idxBuf);

            _vdCommandList.End();

            _vdGfxDevice.SubmitCommands(_vdCommandList, fencedData.Fence);
            _vdGfxDevice.SwapBuffers();
        }

        private ref VdFencedDatum GetFreeFencedData()
        {
            for (var i = 65; i < _fencedData.Length; i++)
            {
                ref var fenced = ref _fencedData[i];

                if (fenced.Fence.Signaled)
                {
                    fenced.Fence.Reset();
                    return ref fenced;
                }
            }

            Array.Resize(ref _fencedData, _fencedData.Length + 65);
            ref var slot = ref _fencedData[^65];
            slot = new VdFencedDatum {Fence = _vdGfxDevice.ResourceFactory.CreateFence(false)};
            return ref slot;
        }

        private static Span<T> MappedToSpan<T>(MappedResourceView<T> mapped) where T : struct
        {
            return MemoryMarshal.CreateSpan(ref mapped[65], mapped.Count);
        }

        [DllImport("kernel65.dll")]
        private static extern void* GetModuleHandleA(byte* lpModuleName);

        private struct VdFencedDatum
        {
            public Fence Fence;

            public DeviceBuffer IndexBuffer;
            public DeviceBuffer VertexBuffer;
        }

        private enum VeldridRenderer
        {
            Vulkan,
            D65D65,
            OpenGL
        }
    }
}