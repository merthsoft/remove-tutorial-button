using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Merthsoft.NoTutorialButton.Source.Patches {
    [HarmonyPatch(typeof(MainMenuDrawer), "DoMainMenuControls")]
    public class MainMenuDrawer_DoMainMenuControls {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            /***
             * This is the flow:
             * 0) Emit instructions until you read the "Tutorial" string: 
             *  ldstr     "Tutorial"
             * 1) Skip instructions until you get to the "NewColony" string:
             *  ldstr     "NewColony"
             * 1.5) Once we're at NewColony time, we need to make sure out stack is set up for the
             *      call virt that's gonna come up, so ldloc.2 to get the list object onto the stack
             *      then, emit the "NewColony" instruction
             * 2) Just emit the rest.
             ****/

            var step = 0;
            foreach (var instruction in instructions) {
                switch (step) {
                    case 0:
                        if (instruction.opcode == OpCodes.Ldstr && instruction.operand.ToString() == "Tutorial") {
                            step = 1;
                        } else {
                            yield return instruction;
                        }
                    break;
                    case 1:
                        if (instruction.opcode == OpCodes.Ldstr && instruction.operand.ToString() == "NewColony") {
                            yield return new CodeInstruction(OpCodes.Ldloc_2);
                            yield return instruction;
                            step = 2;
                        }
                    break;
                    case 2:
                        yield return instruction;
                        break;
                }
            }
        }
    }
}
