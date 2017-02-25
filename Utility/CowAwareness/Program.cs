namespace CowAwareness
{
    using CowAwareness.Detectors;
    using CowAwareness.Drawings;
    using CowAwareness.Features;
    using CowAwareness.Trackers;

    public class Program
    {
        #region Public Methods and Operators

        public static void Main(string[] args)
        {
            var addon =
                new Addon("CowAwareness").Add(new Clock())
                    .Add(new Clone())
                    .Add(new TowerRange())
                    .Add(new Gank())
                    .Add(new Teleport())
                    .Add(new Cooldown())
                    .Add(new Ward())
                    .Add(new WatermarkDisabler());

            addon.MenuInitialized += menu =>
                {
                    menu.AddGroupLabel("Version");
                    menu.AddLabel("release 1.0.2");

                    menu.AddSeparator();
                    menu.AddGroupLabel("Todo List");
                    menu.AddLabel("- try to fix cooldown for some special abilities");

                    menu.AddSeparator();
                    menu.AddGroupLabel("Yardim edenler");
                    menu.AddLabel(" Turkce Ceviri TekinTR");
                    menu.AddLabel(" Eksik,Hatali,Yanlis ceviri varsa bana forum uzerinden ulasin");
                    menu.AddLabel("- Lizzaran for SFXUtility, ondan guzel seyler aldım");
                    menu.AddLabel("- Kurttuu tasarimi yapti");
                    menu.AddLabel("- MrArticuno's Kule menzilini kodladi (kucuk gelistirmeler benden)");
                    menu.AddLabel("- Yapimci by strcow");
                };
        }

        #endregion
    }
}