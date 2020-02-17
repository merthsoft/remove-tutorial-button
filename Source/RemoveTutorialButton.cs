using HarmonyLib;
using System.Reflection;
using Verse;

namespace Merthsoft.NoTutorialButton {
    [StaticConstructorOnStartup]
    public class RemoveTutorialButton {
        static RemoveTutorialButton() {
            var harmony = new Harmony("Merthsoft.NoTutorialButton");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
