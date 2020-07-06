namespace DetectorAPI.V1.Models
{
    public class ProviderDetectionResponse
    {
        public ProviderDetection[] Detections { get; set; }
    }

    public class ProviderDetection
    {
        public string Provider { get; set; }
        public Domain[] Clients { get; set; }
        public ClientError[] ClientErrors { get; set; }
    }

    public class ClientError
    {
        public string Client { get; set; }
        public string ErrorMessage { get; set; }
    }
}
