using System;
using System.Collections.Generic;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    // Improved FlyweightFactory implementation
    public class FlyweightFactory
    {
        private static readonly Dictionary<string, Bitmap> _bitmaps = new Dictionary<string, Bitmap>();
        private static readonly string baseDirectory = AppContext.BaseDirectory;

        public static Bitmap GetBitmap(string name, string relativePath)
        {
            string cacheKey = $"{name}_{relativePath}";

            return _bitmaps.TryGetValue(cacheKey, out var bitmap) 
                ? bitmap 
                : LoadAndCacheBitmap(name, relativePath, cacheKey);
        }

        private static Bitmap LoadAndCacheBitmap(string name, string relativePath, string cacheKey)
        {
            string fullPath = Path.Combine(baseDirectory, relativePath);
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Image file not found: {fullPath}");
            }
    
            var bitmap = new Bitmap(name, fullPath);
            _bitmaps[cacheKey] = bitmap;
            return bitmap;
        }

        // Add cleanup method to release resources when needed
        public static void ClearCache()
        {
            foreach (var bitmap in _bitmaps.Values)
            {
                bitmap.Dispose();
            }
            _bitmaps.Clear();
        }
    }
}