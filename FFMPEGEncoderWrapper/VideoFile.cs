// -----------------------------------------------------------------------
// <copyright file="VideoFile.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace FFMPEGEncoderWrapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VideoFile
    {
        private string _PathToFile;
        public string PathToFile
        {
            get
            {
                return _PathToFile;
            }
            set
            {
                _PathToFile = value;
            }
        }

        public TimeSpan Duration { get; set; }
        public double BitRate { get; set; }
        public string RawAudioFormat { get; set; }
        public string AudioFormat { get; set; }
        public string RawVideoFormat { get; set; }
        public string VideoFormat { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string RawInfo { get; set; }
        public bool InfoGethered { get; set; }

        public VideoFile(string pathToFile)
        {
            _PathToFile = pathToFile;
            Initialize();
        }

        public void Initialize()
        {
            this.InfoGethered = false;
            if (string.IsNullOrEmpty(_PathToFile))
            {
                throw new Exception("No path");
            }
            if(!File.Exists(_PathToFile))
            {
                throw new Exception("Video path"+ _PathToFile + " does not exist!");
            }
        }
    }
}
