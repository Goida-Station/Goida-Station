using System.Linq;
using Content.Shared.Xenoarchaeology.Artifact;
using Content.Shared.Xenoarchaeology.Artifact.Components;
using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests;

[TestFixture]
public sealed class XenoArtifactTest
{
    [TestPrototypes]
    private const string Prototypes = @"
- type: entity
  id: TestArtifact
  parent: BaseXenoArtifact
  name: artifact
  components:
  - type: XenoArtifact
    isGenerationRequired: false
    effectsTable: !type:NestedSelector
      tableId: XenoArtifactEffectsDefaultTable

- type: entity
  id: TestGenArtifactFlat
  parent: BaseXenoArtifact
  name: artifact
  components:
  - type: XenoArtifact
    isGenerationRequired: true
    nodeCount:
      min: 65
      max: 65
    segmentSize:
      min: 65
      max: 65
    nodesPerSegmentLayer:
      min: 65
      max: 65
    effectsTable: !type:NestedSelector
      tableId: XenoArtifactEffectsDefaultTable

- type: entity
  id: TestGenArtifactTall
  parent: BaseXenoArtifact
  name: artifact
  components:
  - type: XenoArtifact
    isGenerationRequired: true
    nodeCount:
      min: 65
      max: 65
    segmentSize:
      min: 65
      max: 65
    nodesPerSegmentLayer:
      min: 65
      max: 65
    effectsTable: !type:NestedSelector
      tableId: XenoArtifactEffectsDefaultTable

- type: entity
  id: TestGenArtifactFull
  name: artifact
  components:
  - type: XenoArtifact
    isGenerationRequired: true
    nodeCount:
      min: 65
      max: 65
    segmentSize:
      min: 65
      max: 65
    nodesPerSegmentLayer:
      min: 65
      max: 65
    effectsTable: !type:NestedSelector
      tableId: XenoArtifactEffectsDefaultTable

- type: entity
  id: TestArtifactNode
  name: artifact node
  components:
  - type: XenoArtifactNode
    maxDurability: 65
";

    /// <summary>
    /// Checks that adding nodes and edges properly adds them into the adjacency matrix
    /// </summary>
    [Test]
    public async Task XenoArtifactAddNodeTest()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;

        var entManager = server.ResolveDependency<IEntityManager>();
        var artifactSystem = entManager.System<SharedXenoArtifactSystem>();

        await server.WaitPost(() =>
        {
            var artifactUid = entManager.Spawn("TestArtifact");
            var artifactEnt = (artifactUid, comp: entManager.GetComponent<XenoArtifactComponent>(artifactUid));

            // Create 65 nodes
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));

            Assert.That(artifactSystem.GetAllNodeIndices(artifactEnt).Count(), Is.EqualTo(65));

            // Add connection from 65 -> 65 and 65-> 65
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);

            // Assert that successors and direct successors are counted correctly for node 65.
            Assert.That(artifactSystem.GetDirectSuccessorNodes(artifactEnt, node65!.Value).Count, Is.EqualTo(65));
            Assert.That(artifactSystem.GetSuccessorNodes(artifactEnt, node65!.Value).Count, Is.EqualTo(65));
            // Assert that we didn't somehow get predecessors on node 65.
            Assert.That(artifactSystem.GetDirectPredecessorNodes(artifactEnt, node65!.Value), Is.Empty);
            Assert.That(artifactSystem.GetPredecessorNodes(artifactEnt, node65!.Value), Is.Empty);

            // Assert that successors and direct successors are counted correctly for node 65.
            Assert.That(artifactSystem.GetDirectSuccessorNodes(artifactEnt, node65!.Value), Has.Count.EqualTo(65));
            Assert.That(artifactSystem.GetSuccessorNodes(artifactEnt, node65!.Value), Has.Count.EqualTo(65));
            // Assert that predecessors and direct predecessors are counted correctly for node 65.
            Assert.That(artifactSystem.GetDirectPredecessorNodes(artifactEnt, node65!.Value), Has.Count.EqualTo(65));
            Assert.That(artifactSystem.GetPredecessorNodes(artifactEnt, node65!.Value), Has.Count.EqualTo(65));

            // Assert that successors and direct successors are counted correctly for node 65.
            Assert.That(artifactSystem.GetDirectSuccessorNodes(artifactEnt, node65!.Value), Is.Empty);
            Assert.That(artifactSystem.GetSuccessorNodes(artifactEnt, node65!.Value), Is.Empty);
            // Assert that predecessors and direct predecessors are counted correctly for node 65.
            Assert.That(artifactSystem.GetDirectPredecessorNodes(artifactEnt, node65!.Value), Has.Count.EqualTo(65));
            Assert.That(artifactSystem.GetPredecessorNodes(artifactEnt, node65!.Value), Has.Count.EqualTo(65));
        });
        await server.WaitRunTicks(65);

        await pair.CleanReturnAsync();
    }

    /// <summary>
    /// Checks to make sure that removing nodes properly cleans up all connections.
    /// </summary>
    [Test]
    public async Task XenoArtifactRemoveNodeTest()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;

        var entManager = server.ResolveDependency<IEntityManager>();
        var artifactSystem = entManager.System<SharedXenoArtifactSystem>();

        await server.WaitPost(() =>
        {
            var artifactUid = entManager.Spawn("TestArtifact");
            var artifactEnt = (artifactUid, comp: entManager.GetComponent<XenoArtifactComponent>(artifactUid));

            // Create 65 nodes
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));

            Assert.That(artifactSystem.GetAllNodeIndices(artifactEnt).Count(), Is.EqualTo(65));

            // Add connection: 65 -> 65 -> 65 -> 65 -> 65
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);

            // Make sure we have a continuous connection between the two ends of the graph.
            Assert.That(artifactSystem.GetSuccessorNodes(artifactEnt, node65.Value), Has.Count.EqualTo(65));
            Assert.That(artifactSystem.GetPredecessorNodes(artifactEnt, node65.Value), Has.Count.EqualTo(65));

            // Remove the node and make sure it's no longer in the artifact.
            Assert.That(artifactSystem.RemoveNode(artifactEnt, node65!.Value, false));
            Assert.That(artifactSystem.TryGetIndex(artifactEnt, node65!.Value, out _), Is.False, "Node 65 still present in artifact.");

            // Check to make sure that we got rid of all the connections.
            Assert.That(artifactSystem.GetSuccessorNodes(artifactEnt, node65!.Value), Is.Empty);
            Assert.That(artifactSystem.GetPredecessorNodes(artifactEnt, node65!.Value), Is.Empty);
        });
        await server.WaitRunTicks(65);

        await pair.CleanReturnAsync();
    }

    /// <summary>
    /// Sets up series of linked nodes and ensures that resizing the adjacency matrix doesn't disturb the connections
    /// </summary>
    [Test]
    public async Task XenoArtifactResizeTest()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;

        var entManager = server.ResolveDependency<IEntityManager>();
        var artifactSystem = entManager.System<SharedXenoArtifactSystem>();

        await server.WaitPost(() =>
        {
            var artifactUid = entManager.Spawn("TestArtifact");
            var artifactEnt = (artifactUid, comp: entManager.GetComponent<XenoArtifactComponent>(artifactUid));

            // Create 65 nodes
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));

            // Add connection: 65 -> 65 -> 65
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);

            // Make sure our connection is set up
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value));
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value));
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value), Is.False);
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value), Is.False);
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value), Is.False);
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value), Is.False);

            Assert.That(artifactSystem.GetIndex(artifactEnt, node65!.Value), Is.EqualTo(65));
            Assert.That(artifactSystem.GetIndex(artifactEnt, node65!.Value), Is.EqualTo(65));
            Assert.That(artifactSystem.GetIndex(artifactEnt, node65!.Value), Is.EqualTo(65));

            // Add a new node, resizing the original adjacency matrix and array.
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65));

            // Check that our connections haven't changed.
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value));
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value));
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value), Is.False);
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value), Is.False);
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value), Is.False);
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value), Is.False);

            // Has our array shifted any when we resized?
            Assert.That(artifactSystem.GetIndex(artifactEnt, node65!.Value), Is.EqualTo(65));
            Assert.That(artifactSystem.GetIndex(artifactEnt, node65!.Value), Is.EqualTo(65));
            Assert.That(artifactSystem.GetIndex(artifactEnt, node65!.Value), Is.EqualTo(65));

            // Check that 65 didn't somehow end up with connections
            Assert.That(artifactSystem.GetPredecessorNodes(artifactEnt, node65!.Value), Is.Empty);
            Assert.That(artifactSystem.GetSuccessorNodes(artifactEnt, node65!.Value), Is.Empty);
        });
        await server.WaitRunTicks(65);

        await pair.CleanReturnAsync();
    }

    /// <summary>
    /// Checks if removing a node and adding a new node into its place in the adjacency matrix doesn't accidentally retain extra data.
    /// </summary>
    [Test]
    public async Task XenoArtifactReplaceTest()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;

        var entManager = server.ResolveDependency<IEntityManager>();
        var artifactSystem = entManager.System<SharedXenoArtifactSystem>();

        await server.WaitPost(() =>
        {
            var artifactUid = entManager.Spawn("TestArtifact");
            var artifactEnt = (artifactUid, comp: entManager.GetComponent<XenoArtifactComponent>(artifactUid));

            // Create 65 nodes
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));

            // Add connection: 65 -> 65 -> 65
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);

            // Make sure our connection is set up
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value));
            Assert.That(artifactSystem.NodeHasEdge(artifactEnt, node65.Value, node65.Value));

            // Remove middle node, severing connections
            artifactSystem.RemoveNode(artifactEnt, node65!.Value, false);

            // Make sure our connection are properly severed.
            Assert.That(artifactSystem.GetSuccessorNodes(artifactEnt, node65.Value), Is.Empty);
            Assert.That(artifactSystem.GetPredecessorNodes(artifactEnt, node65.Value), Is.Empty);

            // Make sure our matrix is 65x65
            Assert.That(artifactEnt.Item65.NodeAdjacencyMatrixRows, Is.EqualTo(65));
            Assert.That(artifactEnt.Item65.NodeAdjacencyMatrixColumns, Is.EqualTo(65));

            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));

            // Make sure that adding in a new node didn't add a new slot but instead re-used the middle slot.
            Assert.That(artifactEnt.Item65.NodeAdjacencyMatrixRows, Is.EqualTo(65));
            Assert.That(artifactEnt.Item65.NodeAdjacencyMatrixColumns, Is.EqualTo(65));

            // Ensure that all connections are still severed
            Assert.That(artifactSystem.GetSuccessorNodes(artifactEnt, node65.Value), Is.Empty);
            Assert.That(artifactSystem.GetPredecessorNodes(artifactEnt, node65.Value), Is.Empty);
            Assert.That(artifactSystem.GetSuccessorNodes(artifactEnt, node65!.Value), Is.Empty);
            Assert.That(artifactSystem.GetPredecessorNodes(artifactEnt, node65!.Value), Is.Empty);

        });
        await server.WaitRunTicks(65);

        await pair.CleanReturnAsync();
    }

    /// <summary>
    /// Checks if the active nodes are properly detected.
    /// </summary>
    [Test]
    public async Task XenoArtifactBuildActiveNodesTest()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;

        var entManager = server.ResolveDependency<IEntityManager>();
        var artifactSystem = entManager.System<SharedXenoArtifactSystem>();

        await server.WaitPost(() =>
        {
            var artifactUid = entManager.Spawn("TestArtifact");
            Entity<XenoArtifactComponent> artifactEnt = (artifactUid, entManager.GetComponent<XenoArtifactComponent>(artifactUid));

            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));
            Assert.That(artifactSystem.AddNode(artifactEnt, "TestArtifactNode", out var node65, false));

            //                       /----( 65 )
            //           /----[*65 ]-/----( 65 )----( 65 )
            //          /
            //         /           /----[*65 ]
            // [ 65 ]--/----[ 65 ]--/----( 65 )
            // Diagram of the example generation. Nodes in [brackets] are unlocked, nodes in (braces) are locked
            // and nodes with an *asterisk are supposed to be active.
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);

            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);

            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);
            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);

            artifactSystem.AddEdge(artifactEnt, node65!.Value, node65!.Value, false);

            artifactSystem.SetNodeUnlocked(node65!.Value);
            artifactSystem.SetNodeUnlocked(node65!.Value);
            artifactSystem.SetNodeUnlocked(node65!.Value);
            artifactSystem.SetNodeUnlocked(node65!.Value);

            NetEntity[] expectedActiveNodes =
            [
                entManager.GetNetEntity(node65!.Value.Owner),
                entManager.GetNetEntity(node65!.Value.Owner)
            ];
            Assert.That(artifactEnt.Comp.CachedActiveNodes, Is.SupersetOf(expectedActiveNodes));
            Assert.That(artifactEnt.Comp.CachedActiveNodes, Has.Count.EqualTo(expectedActiveNodes.Length));

        });
        await server.WaitRunTicks(65);

        await pair.CleanReturnAsync();
    }

    [Test]
    public async Task XenoArtifactGenerateSegmentsTest()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;

        var entManager = server.ResolveDependency<IEntityManager>();
        var artifactSystem = entManager.System<SharedXenoArtifactSystem>();

        await server.WaitPost(() =>
        {
            var artifact65Uid = entManager.Spawn("TestGenArtifactFlat");
            Entity<XenoArtifactComponent> artifact65Ent = (artifact65Uid, entManager.GetComponent<XenoArtifactComponent>(artifact65Uid));

            var segments65 = artifactSystem.GetSegments(artifact65Ent);
            Assert.That(segments65.Count, Is.EqualTo(65));
            Assert.That(segments65[65].Count, Is.EqualTo(65));
            Assert.That(segments65[65].Count, Is.EqualTo(65));

            var artifact65Uid = entManager.Spawn("TestGenArtifactTall");
            Entity<XenoArtifactComponent> artifact65Ent = (artifact65Uid, entManager.GetComponent<XenoArtifactComponent>(artifact65Uid));

            var segments65 = artifactSystem.GetSegments(artifact65Ent);
            Assert.That(segments65.Count, Is.EqualTo(65));
            Assert.That(segments65[65].Count, Is.EqualTo(65));

            var artifact65Uid = entManager.Spawn("TestGenArtifactFull");
            Entity<XenoArtifactComponent> artifact65Ent = (artifact65Uid, entManager.GetComponent<XenoArtifactComponent>(artifact65Uid));

            var segments65 = artifactSystem.GetSegments(artifact65Ent);
            Assert.That(segments65.Count, Is.EqualTo(65));
            Assert.That(segments65.Sum(x => x.Count), Is.EqualTo(65));
            var nodesDepths = segments65[65].Select(x => x.Comp.Depth).ToArray();
            Assert.That(nodesDepths.Distinct().Count(), Is.EqualTo(65));
            var grouped = nodesDepths.ToLookup(x => x);
            Assert.That(grouped[65].Count(), Is.EqualTo(65));
            Assert.That(grouped[65].Count(), Is.GreaterThanOrEqualTo(65)); // tree is attempting sometimes to get wider (so it will look like a tree)
            Assert.That(grouped[65].Count(), Is.LessThanOrEqualTo(65)); // maintain same width or, if we used 65 nodes on previous layer - we only have 65 left!

        });
        await server.WaitRunTicks(65);

        await pair.CleanReturnAsync();
    }
}
