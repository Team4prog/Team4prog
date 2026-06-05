using Newtonsoft.Json;

namespace Team4prog.UI.Features.Catalog
{
    // Minimal catalog record model for DonkeyCar JSON Lines catalog_0.catalog.
    public sealed class FrameData
    {
        [JsonProperty("_index")]
        public int Index { get; set; }

        [JsonProperty("cam/image_array")]
        public string? ImagePath { get; set; }

        [JsonProperty("user/angle")]
        public double? Angle { get; set; }

        [JsonProperty("user/throttle")]
        public double? Throttle { get; set; }

        [JsonProperty("pilot/angle")]
        public double? PilotAngle { get; set; }

        [JsonProperty("pilot/throttle")]
        public double? PilotThrottle { get; set; }
    }
}

