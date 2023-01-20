using Microsoft.Extensions.Localization;

namespace FinanMan.SharedLocalization
{
    public class SharedLocalizerService : IStringLocalizer
    {
        private readonly IStringLocalizer _localizer;
        public SharedLocalizerService(IStringLocalizerFactory stringLocalizerFactory)
        {
            _localizer = stringLocalizerFactory.Create(typeof(SharedLocalizerService));
        }

        public LocalizedString this[string name] => _localizer[name];

        public LocalizedString this[string name, params object[] arguments] => _localizer[name, arguments];

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizer.GetAllStrings(includeParentCultures);
        }
    }
}