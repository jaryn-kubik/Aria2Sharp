using System.Collections.Generic;

namespace Aria2Sharp
{
    public class Aria2Info
    {
        public string Gid { get; set; }
        public InfoStatus Status { get; set; }
        public string BitField { get; set; }
        public string Dir { get; set; }

        public IReadOnlyList<Aria2File> Files { get; set; }

        public long TotalLength { get; set; }
        public long CompletedLength { get; set; }
        public long UploadLength { get; set; }
        public long DownloadSpeed { get; set; }
        public long UploadSpeed { get; set; }

        public long PieceLength { get; set; }
        public long NumPieces { get; set; }
        public long Connections { get; set; }
        public long VerifiedLength { get; set; }
        public bool VerifyIntegrityPending { get; set; }

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        //TODO: bittorrent
        public string InfoHash { get; set; }
        public long NumSeeders { get; set; }
        public bool Seeder { get; set; }
        public string BelongsTo { get; set; }
        public IReadOnlyList<string> followedBy { get; set; }
        public IReadOnlyList<string> following { get; set; }

        public enum InfoStatus { Active, Waiting, Paused, Error, Complete, Removed }
    }
}