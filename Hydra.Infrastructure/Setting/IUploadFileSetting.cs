﻿namespace Hydra.Infrastructure.Setting
{
    public interface IUploadFileSetting
    {
        bool AllowDuplicateFile { get; set; }
        string WhiteListExtensions { get; set; }
        string SignatureValidationExtensions { get; set; }

        long MaxSizeLimitSmallFile { get; set; }

        long MinSizeLimitSmallFile { get; set; }

        long MaxSizeLimitLargeFile { get; set; }

        string ImagesExtensions { get; set; }
        string VideosExtensions { get; set; }
        string MusicExtensions { get; set; }
        string DocumentsExtensions { get; set; }
        int ImageThumbnailSize { get; set; }


    }
}
