namespace Aria2Sharp
{
    public class Aria2Peer
    {
        public string PeerId { get; set; }
        public string Bitfield { get; set; }

        public string Ip { get; set; }
        public long Port { get; set; }

        public bool AmChoking { get; set; }
        public bool PeerChoking { get; set; }
        public bool Seeder { get; set; }

        public long DownloadSpeed { get; set; }
        public long UploadSpeed { get; set; }
    }
}