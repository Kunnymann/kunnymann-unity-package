using System.IO;

namespace Kunnymann.Base.Editor
{
    /// <summary>
    /// 커스텀 하이어라키 상수
    /// </summary>
    public class CustomHierarchyConst
    {
        /// <summary>
        /// 아이콘 경로
        /// </summary>
        public static string PathIcon = Path.Combine("Image","Icon", "img-module-manager");

        /// <summary>
        /// ModuleManager 패턴
        /// </summary>
        public static string PatternModuleManager = @"Module\s?Manager";
        /// <summary>
        /// DivisionLine 패턴
        /// </summary>
        public static string PatternDivisionLine = @"^-{2}";

        /// <summary>
        /// 하이어라키에 렌더링 될 아이콘 사이즈
        /// </summary>
        public static int IconSize = 16;
        /// <summary>
        /// 아이콘 컬러
        /// </summary>
        public static string ColorIcon = "#73F5F5";
        /// <summary>
        /// 유니티 컬러
        /// </summary>
        public static string ColorUnityBackground = "#383838";
    }
}