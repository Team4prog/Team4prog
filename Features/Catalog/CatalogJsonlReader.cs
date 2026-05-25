using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Team4prog.UI.Features.Catalog
{
    // Streaming reader for DonkeyCar JSON Lines catalog files (catalog_0.catalog).
    public static class CatalogJsonlReader
    {
        public static IEnumerable<FrameData> ReadFrames(string catalogPath)
        {
            if (string.IsNullOrWhiteSpace(catalogPath))
                throw new ArgumentException("catalogPath is required.", nameof(catalogPath));

            foreach (var line in File.ReadLines(catalogPath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                FrameData? frame = null;
                try
                {
                    frame = JsonConvert.DeserializeObject<FrameData>(line);
                }
                catch
                {
                    // Skip malformed lines; caller can decide whether to log.
                }

                if (frame != null)
                    yield return frame;
            }
        }
    }
}

