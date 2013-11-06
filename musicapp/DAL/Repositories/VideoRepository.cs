using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using musicapp.Models;
using musicapp;
using musicapp.DAL.Interfaces;
using System.Collections;
using System.Web.Mvc;

namespace musicapp.DAL.Repositories
{
    public class VideoRepository: IVideoRepository
    {
        private DatabaseContext context;

        public VideoRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public VideoTable CreateVideo(int UserID, string Artist, DateTime DateUpload, string Description, string fileName, string Guid, string musicName, string Path, string TypeMusic, string Extension ) 
        {
            Models.VideoTable video = new Models.VideoTable();
            video.UserId = UserID;
            video.musicArtist = Artist;
            video.musicDateUpload = DateUpload;
            video.musicDescription = Description;
            video.musicFileName = fileName;
            video.musicGuid = Guid;
            video.musicName = musicName;
            video.musicPath = Path;
            video.musicType = TypeMusic;
            video.musicTypeExtension = Extension;
           
            context.VideoTables.Add(video);
            context.SaveChanges();

            return GetVideo(video.musicID, "");
        }

        public VideoTable GetVideo(int MusicId, string temporaryArgument)
        {
            var result = from v in context.VideoTables where (v.musicID == MusicId) select v;

            if (result.Count() != 0)
            {
                var videoFound = result.FirstOrDefault();

                return videoFound;
            }
            else
            {
                return null;
            }
        }

        public IQueryable<musicapp.Models.VideoTable> GetVideo(int videoUniqNumber)
        {
            var queryGetVideo = from p in context.VideoTables
                                where (p.musicID.Equals(videoUniqNumber))
                                select p;
            return queryGetVideo;
        }

        public IQueryable<VideoCorrelation> GetTopRelatedVideos(int videoID, int howManyVideos)
        {
            var result = from vid in context.VideoCorrelation
                         where (vid.OriginalVideoID == videoID)
                         select vid;

            result.Take(howManyVideos);

            return result;
        }

        public IEnumerable GetCoverFileNameAutoComplete(string Autocomplete)
        {
            var query = from v in context.VideoTables
                        where (v.musicFileName.StartsWith(Autocomplete, StringComparison.CurrentCultureIgnoreCase))
                        select new {Autocomplete= v.musicFileName};                         
                   
            return query;
        }

    }
}