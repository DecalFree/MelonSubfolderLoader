using System;
using System.IO;
using MelonLoader;
using MelonLoader.Utils;

[assembly: MelonInfo(typeof(MelonSubfolderLoader.MelonSubfolderLoader), "Melon Subfolder Loader", "1.0.0", "DecalFree")]
[assembly: MelonGame(null, null)]

namespace MelonSubfolderLoader;

internal class MelonSubfolderLoader : MelonPlugin {
    public override void OnPreInitialization() {
        string[] melonFolders = [
            MelonEnvironment.ModsDirectory,
            MelonEnvironment.PluginsDirectory,
            MelonEnvironment.UserLibsDirectory
        ];

        foreach (string melonFolder in melonFolders) {
            foreach (string subfolder in Directory.GetDirectories(melonFolder)) {
                foreach (string melonMod in Directory.GetFiles(subfolder, "*.dll", SearchOption.AllDirectories)) {
                    try {
                        MelonAssembly melonAssembly = MelonAssembly.LoadMelonAssembly(melonMod);
                        if (melonAssembly == null) {
                            MelonLogger.Warning($"Failed to load {Path.GetFileName(melonMod)} MelonAssembly");
                            continue;
                        }
                    
                        RegisterSorted(melonAssembly.LoadedMelons);
                        MelonLogger.Msg($"Successfully loaded {Path.GetFileName(melonMod)} MelonAssembly");
                    }
                    catch (Exception exception) {
                        MelonLogger.Error($"Failed to load {melonMod} MelonMod: {exception.Message}");
                    }
                }
            }
        }
    }
}