using System.Collections.Generic;
using System.Linq;
using GeoJSON;
using Kunnymann.Navigation.Data;
using UnityEngine;


namespace Kunnymann.Navigation.Sample
{
    [CreateAssetMenu(fileName = "SampleNavigationConfig", menuName = "Navigation/SampleNavigationConfig")]
    public class SampleNavigationConfig : NavigationConfig
    {
        [SerializeField] private TextAsset geojsonFile;

        private List<INode> _nodes;
        private List<ILink> _links;

        public override void Initialize()
        {
            Load();
        }

        public override List<ILink> GetLinkContainer()
        {
            return _links;
        }

        public override List<INode> GetNodeContainer()
        {
            return _nodes;
        }

        private void Load()
        {
            if (geojsonFile == null)
            {
                Debug.LogError("Can't load geojson data");
                return;
            }

            // Load Nodes
            FeatureCollection rawData = GeoJSONObject.Deserialize(geojsonFile.text);

            _nodes = new List<INode>();
            rawData.features.ForEach(feature =>
            {
                if (feature.geometry is PointGeometryObject)
                {
                    PointGeometryObject geometryPoint = feature.geometry as PointGeometryObject;
                    int nodeID = int.Parse(feature.properties["id"]);
                    Vector3 position = new Vector3(geometryPoint.coordinates.latitude, 0, geometryPoint.coordinates.longitude);

                    Node node = new Node(position, TransitionType.Path, nodeID.ToString(), "Empty DPName");
                    node.connectedNodeIds.AddRange(feature.properties["linked"].TrimStart('[').TrimEnd(']').Split(','));
                    
                    _nodes.Add(node);
                }
            });

            // Load Links
            _links = new List<ILink>();

            _nodes.ForEach(node =>
            {
                (node as Node).connectedNodeIds.ForEach(id =>
                {
                    Node connectedNode = _nodes.Find(n => n.ID == id) as Node;
                    if (node.ConnectedNode == null)
                    {
                        node.AddConnectedNode(connectedNode);
                    }
                    else
                    {
                        if (!node.ConnectedNode.ToList().Contains(connectedNode))
                        {
                            node.AddConnectedNode(connectedNode);
                        }
                    }

                    Link result = _links.Find(l => (l.From == node && l.To == connectedNode) || (l.To == node && l.From == connectedNode)) as Link;

                    if (result == null)
                    {
                        Link newLink = new Link(node, connectedNode, LinkDirection.Bidirection, false, $"LinkData", "Empty DPName");

                        node.AddLink(newLink);
                        node.AddConnectedNode(connectedNode);

                        _links.Add(newLink);

                        return;
                    }

                    node.AddLink(result);
                    node.AddConnectedNode(connectedNode);

                    _links.Add(result);
                });
            });
        }
    }
}