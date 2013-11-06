// -----------------------------------------------------------------------
// <copyright file="EncodedVideo.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace FFMPEGEncoderWrapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EncodedVideo
    {
        public string EncodedVideoPath { get; set; }
        public string ThumbailPath { get; set; }
        public string EncodingLog { get; set; }
        public bool SuccessEncoding { get; set; }
    }
}
