using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchManager.Models
{
    public class SeasonModel
    {
        public string SeasonNumber { get; set; }
        public string SeasonEpisodes { get; set; }


        public SeasonModel(string seasonNumber, string seasonEpisodes)
        {
            SeasonNumber = seasonNumber;
            SeasonEpisodes = seasonEpisodes;
        }


        public override string ToString()
        {
            return $"Сезон: {SeasonNumber}, Серий: {SeasonEpisodes}";
        }
    }
}
