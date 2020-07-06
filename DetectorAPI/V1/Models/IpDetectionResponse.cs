namespace DetectorAPI.V1.Models
{
    public class IpDetectionResponse
    {
        public IpDetection[] Detection { get; set; }
    }

    public class IpDetection
    {
        public string HostAddress { get; set; }
        public string[] Ips { get; set; }
        public string ErrorMessage { get; set; }
    }
}
