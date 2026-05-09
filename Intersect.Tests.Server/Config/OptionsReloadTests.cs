using Intersect.Config;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Intersect.Tests.Server.Config;

[TestFixture]
public class OptionsReloadTests
{
    private string _resourcesDirectory = null!;

    [SetUp]
    public void SetUp()
    {
        _resourcesDirectory = Path.Combine(
            Path.GetTempPath(),
            nameof(OptionsReloadTests),
            Guid.NewGuid().ToString("N")
        );

        Directory.CreateDirectory(_resourcesDirectory);
        Options.ResourcesDirectory = _resourcesDirectory;
        Options.EnsureCreated();

        Options.Instance.AdminOnly = false;
        Options.Instance.Logging.Level = LogLevel.Information;
        Options.Instance.Sprites.AttackFrames = 4;
        Options.Instance.Sprites.IdleFrameDuration = 200;
        Options.Instance.Equipment.WeaponSlot = 2;
        Options.SaveToDisk();
        Options.LoadFromDisk();
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_resourcesDirectory))
        {
            Directory.Delete(_resourcesDirectory, true);
        }
    }

    [Test]
    public void ReloadLiveFromDisk_AppliesOnlyReloadSafeConfiguration()
    {
        var originalInstance = Options.Instance;
        var updated = Options.Instance.DeepClone();
        updated.AdminOnly = true;
        updated.Logging.Level = LogLevel.Debug;
        updated.Sprites.AttackFrames = 8;
        updated.Sprites.IdleFrameDuration = 350;
        updated.Equipment.WeaponSlot = 1;

        var pathToServerConfig = Path.Combine(_resourcesDirectory, "config.json");
        File.WriteAllText(pathToServerConfig, JsonConvert.SerializeObject(updated, Formatting.Indented));

        var result = Options.ReloadLiveFromDisk();

        Assert.Multiple(() =>
        {
            Assert.That(Options.Instance, Is.SameAs(originalInstance));
            Assert.That(Options.Instance.AdminOnly, Is.True);
            Assert.That(Options.Instance.Logging.Level, Is.EqualTo(LogLevel.Debug));
            Assert.That(Options.Instance.Sprites.IdleFrameDuration, Is.EqualTo(350));
            Assert.That(Options.Instance.Sprites.AttackFrames, Is.EqualTo(4));
            Assert.That(Options.Instance.Equipment.WeaponSlot, Is.EqualTo(2));
            Assert.That(result.AppliedChanges, Does.Contain(nameof(Options.AdminOnly)));
            Assert.That(result.AppliedChanges, Does.Contain($"{nameof(Options.Logging)}.{nameof(LoggingOptions.Level)}"));
            Assert.That(
                result.AppliedChanges,
                Does.Contain($"{nameof(Options.Sprites)}.{nameof(SpriteOptions.IdleFrameDuration)}")
            );
            Assert.That(
                result.RestartRequiredChanges,
                Does.Contain($"{nameof(Options.Sprites)}.{nameof(SpriteOptions.AttackFrames)}")
            );
            Assert.That(result.RestartRequiredChanges, Does.Contain(nameof(Options.Equipment)));
        });
    }
}
