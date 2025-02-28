using Kunnymann.Base;
using Kunnymann.Navigation.Data;
using System.Linq;
using UnityEngine;

namespace Kunnymann.Navigation.Sample
{
    public class NavigationService : NavigationBehaviour
    {
        private static Vector3 LINE_ROTATION = new Vector3(90f, 0, 0);
        private static float PATH_HEIGHT_OFFSET = 0.2f;

        [SerializeField] private SampleNavigationConfig _sampleConfig;
        [SerializeField] private string _destinationNodeID;

        private LineRenderer _pathLineRenderer;

        private void Start()
        {
            NavigationModule _navigationModule = ModuleManager.Instance.GetModule<NavigationModule>();
            _navigationModule.RegisterBehavour(this);
            
            Navigation = _navigationModule;
            Navigation.Ready(_sampleConfig, Camera.main.transform);
            DrawNavigationConfig();
        }

        public void StartNavigation()
        {
            INode destination = _sampleConfig.GetNodeContainer().Find(node => node.ID == _destinationNodeID);

            if (destination == null)
            {
                Debug.LogError($"Can't find destination node with ID: {_destinationNodeID}");
                return;
            }

            Navigation.SetupPath(destination, TransitionType.Path);
            Navigation.Start();
        }

        public void StopNavigation()
        {
            Navigation.Cancel();
        }

        public override void OnStartNavigationSession(Path path)
        {
            Debug.Log("Start navigation");
        }

        public override void OnUpdatePath(Path path)
        {
            if (_pathLineRenderer == null)
            {
                GameObject pathLineObject = new GameObject("PathLineRenderer");
                _pathLineRenderer = pathLineObject.AddComponent<LineRenderer>();
                _pathLineRenderer.transform.eulerAngles = LINE_ROTATION;
                _pathLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                _pathLineRenderer.positionCount = path.Count;
                _pathLineRenderer.startWidth = 0.1f;
                _pathLineRenderer.endWidth = 0.1f;
                _pathLineRenderer.alignment = LineAlignment.TransformZ;
                _pathLineRenderer.numCornerVertices = 10;
                _pathLineRenderer.numCapVertices = 10;
                _pathLineRenderer.startColor = Color.green;
                _pathLineRenderer.endColor = Color.green;
            }

            _pathLineRenderer.positionCount = path.Where(element => element is INode).Count();
            int nodeIdx = 0;

            path.Where(element => element is INode).ToList().ForEach((element) =>
            {
                INode node = element as INode;
                _pathLineRenderer.SetPosition(nodeIdx, node.Position + (Vector3.up * PATH_HEIGHT_OFFSET));
                nodeIdx++;
            });
        }

        public override void OnEndNavigationSession(SessionEndType end)
        {
            if (_pathLineRenderer != null)
            {
                _pathLineRenderer.positionCount = 0;
            }

            if (end == SessionEndType.Arrived)
            {
                Debug.Log("Arrived at the destination");
            }
            else if (end == SessionEndType.Error)
            {
                Debug.Log("Error");
            }
            else if (end == SessionEndType.Canceled)
            {
                Debug.Log("Canceled");
            }
        }

        public override void OnRecoveryPath(Path path)
        {
            Debug.Log("Recovery path");
        }

        private void DrawNavigationConfig()
        {
            GameObject rootMapData = new GameObject("RootMapData");

            foreach (var node in _sampleConfig.GetNodeContainer())
            {
                GameObject nodeData = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                nodeData.name = node.ID;
                nodeData.transform.position = node.Position;
                nodeData.transform.localScale = Vector3.one * 0.1f;
                nodeData.transform.parent = rootMapData.transform;
                
            }

            foreach(var link in _sampleConfig.GetLinkContainer())
            {
                GameObject linkData = new GameObject($"Link ({link.From} -> {link.To})");
                linkData.transform.position = (link.From.Position + link.To.Position) / 2;
                linkData.transform.eulerAngles = LINE_ROTATION;
                linkData.transform.parent = rootMapData.transform;

                // 데코레이팅
                LineRenderer linkLineRenderer = linkData.AddComponent<LineRenderer>();
                linkLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                linkLineRenderer.positionCount = 2;
                linkLineRenderer.startWidth = 0.05f;
                linkLineRenderer.endWidth = 0.05f;
                linkLineRenderer.alignment = LineAlignment.TransformZ;
                linkLineRenderer.numCornerVertices = 10;
                linkLineRenderer.numCapVertices = 10;
                linkLineRenderer.startColor = Color.cyan;
                linkLineRenderer.endColor = Color.cyan;

                linkLineRenderer.SetPosition(0, link.From.Position);
                linkLineRenderer.SetPosition(1, link.To.Position);
            }
        }
    }
}