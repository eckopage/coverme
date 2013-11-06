// -----------------------------------------------------------------------
// <copyright file="QuickAudioEncodingCommands.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace FFMPEGEncoderWrapper.ShortCommand
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AudioEncodingCommands
    {
        /// <summary>
        /// MP3
        /// </summary>
        public static string MP3128Kbps = "-y -ab 128k -ar 44100";
        public static string MP396Kbps = "-y -ab 96k -ar 44100";
        public static string MP364Kbps = "-y -ab 64k -ar 44100";
        public static string MP332Kbps = "-y -ab 32k -ar 44100";
    }
}
