using liminal_api.Modules.Image.Domain;
using liminal_api.Modules.LevelClass.Domain.Enums;

namespace liminal_api.Modules.LevelClass.Domain
{
    public class LevelClass
    {
        public int? Id { get; private set; }
        public SurvivalClassType Type { get; private set; }
        public string? ClassNumber { get; private set; }
        public string SecurityLevel { get; private set; }
        public Legitimacy Legitimacy { get; private set; }
        public string DangerLevel { get; private set; }
        public ImageCloudinary IconImage { get; private set; }
        public string Description { get; private set; }

        public LevelClass(
            SurvivalClassType type,
            string? classNumber,
            string securityLevel,
            Legitimacy legitimacy,
            string dangerLevel,
            ImageCloudinary iconImage,
            string description,
            int? id = null)
        {
            Type = type;
            ClassNumber = classNumber;
            SecurityLevel = securityLevel;
            Legitimacy = legitimacy;
            DangerLevel = dangerLevel;
            IconImage = iconImage;
            Description = description;
            Id = id;
        }

        public void ChangeType(SurvivalClassType type) => Type = type;
        public void ChangeClassNumber(string? classNumber) => ClassNumber = classNumber;
        public void ChangeSecurityLevel(string securityLevel) => SecurityLevel = securityLevel;
        public void ChangeLegitimacy(Legitimacy legitimacy) => Legitimacy = legitimacy;
        public void ChangeDangerLevel(string dangerLevel) => DangerLevel = dangerLevel;
        public void ChangeIconImage(ImageCloudinary iconImage) => IconImage = iconImage;
        public void ChangeDescription(string description) => Description = description;
    }
}
