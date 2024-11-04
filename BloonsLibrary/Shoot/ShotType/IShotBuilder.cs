namespace BloonsProject
{
    public interface IShotBuilder
    {
        IShotBuilder SetShotSpeed(double speed);
        IShotBuilder SetDamage(int damage);
        IShotBuilder SetProjectileStationaryTime(double time);
        IShotBuilder SetProjectileDimensions(double length, double width);
        IShotBuilder SetUpgradeCosts(int firerateCost, int rangeCost);
        IShotBuilder SetProjectileProperties(double speed, double size);
        IShotBuilder SetBitmap(string name, string path);
        IShotType Build();
    }
}