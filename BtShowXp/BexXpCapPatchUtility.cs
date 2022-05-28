using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{
    public static class BexXpCapPatchUtility
    {

        private const string OriginalXpCapMethodIlHash = "DJrdfbYWfHJnfvNiaGAY5j0dmF+7SGSAbP5wGGxtFNI=";

        /// <summary>
        /// returns true if the UseBexXpCapFix is true, the BEX_CE dll is loaded
        /// and the original code matches the hash created at the time of this function's writing.
        /// </summary>
        /// <returns></returns>
        public static bool UseDifficutlyXpPatch()
        {
            if (Core.ModSettings.UseBexXpCapFix == false) { return false; }

            //check if the BEX CE assembly is available.
            MethodInfo methodInfo = GetBexXpCapMethod();

            if (methodInfo == null)
            {
                Logger.LogDebug("Method not found.  Using original DifficultyXPCap patch.");
                return false;
            }

            //Only run the patch if the code exactly matches the hash of the original mod.
            string bexMethodHash = GetMethodHash();

            if (bexMethodHash == OriginalXpCapMethodIlHash)
            {
                Logger.LogDebug("Original Code Matched.  Patched DifficultyXPCap.");
                return true;
            }
            else
            {
                Logger.LogDebug("Original Code Changed.  Did not patch DifficultyXPCap.");
                Logger.LogDebug($"Expected: {OriginalXpCapMethodIlHash} Actual: {bexMethodHash}");

                return false;
            }
        }

        private static string GetMethodHash()
        {
            byte[] functionIl = GetBexXpCapMethod().GetMethodBody().GetILAsByteArray();

            var sha256hasher = System.Security.Cryptography.SHA256.Create();
            byte[] hash = sha256hasher.ComputeHash(functionIl);

            return Convert.ToBase64String(hash);
        }

        public static MethodInfo GetBexXpCapMethod()
        {

            var assembly = Assembly.Load("Extended_CE");
            if (assembly == null) return null;

            var type = assembly.GetType("Extended_CE.Functionality.DifficultyXPCap");
            if (type == null) return null;

            var nestedType = type.GetNestedType("AAR_UnitStatusWidget_FillInPilotData");
            if (nestedType == null) return null;

            MethodInfo methodInfo = nestedType.GetMethod("Prefix", BindingFlags.Static | BindingFlags.NonPublic);
            if (methodInfo == null) return null;

            return methodInfo;
        }
    }
}
