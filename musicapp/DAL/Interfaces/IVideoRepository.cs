using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using musicapp.Models;
using System.Collections;

namespace musicapp.DAL.Interfaces
{
    public interface IVideoRepository
    {
        VideoTable CreateVideo(int UserID, string Artist, DateTime DateUpload, string Description, string fileName, string Guid, string musicName, string Path, string TypeMusic, string Extension);
        IQueryable<VideoTable> GetVideo(int MusicId);
        IQueryable<VideoCorrelation> GetTopRelatedVideos(int videoID, int howManyVideos);
        IEnumerable GetCoverFileNameAutoComplete(string Autocomplete);
    }
}
