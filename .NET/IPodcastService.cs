using Sabio.Models.Domain;
using Sabio.Models.Requests.Podcasts;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IPodcastService
    {
        int AddPodcast(PodcastAddRequest model, int userId);
        void DeletePodcast(int id);
        List<Podcast> GetAllPodcast();
        void UpdatePodcast(PodcastUpdateRequest model);
    }
}