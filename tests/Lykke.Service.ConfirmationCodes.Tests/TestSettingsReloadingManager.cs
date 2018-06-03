using System;
using System.IO;
using System.Threading.Tasks;
using Lykke.SettingsReader;

namespace Lykke.Service.ConfirmationCodes.Tests
{
    public class TestSettingsReloadingManager<TSettings> : ReloadingManagerBase<TSettings>
    {
        protected override async Task<TSettings> Load()
        {
            using (var reader = File.OpenText(_path))
            {
                var content = await reader.ReadToEndAsync();
                return SettingsProcessor.Process<TSettings>(content, false);
            }
        }

        private readonly string _path;

        public TestSettingsReloadingManager(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path not specified.", nameof(path));
            }

            _path = path;
        }
    }
}
