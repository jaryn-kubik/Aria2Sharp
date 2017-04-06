namespace Aria2Sharp
{
    public class Aria2Stats
    {
        public long DownloadSpeed { get; set; }
        public long UploadSpeed { get; set; }
        public long NumActive { get; set; }
        public long NumWaiting { get; set; }
        public long NumStopped { get; set; }
        public long NumStoppedTotal { get; set; }
    }
}