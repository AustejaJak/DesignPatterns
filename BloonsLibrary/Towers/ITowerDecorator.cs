using SplashKitSDK;

namespace BloonsProject
{
    public interface ITowerDecorator
    {
        Bitmap GetTowerBitmap();
        int GetFireRateLevel();
        int GetRangeLevel();
    }
}