// -----------------------------------------------------------------------
// <copyright file="Encoder.cs" company="">
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
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Encoder
    {
        /// <summary>
        /// FFmpegPath -test
        /// </summary>
        public string FFmpegPath { get; set; }

        /// <summary>
        /// Params
        /// </summary>
        private string Params { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodingCommand"></param>
        /// <param name="outputFile"></param>
        /// <param name="getVideoThumbail"></param>
        /// <returns></returns>
        public EncodedVideo EncodeVideo(VideoFile input, string encodingCommand, string outputFile, bool getVideoThumbail)
        {
            EncodedVideo encoded = new EncodedVideo();

            Params = string.Format("-i {0} {1} {2}", input.PathToFile, encodingCommand, outputFile);
            string output = RunProcess(Params);
            encoded.EncodingLog = output;
            encoded.EncodedVideoPath = outputFile;


            if (File.Exists(outputFile))
            {
                encoded.SuccessEncoding = true;

                if (getVideoThumbail)
                {
                    string saveThumbailTo = outputFile + "_thumb.jpg";
                    if (GetVideoThumbnail(input, saveThumbailTo))
                    {
                        encoded.ThumbailPath = saveThumbailTo;
                    }
                }
            }
            else
            {
                encoded.SuccessEncoding = false;
            }

            return encoded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="saveThumbnailTo"></param>
        /// <returns></returns>
        public bool GetVideoThumbnail(VideoFile input, string saveThumbnailTo)
        {
            if (!input.InfoGethered)
            {
                GetVideoInfo(input);
            }

            int secs;
            secs = (int)Math.Round(TimeSpan.FromTicks(input.Duration.Ticks / 3).TotalSeconds, 0);
            string Params = string.Format("-i {0} {1} -vcodec mjpeg -ss {2} -vframes 1 -an -f rawvideo", input.PathToFile, saveThumbnailTo, secs);
            string output = RunProcess(Params);

            if (File.Exists(saveThumbnailTo))
            {
                return true;
            }
            else
            {
                //try running again at frame 1 to get something
                Params = string.Format("-i {0} {1} -vcodec mjpeg -ss {2} -vframes 1 -an -f rawvideo", input.PathToFile, saveThumbnailTo, 1);
                output = RunProcess(Params);

                if (File.Exists(saveThumbnailTo))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public void GetVideoInfo(VideoFile input)
        {
            string Params = string.Format("-i {0}", input.PathToFile);
            string output = RunProcess(Params);
            input.RawInfo = output;
            input.Duration = ExtractDuration(input.RawInfo);
            input.BitRate = ExtractBitrate(input.RawInfo);
            input.RawAudioFormat = ExtractRawAudioFormat(input.RawInfo);
            input.AudioFormat = ExtractAudioFormat(input.RawAudioFormat);
            input.RawVideoFormat = ExtractRawVideoFormat(input.RawInfo);
            input.VideoFormat = ExtractVideoFormat(input.RawVideoFormat);
            input.Width = ExtractVideoWidth(input.RawInfo);
            input.Height = ExtractVideoHeight(input.RawInfo);
            input.InfoGethered = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        private string RunProcess(string Parameters)
        {
            ProcessStartInfo oInfo = new ProcessStartInfo(this.FFmpegPath, Parameters);
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;

            string output = null; 
            StreamReader srOutput = null;

            try
            {
                Process proc = System.Diagnostics.Process.Start(oInfo);

                proc.WaitForExit();
                srOutput = proc.StandardError;
                output = srOutput.ReadToEnd();
               
                proc.Close();
            }
            catch (Exception)
            {
                output = string.Empty;
            }
            finally
            {
                if (srOutput != null)
                {
                    srOutput.Close();
                    srOutput.Dispose();
                }
            }
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawInfo"></param>
        /// <returns></returns>
        private TimeSpan ExtractDuration(string rawInfo)
        {
            TimeSpan t = new TimeSpan(0);
            Regex re = new Regex("[D|d]uration:.((\\d|:|\\.)*)", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);

            if (m.Success)
            {
                string duration = m.Groups[1].Value;
                string[] timepieces = duration.Split(new char[] { ':', '.' });
                if (timepieces.Length == 4)
                {
                    t = new TimeSpan(0, Convert.ToInt16(timepieces[0]), Convert.ToInt16(timepieces[1]), Convert.ToInt16(timepieces[2]), Convert.ToInt16(timepieces[3]));
                }
            }

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawInfo"></param>
        /// <returns></returns>
        private double ExtractBitrate(string rawInfo)
        {
            Regex re = new Regex("[B|b]itrate:.((\\d|:)*)", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            double kb = 0.0;
            if (m.Success)
            {
                Double.TryParse(m.Groups[1].Value, out kb);
            }
            return kb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawInfo"></param>
        /// <returns></returns>
        private string ExtractRawAudioFormat(string rawInfo)
        {
            string a = string.Empty;
            Regex re = new Regex("[A|a]udio:.*", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                a = m.Value;
            }
            return a.Replace("Audio: ", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawAudioFormat"></param>
        /// <returns></returns>
        private string ExtractAudioFormat(string rawAudioFormat)
        {
            string[] parts = rawAudioFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Audio: ", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawInfo"></param>
        /// <returns></returns>
        private string ExtractRawVideoFormat(string rawInfo)
        {
            string v = string.Empty;
            Regex re = new Regex("[V|v]ideo:.*", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                v = m.Value;
            }
            return v.Replace("Video: ", ""); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawVideoFormat"></param>
        /// <returns></returns>
        private string ExtractVideoFormat(string rawVideoFormat)
        {
            string[] parts = rawVideoFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Video: ", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawInfo"></param>
        /// <returns></returns>
        private int ExtractVideoWidth(string rawInfo)
        {
            int width = 0;
            Regex re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                int.TryParse(m.Groups[1].Value, out width);
            }
            return width;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawInfo"></param>
        /// <returns></returns>
        private int ExtractVideoHeight(string rawInfo)
        {
            int height = 0;
            Regex re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                int.TryParse(m.Groups[2].Value, out height);
            }
            return height;
        } 
    }
}
