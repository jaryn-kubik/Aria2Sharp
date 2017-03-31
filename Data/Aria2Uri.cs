namespace Aria2.NET
{
    public class Aria2Uri
    {
        public string Uri { get; set; }
        public UriStatus Status { get; set; }

        public enum UriStatus { Used, Waiting }
    }
}