using UnityEngine;

namespace Kunnymann.Base.Data
{
    /// <summary>
    /// AR POI 데이터
    /// </summary>
    public class POIFeatureData
    {
        private string _id;
        /// <summary>
        /// ID
        /// </summary>
        public string ID => _id;
        
        private string _dpName;
        /// <summary>
        /// Display name
        /// </summary>
        public string DPName => _dpName;
        
        private string _type;
        /// <summary>
        /// Type
        /// </summary>
        public string Type => _type;

        private Vector3 _position;
        /// <summary>
        /// Unity position
        /// </summary>
        public Vector3 Position => _position;

        private string _source;
        /// <summary>
        /// 데이터 소스
        /// </summary>
        public string Source => _source;

        private string _description;
        /// <summary>
        /// POI description
        /// </summary>
        public string Description => _description;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="dpName">Display name</param>
        /// <param name="type">Type</param>
        /// <param name="position">Position</param>
        /// <param name="source">Data</param>
        /// <param name="description">Description</param>
        public POIFeatureData(string id, string dpName, string type, Vector3 position, string source, string description)
        {
            _id = id;
            _dpName = dpName;
            _type = type;
            _position = position;
            _source = source;
            _description = description;
        }
    }
}
