using System.Collections.Generic;

namespace ContentAggregator.Common
{
    public static class Consts
    {
        // consts fields are hardcoded in assemblies
        // if values are changed, need to recompile while solution
        public const int UsernameMaxLength = 25;
        public const int DescriptionMaxLength = 300;
        public const int PostTitleLength = 50;
        public const int PostContentLength = 2000;
        public const int CommentContentLength = 1000;

        public static class Picture
        {
            public static readonly List<string> AvailableMimeTypes = new List<string>
            {
                ContentTypes.ImageJpeg,
                ContentTypes.ImagePng
            };
        }

        public static class ContentTypes
        {
            public static readonly string ImagePng = "image/png";
            public static readonly string ImageJpeg = "image/jpeg";
            public static readonly string Pdf = "application/pdf";
            public static readonly string Json = "application/json";
        }

    }
}